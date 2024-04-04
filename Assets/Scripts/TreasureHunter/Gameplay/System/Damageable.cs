using System.Collections;
using System.Collections.Generic;
using TreasureHunter.Gameplay.System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace TreasureHunter.Gameplay.System
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 6;
        [SerializeField] private int health = 6;

        [SerializeField] private bool isAlive = true;

        // Use for invincibility frame
        [SerializeField] private bool isInvincible = false;
        public bool IsInvincible => isInvincible;

        public UnityEvent<int, Vector2> damageableHit;
        public UnityEvent damageableDeath;
        public UnityEvent<int, int> healthChange;
        private Animator _animator;
        private GameManager _gameManager;
        private float _timeSinceHit = 0f;
        [FormerlySerializedAs("invicibilityTime")]
        public float invincibilityTime = 0.5f;
        public List<DamageType> immuneTypes = new();

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
            _gameManager = GameManager.Instance;
        }

        public void BecomeInvincible()
        {
            // Manually, adjust
            isInvincible = true;
        }

        public void BecomeInvincible(float time)
        {
            // This is for manual i frame.
            // Useful for spike or environmental damage.
            StartCoroutine(ManualInvincibility(time));
        }

        public void BecomeVulnerable()
        {
            isInvincible = false;
        }

        private IEnumerator ManualInvincibility(float time)
        {
            isInvincible = true;
            yield return new WaitForSeconds(time);
            isInvincible = false;
        }

        private void Update()
        {
            if (_gameManager.IsGamePaused)
            {
                return;
            }

            if (isInvincible)
            {
                if (_timeSinceHit > invincibilityTime)
                {
                    isInvincible = false;
                    _timeSinceHit = 0;
                }
            }

            _timeSinceHit += Time.deltaTime;
        }

        public bool Hit(int attackDamage, Vector2 knockback, bool bypassInvincibility = false,
            DamageType damageType = DamageType.Melee)
        {
            if (!IsAlive) return false;
            if (isInvincible && !bypassInvincibility) return false;
            if (immuneTypes.Contains(damageType)) return false;

            _timeSinceHit = 0;
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
    }
}