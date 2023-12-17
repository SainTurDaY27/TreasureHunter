using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    
    // Please set it to either 1 or 2.
    public int attackDamage = 1;
    public Vector2 knockback = Vector2.zero;

    void OnTriggerEnter2D(Collider2D other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable)
        {
            // IsAlive is checked in the other script.
            Vector2 actualKnockback = 
                transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            if (damageable.Hit(attackDamage, actualKnockback))
            {
                // Debug.Log("Hit");
            }
        }

    }
}
