using System;
using System.Collections.Generic;
using System.Linq;
using TreasureHunter.Core.Data;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class DynamicSpawn : MonoBehaviour
    {
        public bool useSkillCondition = true;
        public bool useTreasureCondition = false;
        public List<SkillKey> skillConditions;
        public int requiredTreasure = 0;

        private void Awake()
        {
            var dataManager = DataManager.Instance;
            if (dataManager == null)
            {
                Debug.LogWarning("Data manager does not exist. Not spawning object");
                Destroy(gameObject);
                return;
            }

            if (useSkillCondition && !skillConditions.All(skill => dataManager.PlayerData.GetObtainedSkills().Contains(skill)))
            {
                Destroy(gameObject);
            }

            var treasureCount = dataManager.GameData.TreasureCount;

            if (useTreasureCondition && treasureCount < requiredTreasure)
            {
                Destroy(gameObject);
            }
        }
    }
}