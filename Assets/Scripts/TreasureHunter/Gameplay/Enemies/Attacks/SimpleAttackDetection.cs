using System;
using UnityEngine;

namespace Enemies.Attacks
{
    [RequireComponent(typeof(Animator))]
    public class SimpleAttackDetection : MonoBehaviour
    {
        public DetectionZone attackDetectionZone;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetBool(AnimationStrings.HasTarget, attackDetectionZone.detected.Count > 0);
        }
    }
}