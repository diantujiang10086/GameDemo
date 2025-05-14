using System.Reflection;

public static class Entry
{
    public static void Start(Assembly[] assemblies)
    {
        AssemblyHelper.SetAssemblies(assemblies);
        World.Instance.AddSigleton<GenerateId>();
        World.Instance.AddSigleton<EventSystem>();
        World.Instance.AddSigleton<EntitySystem>();
        World.Instance.AddSigleton<ResourceManager>();

        EventSystem.Instance.Publish(default(GameInitialization));

        World.Instance.AddSigleton<UnitManager>();
        World.Instance.AddSigleton<ConfigManager>();

        EventSystem.Instance.Publish(default(GameStart));
    }
}