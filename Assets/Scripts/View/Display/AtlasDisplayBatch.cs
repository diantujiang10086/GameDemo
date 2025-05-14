using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class AtlasDisplayBatch : DisplayBatch
{
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
    }

    protected override void OnInitialize()
    {
        try
        {
            var displayConfig = ConfigManager.Instance.GetConfig<DisplayConfig>(displayId);
            var data = ResourceManager.Instance.Load<AtlasAnimationData>(displayConfig.atlasPath);

            atlasAnimations = data.atlasAnimations;

            int length = 0;
            for (int i = 0; i < atlasAnimations.Length; i++)
                length += atlasAnimations[i].tiles.Length;

            tiles = new NativeArray<float4>(length, Allocator.Persistent);
            sizes = new NativeArray<float4>(length, Allocator.Persistent);
            animationDatas = new NativeArray<int4>(atlasAnimations.Length, Allocator.Persistent);
            animtions = new NativeArray<float2>(maxInstance, Allocator.Persistent);

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
        }
        catch (Exception ex)
        {
            Log.Warning($"load AtlasAnimationConfig fail! {displayId}\n{ex.Message}");
        }
    }

    protected override BatchDataBuffer CreateBatchDataBuffer()
    {
        return new AtlasBatchDataBuffer();
    }

    protected override void UpdateNativeArrays(int index, Display display)
    {
        animtions[index] = new float2(display.atlasAnimationIndex, display.animationStartTime);
    }

    protected override void OnDispose()
    {
        tiles.Dispose();
        sizes.Dispose();
        animtions.Dispose();
        animationDatas.Dispose();
    }
}
