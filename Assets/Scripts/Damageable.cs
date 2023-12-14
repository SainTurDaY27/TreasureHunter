using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChange;
    private Animator _animator;
    [SerializeField] private int maxHealth = 6;

    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    [SerializeField] private int health = 6;

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

    [SerializeField] private bool isAlive = true;

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

    // Use for invincibility frame
    [SerializeField] private bool isInvincible = false;
    private float _timeSinceHit = 0f;
    public float invicibilityTime = 0.5f;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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
            // damageableHit could be null
            damageableHit?.Invoke(attackDamage, knockback);
            // TODO: Add character events
            // CharacterEvents.characterDamaged.Invoke(gameObject, damage);
            return true;
        }

        return false;
    }
}