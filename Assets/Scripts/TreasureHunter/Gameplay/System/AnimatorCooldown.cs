using System;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorCooldown : MonoBehaviour
    {
        public string parameterName;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            var parameter = _animator.GetFloat(parameterName);
            if (parameter > 0)
            {
                _animator.SetFloat(parameterName, parameter - Time.deltaTime);
            }
        }
    }
}