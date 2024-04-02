using System;
using UnityEngine;
using TreasureHunter.Gameplay.System;
using UnityEngine.Events;

namespace TreasureHunter.Gameplay.Enemies.Attacks
{
    [RequireComponent(typeof(Animator))]
    public class SimpleAttackDetection : MonoBehaviour
    {
        public string boolName = AnimationStrings.HasTarget;
        public UnityEvent afterDetectPlayer;
        public DetectionZone attackDetectionZone;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            var detected = attackDetectionZone.detected.Count > 0;
            _animator.SetBool(boolName, detected);
            if (detected)
            {
                afterDetectPlayer?.Invoke();
            }
        }
    }
}