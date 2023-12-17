using System.Numerics;
using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(TouchingDirections)),
     RequireComponent(typeof(Damageable))]
    public class SimpleWalkingEnemy : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Damageable _damageable;
        private Animator _animator;
        public DetectionZone cliffDetectionZone;

        public float walkSpeed = 3f;

        // These mess up flip direction.
        // Probably remove after discussion
        // public float walkAcceleration = 5f;
        // public float maxSpeed = 3f;
        // public float walkStopRate = 0.05f;
        public bool startOnRight = true;
        // private Vector2 _walkDirectionVector = Vector2.left;

        public bool CanMove => _animator.GetBool(AnimationStrings.CanMove);

        public enum WalkableDirection
        {
            Left,
            Right
        }

        private WalkableDirection _walkDirection = WalkableDirection.Left;

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

        private TouchingDirections _touchingDirections;

        public void OnHit(int damage, Vector2 knockback)
        {
            if (_damageable.IsAlive)
            {
                _rb.velocity = new(knockback.x, _rb.velocity.y + knockback.y);
            }
        }

        // Start is called before the first frame update
        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _touchingDirections = GetComponent<TouchingDirections>();
            _damageable = GetComponent<Damageable>();
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
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