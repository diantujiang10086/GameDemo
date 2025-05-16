using Unity.Mathematics;
using UnityEngine;

public class BindUnit : MonoBehaviour
{
    public Unit unit;
    private void Update()
    {
        transform.position = unit.position;
        if (unit.IsDisposed)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        var collision2DComponent =  unit.GetComponent<Collision2DComponent>();
        if (collision2DComponent == null)
            return;

        var shape = collision2DComponent.colliderShape;
        var geometry = collision2DComponent.geometry;
        if (shape == ColliderShape.Circle)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(unit.position, (geometry as CircleGeometry).radius);
        }
        else if (shape == ColliderShape.Box)
        {
            Gizmos.color = Color.blue;
            var size = (geometry as BoxGeometry).size;
            BoxAndBoxCollidersExtensions.CalculateBoxCorners(geometry as BoxGeometry, out float2 corners0, out float2 corners1, out float2 corners2, out float2 corners3);
            Vector2[] corners = new Vector2[4];
            corners[0] = corners0;
            corners[1] = corners1;
            corners[2] = corners2;
            corners[3] = corners3;

            for (int i = 0; i < 4; i++)
            {
                var p = new Vector3(corners[i].x, corners[i].y, 0);
                Gizmos.DrawLine(p, new Vector3(corners[(i + 1) % 4].x, corners[(i + 1) % 4].y, 0));
            }
        }
    }
}
