using System;
using System.Collections.Generic;

public class DisplayBatchManager : Singleton<DisplayBatchManager>, IFixedUpdate, IUpdate
{
    public const int maxInstance = 10000;
    public Dictionary<int, DisplayBatch> batchs = new Dictionary<int, DisplayBatch>();

    public Display CreateDisplay(int displayId)
    {
        if (!batchs.TryGetValue(displayId, out var batchDisplay))
        {
            var displayConfig = ConfigManager.Instance.GetConfig<DisplayConfig>(displayId);
            if (displayConfig == null)
                return default;
            if (displayConfig.materialType == MaterialType.Sprine)
            {
                batchDisplay = new DisplayBatch(displayId, ViewHelper.MakeQuad(), maxInstance);
            }
            else if (displayConfig.materialType == MaterialType.Atlas)
            {
                batchDisplay = new AtlasDisplayBatch(displayId, ViewHelper.MakeQuad(), maxInstance);
            }
            batchs[displayId] = batchDisplay;
        }
        var display = batchDisplay.CreateDisplay();
        display.batchId = displayId;
        return display;
    }

    public void RemoveDisplay(Display display)
    {
        if (batchs.TryGetValue(display.batchId, out var batchDisplay))
        {
            batchDisplay.RemoveDisplay(display);
        }
    }

    void IFixedUpdate.FixedUpdate(float elaspedTime)
    {
        foreach (var batch in batchs.Values)
        {
            batch.FillData();
        }
    }

    void IUpdate.Update()
    {
        foreach (var batch in batchs.Values)
        {
            batch.Update();
        }
    }

    protected override void Destroy()
    {
        foreach (var display in batchs.Values)
        {
            display.Dispose();
        }
        batchs.Clear();
    }

}
