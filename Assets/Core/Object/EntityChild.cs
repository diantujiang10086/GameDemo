using System;
using System.Collections.Generic;

public partial class Entity
{
    private Dictionary<long, Entity> children;
    private Dictionary<long, Entity> Children
    {
        get => this.children ??= new Dictionary<long, Entity>();
    }

    public Entity Parent
    {
        get => parent;
        protected set
        {
            if (this.parent != null)
            {
                if(this.parent == value)
                {
                    return;
                }
                this.parent.RemoveFromChildren(this);
            }
            this.parent = value;
            this.parent?.AddToChildren(this);
        }
    }
    public Entity AddChild(Entity entity)
    {
        entity.Parent = this;
        return entity;
    }

    public T AddChild<T>() where T : Entity
    {
        Type type = typeof(T);
        var component = Entity.Create(type) as T;
        component.Id = IdGeneraterSystem.GenerateId();
        component.Parent = this;
        AwakeSystem.Awake(component);
        return component;
    }
    public T AddChild<T, A>(A a) where T : Entity
    {
        Type type = typeof(T);
        var component = Entity.Create(type) as T;
        component.Id = IdGeneraterSystem.GenerateId();
        component.Parent = this;
        AwakeSystem.Awake(component, a);
        return component;
    }
    public T AddChild<T, A, B>(A a, B b) where T : Entity
    {
        Type type = typeof(T);
        var component = Entity.Create(type) as T;
        component.Id = IdGeneraterSystem.GenerateId();
        component.Parent = this;
        AwakeSystem.Awake(component, a, b);
        return component;
    }
    public T AddChild<T, A, B, C>(A a, B b, C c) where T : Entity
    {
        Type type = typeof(T);
        var component = Entity.Create(type) as T;
        component.Id = IdGeneraterSystem.GenerateId();
        component.Parent = this;
        AwakeSystem.Awake(component, a, b, c);
        return component;
    }

    public T AddChildWithId<T>(long id) where T : Entity
    {
        Type type = typeof(T);
        var component = Entity.Create(type) as T;
        component.Id = id;
        component.Parent = this;
        AwakeSystem.Awake(component);
        return component;
    }
    public T AddChildWithId<T, A>(long id, A a) where T : Entity
    {
        Type type = typeof(T);
        var component = Entity.Create(type) as T;
        component.Id = id;
        component.Parent = this;
        AwakeSystem.Awake(component, a);
        return component;
    }
    public T AddChildWithId<T, A, B>(long id, A a, B b) where T : Entity
    {
        Type type = typeof(T);
        var component = Entity.Create(type) as T;
        component.Id = id;
        component.Parent = this;
        AwakeSystem.Awake(component, a, b);
        return component;
    }
    public T AddChildWithId<T, A, B, C>(long id, A a, B b, C c) where T : Entity
    {
        Type type = typeof(T);
        var component = Entity.Create(type) as T;
        component.Id = id;
        component.Parent = this;
        AwakeSystem.Awake(component, a, b, c);
        return component;
    }

    public void RemoveChild(Entity entity)
    {
        RemoveChild(entity.Id);
    }

    public void RemoveChild(long id)
    {
        if (this.children == null)
            return;

        if (!this.children.TryGetValue(id, out var child))
            return;
        this.children.Remove(id);
        child.Dispose();
    }

    private void RemoveFromChildren(Entity entity)
    {
        if (this.children == null)
            return;

        this.children.Remove(entity.Id);

        if(this.children.Count == 0)
        {
            this.children = null;
        }
    }

    private void AddToChildren(Entity entity)
    {
        this.Children.Add(entity.Id, entity);
    }
}
