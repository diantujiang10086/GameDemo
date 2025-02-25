public class World : Entity
{
    private static World _inst;
    public static World inst => _inst ??= Entity.Create(typeof(World)) as World;

    public float DeltaTime
    {
        get
        {
            return UnityEngine.Time.deltaTime;
        }
    }
}