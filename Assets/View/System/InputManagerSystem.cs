using UnityEngine;

public class InputManagerSystem : Entity, IUpdate
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    private Vector2 axisRaw;
    public void Update()
    {
        axisRaw.Set(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
        
        var data = default(ActorMoveInfo);
        data.value = axisRaw;
        ActorEventSystem.Publish(data);
    }
}