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

    public class LinearSpawner : Spawner
    {
        public List<LinearSpawnCondition> spawnConditions;
        public bool useFallback;
        public GameObject fallbackEnemy;

        private void Start()
        {
            SpawnEnemy();
        }

        public override void SpawnEnemy()
        {
            foreach (var spawnCondition in spawnConditions)
            {
                var shouldSpawn = ConditionEvaluator.EvaluateDynamicCondition(spawnCondition.condition);

                if (!shouldSpawn) continue;


                // So, the condition finally met.
                SpawnEnemyIntoScene(spawnCondition.enemy);
                Debug.Log("Spawn enemy from condition");
                return;
            }

            // If this is reached, spawn fallback.
            if (!useFallback) return;
            SpawnEnemyIntoScene(fallbackEnemy);
            Debug.Log("Spawn enemy from fallback");
        }
    }
}