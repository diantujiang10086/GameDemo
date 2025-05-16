using System;
using Unity.Mathematics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class SparseGridCollision2DManager : Singleton<SparseGridCollision2DManager>, IAwake
{
    public struct CollisionPair : IEquatable<CollisionPair>
    {
        public long unitA;
        public long unitB;
        public CollisionPair(long a, long b)
        {
            if (a < b)
            {
                unitA = a;
                unitB = b;
            }
            else
            {
                unitA = b;
                unitB = a;
            }
        }
        public bool Equals(CollisionPair other) => unitA == other.unitA && unitB == other.unitB;
        public override bool Equals(object obj) => obj is CollisionPair other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(unitA, unitB);
    }

    private float cellSize = 1f;
    private HashSet<CollisionPair> previousFramePairs = new(10240);
    private HashSet<CollisionPair> currentFramePairs = new(10240);

    private Queue<Collision2DComponent> colliderDetections = new(10240);
    private Dictionary<int2, List<Collision2DComponent>> dict = new(10240);
    public void Awake()
    {
    }

    public void Register(Collision2DComponent collision2DComponent)
    {
        collision2DComponent.UpdateBounds();
        UpdateCollisionDetection(collision2DComponent);
        AddToCell(collision2DComponent);
    }

    //当isEnableColliderDetection改变时调用
    public void UpdateCollisionDetection(Collision2DComponent collision2DComponent)
    {
        if (collision2DComponent.IsEnableColliderDetection)
        {
            colliderDetections.Enqueue(collision2DComponent);
        }
    }

    public void UpdateCollisionGrid(Collision2DComponent collision2DComponent) 
    {
        collision2DComponent.UpdateBounds();
        Remove(collision2DComponent);

        collision2DComponent.Clear();

        AddToCell(collision2DComponent);
    }

    public void CalculateAllCollision()
    {
        int count = colliderDetections.Count;
        while (count-- > 0)
        {
            var component = colliderDetections.Dequeue();
            if(!component.IsEnableColliderDetection || component.IsDisposed)
            {
                Remove(component);
                continue;
            }

            colliderDetections.Enqueue(component);

            CalculateCollision(component);
        }
        FinalizeCollisionEvents();
    }

    private void AddToCell(Collision2DComponent collision2DComponent)
    {
        float2 min = collision2DComponent.geometry.min;
        float2 max = collision2DComponent.geometry.max;

        int2 minCell = WorldToGridPos(min, cellSize);
        int2 maxCell = WorldToGridPos(max, cellSize);

        for (int x = minCell.x; x <= maxCell.x; ++x)
        {
            for (int y = minCell.y; y <= maxCell.y; ++y)
            {
                int2 cell = new int2(x, y);
                if (!dict.TryGetValue(cell, out var list))
                    dict[cell] = list = ListPool<Collision2DComponent>.Get();

                list.Add(collision2DComponent);
                collision2DComponent.Add(cell);
            }
        }
    }

    private void Remove(Collision2DComponent collision2DComponent)
    {
        foreach (var cell in collision2DComponent.GetOverlayCells())
        {
            if (dict.TryGetValue(cell, out var list))
            {
                list.Remove(collision2DComponent);
                if(list.Count == 0)
                {
                    ListPool<Collision2DComponent>.Release(list);
                    dict.Remove(cell);
                }
            }
        }
    }

    private void CalculateCollision(Collision2DComponent colliderComponent)
    {
        foreach (var cell in colliderComponent.GetOverlayCells())
        {
            if(dict.TryGetValue(cell, out var list))
            {
                int index = list.Count;
                while (index-- > 0)
                {
                    var collider = list[index];
                    if (colliderComponent == collider || !colliderComponent.CanCollideWith(collider))
                        continue;

                    var pair = CreateSortedPair(colliderComponent.UnitId, collider.UnitId);
                    if (currentFramePairs.Contains(pair))
                        continue;

                    if (!CollisionHelper.AABBOverlap(colliderComponent.geometry, collider.geometry))
                        continue;

                    if (!CollisionHelper.Overlap(colliderComponent.geometry, collider.geometry))
                        continue;

                    currentFramePairs.Add(pair);

                    if (!previousFramePairs.Contains(pair))
                    {
                        EventSystem.Instance.Publish(new CollisionEnter { unitA = colliderComponent.UnitId, unitB = collider.UnitId });
                    }

                    EventSystem.Instance.Publish(new CollisionStay { unitA = colliderComponent.UnitId, unitB = collider.UnitId });
                }
            }
        }
    }

    public void FinalizeCollisionEvents()
    {
        foreach (var pair in previousFramePairs)
        {
            if (!currentFramePairs.Contains(pair))
            {
                EventSystem.Instance.Publish(new CollisionExit { unitA = pair.unitA, unitB = pair.unitB });
            }
        }

        previousFramePairs.Clear();
        foreach (var pair in currentFramePairs)
            previousFramePairs.Add(pair);

        currentFramePairs.Clear();
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static CollisionPair CreateSortedPair(long a, long b)
    {
        return new CollisionPair(a, b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int2 WorldToGridPos(float2 worldPos, float cellSize)
    {
        return (int2)math.floor(worldPos / cellSize);
    }
}

