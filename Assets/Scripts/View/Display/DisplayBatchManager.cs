using System;
using System.Collections.Generic;

public class DisplayBatchManager : IDisposable
{
    public const int maxInstance = 10000;
    public Dictionary<int, DisplayBatch> batchs = new Dictionary<int, DisplayBatch>();

    public Display CreateDisplay(int materialId)
    {
        if (!batchs.TryGetValue(materialId, out var batchDisplay))
        {
            var materialConfig = ConfigManager.Instance.GetConfig<MaterialConfig>(materialId);
            if (materialConfig.materialType == MaterialType.Sprine)
            {
                batchDisplay = new DisplayBatch(materialId, ViewHelper.MakeQuad(), maxInstance);
            }
            else if (materialConfig.materialType == MaterialType.Atlas)
            {
                batchDisplay = new AtlasDisplayBatch(materialId, ViewHelper.MakeQuad(), maxInstance);
            }
            batchs[materialId] = batchDisplay;
        }
        var display = batchDisplay.CreateDisplay();
        display.batchId = materialId;
        return display;
    }

    public void RemoveDisplay(Display display)
    {
        if (batchs.TryGetValue(display.batchId, out var batchDisplay))
        {
            batchDisplay.RemoveDisplay(display);
        }
    }

    public void FixedUpdate()
    {
        foreach (var display in batchs.Values)
        {
            display.FixedUpdate();
        }
    }

    public void Update()
    {
        foreach (var display in batchs.Values)
        {
            display.Update();
        }
    }

    public void Dispose()
    {
        foreach (var display in batchs.Values)
        {
            display.Dispose();
        }
        batchs.Clear();
    }

}
