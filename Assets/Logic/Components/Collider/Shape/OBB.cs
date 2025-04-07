using Unity.Mathematics;
using UnityEngine;

public class OBB : IShape
{
    public float2 Center;
    public float2 Size;
    public float Rotation;

    public float2 Extents
    {
        get
        {
            return Size * 0.5f;
        }
    }

    public OBB() { }
    public OBB(float2 center, float2 size, float rotation)
    {
        Center = center;
        Size = size;
        Rotation = rotation;
    }

    public void ComputeAABB(ref AABB aabb, Unit unit)
    {
        float2 halfSize = Size / 2;

        // 局部坐标系的四个角
        float2[] localCorners = new float2[4]
        {
        new float2(-halfSize.x, -halfSize.y),
        new float2(halfSize.x, -halfSize.y),
        new float2(-halfSize.x, halfSize.y),
        new float2(halfSize.x, halfSize.y)
        };

        float2 worldPosition = new float2(unit.Position.x, unit.Position.z);
        float2 worldCenter = worldPosition + new float2(Center.x, Center.y);

        float2 min = new float2(float.MaxValue, float.MaxValue);
        float2 max = new float2(float.MinValue, float.MinValue);

        foreach (var corner in localCorners)
        {
            float2 rotatedCorner = ColliderHelper.RotateVector(corner, Rotation);
            var center = Center + rotatedCorner;
            float2 worldCorner = worldPosition + new float2(center.x, center.y);
            min = math.min(min, worldCorner);
            max = math.max(max, worldCorner);
        }
        aabb.Min = min;
        aabb.Max = max;
    }

    public void ComputeCorners(float2 center, out float2[] corners, out float2 min, out float2 max)
    {
        float halfWidth = Size.x * 0.5f;
        float halfHeight = Size.y * 0.5f;
        float cos = math.cos(Rotation);
        float sin = math.sin(Rotation);
        float2 halfcos = new float2(halfWidth * cos, halfHeight * cos);
        float2 halfsin = new float2(halfWidth * sin, halfHeight * sin);

        corners = new float2[4];
        corners[0] = center + new float2(-halfcos.x - halfsin.y, halfsin.x - halfcos.y);
        corners[1] = center + new float2(halfcos.x - halfsin.y, -halfsin.x - halfcos.y);
        corners[2] = center + new float2(halfcos.x + halfsin.y, -halfsin.x + halfcos.y);
        corners[3] = center + new float2(-halfcos.x + halfsin.y, halfsin.x + halfcos.y);
        
        ProjectOntoAxis(corners, new float2(1, 0), out var minX, out var maxX);
        ProjectOntoAxis(corners, new float2(0, 1), out var minY, out var maxY);
        min = new float2(minX, minY);
        max = new float2(maxX, maxY);
    }

    private static void ProjectOntoAxis(float2[] points, float2 axis, out float min, out float max)
    {
        float dot = math.dot(points[0], axis);
        min = dot;
        max = dot;
        for (int i = 1; i < points.Length; i++)
        {
            dot = math.dot(points[i], axis);
            if (dot < min) min = dot;
            if (dot > max) max = dot;
        }
    }
    public bool Contains(float2 point)
    {
        float2 direction = point - Center;
        float2 axisU = Rotation * new float2(1,0);
        float2 axisV = Rotation * new float2(0,1);
        // 计算点在局部坐标系中的投影
        float projU = math.dot(direction, axisU);
        float projV = math.dot(direction, axisV);

        // 检查是否在所有轴范围内
        return math.abs(projU) <= Extents.x &&
               math.abs(projV) <= Extents.y;
    }

    public float2 GetAxis(int index)
    {
        var axis = default(float2);
        axis[index] = 1;
        return ColliderHelper.RotateVector(axis, -Rotation);
    }

    /// <summary>
    /// 计算OBB的轴
    /// </summary>
    /// <param name="axisX"></param>
    /// <param name="axisY"></param>
    public void GetAllAxis(out float2 axisX, out float2 axisY)
    {
        float2x2 rotationMatrix = float2x2.Rotate(-Rotation);
        axisX = new float2(rotationMatrix.c0);
        axisY = new float2(rotationMatrix.c1);
    }

    public float GetHalfLength(int index) => Size[index] / 2;

    public bool TestOverlap(float2 point) => CollisionUtils.PointToOBB(point, this);

    public bool TestOverlap(IShape shape) => shape switch
    {
        Circle circleB => CollisionUtils.CircleToOBB(circleB, this),
        AABB aabb => CollisionUtils.AABBToOBB(aabb, this),
        OBB obb => CollisionUtils.OBBToOBB(this, obb),
        Sector sector => CollisionUtils.OBBToSector(this, sector),
        LineSegment lineSeg => CollisionUtils.OBBToLineSegment(this, lineSeg),
        _ => false
    };

}
