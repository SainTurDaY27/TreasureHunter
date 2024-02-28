using System;
using TreasureHunter.Gameplay.System;
using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies.Movements
{
    /// <summary>
    /// This movement is for enemies that walks a little around themselve.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(TouchingDirections))]
    public class PatrolWalkingMovement : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Animator _animator;
        private TouchingDirections _touchingDirections;
        private Vector2 _originalPosition;

        public float walkSpeed = 3f;
        public float patrolDistance = 10f;
        public float targetReachedThreshold = 0.1f;
        [SerializeField] private WalkableDirection walkDirection = WalkableDirection.Left;
        private bool LockVelocity => _animator.GetBool(AnimationStrings.LockVelocity);
        private bool CanMove => _animator.GetBool(AnimationStrings.CanMove);

        public WalkableDirection WalkDirection
        {
            get => walkDirection;

            set
            {
                if (walkDirection != value)
                {
                    // Flip scale
                    var o = gameObject;
                    var oldLocaleScale = o.transform.localScale;
                    o.transform.localScale = new Vector2(oldLocaleScale.x * -1, oldLocaleScale.y);
                }

                walkDirection = value;
            }
        }

        private Vector2 WalkDirectionVector
        {
            get => WalkDirection == WalkableDirection.Left ? Vector2.left : Vector2.right;
        }

        private float targetX => WalkDirection == WalkableDirection.Left
            ? _originalPosition.x - patrolDistance
            : _originalPosition.x + patrolDistance;

        public void FlipDirection()
        {
            WalkDirection = WalkDirection == WalkableDirection.Left ? WalkableDirection.Right : WalkableDirection.Left;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _touchingDirections = GetComponent<TouchingDirections>();
        }

        private void Start()
        {
            _originalPosition = transform.position;
        }

        private void FixedUpdate()
        {
            if (_touchingDirections.IsGround && _touchingDirections.IsOnWall)
            {
                FlipDirection();
            }


            if (LockVelocity) return;

            if (CanMove)
            {
                _animator.SetBool(AnimationStrings.IsMoving, true);
                _rb.velocity = new Vector2(walkSpeed * WalkDirectionVector.x, _rb.velocity.y);
                // TODO: Handle case when the character went out of range.
                if (Mathf.Abs(transform.position.x - targetX) <= targetReachedThreshold)
                {
                    FlipDirection();
                }
            }
        }
    }
}