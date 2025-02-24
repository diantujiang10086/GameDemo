
using System;
using System.Collections.Generic;

public partial class Entity
{
    private Dictionary<Type, Entity> components;
    private Dictionary<Type, Entity> Components
    {
        get
        {
            return this.components ??= new Dictionary<Type, Entity>();
        }
    }
    private Entity ComponentParent
    {
        set
        {
            if (this.parent != null)
            {
                if (this.parent == value)
                {
                    return;
                }
                this.parent.RemoveFromComponents(this);
            }

            this.parent = value;
            this.parent?.AddToComponents(this);
        }
    }

    public Entity AddComponent(Type type)
    {
        var component = Create(type);
        component.Id = this.Id;
        component.ComponentParent = this;
        AwakeSystem.Awake(component);
        return component;
    }

    public K AddComponentWithId<K>(long id) where K : Entity, IAwake, new()
    {
        Type type = typeof(K);
        if (this.components != null && this.components.TryGetValue(type, out var component))
        {
            return component as K;
        }

        component = Create(type);
        component.Id = id;
        component.ComponentParent = this;
        AwakeSystem.Awake(component);
        return component as K;
    }
    public K AddComponentWithId<K, P1>(long id, P1 p1) where K : Entity, IAwake<P1>, new()
    {
        Type type = typeof(K);
        if (this.components != null && this.components.TryGetValue(type, out var component))
        {
            return component as K;
        }

        component = Create(type);
        component.Id = id;
        component.ComponentParent = this;
        AwakeSystem.Awake(component, p1);
        return component as K;
    }
    public K AddComponentWithId<K, P1, P2>(long id, P1 p1, P2 p2) where K : Entity, IAwake<P1, P2>, new()
    {
        Type type = typeof(K);
        if (this.components != null && this.components.TryGetValue(type, out var component))
        {
            return component as K;
        }

        component = Create(type);
        component.Id = id;
        component.ComponentParent = this;
        AwakeSystem.Awake(component, p1, p2);
        return component as K;
    }
    public K AddComponentWithId<K, P1, P2, P3>(long id, P1 p1, P2 p2, P3 p3) where K : Entity, IAwake<P1, P2, P3>, new()
    {
        Type type = typeof(K);
        if (this.components != null && this.components.TryGetValue(type, out var component))
        {
            return component as K;
        }

        component = Create(type);
        component.Id = id;
        component.ComponentParent = this;
        AwakeSystem.Awake(component, p1, p2, p3);
        return component as K;
    }

    public K AddComponent<K>() where K : Entity, IAwake, new()
    {
        return this.AddComponentWithId<K>(this.Id);
    }
    public K AddComponent<K, P1>(P1 p1) where K : Entity, IAwake<P1>, new()
    {
        return this.AddComponentWithId<K, P1>(this.Id, p1);
    }
    public K AddComponent<K, P1, P2>(P1 p1, P2 p2) where K : Entity, IAwake<P1, P2>, new()
    {
        return this.AddComponentWithId<K, P1, P2>(this.Id, p1, p2);
    }
    public K AddComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where K : Entity, IAwake<P1, P2, P3>, new()
    {
        return this.AddComponentWithId<K, P1, P2, P3>(this.Id, p1, p2, p3);
    }

    private void AddToComponents(Entity component)
    {
        this.Components.Add(component.GetType(), component);
    }

    private void RemoveFromComponents(Entity component)
    {
        if (this.components == null)
            return;

        this.components.Remove(component.GetType());

        if (this.components.Count == 0)
        {
            this.components = null;
        }
    }

    public void RemoveComponent<K>() where K : Entity
    {
        if (this.IsDisposed)
            return;
        if (this.components == null)
            return;
        var type = typeof(K);
        if (!this.components.TryGetValue(type, out var c))
            return;
        this.RemoveFromComponents(c);
        c.Dispose();
    }

    private void RemoveComponent(Entity component)
    {
        if (this.IsDisposed)
            return;
        if (this.components == null)
            return;
        if (!this.components.TryGetValue(component.GetType(), out var c))
            return;
        if (c.InstanceId != component.InstanceId)
            return;
        this.RemoveFromComponents(c);
        c.Dispose();
    }
}
