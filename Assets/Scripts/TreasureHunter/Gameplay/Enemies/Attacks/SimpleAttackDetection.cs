using System;
using UnityEngine;
using TreasureHunter.Gameplay.System;

namespace TreasureHunter.Gameplay.Enemies.Attacks
{
    [RequireComponent(typeof(Animator))]
    public class SimpleAttackDetection : MonoBehaviour
    {
        public string boolName = AnimationStrings.HasTarget;
        public DetectionZone attackDetectionZone;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetBool(boolName, attackDetectionZone.detected.Count > 0);
        }
    }
}