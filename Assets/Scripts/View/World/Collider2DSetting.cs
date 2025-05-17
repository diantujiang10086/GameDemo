using System.IO;
using Unity.Mathematics;
using UnityEngine;


public class Collider2DSetting : MonoBehaviour
{
    public bool isCollisionDestory;
    public bool isEnableColliderDetection;
    public int layer;
    public int colliderLayer;

    public string CreateConfig(int id)
    {
        var boxCollider2D = GetComponent<BoxCollider2D>();
        var circleCollider2D = GetComponent<CircleCollider2D>();
        
        ColliderShape shape = ColliderShape.None;
        float2 offset = float2.zero;
        float radius = 0;
        float2 size = float2.zero;
        if (boxCollider2D != null)
        {
            size = boxCollider2D.size;
            offset = boxCollider2D.offset;
            shape = ColliderShape.Box;
        }
        if(circleCollider2D != null)
        {
            radius = circleCollider2D.radius;
            offset = circleCollider2D.offset;
            shape = ColliderShape.Circle;
        }

        var config = new CollisionConfig 
        {
            isEnableColliderDetection = isEnableColliderDetection,
            isCollisionDestory = isCollisionDestory,
            colliderLayer = colliderLayer,
            colliderShape = shape,
            offset = offset,
            radius = radius,
            layer = layer,
            size = size
        };

        return $"{id}\t{isEnableColliderDetection}\t{isCollisionDestory}\t" +
            $"{colliderLayer}\t{shape}\t{offset.x}#{offset.y}\t{radius}\t{layer}\t{size.x}#{size.y}";
    }
}