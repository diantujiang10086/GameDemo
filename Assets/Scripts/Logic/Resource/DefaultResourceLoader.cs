using System.IO;

public class DefaultResourceLoader : IResourceLoader
{
    public T Load<T>(string path) where T : class
    {
        return default;
    }

    public string LoadText(string path)
    {
        return File.ReadAllText(path);
    }
}