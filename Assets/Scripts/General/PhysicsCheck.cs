using System;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D _capsuleCollider;

    [Header("Check Param")] public bool manual;
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;


    public float checkRadius;

    public LayerMask groundLayer;


    [Header("State")] public bool isGround;

    public bool touchLeftWall;

    public bool touchRightWall;

    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        if (!manual)
        {
            rightOffset = new Vector2((_capsuleCollider.bounds.size.x + _capsuleCollider.offset.x) / 2,
                _capsuleCollider.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
    }

    private void Update()
    {
        Check();
    }

    private void FixedUpdate()
    {
        
    }


    public void Check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset*transform.localScale, checkRadius, groundLayer);
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRadius, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset*transform.localScale , checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRadius);
    }
}