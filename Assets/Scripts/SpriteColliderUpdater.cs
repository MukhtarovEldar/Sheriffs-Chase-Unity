using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColliderUpdater : MonoBehaviour
{
    PolygonCollider2D col;
    SpriteRenderer spriteRenderer;
    List<Vector2> physicsShape = new List<Vector2>();
    
    void Start()
    {
        col = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void LateUpdate()
    {
        spriteRenderer.sprite.GetPhysicsShape(0, physicsShape);
        col.SetPath(0, physicsShape);
    }
}

