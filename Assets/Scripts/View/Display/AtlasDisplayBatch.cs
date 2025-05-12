using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class AtlasDisplayBatch : DisplayBatch
{
    private bool isInitialize = false;
    private AtlasAnimation[] atlasAnimations;
    private NativeArray<int4> animationDatas;
    private NativeArray<float4> tiles;
    private NativeArray<float4> sizes;
    private NativeArray<float2> animtions;

    public NativeArray<int4> AnimationDatas => animationDatas;
    public NativeArray<float4> Tiles => tiles;
    public NativeArray<float4> Sizes => sizes;
    public NativeArray<float2> Animations => animtions;

    public AtlasDisplayBatch(int materialId, Mesh mesh, int maxInstance) : base(materialId, mesh, maxInstance)
    {
        try
        {
            var materialConfig = ConfigManager.Instance.Get<MaterialConfig>(materialId);
            var data = ResourceManager.Load<AtlasAnimationData>(materialConfig.atlasPath);

            atlasAnimations = data.atlasAnimations;

            int length = 0;
            for (int i = 0; i < atlasAnimations.Length; i++)
                length += atlasAnimations[i].tiles.Length;

            tiles = new NativeArray<float4>(length, Allocator.Persistent);
            sizes = new NativeArray<float4>(length, Allocator.Persistent);
            animationDatas = new NativeArray<int4>(atlasAnimations.Length, Allocator.Persistent);

            int offset = 0;
            for (int i = 0; i < atlasAnimations.Length; i++)
            {
                var atlasAnimation = atlasAnimations[i];
                for (int j = 0; j < atlasAnimation.tiles.Length; j++)
                {
                    tiles[offset + j] = atlasAnimation.tiles[j];
                    var size = atlasAnimation.sizes[j];
                    sizes[offset + j] = new float4(size.x, size.y, 0, 0);
                }
                animationDatas[i] = new int4(
                    atlasAnimation.isLoop == true ? 1 : 0,
                    atlasAnimation.frameRate,
                    offset,
                    atlasAnimation.tiles.Length);

                offset += atlasAnimation.tiles.Length;
            }
            animtions = new NativeArray<float2>(maxInstance, Allocator.Persistent);
            isInitialize = true;
        }
        catch(Exception ex)
        {
            Log.Error(ex);
            Log.Warning($"load AtlasAnimationConfig fail! {materialId}");
        }
    }

    protected override BatchDataBuffer CreateBatchDataBuffer()
    {
        return new AtlasBatchDataBuffer();
    }

    protected override void UpdateNativeArrays(int index, Display display)
    {
        if (!isInitialize)
            return;
        animtions[index] = new float2(display.atlasAnimationIndex, display.animationStartTime);
    }

    protected override void OnDispose()
    {
        if (!isInitialize)
            return;

        tiles.Dispose();
        sizes.Dispose();
        animtions.Dispose();
        animationDatas.Dispose();
    }
}
