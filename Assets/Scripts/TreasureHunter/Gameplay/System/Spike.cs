using System;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    [RequireComponent(typeof(Collider2D))]
    public class Spike : MonoBehaviour
    {
        private SpikeRecover _spikeRecover;
        public int spikeDamage = 1;

        private void Awake()
        {
            _spikeRecover = FindObjectOfType<SpikeRecover>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var damageable = other.GetComponent<Damageable>();
            if (!damageable) return;
            if (other.CompareTag("Player"))
            {
                damageable.Hit(spikeDamage, Vector2.zero, bypassInvincibility: true);
                _spikeRecover.RecoverPlayerWithDelay();
            }
            else
            {
                // Enemy is basically instant kill.
                damageable.Hit(damageable.MaxHealth, Vector2.zero, bypassInvincibility: true);
            }
        }
    }
}