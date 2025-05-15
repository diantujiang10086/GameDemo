using System;
using System.Collections.Generic;

public class Entity : DisposeObject
{
    private bool isDisposed;
    public long InstanceId { get; protected set; }
    public bool IsDisposed => isDisposed;

    private bool isComponent;
    private Entity parent;
    private Dictionary<long, Entity> childs;
    private Dictionary<Type, Entity> components;
    private Dictionary<long, Entity> Childs => childs ??= new Dictionary<long, Entity>();
    private Dictionary<Type, Entity> Components => components ??= new Dictionary<Type, Entity>();

    internal bool IsComponent => isComponent;
    public Entity Parent
    {
        get => parent;
        set
        {
            parent = value;
            RegisterSystem();
        }
    }


    public Entity()
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

    public T AddComponent<T>() where T : Entity
    {
        if (Components.TryGetValue(typeof(T), out var component))
        {
            return component as T;
        }

        component = Create(typeof(T));
        component.isComponent = true;
        component.Parent = this;
        component.InstanceId = this.InstanceId;
        (component as IAwake)?.Awake();
        RegisterSystem();
        Components[typeof(T)] = component;
        return component as T;
    }

    public T AddComponent<T, A>(A a) where T : Entity
    {
        if (Components.TryGetValue(typeof(T), out var component))
        {
            return component as T;
        }

        component = Create(typeof(T));
        component.isComponent = true;
        component.Parent = this;
        component.InstanceId = this.InstanceId;
        (component as IAwake<A>)?.Awake(a);
        RegisterSystem();
        Components[typeof(T)] = component;
        return component as T;
    }
    public T AddComponent<T, A, B>(A a, B b) where T : Entity
    {
        if (Components.TryGetValue(typeof(T), out var component))
        {
            return component as T;
        }

        component = Create(typeof(T));
        component.isComponent = true;
        component.Parent = this;
        component.InstanceId = this.InstanceId;
        (component as IAwake<A, B>)?.Awake(a, b);
        RegisterSystem();
        Components[typeof(T)] = component;
        return component as T;
    }
    public T AddComponent<T, A, B, C>(A a, B b, C c) where T : Entity
    {
        if (Components.TryGetValue(typeof(T), out var component))
        {
            return component as T;
        }

        component = Create(typeof(T));
        component.isComponent = true;
        component.Parent = this;
        component.InstanceId = this.InstanceId;
        (component as IAwake<A, B, C>)?.Awake(a, b, c);
        RegisterSystem();
        Components[typeof(T)] = component;
        return component as T;
    }
    public T AddComponent<T, A, B, C, D>(A a, B b, C c, D d) where T : Entity
    {
        if (Components.TryGetValue(typeof(T), out var component))
        {
            return component as T;
        }

        component = Create(typeof(T));
        component.isComponent = true;
        component.Parent = this;
        component.InstanceId = this.InstanceId;
        (component as IAwake<A, B, C, D>)?.Awake(a, b, c, d);
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
        RemoveComponent(typeof(T));
    }

    public void RemoveComponent(Entity component)
    {
        RemoveComponent(component.GetType());
    }
    public void RemoveComponent(Type type)
    {
        if (components == null)
            return;

        Components.TryGetValue(type, out var component);
        if (component != null)
        {
            component.Dispose();
            Components.Remove(type);
        }
    }

    public T AddChild<T>() where T : Entity
    {
        var child = Create(typeof(T));
        Childs[child.InstanceId] = child;
        child.Parent = this;
        (child as IAwake)?.Awake();
        return child as T;
    }
    public T AddChild<T, A>(A a) where T : Entity
    {
        var child = Create(typeof(T));
        child.Parent = this;
        Childs[child.InstanceId] = child;
        (child as IAwake<A>)?.Awake(a);
        return child as T;
    }
    public T AddChild<T, A, B>(A a, B b) where T : Entity
    {
        var child = Create(typeof(T));
        child.Parent = this;
        Childs[child.InstanceId] = child;
        (child as IAwake<A, B>)?.Awake(a, b);
        return child as T;
    }
    public T AddChild<T, A, B, C>(A a, B b, C c) where T : Entity
    {
        var child = Create(typeof(T));
        child.Parent = this;
        Childs[child.InstanceId] = child;
        (child as IAwake<A, B, C>)?.Awake(a, b, c);
        return child as T;
    }


    public T GetChild<T>(long instanceId) where T : Entity
    {
        Childs.TryGetValue(instanceId, out var child);
        return child as T;
    }

    public void RemoveChild(Entity entity)
    {
        RemoveChild(entity.InstanceId);
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


    public override void Dispose()
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

        if (this is IDestory destory)
            destory.Destory();

        if (parent != null && !this.parent.IsDisposed)
        {
            if (this.isComponent)
            {
                parent.RemoveComponent(this);
            }
            else
            {
                parent.RemoveChild(this);
            }
            parent = null;
        }

        base.Dispose();
    }
}
