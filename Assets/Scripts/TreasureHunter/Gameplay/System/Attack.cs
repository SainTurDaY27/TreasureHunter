using UnityEngine;
using UnityEngine.Events;

namespace TreasureHunter.Gameplay.System
{
    public class Attack : MonoBehaviour
    {
        // Please set it to either 1 or 2.
        public int attackDamage = 1;

        public Vector2 knockback = Vector2.zero;
        public bool isContactDamage = false;

        // localScale.x is inverted if left
        public bool startRight = true;
        public UnityEvent attackHit;

        void OnTriggerEnter2D(Collider2D other)
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (!damageable) return;
            // IsAlive is checked in the other script.
            Vector2 actualKnockback;
            if (isContactDamage)
            {
                float xDistance = other.transform.position.x - transform.position.x;
                float knockbackX = xDistance > 0 ? knockback.x : -knockback.x;
                actualKnockback = new Vector2(knockbackX, knockback.y);
            }
            // not contact damage
            else if (startRight)
            {
                actualKnockback =
                    transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            }
            else
            {
                actualKnockback =
                    transform.parent.localScale.x < 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            }

            if (damageable.Hit(attackDamage, actualKnockback))
            {
                attackHit?.Invoke();
            }
        }
    }
}