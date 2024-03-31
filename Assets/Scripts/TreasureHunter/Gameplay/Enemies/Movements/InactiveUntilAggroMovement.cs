using System;
using TreasureHunter.Gameplay.Enemies.Attacks;
using TreasureHunter.Gameplay.System;
using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies.Movements
{
    public class InactiveUntilAggroMovement : MonoBehaviour
    {
        public DetectionZone playerDetectionZone;
        public float activeDuration = 10f;
        // Not sure if interface should be used.
        public MonoBehaviour movementScript;

        private bool _isActive = false;
        private Animator _animator;
        private float _activeTime = 0f;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            // Check if player is detected
            if (playerDetectionZone.detected.Count > 0)
            {
                _animator.SetBool(AnimationStrings.IsActive, true);
                movementScript.enabled = true;
                _activeTime = 0f;
            }
            else
            {
                // Player not detected
                _activeTime += Time.deltaTime;
                if (_activeTime >= activeDuration)
                {
                    _animator.SetBool(AnimationStrings.IsActive, false);
                    movementScript.enabled = false;
                    // Unlike the previous one, this one will change based on animator only.
                }
            }
        }
    }
}