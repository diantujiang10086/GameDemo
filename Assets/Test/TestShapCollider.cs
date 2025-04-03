using UnityEngine;

public class TestShapCollider : MonoBehaviour
{
    private TestDrawShape[] drawShapes;
    private void OnDrawGizmos()
    {
        drawShapes = Transform.FindObjectsOfType<TestDrawShape>();

        foreach (var shape in drawShapes)
        {
            shape.UpdateShape();
        }

        foreach (var shape in drawShapes)
        {
            var iShape = shape.GetShape();
            bool isCollider = false;
            foreach(var shape2 in drawShapes)
            {
                var iShape2 = shape2.GetShape();
                if (iShape == iShape2)
                    continue;
                isCollider |= iShape.TestOverlap(iShape2);
            }
            ShapeVisualizer.DrawShape(iShape, isCollider ? Color.red : Color.green);
        }
    }
}