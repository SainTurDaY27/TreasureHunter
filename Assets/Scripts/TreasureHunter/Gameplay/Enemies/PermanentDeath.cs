using System;
using TreasureHunter.Core.Data;
using TreasureHunter.Gameplay.System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TreasureHunter.Gameplay.Enemies
{
    [RequireComponent(typeof(Damageable))]
    public class PermanentDeath : MonoBehaviour
    {
        public string permanentDeathStateID;

        private Damageable _damageable;

        private void Awake()
        {
            if (string.IsNullOrEmpty(permanentDeathStateID))
            {
                Debug.LogError("Permanent death state ID is missing");
            }

            _damageable = GetComponent<Damageable>();
        }

        private void Start()
        {
            DataManager.Instance.GameData.GetBoolState(permanentDeathStateID, out var isDead);
            if (!isDead) return;
            Destroy(gameObject);
        }

        public void SetPermanentDeath()
        {
            DataManager.Instance.GameData.SetBoolState(permanentDeathStateID, true);
        }
    }
}