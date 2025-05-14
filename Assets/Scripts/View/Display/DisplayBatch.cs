using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using System;

public class DisplayBatch : IDisposable
{
    protected int maxInstance;
    protected int displayId;
    protected Material material;
    private Pool<Display> displays;
    protected BatchDataBuffer batchDataBuffer;
    private BRGContainer brgContainer;

    private NativeArray<float3> positions;
    private NativeArray<float3> scales;
    private NativeArray<quaternion> rotations;

    public int DisplayCount => displays.Count;

    public NativeArray<float3> Positions => positions;
    public NativeArray<float3> Scales => scales;
    public NativeArray<quaternion> Rotations => rotations;

    public DisplayBatch(int displayId, Mesh mesh, int maxInstance)
    {
        this.maxInstance = maxInstance;
        this.displayId = displayId;
        var displayConfig = ConfigManager.Instance.GetConfig<DisplayConfig>(displayId);
        material = ResourceManager.Instance.Load<Material>(displayConfig.materialPath);

        displays = new Pool<Display>(maxInstance);
        positions = new NativeArray<float3>(maxInstance, Allocator.Persistent);
        scales = new NativeArray<float3>(maxInstance, Allocator.Persistent);
        rotations = new NativeArray<quaternion>(maxInstance, Allocator.Persistent);
        batchDataBuffer = CreateBatchDataBuffer();
        batchDataBuffer.Initialize(maxInstance);
        brgContainer = new BRGContainer(batchDataBuffer, mesh, material);
        OnInitialize();
    }

    public Display CreateDisplay()
    {
        var display = displays.CreateElement(out var index);
        display.batchIndex = index;
        OnCreateDisplay(display);
        return display;
    }

    public void RemoveDisplay(Display display)
    {
        if (displays.RemoveAtSwapBack(display.batchIndex, out var swapIndex))
        {
            var swappedDisplay = displays.ElementAt(swapIndex);
            swappedDisplay.batchIndex = display.batchIndex;
        }
    }

    public void FillData()
    {
        batchDataBuffer.Complete();

        for (int i = 0; i < displays.Count; i++)
        {
            var display = displays.ElementAt(i);
            UpdateNativeArrays(i, display);
            if (display.isDirty)
            {
                positions[i] = display.position;
                scales[i] = display.scale;
                rotations[i] = display.rotation;
                display.ResetDirty();
            }
        }

        batchDataBuffer.UpdateSystemBuffer(this, displays.Count);
    }

    public void Update()
    {
        batchDataBuffer.UploadGpuData(displays.Count);
    }

    public void Dispose()
    {
        batchDataBuffer.Dispose();
        brgContainer.Dispose();
        positions.Dispose();
        scales.Dispose();
        rotations.Dispose();
        OnDispose();
    }

    protected virtual BatchDataBuffer CreateBatchDataBuffer()
    {
        return new SpriteBatchDataBuffer();
    }

    protected virtual void OnInitialize()
    {

    }

    protected virtual void OnCreateDisplay(Display display)
    {

    }
    protected virtual void UpdateNativeArrays(int index, Display display)
    {

    }
    protected virtual void OnDispose()
    {

    }
}
