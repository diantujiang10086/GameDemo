using System.Collections.Generic;

public class DisplayManager : Singleton<DisplayManager>, IUpdate
{
    private List<Display> displays = new List<Display>(10000);
    private Dictionary<long, Display> displayDict = new Dictionary<long, Display>(10000);

    public void CreateDisplay(DisplayComponent displayComponent)
    {
        var config = displayComponent.GetParent<Unit>().Config;
        
        var display = DisplayBatchManager.Instance.CreateDisplay(config.displayId);
        if (display == null)
            return;
        display.SetDisplayComponent(displayComponent);
        displayDict[displayComponent.InstanceId] = display;
        displays.Add(display);
    }

    public Display GetDisplay(Unit unit)
    {
        return GetDisplay(unit.InstanceId);
    }

    public Display GetDisplay(DisplayComponent displayComponent)
    {
        return GetDisplay(displayComponent.InstanceId);
    }

    public Display GetDisplay(long unitInstanceId)
    {
        displayDict.TryGetValue(unitInstanceId, out var display);
        return display;
    }

    public void RemoveDisplay(DisplayComponent displayComponent)
    {
        RemoveDisplay(displayComponent.InstanceId);
    }

    public void RemoveDisplay(long actorId)
    {
        if (displayDict.TryGetValue(actorId, out var display))
        {
            DisplayBatchManager.Instance.RemoveDisplay(display);
            displays.Remove(display);
        }
    }

    public void Update()
    {
        foreach (var display in displays)
        {
            display.Update();
        }
    }

}
