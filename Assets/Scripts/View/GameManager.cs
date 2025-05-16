using UnityEngine;

public class GameManager : MonoBehaviour
{
    private World world;
    private SparseGridCollision2DManager sparseGridCollision2DManager;
    private void Awake()
    {
        world = World.Instance;
    }

    private void Start()
    {
        sparseGridCollision2DManager = SparseGridCollision2DManager.Instance;
    }

    private void FixedUpdate()
    {
        TimeSystem.Instance.time = Time.time;
        world.FixedUpdate(Time.fixedDeltaTime);
        sparseGridCollision2DManager.CalculateAllCollision();
    }

    private void Update()
    {
        world.Update();
    }

    private void LateUpdate()
    {
        world.LateUpdate();
    }

    private void OnDestroy()
    {
        world.Dispose();
    }
}