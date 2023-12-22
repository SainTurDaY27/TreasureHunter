using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(TouchingDirections)),
     RequireComponent(typeof(Damageable))]
    public class SimpleAttackingEnemy : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Damageable _damageable;
        private Animator _animator;
        private TouchingDirections _touchingDirections;
        public Collider2D contactDamageCollider;
        public DetectionZone attackDetectionZone;

        public float walkSpeed = 3f;

        public bool CanMove => _animator.GetBool(AnimationStrings.CanMove);

        // TODO: Refactor this
        public enum WalkableDirection
        {
            Left,
            Right
        }

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

        private Vector2 WalkDirectionVector
        {
            get
            {
                if (WalkDirection == WalkableDirection.Left)
                {
                    return Vector2.left;
                }
                else
                {
                    return Vector2.right;
                }
            }
        }

        public void OnCliffDetected()
        {
            if (_touchingDirections.IsGround) FlipDirection();
        }

        private void FlipDirection()
        {
            if (WalkDirection == WalkableDirection.Left)
            {
                WalkDirection = WalkableDirection.Right;
            }
            else if (WalkDirection == WalkableDirection.Right)
            {
                WalkDirection = WalkableDirection.Left;
            }
            else
            {
                Debug.Log("Something went wrong");
            }
        }

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _touchingDirections = GetComponent<TouchingDirections>();
            _damageable = GetComponent<Damageable>();
            _animator = GetComponent<Animator>();
        }

        [FormerlySerializedAs("_hasTarget")] public bool hasTarget = false;

        public bool HasTarget
        {
            get => hasTarget;
            private set
            {
                hasTarget = value;
                _animator.SetBool(AnimationStrings.HasTarget, value);
            }
        }

        public void OnHit(int damage, Vector2 knockback)
        {
            if (_damageable.IsAlive)
            {
                _rb.velocity = new(knockback.x, _rb.velocity.y + knockback.y);
            }
        }

        public void OnDeath()
        {
            contactDamageCollider.enabled = false;
        }

        void Update()
        {
            HasTarget = attackDetectionZone.detected.Count > 0;
        }

        void FixedUpdate()
        {
            if (_touchingDirections.IsGround && _touchingDirections.IsOnWall)
            {
                FlipDirection();
            }

            if (_damageable.LockVelocity) return;
            if (CanMove)
            {
                _rb.velocity = new Vector2(walkSpeed * WalkDirectionVector.x, _rb.velocity.y);
            }

            // Something is wrong with this code.
            // if ( /*CanMove &&*/ _touchingDirections.IsGround)
            // {
            //     float xVelocity =
            //         Mathf.Clamp(_rb.velocity.x + (walkAcceleration * _walkDirectionVector.x * Time.deltaTime), -maxSpeed,
            //             maxSpeed);
            //     _rb.velocity = new Vector2(xVelocity, _rb.velocity.y);
            //     
            // }
            // else
            // {
            //     _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, 0, walkStopRate), _rb.velocity.y);
            // }
        }
    }
}