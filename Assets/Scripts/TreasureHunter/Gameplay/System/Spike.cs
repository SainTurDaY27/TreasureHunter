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
            damageable.Hit(spikeDamage, Vector2.zero, bypassInvincibility: true);
            _spikeRecover.RecoverPlayer();
        }
    }
}