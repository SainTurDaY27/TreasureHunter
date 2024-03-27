using System;
using System.Collections.Generic;
using System.Linq;
using TreasureHunter.Core.Data;
using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies.Spawners
{
    public class TieredSpawner : MonoBehaviour
    {
        public List<SpawnCondition> spawnConditions;
        public bool useFallback;
        public GameObject fallbackEnemy;

        public void SpawnEnemy()
        {
            var dataManager = DataManager.Instance;
            foreach (var condition in spawnConditions)
            {
                if (condition.useSkillCondition)
                {
                    if (!condition.skillConditions.All(skill => dataManager.PlayerData.HasSkill(skill)))
                    {
                        continue;
                    }
                }

                if (condition.useTreasureCondition)
                {
                    if (dataManager.GameData.TreasureCount < condition.requiredTreasure)
                    {
                        continue;
                    }
                }

                // So, the condition finally met.
                Instantiate(condition.enemy, transform.position, transform.rotation);
                Debug.Log("Spawn enemy from condition");
                return;
            }

            // If this is reached, spawn fallback.
            if (!useFallback) return;
            Instantiate(fallbackEnemy, transform.position, transform.rotation);
            Debug.Log("Spawn enemy from fallback");
        }

        private void Start()
        {
            SpawnEnemy();
        }
    }
}