using UnityEngine;
using Unity.Mathematics;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Test : MonoBehaviour
{
    float angularSpeed = 1f;
    float3 center = new float3(0, 0, 0);


    List<Actor> actors = new List<Actor>();
    List<float> baseAngles = new List<float>();
    List<float> radii = new List<float>();

    private void Awake()
    {
        
        for (int ring = 0; ring < 50; ring++)
        {
            float radius = Mathf.Lerp(2, 30, (float)ring / (50 - 1));
            for (int i = 0; i < 50; i++)
            {
                float angleOffset = (2 * Mathf.PI / 50) * i;
                var actor = ActorManager.Instance.Create(Random.Range(0, 4));
                actor.scale = new float3(1, 1, 1);
                //DisplayManager.Instance.CreateDisplay(actor);
                actors.Add(actor);
                baseAngles.Add(angleOffset);
                radii.Add(radius);
            }
        }
    }

    private void Update()
    {
        float timeAngle = Time.time * angularSpeed;

        for (int i = 0; i < actors.Count; i++)
        {
            float angle = timeAngle + baseAngles[i];
            float radius = radii[i];

            actors[i].position = new float3(
                center.x + Mathf.Cos(angle) * radius,
                center.y + Mathf.Sin(angle) * radius,
                center.z
            );
        }
    }
}

public class BindActor : MonoBehaviour
{
    public Actor actor;
    private void Update()
    {
        actor.position = transform.position;
    }
}