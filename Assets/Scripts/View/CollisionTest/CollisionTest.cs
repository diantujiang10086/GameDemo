using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var shapes = Transform.FindObjectsByType<Shape>(FindObjectsSortMode.InstanceID);
        foreach (var shape in shapes)
        {
            shape.UpdateGeometry();
        }

        foreach (var shape in shapes)
        {
            bool isCollision = false;
            foreach (var s in shapes)
            {
                if (s == shape)
                    continue;
                isCollision |= CollisionHelper.Overlap(s.geometry, shape.geometry);
            }
            shape.Draw(isCollision ? Color.red : Color.green);
        }
    }
}