using System;
using System.Collections.Generic;

public class World : IDisposable
{
    private static World instance;
    private readonly Stack<Type> stack = new Stack<Type>();
    private Dictionary<Type, ASingleton> singletons = new Dictionary<Type, ASingleton>();
    private List<ASingleton>[] singletonSystem = new List<ASingleton>[3];
    public static World Instance
    {
        get
        {
            return instance ??= new World();
        }
    }

    public Scene MainScene { get; private set; } = new Scene();

    private World()
    {
        singletonSystem[SystemType.fixedUpdate] = new List<ASingleton>();
        singletonSystem[SystemType.update] = new List<ASingleton>();
        singletonSystem[SystemType.lateUpdate] = new List<ASingleton>();
    }

    public T AddSigleton<T>() where T : ASingleton, new()
    {
        T singleton = new T();
        (singleton as IAwake)?.Awake();
        AddSingleton(singleton);
        return singleton;
    }

    public T AddSigleton<T, A>(A a) where T : ASingleton, IAwake<A>, new()
    {
        T singleton = new T();
        singleton.Awake(a);
        AddSingleton(singleton);
        return singleton;
    }

    public T AddSigleton<T, A, B>(A a, B b) where T : ASingleton, IAwake<A, B>, new()
    {
        T singleton = new T();
        singleton.Awake(a, b);
        AddSingleton(singleton);
        return singleton;
    }

    public T AddSigleton<T, A, B, C>(A a, B b, C c) where T : ASingleton, IAwake<A, B, C>, new()
    {
        T singleton = new T();
        singleton.Awake(a, b, c);
        AddSingleton(singleton);
        return singleton;
    }

    public T AddSigleton<T, A, B, C, D>(A a, B b, C c, D d) where T : ASingleton, IAwake<A, B, C, D>, new()
    {
        T singleton = new T();
        singleton.Awake(a, b, c, d);
        AddSingleton(singleton);
        return singleton;
    }

    public T GetSigleton<T>() where T : ASingleton
    {
        singletons.TryGetValue(typeof(T), out var sigleton);
        return (T)sigleton;
    }

    public void Update()
    {
        var list = singletonSystem[SystemType.update];
        int count = list.Count;
        while(count-- > 0)
        {
            var update = list[count] as IUpdate;
            try
            {
                update.Update();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }

    public void FixedUpdate(float elaspedTime)
    {
        var list = singletonSystem[SystemType.fixedUpdate];
        int count = list.Count;
        while (count-- > 0)
        {
            var update = list[count] as IFixedUpdate;
            try
            {
                update.FixedUpdate(elaspedTime);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }

    public void LateUpdate()
    {
        var list = singletonSystem[SystemType.lateUpdate];
        int count = list.Count;
        while (count-- > 0)
        {
            var update = list[count] as ILateUpdate;
            try
            {
                update.LateUpdate();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }

    private void AddSingleton(ASingleton singleton)
    {
        lock (this)
        {
            var type = singleton.GetType();
            this.stack.Push(type);
            singletons[type] = singleton;
        }
        singleton.Register();

        if(singleton is IUpdate)
        {
            singletonSystem[SystemType.update].Add(singleton);
        }
        if (singleton is IFixedUpdate)
        {
            singletonSystem[SystemType.fixedUpdate].Add(singleton);
        }
        if (singleton is ILateUpdate)
        {
            singletonSystem[SystemType.lateUpdate].Add(singleton);
        }
    }

    public void Dispose()
    {
        instance = null;

        MainScene.Dispose();

        lock (this)
        {
            while (this.stack.Count > 0)
            {
                Type type = this.stack.Pop();
                if (this.singletons.Remove(type, out var singleton))
                {
                    singleton.Dispose();
                }
            }

            foreach (var kv in singletons)
            {
                kv.Value.Dispose();
            }
        }
        singletonSystem = default;
    }
}
