using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
using Unity.Jobs;

public class SpriteBatchDataBuffer : BatchDataBuffer
{
    protected override KeyValuePair<int, int>[] PropertyToID()
    {
        return new KeyValuePair<int, int>[]
        {
            new KeyValuePair<int, int>(Shader.PropertyToID("unity_ObjectToWorld"),3),
        };
    }

    protected override JobHandle OnUpdateSystemBuffer(DisplayBatch displayBatch, int instanceCount, JobHandle inputJob)
    {
        return new UpdateGraphicsBuffer
        {
            positions = displayBatch.Positions,
            scales = displayBatch.Scales,
            rotations = displayBatch.Rotations,
            graphicsBufferData = m_sysmemBuffer
        }.ScheduleParallel(instanceCount, 128, inputJob);

    }

    [BurstCompile]
    public struct UpdateGraphicsBuffer : IJobFor
    {
        [ReadOnly] public NativeArray<float3> positions;
        [ReadOnly] public NativeArray<float3> scales;
        [ReadOnly] public NativeArray<quaternion> rotations;

        [WriteOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<float4> graphicsBufferData;
        public void Execute(int index)
        {
            var pos = positions[index];
            var rot = math.mul(math.float3x3(rotations[index]), float3x3.Scale(scales[index]));

            var objectToWorldOffset = index * 3;
            graphicsBufferData[objectToWorldOffset + 0] = new float4(rot.c0.x, rot.c0.y, rot.c0.z, rot.c1.x);
            graphicsBufferData[objectToWorldOffset + 1] = new float4(rot.c1.y, rot.c1.z, rot.c2.x, rot.c2.y);
            graphicsBufferData[objectToWorldOffset + 2] = new float4(rot.c2.z, pos.x, pos.y, pos.z);
        }
    }
}
