using System;
using System.Collections.Generic;

public class Entity : IDisposable
{
    private bool isDisposed;
    private Entity parent;
    private Dictionary<long, Entity> childs;
    private Dictionary<Type, Entity> components;
    private Dictionary<long, Entity> Childs => childs ??= new Dictionary<long, Entity>();
    private Dictionary<Type, Entity> Components => components ??= new Dictionary<Type, Entity>();

    public bool IsDisposed => isDisposed;
    public long InstanceId { get; protected set; }

    public Entity Parent
    {
        get => parent;
        set
        {
            parent = value;
            RegisterSystem();
        }
    }

    protected Entity()
    {

    }

    public T As<T>() where T : Entity
    {
        return this as T;
    }

    public T GetParent<T>() where T : Entity
    {
        return parent as T;
    }
    protected virtual void RegisterSystem()
    {
        EntitySystem.Instance.AddUpdate(this);
    }

    public T AddComponent<T>() where T: Entity, IAwake
    {
        if (Components.TryGetValue(typeof(T), out var component))
        {
            return component as T;
        }

        component = Create(typeof(T));
        component.Parent = this;
        (component as IAwake)?.Awake();
        RegisterSystem();
        Components[typeof(T)] = component;
        return component as T;
    }

    public T AddComponent<T, A>(A a) where T : Entity, IAwake<A>
    {
        if (Components.TryGetValue(typeof(T), out var component))
        {
            return component as T;
        }

        component = Create(typeof(T));
        component.Parent = this;
        (component as IAwake<A>)?.Awake(a);
        RegisterSystem();
        Components[typeof(T)] = component;
        return component as T;
    }

    public T GetComponent<T>() where T : Entity
    {
        Components.TryGetValue(typeof(T), out var component);
        return component as T;
    }

    public void RemoveComponent<T>()
    {
        if (components == null)
            return;
        Components.TryGetValue(typeof(T), out var component);
        if (component != null)
        {
            component.Dispose();
            Components.Remove(typeof(T));
        }
    }

    public T AddChild<T>() where T : Entity, IAwake
    {
        var child = Create(typeof(T));
        (child as IAwake)?.Awake();
        Childs[child.InstanceId] = child;
        child.Parent = this;
        return child as T;
    }
    public T AddChild<T, A>(A a) where T : Entity, IAwake<A>
    {
        var child = Create(typeof(T));
        child.Parent = this;
        (child as IAwake<A>)?.Awake(a);
        Childs[child.InstanceId] = child;
        return child as T;
    }
    public T AddChild<T, A, B>(A a, B b) where T : Entity, IAwake<A,B>
    {
        var child = Create(typeof(T));
        child.Parent = this;
        (child as IAwake<A,B>)?.Awake(a,b);
        Childs[child.InstanceId] = child;
        return child as T;
    }


    public T GetChild<T>(long instanceId) where T : Entity
    {
        Childs.TryGetValue(instanceId, out var child);
        return child as T;
    }

    public void RemoveChild(long instanceId)
    {
        if (childs == null)
            return;

        Childs.TryGetValue(instanceId, out var child);
        if (child != null)
        {
            child.Dispose();
            Childs.Remove(instanceId);
        }
    }

    private Entity Create(Type type)
    {
        var entity = Activator.CreateInstance(type) as Entity;
        entity.isDisposed = false;
        entity.InstanceId = GenerateId.Instance.Create();
        return entity;
    }

    public override string ToString()
    {
        return this.GetType().Name;
    }


    public virtual void Dispose()
    {
        if (isDisposed)
            return;
        isDisposed = true;
        if (childs != null)
        {
            foreach (var kv in childs)
            {
                kv.Value.Dispose();
            }
            childs.Clear();
            childs = null;
        }

        if (components != null)
        {
            foreach (var kv in components)
            {
                kv.Value.Dispose();
            }
            components.Clear();
            components = null;
        }
    }

}
