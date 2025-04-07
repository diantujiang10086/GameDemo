using System;
using Unity.Mathematics;
using UnityEngine;

public static partial class CollisionUtils
{
    public static bool AABBToAABB(AABB a, AABB b)
    {
        bool overlapX = a.Min.x <= b.Max.x && a.Max.x >= b.Min.x;
        bool overlapY = a.Min.y <= b.Max.y && a.Max.y >= b.Min.y;
        return overlapX && overlapY;
    }
    public static bool AABBToOBB(AABB aabb, OBB obb)
    {
        obb.GetAllAxis(out var obbAxisX, out var obbAxisY);

        // 计算AABB的中心和半长
        float2 aabbHalfSize = aabb.Size * 0.5f;

        // 计算OBB的半长
        float2 obbHalfSize = obb.Size * 0.5f;

        // 计算中心点向量
        float2 centerVector = obb.Center - aabb.Center;

        //  测试X轴
        if (!OverlapOnAxis(aabbHalfSize.x, obbHalfSize, obbAxisX, centerVector, new float2(1, 0)))
            return false;

        //  测试Y轴
        if (!OverlapOnAxis(aabbHalfSize.y, obbHalfSize, obbAxisY, centerVector, new float2(0, 1)))
            return false;

        //  测试OBB的X轴
        if (!OverlapOnAxis(aabbHalfSize, obbHalfSize.x, centerVector, obbAxisX))
            return false;

        //  测试OBB的Y轴
        if (!OverlapOnAxis(aabbHalfSize, obbHalfSize.y, centerVector, obbAxisY))
            return false;

        return true;

    }
    /// <summary>
    /// 在单个轴上检测投影是否重叠
    /// 公式: |T·L| ≤ |a·L| + |(b_x * u_x + b_y * u_y)·L|
    /// 其中:
    /// T = 中心点向量
    /// L = 测试轴
    /// a = AABB半长
    /// b = OBB半长
    /// u = OBB的轴
    /// </summary>
    private static bool OverlapOnAxis(float aHalfSize, float2 bHalfSize, float2 bAxis, float2 centerVector, float2 testAxis)
    {
        // 计算AABB在该轴上的投影半径
        float aProjection = aHalfSize;

        // 计算OBB在该轴上的投影半径
        float bProjection = bHalfSize.x * math.abs(math.dot(bAxis.x, testAxis)) +
                           bHalfSize.y * math.abs(math.dot(bAxis.y, testAxis));

        // 计算中心点向量在该轴上的投影
        float centerProjection = math.abs(math.dot(centerVector, testAxis));

        // 检查投影是否重叠
        return centerProjection <= (aProjection + bProjection);
    }

    /// <summary>
    /// 重载方法，用于测试OBB的轴
    /// </summary>
    private static bool OverlapOnAxis(float2 aHalfSize, float bHalfSize, float2 centerVector, float2 testAxis)
    {
        // 计算AABB在该轴上的投影半径
        float aProjection = aHalfSize.x * math.abs(math.dot(new float2(1, 0), testAxis)) +
                           aHalfSize.y * math.abs(math.dot(new float2(0, 1), testAxis));

        // 计算OBB在该轴上的投影半径
        float bProjection = bHalfSize;

        // 计算中心点向量在该轴上的投影
        float centerProjection = math.abs(math.dot(centerVector, testAxis));

        // 检查投影是否重叠
        return centerProjection <= (aProjection + bProjection);
    }

    // AABB vs Sector
    public static bool AABBToSector(AABB aabb, Sector sector)
    {
        // 1. 检查AABB是否完全包含扇形中心
        if (PointToAABB(sector.Center, aabb)) return true;

        // 2. 检查扇形是否与AABB的边相交
        LineSegment[] edges = GetAABBEdges(aabb);
        foreach (var edge in edges)
        {
            if (SectorToLineSegment(sector, edge))
                return true;
        }

        // 3. 检查AABB的角点是否在扇形内
        float2[] corners = GetAABBCorners(aabb);
        foreach (var corner in corners)
        {
            if (PointToSector(corner, sector))
                return true;
        }

        return false;
    }

    // AABB vs LineSegment
    public static bool AABBToLineSegment(AABB aabb, LineSegment line)
    {
        float2 aabbCenter = (aabb.Min + aabb.Max) * 0.5f;
        float2 aabbExtents = (aabb.Max - aabb.Min) * 0.5f;

        float2 lineVec = line.End - line.Start;
        float2 lineCenter = (line.Start + line.End) * 0.5f;
        float2 lineDir = math.normalizesafe(lineVec);
        float lineHalfLen = math.length(lineVec) * 0.5f;

        float2x2 axes = new float2x2(new float2(1, 0), new float2(0, 1));

        float2 centerDelta = lineCenter - aabbCenter;

        for (int i = 0; i < 2; i++)
        {
            float2 axis = axes[i];
            float dist = math.abs(math.dot(centerDelta, axis));
            float r1 = aabbExtents[i];
            float r2 = lineHalfLen * math.abs(math.dot(lineDir, axis));
            if (dist > r1 + r2)
                return false;
        }

        float2 perp = new float2(-lineDir.y, lineDir.x);
        float distPerp = math.abs(math.dot(centerDelta, perp));
        float r1Perp = aabbExtents.x * math.abs(perp.x) + aabbExtents.y * math.abs(perp.y);
        if (distPerp > r1Perp)
            return false;

        return true;
    }
    private static LineSegment[] GetAABBEdges(AABB aabb)
    {
        float2 min = aabb.Min;
        float2 max = aabb.Max;

        return new LineSegment[]
        {
        new LineSegment(new float2(min.x, min.y), new float2(max.x, min.y)), // 下边
        new LineSegment(new float2(max.x, min.y), new float2(max.x, max.y)), // 右边
        new LineSegment(new float2(max.x, max.y), new float2(min.x, max.y)), // 上边
        new LineSegment(new float2(min.x, max.y), new float2(min.x, min.y))  // 左边
        };
    }
    private static float2[] GetAABBCorners(AABB aabb)
    {
        return new float2[]
        {
        aabb.Min,
        new float2(aabb.Max.x, aabb.Min.y),
        aabb.Max,
        new float2(aabb.Min.x, aabb.Max.y)
        };
    }
}