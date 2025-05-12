using System;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : Singleton<DisplayManager>, IDisposable
{
    private List<Display> displays = new List<Display>(1000);
    private Dictionary<int, Display> displayDict = new Dictionary<int, Display>();
    private DisplayBatchManager displayBatchManager = new DisplayBatchManager();

    public void CreateDisplay(Actor actor)
    {
        var config = ConfigManager.Instance.Get<ActorConfig>(actor.configId);
        var display = displayBatchManager.CreateDisplay(config.materialId);
        display.SetActor(actor);
        displayDict[actor.id] = display;
        displays.Add(display);
    }

    public Display GetDisplay(Actor actor)
    {
        return GetDisplay(actor.id);
    }

    public Display GetDisplay(int actorId)
    {
        displayDict.TryGetValue(actorId, out var display);
        return display;
    }

    public void RemoveDisplay(Actor actor)
    {
        RemoveDisplay(actor.id);
    }

    public void RemoveDisplay(int actorId)
    {
        if (displayDict.TryGetValue(actorId, out var display))
        {
            displayBatchManager.RemoveDisplay(display);
            displays.Remove(display);
        }
    }

    public void FixedUpdate()
    {
        foreach (var display in displays) 
        {
            display.FixedUpdate();
        }
        displayBatchManager.FixedUpdate();
    }

    public void Dispose()
    {
        displayBatchManager.Dispose();
    }

    public void Update()
    {
        displayBatchManager.Update();
    }

}
