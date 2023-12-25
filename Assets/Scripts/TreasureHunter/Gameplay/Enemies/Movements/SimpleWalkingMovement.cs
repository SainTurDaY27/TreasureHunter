using System;
using UnityEngine;
using TreasureHunter.Gameplay.System;

namespace TreasureHunter.Gameplay.Enemies.Movements
{
    /// <summary>
    /// This script handles enemies with simple walking movement.
    /// Basically, it walks until there is a cliff or wall and turnaround.
    /// Very simple
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(TouchingDirections))]
    public class SimpleWalkingMovement : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Animator _animator;
        private TouchingDirections _touchingDirections;

        public float walkSpeed = 3f;

        // It is for setting walk direction from the inspector.
        public WalkableDirection _walkDirection = WalkableDirection.Left;

        public WalkableDirection WalkDirection
        {
            get => _walkDirection;

            set
            {
                if (_walkDirection != value)
                {
                    // Flip scale
                    var o = gameObject;
                    var oldLocaleScale = o.transform.localScale;
                    o.transform.localScale = new Vector2(oldLocaleScale.x * -1, oldLocaleScale.y);
                }

                _walkDirection = value;
            }
        }

        public void FlipDirection()
        {
            WalkDirection = WalkDirection == WalkableDirection.Left ? WalkableDirection.Right : WalkableDirection.Left;
        }

        private Vector2 WalkDirectionVector
        {
            get => WalkDirection == WalkableDirection.Left ? Vector2.left : Vector2.right;
        }

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _touchingDirections = GetComponent<TouchingDirections>();
        }

        private bool LockVelocity => _animator.GetBool(AnimationStrings.LockVelocity);
        private bool CanMove => _animator.GetBool(AnimationStrings.CanMove);

        private void FixedUpdate()
        {
            if (_touchingDirections.IsGround && _touchingDirections.IsOnWall)
            {
                FlipDirection();
            }

            if (LockVelocity) return;
            if (CanMove)
            {
                _rb.velocity = new Vector2(walkSpeed * WalkDirectionVector.x, _rb.velocity.y);
            }
        }

    }
}