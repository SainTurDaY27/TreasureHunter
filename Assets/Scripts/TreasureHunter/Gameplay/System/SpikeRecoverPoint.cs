using System;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    [RequireComponent(typeof(Collider2D))]
    public class SpikeRecoverPoint : MonoBehaviour
    {
        private SpikeRecover _spikeRecover;

        private void Awake()
        {
            // There is only one per scene!
            _spikeRecover = FindObjectOfType<SpikeRecover>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _spikeRecover.CurrentRecoverPoint = transform;
        }
    }
}