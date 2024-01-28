using System;
using TreasureHunter.Core.Data;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class SkillPickup : MonoBehaviour
    {
        // Start is called before the first frame update
        private DataManager _dataManager;
        public SkillKey skill;
        void Awake()
        {
            _dataManager = FindObjectOfType<DataManager>();
        }

        private void Start()
        {
            if (_dataManager.PlayerData.HasSkill(skill))
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _dataManager.PlayerData.ObtainSkill(skill);
            Destroy(gameObject);
        }
    }
}
