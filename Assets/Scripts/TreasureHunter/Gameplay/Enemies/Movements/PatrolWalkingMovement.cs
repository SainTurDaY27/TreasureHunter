using System;
using System.Collections;
using TreasureHunter.Gameplay.System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TreasureHunter.Gameplay.Enemies.Movements
{
    /// <summary>
    /// This movement is for enemies that just patrolling around for a few distance.
    /// It can wait for a specified time and then flip direction and move back.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(TouchingDirections))]
    public class PatrolWalkingMovement : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Animator _animator;
        private TouchingDirections _touchingDirections;
        private Vector2 _originalPosition;
        private bool _stayStill = false;
        private bool _shouldResetOrigin = false;

        public float walkSpeed = 3f;
        public float patrolDistance = 10f;
        public float targetReachedThreshold = 0.1f;
        [FormerlySerializedAs("flipDirectionDelay")]
        public float patrolWaitTime = 5f;
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

        private float TargetX => WalkDirection == WalkableDirection.Left
            ? _originalPosition.x - patrolDistance
            : _originalPosition.x + patrolDistance;

        private float LeftTargetX => _originalPosition.x - patrolDistance;
        private float RightTargetX => _originalPosition.x + patrolDistance;
        private float _currentTarget;

        public void FlipDirection()
        {
            WalkDirection = WalkDirection == WalkableDirection.Left ? WalkableDirection.Right : WalkableDirection.Left;
            SetTargetX();
        }

        private void SetTargetX()
        {
            if (WalkDirection == WalkableDirection.Left)
            {
                _currentTarget = LeftTargetX;
            }
            else
            {
                _currentTarget = RightTargetX;
            }
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _touchingDirections = GetComponent<TouchingDirections>();
        }

        private void Start()
        {
            SetOrigin();
            SetTargetX();
        }

        private void OnEnable()
        {
            SetOrigin();
            SetTargetX();
            _animator.SetBool(AnimationStrings.CanMove, true);
        }

        private void OnDisable()
        {
            _animator.SetBool(AnimationStrings.CanMove, false);
        }

        private void SetOrigin()
        {
            _originalPosition = transform.position;
        }

        private void FixedUpdate()
        {
            if (_touchingDirections.IsGround && _touchingDirections.IsOnWall)
            {
                FlipDirection();
            }

            if (!_touchingDirections.IsGround)
            {
                // It need to recalculate patrol distance when reached the ground.
                _shouldResetOrigin = true;
            }

            if (_touchingDirections.IsGround && _shouldResetOrigin)
            {
                _shouldResetOrigin = false;
                SetOrigin();
            }

            if (_stayStill)
            {
                _animator.SetBool(AnimationStrings.IsMoving, false);
                return;
            }

            if (LockVelocity) return;

            if (CanMove)
            {
                _animator.SetBool(AnimationStrings.IsMoving, true);
                _rb.velocity = new Vector2(walkSpeed * WalkDirectionVector.x, _rb.velocity.y);

                // if reached target
                if (Mathf.Abs(transform.position.x - TargetX) <= targetReachedThreshold)
                {
                    _stayStill = true;
                    StartCoroutine(nameof(SetWalkingDelay));
                }
            }
        }

        private IEnumerator SetWalkingDelay()
        {
            yield return new WaitForSeconds(patrolWaitTime);
            FlipDirection();
            _stayStill = false;
        }
    }
}