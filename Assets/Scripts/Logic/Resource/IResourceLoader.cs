public interface IResourceLoader
{
    T Load<T>(string path) where T : class;
    string LoadText(string path);
}
