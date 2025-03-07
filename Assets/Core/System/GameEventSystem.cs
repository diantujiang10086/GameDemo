using System;
using System.Collections.Generic;
using System.Reflection;
public interface IEvent
{
    Type Type { get; }

}

public abstract class AEvent<A> : IEvent
{
    public Type Type => typeof(A);

    protected abstract void Run(A a);

    public void Handle(A a)
    {
        try
        {
            Run(a);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }
}

public class GameEventSystem 
{
    private static GameEventSystem instance;
    public static GameEventSystem inst => instance ??= new GameEventSystem();
    private List<Assembly> assemblies = new List<Assembly>();
    private Dictionary<Type, IEvent> allEvents = new Dictionary<Type, IEvent>();

    public void AddAssembly(Assembly assembly)
    {
        assemblies.Add(assembly);
        allEvents.Clear();
        foreach (var item in Utils.GetTypes<EventAttribute>(this.assemblies))
        {
            IEvent obj = Activator.CreateInstance(item.type) as IEvent;
            if (obj == null)
            {
                Log.Error($"type not is AEvent: {item.type.Name}");
                continue;
            }
            var eventType = obj.Type;
            allEvents[eventType] = obj;
        }
    }

    public static void Publish<T>(T a) where T : struct
    {
        if (!inst.allEvents.TryGetValue(typeof(T), out var _event))
            return;

        if (_event is AEvent<T> aEvent)
        {
            aEvent.Handle(a);
        }
    }
}
