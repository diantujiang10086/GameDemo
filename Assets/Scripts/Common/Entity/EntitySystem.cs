using System;
using System.Collections.Generic;

public class SystemType
{
    public const int fixedUpdate = 0;
    public const int update = 1;
    public const int lateUpdate = 2;
    public const int max = 3;
}


public class EntitySystem : Singleton<EntitySystem>, IAwake, IFixedUpdate, IUpdate, ILateUpdate
{
    private Queue<Entity>[] queues;

    public void Awake()
    {
        queues = new Queue<Entity>[SystemType.max];
        queues[SystemType.fixedUpdate] = new Queue<Entity>(10240);
        queues[SystemType.update] = new Queue<Entity>(10240);
        queues[SystemType.lateUpdate] = new Queue<Entity>(10240);
    }
    public void AddUpdate(Entity entity)
    {
        if (entity == null)
            return;

        if (entity is IFixedUpdate)
            queues[SystemType.fixedUpdate].Enqueue(entity);

        if (entity is IUpdate)
            queues[SystemType.update].Enqueue(entity);

        if (entity is ILateUpdate)
            queues[SystemType.lateUpdate].Enqueue(entity);
    }

    public void FixedUpdate(float elaspedTime)
    {
        var queue = queues[SystemType.fixedUpdate];
        int count = queue.Count;
        while (count-- > 0)
        {
            var component = queue.Dequeue();
            if (component == null || component.IsDisposed)
                continue;

            if (component is IFixedUpdate fixedUpdateSystem)
            {
                queue.Enqueue(component);
                try
                {
                    fixedUpdateSystem.FixedUpdate(elaspedTime);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }
    }

    public void Update()
    {
        var queue = queues[SystemType.update];
        int count = queue.Count;
        while (count-- > 0)
        {
            var component = queue.Dequeue();
            if (component == null || component.IsDisposed)
                continue;

            if (component is IUpdate UpdateSystem)
            {
                queue.Enqueue(component);
                try
                {
                    UpdateSystem.Update();
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }
    }

    public void LateUpdate()
    {
        var queue = queues[SystemType.lateUpdate];
        int count = queue.Count;
        while (count-- > 0)
        {
            var component = queue.Dequeue();
            if (component == null || component.IsDisposed)
                continue;

            if (component is ILateUpdate lateUpdateSystem)
            {
                queue.Enqueue(component);
                try
                {
                    lateUpdateSystem.LateUpdate();
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }
    }

}