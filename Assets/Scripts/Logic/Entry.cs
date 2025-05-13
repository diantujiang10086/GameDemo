using System.Reflection;

public static class Entry
{
    public static void Start(Assembly[] assemblies)
    {
        AssemblyHelper.SetAssemblies(assemblies);
        EventSystem.Instance.AddEvents();
        ResourceManager.resourceLoader = default;

        EventSystem.Instance.Publish(default(GameInitialization));
        EventSystem.Instance.Publish(default(GameStart));
    }
}