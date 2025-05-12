public static class Entry
{
    public static void Start()
    {
        EventSystem.Instance.AddEvents();
        ResourceManager.resourceLoader = default;

        EventSystem.Instance.Publish(default(GameInitialization));
    }
}