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

public class GameEventSystem : ISystem, IAwake
{
    private static GameEventSystem instance;
    private List<Assembly> assemblies = new List<Assembly>();
    private Dictionary<Type, IEvent> allEvents = new Dictionary<Type, IEvent>();
    public void Awake()
    {
        instance = this;
    }

    public void AddAssembly(Assembly assembly)
    {
        assemblies.Add(assembly);
        allEvents.Clear();
        foreach (var item in GetTypes<EventAttribute>())
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

    private IEnumerable<(Type type, T attribute)> GetTypes<T>() where T : BaseAttribute
    {
        var attrType = typeof(T);
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                var attrs = type.GetCustomAttributes(attrType, true);
                if (attrs.Length == 0)
                    continue;

                yield return (type, attrs[0] as T);
            }
        }
    }

    public static void Publish<T>(T a) where T : struct
    {
        if (!instance.allEvents.TryGetValue(typeof(T), out var _event))
            return;

        if (_event is AEvent<T> aEvent)
        {
            aEvent.Handle(a);
        }
    }

}
