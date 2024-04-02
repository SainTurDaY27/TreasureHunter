using System;
using TreasureHunter.Gameplay.System;
using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies.Movements
{
    public class CircleFlyingMovement : MonoBehaviour, ITargetable
    {
        public Transform target;
        // Tangential speed
        // Angular speed is calculated from it.
        public float movingSpeed = 3f;
        public float radius = 3f;
        public bool lookAtTarget = true;
        public bool startRight = true;

        private Animator _animator;
        private Rigidbody2D _rb;
        private float _angle = 0f;
        private float AngularSpeed => movingSpeed / radius;

        private bool CanMove => _animator.GetBool(AnimationStrings.CanMove);
        private bool LockVelocity => _animator.GetBool(AnimationStrings.LockVelocity);

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            // no target? = target player
            if (target == null)
            {
                target = GameObject.FindWithTag("Player").transform;
            }
        }

        private void FixedUpdate()
        {
            if (LockVelocity) return;
            if (!CanMove) return;


            Vector2 center = target.position;
            var position = _rb.position;
            Vector2 offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * radius;
            Vector2 destination = center + offset;
            Vector2 direction = (destination - position).normalized;
            
            // flip direction
            if (lookAtTarget)
            {
                Vector2 vectorToTarget = center - position;
                var directionX = Mathf.Sign(vectorToTarget.x);
                var localScale = transform.localScale;
                var localScaleX = localScale.x;
                // Assume start right
                if (Math.Abs(directionX - Mathf.Sign(localScaleX)) > 0.001 && startRight)
                {
                    localScaleX *= -1;
                }

                transform.localScale = new Vector3(localScaleX, localScale.y, localScale.z);

            }
            _rb.velocity = direction * movingSpeed;

            _angle += AngularSpeed * Time.fixedDeltaTime;
        }

        public void SetTarget(Transform theTarget)
        {
            this.target = theTarget;
        }

    }
}