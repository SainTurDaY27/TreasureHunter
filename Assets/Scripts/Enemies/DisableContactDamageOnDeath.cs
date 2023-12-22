using UnityEngine;

namespace Enemies
{
    public class DisableContactDamageOnDeath : MonoBehaviour
    {
        public Collider2D contactDamageCollider;

        public void OnDeath()
        {
            contactDamageCollider.enabled = false;
        }
    }
}