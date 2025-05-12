public interface IResourceLoader
{
    T Load<T>(string path) where T : UnityEngine.Object;
}
