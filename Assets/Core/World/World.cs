using System;
using System.Collections.Generic;

public class World : IDisposable
{
    private static World instance;
    private readonly Stack<Type> stack = new Stack<Type>();
    private Dictionary<Type, ASingleton> singletons = new Dictionary<Type, ASingleton>();
    public static World Instance
    {
        get
        {
            return instance ??= new World();
        }
    }

    public Scene MainScene { get; set; } = new Scene();

    public T AddSigleton<T>() where T : ASingleton, IAwake, new()
    {
        T singleton = new T();
        singleton.Awake();
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

    private void AddSingleton(ASingleton singleton)
    {
        lock (this)
        {
            var type = singleton.GetType();
            this.stack.Push(type);
            singletons[type] = singleton;
        }
        singleton.Register();
    }

    public void Dispose()
    {
        instance = null;

        lock (this)
        {
            while(this.stack.Count > 0)
            {
                Type type = this.stack.Pop();
                if(this.singletons.Remove(type, out var singleton))
                {
                    singleton.Dispose();
                }
            }

            foreach (var kv in singletons)
            {
                kv.Value.Dispose();
            }
        }
    }
}
