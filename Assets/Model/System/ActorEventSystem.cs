
using System;
using System.Collections.Generic;
using System.Reflection;

public abstract class ActorEvent<A> : IEvent
{
    public Type Type => typeof(A);

    protected abstract void Run(Actor actor, A a);

    public void Handle(Actor actor, A a)
    {
        try
        {
            Run(actor, a);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }
}

public class ActorEventSystem
{
    private static ActorEventSystem instance;
    public static ActorEventSystem inst => instance ??= new ActorEventSystem();

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

    public static void Publish<T>(T a, int actorId = -1) where T : struct
    {
        if (!inst.allEvents.TryGetValue(typeof(T), out var _event))
            return;

        if (_event is ActorEvent<T> aEvent)
        {
            Actor actor = default;
            if (actorId == -1)
            {
                var controlActor = World.inst.GetComponent<ControlActor>();
                actorId = controlActor.actorId;
            }
            var actorManager = World.inst.GetComponent<ActorManagerSystem>();
            actor = actorManager.GetActor(actorId);
            if (actor == null)
                return;
            aEvent.Handle(actor, a);
        }
    }
}