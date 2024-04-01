using System;
using TreasureHunter.Gameplay.System;
using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies.Movements
{
    public class CircleFlyingMovement : MonoBehaviour, ITargetable
    {
        public Transform target;
        // Tangential speed
        public float movingSpeed = 3f;
        // public float rotationSpeed = .75f;
        public float radius = 3f;

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

        private void FixedUpdate()
        {
            if (LockVelocity) return;
            if (!CanMove) return;


            Vector2 center = target.position;
            var position = _rb.position;
            Vector2 offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * radius;
            Vector2 destination = center + offset;
            Vector2 direction = (destination - position).normalized;
            _rb.velocity = direction * movingSpeed;

            _angle += AngularSpeed * Time.fixedDeltaTime;
        }

        public void SetTarget(Transform theTarget)
        {
            this.target = theTarget;
        }
    }
}