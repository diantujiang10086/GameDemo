using System;
using System.Collections.Generic;
using UnityEngine.Profiling;

public class PhysicsWorld : Singleton<PhysicsWorld>, IAwake, IFixedUpdate
{
    private struct Key : IEquatable<Key>
    {
        public long UnitA { get; }
        public long UnitB { get; }

        public Key(long unitA, long unitB)
        {
            UnitA = unitA;
            UnitB = unitB;
        }

        public bool Equals(Key other) => UnitA == other.UnitA && UnitB == other.UnitB;

        public override bool Equals(object obj) => obj is Key other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(UnitA, UnitB);

        public static bool operator ==(Key left, Key right) => left.Equals(right);

        public static bool operator !=(Key left, Key right) => !left.Equals(right);
    }

    private Dictionary<long, ColliderComponent> colliders = new Dictionary<long, ColliderComponent>(2000);
    private List<ColliderComponent> checkColliders = new List<ColliderComponent>(2000);
    private Dictionary<Key, ColliderInfo> previousCollisions = new Dictionary<Key, ColliderInfo>(2000);
    private Dictionary<Key, ColliderInfo> currentCollisions = new Dictionary<Key, ColliderInfo>(2000);

    public void Awake()
    {

    }

    public IEnumerable<ColliderComponent> GetColliders()
    {
        foreach (var component in colliders)
        {
            yield return component.Value;
        }
    }

    public void AddCollider(ColliderComponent colliderComponent)
    {
        colliders[colliderComponent.GetUnit().InstanceId] = colliderComponent;
    }

    public void RemoveCollider(ColliderComponent colliderComponent)
    {
        colliders.Remove(colliderComponent.GetUnit().InstanceId);
    }

    public void AddCheckCollider(ColliderComponent colliderComponent)
    {
        checkColliders.Add(colliderComponent);
    }

    public void RemoveCheckCollider(ColliderComponent colliderComponent)
    {
        checkColliders.Remove(colliderComponent);
    }

    public void FixedUpdate()
    {
        for (int index = checkColliders.Count -1; index >= 0; index--)
        {
            var colliderComponent = checkColliders[index];
            var unit = colliderComponent.GetUnit();
            foreach (var unitId in GridManager.Instance.Query(colliderComponent.GetAABB()))
            {
                if (unitId == unit.InstanceId || !colliders.TryGetValue(unitId, out var queryColliderComponent))
                    continue;

                if (queryColliderComponent == null || !colliderComponent.IsCollider(queryColliderComponent.Layer))
                    continue;
                var aabb = colliderComponent.GetAABB();
                var queryAABB = queryColliderComponent.GetAABB();

                if (aabb.TestOverlap(queryAABB))
                {
                    if (colliderComponent.TestOverlap(queryColliderComponent))
                    {
                        currentCollisions[new Key(unit.InstanceId, unitId)] = new ColliderInfo { entityA = unit, entityB = queryColliderComponent.GetUnit() };
                    }
                }
            }
        }

        foreach (var previousCollision in previousCollisions)
        {
            if (!currentCollisions.ContainsKey(previousCollision.Key))
            {
                EventSystem.Instance.Publish(new CollisionExit { entityA = previousCollision.Value.entityA, entityB = previousCollision.Value.entityB });
            }
        }

        foreach (var currentCollision in currentCollisions)
        {
            if (previousCollisions.ContainsKey(currentCollision.Key))
            {
                EventSystem.Instance.Publish(new CollisionStay { entityA = currentCollision.Value.entityA, entityB = currentCollision.Value.entityB });
            }
            else
            {
                EventSystem.Instance.Publish(new CollisionEnter { entityA = currentCollision.Value.entityA, entityB = currentCollision.Value.entityB });
            }
        }

        (previousCollisions, currentCollisions) = (currentCollisions, previousCollisions);
        currentCollisions.Clear();

    }
}
