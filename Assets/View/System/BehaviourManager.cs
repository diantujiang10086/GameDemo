using UnityEngine;

public class BehaviourManager : MonoBehaviour
{
    UpdateSystem updateSystem;
    private void Awake()
    {
        updateSystem = UpdateSystem.inst;
    }

    private void FixedUpdate()
    {
        updateSystem.FixedUpdate();
    }

    private void Update()
    {
        updateSystem.Update();
    }

    private void LateUpdate()
    {
        updateSystem.LateUpdate();
    }
}