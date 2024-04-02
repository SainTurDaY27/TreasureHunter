using System;
using TreasureHunter.Gameplay.System;
using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies.Movements
{
    public class ChasePlayerMovement : MonoBehaviour
    {
        public float chasingSpeed = 3f;
        public float playerReachedDistance = 0.2f;

        private Animator _animator;
        private Rigidbody2D _rb;

        private bool CanMove => _animator.GetBool(AnimationStrings.CanMove);
        private bool LockVelocity => _animator.GetBool(AnimationStrings.LockVelocity);


        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!CanMove) return;
            if (CanMove && !LockVelocity)
            {
                Chase();
            }
            else 
            {
                _rb.velocity = Vector2.zero;
            }
        }

        private void Chase()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                _rb.velocity = Vector2.zero;
                return;
            }

            var playerPosition = player.transform.position;
            var currentPosition = transform.position;
            var direction = (playerPosition - currentPosition).normalized;
            float distance = Vector2.Distance(playerPosition, currentPosition);
            if (distance <= playerReachedDistance)
            {
                _rb.velocity = Vector2.zero;
            }
            else
            {
                _rb.velocity = direction * chasingSpeed;
                UpdateDirection();
            }
        }

        private void UpdateDirection()
        {
            Vector3 localScale = transform.localScale;
            Vector2 currentVelocity = _rb.velocity;

            if ((localScale.x > 0 && currentVelocity.x < 0) || (localScale.x < 0 && currentVelocity.x > 0))
            {
                transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
            }
        }
    }
}