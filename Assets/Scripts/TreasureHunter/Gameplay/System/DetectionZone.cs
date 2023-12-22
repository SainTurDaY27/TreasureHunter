using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TreasureHunter.Gameplay.System
{
    public class DetectionZone : MonoBehaviour
    {
        public UnityEvent noColliderRemains;
        public List<Collider2D> detected = new();

        private void OnTriggerEnter2D(Collider2D other)
        {
            detected.Add(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            detected.Remove(other);
            if (detected.Count <= 0)
            {
                noColliderRemains?.Invoke();
            }

        }
    }
}