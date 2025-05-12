using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class AtlasBatchDataBuffer : BatchDataBuffer
{
    protected override KeyValuePair<int, int>[] PropertyToID()
    {
        return new KeyValuePair<int, int>[]
        {
            new KeyValuePair<int, int>(Shader.PropertyToID("unity_ObjectToWorld"),3),
            new KeyValuePair<int, int>(Shader.PropertyToID("_TileParams"),1),
            new KeyValuePair<int, int>(Shader.PropertyToID("_Size"),1),
        };
    }
    
    protected override JobHandle OnUpdateSystemBuffer(DisplayBatch displayBatch, int instanceCount, JobHandle inputJob)
    {
        var atlasDisplayBatch = displayBatch as AtlasDisplayBatch;
        return new UpdateGraphicsBuffer
        {
            time = Time.time,
            instancePerWindow = m_maxInstancePerWindow,
            windowSizeInFloat4 = WindowSizeInFloat4,
            animationDatas = atlasDisplayBatch.AnimationDatas,
            animationTiles = atlasDisplayBatch.Tiles,
            animationSizes = atlasDisplayBatch.Sizes,
            animations = atlasDisplayBatch.Animations,
            positions = displayBatch.Positions,
            scales = displayBatch.Scales,
            rotations = displayBatch.Rotations,
            graphicsBufferData = m_sysmemBuffer,
        }.ScheduleParallel(instanceCount, 128, inputJob);
    }

    [BurstCompile]
    public struct UpdateGraphicsBuffer : IJobFor
    {
        public float time;
        public int instancePerWindow;
        public int windowSizeInFloat4;

        [ReadOnly] public NativeArray<int4> animationDatas;
        [ReadOnly] public NativeArray<float4> animationTiles;
        [ReadOnly] public NativeArray<float4> animationSizes;

        [ReadOnly] public NativeArray<float2> animations;
        [ReadOnly] public NativeArray<float3> positions;
        [ReadOnly] public NativeArray<float3> scales;
        [ReadOnly] public NativeArray<quaternion> rotations;

        [WriteOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<float4> graphicsBufferData;
        public void Execute(int index)
        {
            int windowId = System.Math.DivRem(index, instancePerWindow, out var i);
            int windowOffsetInFloat4 = windowId * windowSizeInFloat4;

            var pos = positions[index];
            var rot = math.mul(math.float3x3(rotations[index]), float3x3.Scale(scales[index]));

            var objectToWorldOffset = windowOffsetInFloat4 + index * 3;
            graphicsBufferData[objectToWorldOffset + 0] = new float4(rot.c0.x, rot.c0.y, rot.c0.z, rot.c1.x);
            graphicsBufferData[objectToWorldOffset + 1] = new float4(rot.c1.y, rot.c1.z, rot.c2.x, rot.c2.y);
            graphicsBufferData[objectToWorldOffset + 2] = new float4(rot.c2.z, pos.x, pos.y, pos.z);

            var animationIndex = (int)animations[index].x;
            var startTime = animations[index].y;

            var animation = animationDatas[animationIndex];
            int loop = animation.x;
            int frameRate = animation.y;
            int animationOffset = animation.z;
            int animationCount = animation.w;

            int currentFrame = (int)((time - startTime) * frameRate);
            
            if (loop == 1)
            {
                currentFrame %= animationCount;
            }
            else
            {
                currentFrame = math.min(currentFrame, animationCount - 1); 
            }

            int curAnimationIndex = animationOffset + currentFrame;

            var tileOffset = windowOffsetInFloat4 + instancePerWindow * 3 + i;
            graphicsBufferData[tileOffset] = animationTiles[curAnimationIndex];

            var sizeOffset = tileOffset + instancePerWindow;
            graphicsBufferData[sizeOffset] = animationSizes[curAnimationIndex];
        }
    }
}
