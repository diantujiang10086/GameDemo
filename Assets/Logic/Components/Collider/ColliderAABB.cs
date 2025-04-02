using Unity.Mathematics;

public static partial class CollisionUtils
{
    public static bool AABBToAABB(AABB a, AABB b)
    {
        return a.Max.x >= b.Min.x && a.Min.x <= b.Max.x &&
               a.Max.y >= b.Min.y && a.Min.y <= b.Max.y;
    }
    public static bool AABBToOBB(AABB aabb, OBB obb)
    {
        // ��AABB��Ϊ�����OBB������ת��
        OBB aabbAsObb = new OBB(
            (aabb.Min + aabb.Max) / 2,
            aabb.Max - aabb.Min,
            0f
        );
        return OBBToOBB(aabbAsObb, obb);
    }
    // AABB vs Sector
    public static bool AABBToSector(AABB aabb, Sector sector)
    {
        // 1. ���AABB�Ƿ���ȫ������������
        if (PointToAABB(sector.Center, aabb)) return true;

        // 2. ��������Ƿ���AABB�ı��ཻ
        LineSegment[] edges = GetAABBEdges(aabb);
        foreach (var edge in edges)
        {
            if (SectorToLineSegment(sector, edge))
                return true;
        }

        // 3. ���AABB�Ľǵ��Ƿ���������
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
        // ʹ�÷����ᶨ��
        float2 aabbCenter = (aabb.Min + aabb.Max) / 2;
        float2 aabbExtents = (aabb.Max - aabb.Min) / 2;

        float2 lineDir = line.End - line.Start;
        float2 lineCenter = (line.Start + line.End) / 2;
        float lineHalfLength = math.length(lineDir) / 2;
        lineDir = math.normalize(lineDir);

        // �����AABB���ϵ�ͶӰ
        for (int i = 0; i < 2; i++)
        {
            float2 axis = i == 0 ? new float2(1, 0) : new float2(0, 1);
            float aabbProj = math.dot(aabbCenter, axis);
            float aabbRadius = aabbExtents[i];

            float lineProj = math.dot(lineCenter, axis);
            float lineRadius = lineHalfLength * math.abs(math.dot(lineDir, axis));

            if (math.abs(aabbProj - lineProj) > aabbRadius + lineRadius)
                return false;
        }

        // ������߶δ�ֱ���ϵ�ͶӰ
        float2 perp = new float2(-lineDir.y, lineDir.x);
        float aabbProjPerp = math.dot(aabbCenter, perp);
        float aabbRadiusPerp = aabbExtents.x * math.abs(math.dot(new float2(1, 0), perp)) +
                              aabbExtents.y * math.abs(math.dot(new float2(0, 1), perp));

        float lineProjPerp = math.dot(lineCenter, perp);

        if (math.abs(aabbProjPerp - lineProjPerp) > aabbRadiusPerp)
            return false;

        return true;
    }
    private static LineSegment[] GetAABBEdges(AABB aabb)
    {
        float2 min = aabb.Min;
        float2 max = aabb.Max;

        return new LineSegment[]
        {
        new LineSegment(new float2(min.x, min.y), new float2(max.x, min.y)), // �±�
        new LineSegment(new float2(max.x, min.y), new float2(max.x, max.y)), // �ұ�
        new LineSegment(new float2(max.x, max.y), new float2(min.x, max.y)), // �ϱ�
        new LineSegment(new float2(min.x, max.y), new float2(min.x, min.y))  // ���
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