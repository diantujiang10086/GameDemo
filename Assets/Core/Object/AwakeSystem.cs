public class AwakeSystem
{
    public static void Awake(Entity entity)
    {
        if(entity is IAwake awake)
        {
            awake.Awake();
        }
    }

    public static void Awake<A>(Entity entity, A a)
    {
        if (entity is IAwake<A> awake)
        {
            awake.Awake(a);
        }
    }

    public static void Awake<A, B>(Entity entity, A a, B b)
    {
        if (entity is IAwake<A, B> awake)
        {
            awake.Awake(a, b);
        }
    }

    public static void Awake<A, B, C>(Entity entity, A a, B b, C c)
    {
        if (entity is IAwake<A, B, C> awake)
        {
            awake.Awake(a, b, c);
        }
    }
}