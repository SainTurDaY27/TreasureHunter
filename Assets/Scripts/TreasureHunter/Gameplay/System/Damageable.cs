using TreasureHunter.Gameplay.System;
using UnityEngine;
using UnityEngine.Events;

namespace TreasureHunter.Gameplay.System
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] 
        private int maxHealth = 6;
        [SerializeField] 
        private int health = 6;
        [SerializeField] 
        private bool isAlive = true;
        // Use for invincibility frame
        [SerializeField] 
        private bool isInvincible = false;

        public UnityEvent<int, Vector2> damageableHit;
        public UnityEvent damageableDeath;
        public UnityEvent<int, int> healthChange;
        private Animator _animator;
        private float _timeSinceHit = 0f;
        public float invicibilityTime = 0.5f;

        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public int Health
        {
            get => health;
            set
            {
                health = value;
                healthChange?.Invoke(health, MaxHealth);
                if (health <= 0)
                {
                    IsAlive = false;
                }
            }
        }

        public bool IsAlive
        {
            get => isAlive;
            set
            {
                isAlive = value;
                _animator.SetBool(AnimationStrings.IsAlive, value);
                if (!value) damageableDeath?.Invoke();
            }
        }

        public bool LockVelocity
        {
            get => _animator.GetBool(AnimationStrings.LockVelocity);
            set => _animator.SetBool(AnimationStrings.LockVelocity, value);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (isInvincible)
            {
                if (_timeSinceHit > invicibilityTime)
                {
                    isInvincible = false;
                    _timeSinceHit = 0;
                }
            }

            _timeSinceHit += Time.deltaTime;
        }

        public bool Hit(int attackDamage, Vector2 knockback)
        {
            if (IsAlive && !isInvincible)
            {
                Health -= attackDamage;
                isInvincible = true; // Never been hit again

                _animator.SetTrigger(AnimationStrings.HitTrigger);
                LockVelocity = true;
                // damageableHit could be null
                damageableHit?.Invoke(attackDamage, knockback);
                // TODO: Add character events
                // CharacterEvents.characterDamaged.Invoke(gameObject, damage);
                return true;
            }

            return false;
        }
    }
}