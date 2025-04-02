using System;
using System.Collections.Generic;

public class EntitySystem : Singleton<EntitySystem>, IAwake
{
    private const int fixedUpdate = 0;
    private const int update = 1;
    private const int lateUpdate = 2;
    private const int max = 3;

    private Queue<Entity>[] queues;

    public void Awake()
    {
        queues = new Queue<Entity>[max];
        queues[fixedUpdate] = new Queue<Entity>();
        queues[update] = new Queue<Entity>();
        queues[lateUpdate] = new Queue<Entity>();
    }
    public void AddUpdate(Entity entity)
    {
        if (entity == null)
            return;

        if (entity is IFixedUpdate)
            queues[fixedUpdate].Enqueue(entity);

        if (entity is IUpdate)
            queues[update].Enqueue(entity);

        if (entity is ILateUpdate)
            queues[lateUpdate].Enqueue(entity);
    }

    public void FixedUpdate()
    {
        var queue = queues[fixedUpdate];
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
                    fixedUpdateSystem.FixedUpdate();
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
        var queue = queues[update];
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
        var queue = queues[lateUpdate];
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