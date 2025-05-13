public class ResourceManager
{
    public static IResourceLoader resourceLoader = new DefaultResourceLoader();
    public static T Load<T>(string path) where T : class
    {
        return resourceLoader.Load<T>(path);
    }

    public static string LoadText(string path)
    {
        return resourceLoader.LoadText(path);
    }
}
