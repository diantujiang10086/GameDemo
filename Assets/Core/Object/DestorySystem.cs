public class DestorySystem
{
    public static void Destory(Entity entity)
    {
        if (entity is IDestroy destory)
        {
            destory.Destroy();
        }
    }
}
