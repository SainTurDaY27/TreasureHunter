using System;
using System.Collections.Generic;
using System.Linq;
using TreasureHunter.Core.Data;
using TreasureHunter.Gameplay.System;
using TreasureHunter.Gameplay.System.DynamicDifficulty;
using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies.Spawners
{

    [Serializable]
    public struct LinearSpawnCondition
    {
        public DynamicCondition condition;
        public GameObject enemy;
    }

    public class LinearSpawner : MonoBehaviour
    {
        public List<LinearSpawnCondition> spawnConditions;
        public bool useFallback;
        public GameObject fallbackEnemy;

        public void SpawnEnemy()
        {
            var dataManager = DataManager.Instance;
            foreach (var spawnCondition in spawnConditions)
            {
                if (spawnCondition.condition.useSkillCondition)
                {
                    if (!spawnCondition.condition.skillConditions.All(skill => dataManager.PlayerData.HasSkill(skill)))
                    {
                        continue;
                    }
                }

                if (spawnCondition.condition.useTreasureCondition)
                {
                    if (dataManager.GameData.TreasureCount < spawnCondition.condition.requiredTreasure)
                    {
                        continue;
                    }
                }

                // So, the condition finally met.
                Instantiate(spawnCondition.enemy, transform.position, transform.rotation);
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