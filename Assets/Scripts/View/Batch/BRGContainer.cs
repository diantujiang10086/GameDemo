using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;

public unsafe class BRGContainer : IDisposable
{
    private static Bounds globalBound = new Bounds(Vector3.zero, new Vector3(1048576.0f, 1048576.0f, 1048576.0f));

    private bool m_castShadows;
    private BatchID[] m_batchIDs;
    private BatchMeshID m_meshID;
    private BatchMaterialID m_materialID;
    private BatchRendererGroup m_BatchRendererGroup;
    private BatchDataBuffer batchDataBuffer;

    public BatchDataBuffer BatchDataBuffer => batchDataBuffer;

    public BRGContainer(BatchDataBuffer batchDataBuffer, Mesh mesh, Material material, bool castShadows = false)
    {
        m_castShadows = castShadows;
        this.batchDataBuffer = batchDataBuffer;
        m_BatchRendererGroup = new BatchRendererGroup(OnPerformCulling, IntPtr.Zero);
        m_BatchRendererGroup.SetGlobalBounds(globalBound);
        if (mesh) m_meshID = m_BatchRendererGroup.RegisterMesh(mesh);
        if (material) m_materialID = m_BatchRendererGroup.RegisterMaterial(material);

        m_batchIDs = batchDataBuffer.AddBatch(m_BatchRendererGroup);
    }

    public void Dispose()
    {
        for (int i = 0; i < m_batchIDs.Length; i++)
        {
            m_BatchRendererGroup.RemoveBatch(m_batchIDs[i]);
        }

        m_BatchRendererGroup.UnregisterMaterial(m_materialID);
        m_BatchRendererGroup.UnregisterMesh(m_meshID);
        m_BatchRendererGroup.Dispose();
    }

    private static T* Malloc<T>(uint count) where T : unmanaged
    {
        return (T*)UnsafeUtility.Malloc(
            UnsafeUtility.SizeOf<T>() * count,
            UnsafeUtility.AlignOf<T>(),
            Allocator.TempJob);
    }

    [BurstCompile]
    private JobHandle OnPerformCulling(BatchRendererGroup rendererGroup, BatchCullingContext cullingContext, BatchCullingOutput cullingOutput, IntPtr userContext)
    {
        BatchCullingOutputDrawCommands drawCommands = new BatchCullingOutputDrawCommands();

        // calculate the amount of draw commands we need in case of UBO mode (one draw command per window)
        int drawCommandCount = (batchDataBuffer.InstanceCount + batchDataBuffer.InstancePerWindow - 1) / batchDataBuffer.InstancePerWindow;
        int maxInstancePerDrawCommand = batchDataBuffer.InstancePerWindow;
        drawCommands.drawCommandCount = drawCommandCount;

        // Allocate a single BatchDrawRange. ( all our draw commands will refer to this BatchDrawRange)
        drawCommands.drawRangeCount = 1;
        drawCommands.drawRanges = Malloc<BatchDrawRange>(1);
        drawCommands.drawRanges[0] = new BatchDrawRange
        {
            drawCommandsBegin = 0,
            drawCommandsCount = (uint)drawCommandCount,
            filterSettings = new BatchFilterSettings
            {
                renderingLayerMask = 1,
                layer = 0,
                motionMode = MotionVectorGenerationMode.Camera,
                shadowCastingMode = m_castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off,
                receiveShadows = true,
                staticShadowCaster = false,
                allDepthSorted = false
            }
        };

        if (drawCommands.drawCommandCount > 0)
        {
            // as we don't need culling, the visibility int array buffer will always be {0,1,2,3,...} for each draw command
            // so we just allocate maxInstancePerDrawCommand and fill it
            int visibilityArraySize = maxInstancePerDrawCommand;
            if (batchDataBuffer.InstanceCount < visibilityArraySize)
                visibilityArraySize = batchDataBuffer.InstanceCount;

            drawCommands.visibleInstances = Malloc<int>((uint)visibilityArraySize);

            // As we don't need any frustum culling in our context, we fill the visibility array with {0,1,2,3,...}
            for (int i = 0; i < visibilityArraySize; i++)
                drawCommands.visibleInstances[i] = i;

            // Allocate the BatchDrawCommand array (drawCommandCount entries)
            // In SSBO mode, drawCommandCount will be just 1
            drawCommands.drawCommands = Malloc<BatchDrawCommand>((uint)drawCommandCount);
            int left = batchDataBuffer.InstanceCount;
            for (int b = 0; b < drawCommandCount; b++)
            {
                int inBatchCount = left > maxInstancePerDrawCommand ? maxInstancePerDrawCommand : left;
                drawCommands.drawCommands[b] = new BatchDrawCommand
                {
                    visibleOffset = (uint)0,    // all draw command is using the same {0,1,2,3...} visibility int array
                    visibleCount = (uint)inBatchCount,
                    batchID = m_batchIDs[b],
                    materialID = m_materialID,
                    meshID = m_meshID,
                    submeshIndex = 0,
                    splitVisibilityMask = 0xff,
                    flags = BatchDrawCommandFlags.None,
                    sortingPosition = 0
                };
                left -= inBatchCount;
            }
        }

        cullingOutput.drawCommands[0] = drawCommands;
        drawCommands.instanceSortingPositions = null;
        drawCommands.instanceSortingPositionFloatCount = 0;
        return default;
    }
}
