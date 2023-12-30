using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;

namespace TreasureHunter.Gameplay.System
{
    public class TouchingDirections : MonoBehaviour
    {
        [SerializeField] 
        private bool isGrounded = true;
        [SerializeField] 
        private bool isOnWall = true;
        [SerializeField] 
        private bool isOnCeiling = true;

        public ContactFilter2D castFilter;
        public RaycastHit2D[] groundHits = new RaycastHit2D[5];
        public RaycastHit2D[] wallHits = new RaycastHit2D[5];
        public RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
        public bool startOnRight = true;
        public float groundDistance = 0.05f;
        public float wallCheckDistance = 0.2f;
        public float ceilingCheckDistance = 0.05f;
        private CapsuleCollider2D touchingCol; // It is easier to fix the result this way
        private Animator _animator;

        private Vector2 wallCheckDirection
        {
            get
            {
                if (startOnRight)
                {
                    return gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                }
                else
                {

                    return gameObject.transform.localScale.x > 0 ? Vector2.left : Vector2.right;
                }
            }
        }

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
                _animator.SetBool(AnimationStrings.IsOnCeiling, value);

            }
        }

        public bool IsOnWall
        {
            get => isOnWall;
            private set
            {
                isOnWall = value;
                _animator.SetBool(AnimationStrings.IsOnWall, value);

            }
        }


        public bool IsGround
        {
            get => isGrounded;
            private set
            {
                isGrounded = value;
                _animator.SetBool(AnimationStrings.IsGrounded, value);
            }
        }
    }
}