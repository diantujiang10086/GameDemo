using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;

public class GridManager : Singleton<GridManager>, IAwake<float> 
{
    public struct GridKey : IEquatable<GridKey>
    {
        public int X;
        public int Y;

        public GridKey(int x, int y)
        {
            X = x;
            Y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(GridKey other) => X == other.X && Y == other.Y;

        public override bool Equals(object obj) => obj is GridKey other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public static bool operator ==(GridKey left, GridKey right) => left.Equals(right);

        public static bool operator !=(GridKey left, GridKey right) => !left.Equals(right);
    }

    public class Grid
    {
        public int x, y;
        public float2 min, max;// 核心区域（收缩后）
        public float2 outerMin, outerMax;// 原始格子边界

        public List<long> list;
        public Grid(int x, int y, float cellSize, float boundaryThreshold)
        {
            this.x = x;
            this.y = y;
            outerMin.x = x * cellSize;
            outerMax.x = (x + 1) * cellSize;
            outerMin.y = y * cellSize;
            outerMax.y = (y + 1) * cellSize;

            float shrinkSize = cellSize * boundaryThreshold;
            min.x = outerMin.x + shrinkSize;
            max.x = outerMax.x - shrinkSize;
            min.y = outerMin.y + shrinkSize;
            max.y = outerMax.y - shrinkSize;

            list = new List<long>(100);
        }

        public IEnumerable<GridKey> GetGrids(AABB aabb)
        {
            yield return new GridKey(x, y);

            bool needLeft = aabb.Min.x < min.x;
            bool needRight = aabb.Max.x > max.x;
            bool needBottom = aabb.Min.y < min.y;
            bool needTop = aabb.Max.y > max.y;

            if (!(needLeft || needRight || needBottom || needTop)) yield break;

            // 左
            if (needLeft) yield return new GridKey(x - 1, y);
            // 右
            if (needRight) yield return new GridKey(x + 1, y);
            // 下
            if (needBottom) yield return new GridKey(x, y - 1);
            // 上
            if (needTop) yield return new GridKey(x, y + 1);
            // 左下
            if (needLeft && needBottom) yield return new GridKey(x - 1, y - 1);
            // 左上
            if (needLeft && needTop) yield return new GridKey(x - 1, y + 1);
            // 右下
            if (needRight && needBottom) yield return new GridKey(x + 1, y - 1);
            // 右上
            if (needRight && needTop) yield return new GridKey(x + 1, y + 1);
        }
    }

    private float boundaryThreshold = 0.1f;
    private float cellSize;
    private Dictionary<GridKey, Grid> grids = new Dictionary<GridKey, Grid>();

    public void Awake(float cellSize)
    {
        this.cellSize = cellSize;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Grid GetGrid(GridKey gridKey)
    {
        grids.TryGetValue(gridKey, out var grid);
        return grid;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private GridKey GetCellKey(float3 position) => new GridKey(Mathf.FloorToInt(position.x / cellSize), Mathf.FloorToInt(position.z / cellSize));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private GridKey GetCellKey(float2 position) => new GridKey(Mathf.FloorToInt(position.x / cellSize), Mathf.FloorToInt(position.y / cellSize));

    public void Add(long unitId, float3 position)
    {
        var key = GetCellKey(position);
        if (!grids.TryGetValue(key, out var grid))
        {
            grid = new Grid(key.X, key.Y, cellSize, boundaryThreshold);
            grids[key] = grid;
        }
        grid.list.Add(unitId);
    }

    public void Remove(long unitId, float3 position)
    {
        var key = GetCellKey(position);
        if (grids.TryGetValue(key, out var grid))
        {
            grid.list.Remove(unitId);
            if (grid.list.Count == 0) grids.Remove(key); 
        }
    }

    public GridKey Move(long unitId, float3 oldPos, float3 newPos)
    {
        var oldKey = GetCellKey(oldPos);
        var newKey = GetCellKey(newPos);

        if (oldKey != newKey) 
        {
            Remove(unitId, oldPos);
            Add(unitId, newPos);
        }
        return newKey;
    }

    public IEnumerable<long> Query(AABB aabb)
    {
        var key = GetCellKey(aabb.Center);
        if (grids.TryGetValue(key, out var grid))
        {
            foreach (var gridKey in grid.GetGrids(aabb))
            {
                if (grids.TryGetValue(gridKey, out var _grid))
                {
                    for (int i = _grid.list.Count - 1; i >= 0; i--)
                    {
                        yield return _grid.list[i];
                    }
                }
            }
        }
    }
}
