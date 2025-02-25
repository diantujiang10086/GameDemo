using System;

public partial class Entity : IDisposable
{
    public long Id { get; private set; }
    public long InstanceId { get; private set; }
    public bool IsDisposed => InstanceId == 0;
    private Entity parent;
    protected bool IsComponent;
    public virtual void Dispose()
    {
        if (IsDisposed)
            return;
        this.InstanceId = 0;

        if(this.children != null)
        {
            foreach(var child in this.children.Values)
            {
                child.Dispose();
            }
            this.children.Clear();
            this.children = null;
        }

        if(this.components != null)
        {
            foreach(var component in this.components.Values)
            {
                component.Dispose();
            }
            this.components.Clear();
            this.components = null;
        }

        DestorySystem.Destory(this);

        if(this.parent != null && !this.parent.IsDisposed)
        {
            if(IsComponent)
            {
                this.parent.RemoveComponent(this);
            }
            else
            {
                this.parent.RemoveFromChildren(this);
            }
        }

        this.parent = null;
    }

    protected static Entity Create(Type type)
    {
        Entity component = Activator.CreateInstance(type) as Entity;
        component.InstanceId = IdGenerater.GenerateId();
        UpdateSystem.inst.AddUpdate(component);
        return component;
    }

}
