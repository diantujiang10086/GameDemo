﻿using System;
using System.Collections.Generic;
using System.Reflection;

public class EventSystem : Singleton<EventSystem>, IAwake
{
    private List<Assembly> assemblies = new List<Assembly>();
    private Dictionary<Type, List<IEvent>> allEvents = new Dictionary<Type, List<IEvent>>();

    public void Awake()
    {

    }

    public void AddAssembly(Assembly assembly)
    {
        assemblies.Add(assembly);
        foreach (var item in GetTypes<EventAttribute>())
        {
            IEvent obj = Activator.CreateInstance(item.type) as IEvent;
            if (obj == null)
            {
                Log.Error($"type not is AEvent: {item.type.Name}");
                continue;
            }
            var eventType = obj.Type;
            if (!allEvents.TryGetValue(eventType, out var events))
            {
                events = new List<IEvent>();
                allEvents[eventType] = events;
            }
            events.Add(obj);
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

    public void Publish<T>(T a) where T : struct
    {
        if (allEvents.TryGetValue(typeof(T), out var list))
        {
            foreach (var e in list)
            {
                if(e is AEvent<T> aEvent)
                {
                    aEvent.Handle(a);
                }
            }
        }
    }

}
