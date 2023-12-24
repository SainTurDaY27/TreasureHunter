using System;
using UnityEngine;
using TreasureHunter.Gameplay.System;

[RequireComponent(typeof(Damageable), typeof(Rigidbody2D))]
public class KnockbackReceiver : MonoBehaviour
{
    private Damageable _damageable;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        if (_damageable.IsAlive)
        {
            _rb.velocity = new(knockback.x, _rb.velocity.y + knockback.y);
        }
    }
}