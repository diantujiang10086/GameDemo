using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class BatchDataBuffer : IDisposable
{
    private const uint kIsPerInstanceBit = 0x80000000;
    protected static int Float4Size = 16; // float4
    protected static bool UseConstantBuffer = BatchRendererGroup.BufferTarget == BatchBufferTarget.ConstantBuffer;

    private bool m_isInitialized;
    private int m_instanceCount;
    private int m_maxInstances;
    private int m_gpuWindowCount;
    private int m_totalGpuBufferSize;
    private int m_alignedGPUWindowSize;
    protected int m_windowSizeInFloat4;
    protected int m_maxInstancePerWindow;
    private GraphicsBuffer m_GPUInstanceData;
    private JobHandle updateGraphicsBufferFence;
    protected KeyValuePair<int, int>[] propertyIDs;
    protected NativeArray<float4> m_sysmemBuffer;

    public int InstanceCount => m_instanceCount;
    public int InstancePerWindow => m_maxInstancePerWindow;

    public int WindowSizeInFloat4 => m_windowSizeInFloat4;

    public bool IsJobCompleted => updateGraphicsBufferFence.IsCompleted;

    public void Initialize(int maxInstance)
    {
        m_maxInstances = maxInstance;

        propertyIDs = PropertyToID();

        int gpuItemSize = 0;
        for (int i = 0; i < propertyIDs.Length; i++)
            gpuItemSize += propertyIDs[i].Value;
        gpuItemSize *= Float4Size;

        if (UseConstantBuffer)
        {
            m_alignedGPUWindowSize = BatchRendererGroup.GetConstantBufferMaxWindowSize();
            m_maxInstancePerWindow = m_alignedGPUWindowSize / gpuItemSize;
            m_gpuWindowCount = (m_maxInstances + m_maxInstancePerWindow - 1) / m_maxInstancePerWindow;
            m_totalGpuBufferSize = m_gpuWindowCount * m_alignedGPUWindowSize;
            m_GPUInstanceData = new GraphicsBuffer(GraphicsBuffer.Target.Constant, m_totalGpuBufferSize / 16, 16);
        }
        else
        {
            m_alignedGPUWindowSize = (m_maxInstances * gpuItemSize + 15) & (-16);
            m_maxInstancePerWindow = m_maxInstances;
            m_gpuWindowCount = 1;
            m_totalGpuBufferSize = m_gpuWindowCount * m_alignedGPUWindowSize;
            m_GPUInstanceData = new GraphicsBuffer(GraphicsBuffer.Target.Raw, m_totalGpuBufferSize / 4, 4);
        }
        m_windowSizeInFloat4 = m_alignedGPUWindowSize / Float4Size;
        m_sysmemBuffer = new NativeArray<float4>(m_totalGpuBufferSize / Float4Size, Allocator.Persistent, NativeArrayOptions.ClearMemory);
        m_isInitialized = true;
    }

    public void UpdateSystemBuffer(DisplayBatch displayBatch, int instanceCount)
    {
        updateGraphicsBufferFence = OnUpdateSystemBuffer(displayBatch, instanceCount, updateGraphicsBufferFence);
    }

    [BurstCompile]
    public void UploadGpuData(int instanceCount)
    {
        if ((uint)instanceCount > (uint)m_maxInstances)
            return;

        updateGraphicsBufferFence.Complete();

        m_instanceCount = instanceCount;
        int completeWindows = m_instanceCount / m_maxInstancePerWindow;

        if (completeWindows > 0)
        {
            int sizeInFloat4 = (completeWindows * m_alignedGPUWindowSize) / Float4Size;
            m_GPUInstanceData.SetData(m_sysmemBuffer, 0, 0, sizeInFloat4);
        }

        int lastBatchId = completeWindows;
        int itemInLastBatch = m_instanceCount - m_maxInstancePerWindow * completeWindows;

        if (itemInLastBatch > 0)
        {
            int windowOffsetInFloat4 = (lastBatchId * m_alignedGPUWindowSize) / Float4Size;
            int offset = 0;
            for (int i = 0; i < propertyIDs.Length; i++)
            {
                m_GPUInstanceData.SetData(m_sysmemBuffer, offset, offset, itemInLastBatch * propertyIDs[i].Value);
                offset += windowOffsetInFloat4 + m_maxInstancePerWindow * propertyIDs[i].Value;
            }
        }
    }

    public int GetMaxInstanceCount()
    {
        return m_maxInstances;
    }

    public BatchID[] AddBatch(BatchRendererGroup batchRendererGroup)
    {
        var meta = new NativeArray<MetadataValue>(propertyIDs.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

        var batchIds = new BatchID[m_gpuWindowCount];
        for (int i = 0; i < m_gpuWindowCount; i++)
        {
            int offset = 0;
            for (int j = 0; j < propertyIDs.Length; j++)
            {
                var propertyID = propertyIDs[j];
                meta[j] = CreateMetadataValue(propertyID.Key, offset);
                offset += m_maxInstancePerWindow * propertyID.Value * Float4Size;
            }
            batchIds[i] = batchRendererGroup.AddBatch(meta, m_GPUInstanceData.bufferHandle,
                (uint)(i * m_alignedGPUWindowSize), UseConstantBuffer ? (uint)m_alignedGPUWindowSize : 0);
        }

        meta.Dispose();
        return batchIds;
    }

    public virtual void Dispose()
    {
        if (!m_isInitialized)
            return;

        updateGraphicsBufferFence.Complete();
        m_sysmemBuffer.Dispose();
        m_GPUInstanceData.Dispose();
    }

    protected static MetadataValue CreateMetadataValue(int nameID, int gpuOffset)
    {
        return new MetadataValue
        {
            NameID = nameID,
            Value = (uint)gpuOffset | kIsPerInstanceBit,
        };
    }

    protected abstract KeyValuePair<int, int>[] PropertyToID();
    protected abstract JobHandle OnUpdateSystemBuffer(DisplayBatch displayBatch, int instanceCount, JobHandle inputJob);
}
