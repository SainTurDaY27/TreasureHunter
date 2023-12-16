using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(TouchingDirections)),
     RequireComponent(typeof(Damageable))]
    public class SimpleWalkingEnemy : MonoBehaviour
    {
        private Rigidbody2D _rb;
        public float walkSpeed = 3f;
        public float walkAcceleration = 5f;
        public float maxSpeed = 3f;
        public float walkStopRate = 0.05f;

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
            _rb.velocity = new(knockback.x, _rb.velocity.y + knockback.y);
        }

        // Start is called before the first frame update
        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _touchingDirections = GetComponent<TouchingDirections>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (_touchingDirections.IsGround && _touchingDirections.IsOnWall)
            {
                Debug.Log("Flip direction");
                FlipDirection();
            }

            if ( /*CanMove &&*/ _touchingDirections.IsGround)
            {
                float xVelocity =
                    Mathf.Clamp(_rb.velocity.x + (walkAcceleration * WalkDirectionVector.x * Time.deltaTime), -maxSpeed,
                        maxSpeed);
                _rb.velocity = new Vector2(xVelocity, _rb.velocity.y);
            }
            else
            {
                _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, 0, walkStopRate), _rb.velocity.y);
            }
        }
    }
}