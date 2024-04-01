using System;
using System.Collections;
using TreasureHunter.Gameplay.System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TreasureHunter.Gameplay.Enemies.Attacks
{
    public class FlyingChargeAttack : MonoBehaviour, ITargetable
    {
        [FormerlySerializedAs("chargingTarget")] public Transform target;
        public float chargingVelocity = 20f;
        public float chargingDistance = 8f;
        public float chargingDelayTime = 2f;
        public float chargingTime = 10f;

        private Animator _animator;
        private Rigidbody2D _rb;
        private Coroutine _chargingCoroutine;
        private float _chargingTimer = 0;


        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            // Charge
            if (_chargingTimer >= chargingTime)
            {
                // Prevent normal movement from moving.
                if (_chargingCoroutine == null)
                {
                    _chargingCoroutine = StartCoroutine(nameof(Charge));
                }
            }
            else
            {
                _chargingTimer += Time.fixedDeltaTime;
            }
        }

        private IEnumerator Charge()
        {
            _animator.SetBool(AnimationStrings.CanMove, false);
            _animator.SetBool(AnimationStrings.LockedInTarget, true);
            Vector2 directionToTarget = (target.transform.position - transform.position).normalized;
            Vector2 reversedDirection = -directionToTarget;
            // Prepare
            _rb.velocity = reversedDirection * (chargingDistance / chargingDelayTime);
            yield return new WaitForSeconds(chargingDelayTime);
            // Charge
            _rb.velocity = (chargingDistance + chargingVelocity) * directionToTarget;
            yield return new WaitForSeconds(1);
            _animator.SetBool(AnimationStrings.CanMove, true);
            _animator.SetBool(AnimationStrings.LockedInTarget, false);
            _chargingTimer = 0;
            _chargingCoroutine = null;
        }

        public void SetTarget(Transform theTarget)
        {
            target = theTarget;
        }
    }
}