using UnityEngine;

public class GameManager : MonoBehaviour
{
    private World world;
    
    private void Awake()
    {
        world = World.Instance;
    }

    private void FixedUpdate()
    {
        TimeSystem.Instance.time = Time.time;
        world.FixedUpdate(Time.fixedDeltaTime);
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