using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    private CapsuleCollider2D touchingCol; // It is easier to fix the result this way
    private Animator _animator;

    private RaycastHit2D[] groundHits = new RaycastHit2D[5];
    private RaycastHit2D[] wallHits = new RaycastHit2D[5];
    private RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    
    public float groundDistance = 0.05f;
    public float wallCheckDistance = 0.2f;
    public float ceilingCheckDistance = 0.05f;

    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isOnWall = true;
    [SerializeField] private bool isOnCeiling = true;
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        IsGround = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallCheckDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingCheckDistance) > 0;

    }

    public bool IsOnCeiling
    {
        get => isOnCeiling;
        private set
        {
            isOnCeiling = value;
            _animator.SetBool(AnimationStrings.isOnCeiling, value);

        }
    }
    
    public bool IsOnWall
    {
        get => isOnWall;
        private set
        {
            isOnWall = value;
            _animator.SetBool(AnimationStrings.isOnWall, value);

        }
    }


    public bool IsGround
    {
        get => isGrounded;
        private set
        {
            isGrounded = value;
            _animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }
}