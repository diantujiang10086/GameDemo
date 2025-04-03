using Unity.Mathematics;
using UnityEngine;

public class DrawLineSegmentShape : TestDrawShape
{
    public Vector2 startOffset = new Vector2(-1, 0);
    public Vector2 endOffset = new Vector2(1, 0);
    private LineSegment lineSegment = new LineSegment();
    public override IShape GetShape()
    {
        return lineSegment;
    }

    public override void UpdateShape()
    {
        float2 center = new float2(transform.position.x, transform.position.z);
        float2 startOffset2 = new float2(startOffset.x, startOffset.y);
        float2 endOffset2 = new float2(endOffset.x, endOffset.y);
        lineSegment.Start = center + startOffset2;
        lineSegment.End = center + endOffset2;
    }
}
