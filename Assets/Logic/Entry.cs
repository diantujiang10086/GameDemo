public class Entry
{
    public static void Start()
    {
        World.Instance.AddSigleton<GenerateId>();
        World.Instance.AddSigleton<EventSystem>();
        World.Instance.AddSigleton<EntitySystem>();
        World.Instance.AddSigleton<PhysicsWorld>();

        EventSystem.Instance.AddAssembly(typeof(World).Assembly);
        EventSystem.Instance.AddAssembly(typeof(Entry).Assembly);

        EventSystem.Instance.Publish(default(GameStart));
    }
}
