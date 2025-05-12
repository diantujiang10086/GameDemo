using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct UpdateGraphicsBuffer : IJobFor
{
    public int instancePerWindow;
    public int windowSizeInFloat4;
    public float4 textureOffset;
    public float4 textureSize;
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
        var rot = math.mul(math.float3x3(rotations[index]),float3x3.Scale(scales[index]));

        var objectToWorldOffset = windowOffsetInFloat4 + index * 3;
        graphicsBufferData[objectToWorldOffset + 0] = new float4(rot.c0.x, rot.c0.y, rot.c0.z, rot.c1.x);
        graphicsBufferData[objectToWorldOffset + 1] = new float4(rot.c1.y, rot.c1.z, rot.c2.x, rot.c2.y);
        graphicsBufferData[objectToWorldOffset + 2] = new float4(rot.c2.z, pos.x, pos.y, pos.z);
        
        var tileOffset = windowOffsetInFloat4 + instancePerWindow * 3 + i;
        graphicsBufferData[tileOffset] = textureOffset;

        var sizeOffset = tileOffset + instancePerWindow;
        graphicsBufferData[sizeOffset] = textureSize;
    }
}
