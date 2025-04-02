using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ColliderComponent playerColliderComponent;
    private void Awake()
    {
        Entry.Start();

        playerColliderComponent = Player.Instance.GetUnit().GetComponent<ColliderComponent>();
    }
    private void FixedUpdate()
    {
        EntitySystem.Instance.FixedUpdate();
    }
    private void Update()
    {
        EntitySystem.Instance.Update();
    }
    private void LateUpdate()
    {
        EntitySystem.Instance.LateUpdate();
        PhysicsWorld.Instance.FixedUpdate();
    }

    private void OnDrawGizmos()
    {
        if (PhysicsWorld.Instance == null)
            return;

        if(playerColliderComponent != null)
        {
            Gizmos.color = Color.red;
            var aabb = playerColliderComponent.GetAABB();
            var grid = GridManager.Instance.GetGrid(playerColliderComponent.GridKey);
            foreach (var gridKey in grid.GetGrids(aabb))
            {
                var _grid = GridManager.Instance.GetGrid(gridKey);
                if (_grid == null)
                    continue;
                var center = (_grid.outerMin + _grid.outerMax) / 2;
                var size = _grid.outerMax - _grid.outerMin;
                Gizmos.DrawWireCube(new Vector3(center.x, 0, center.y), new Vector3(size.x, 5, size.y));
            }
        }
        Gizmos.color = Color.green;
        foreach (var collider in PhysicsWorld.Instance.GetColliders())
        {
            var aabb = collider.GetAABB();
            Gizmos.DrawWireCube(new Vector3(aabb.Center.x, 0, aabb.Center.y), new Vector3(aabb.Size.x, 0, aabb.Size.y));
        }
        Gizmos.color = Color.white;
    }
}
