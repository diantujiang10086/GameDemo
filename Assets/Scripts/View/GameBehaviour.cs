using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    private DisplayManager displayManager;
    private void Awake()
    {
        displayManager = DisplayManager.Instance;
    }

    private void FixedUpdate()
    {
        displayManager.FixedUpdate();
    }
    private void Update()
    {
        displayManager.Update();
    }

    private void OnDestroy()
    {
        displayManager.Dispose();
    }
}