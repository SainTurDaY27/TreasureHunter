using System;
using System.Collections.Generic;
using TreasureHunter.Core.Data;
using TreasureHunter.Gameplay.System;
using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies.Spawners
{
    [Serializable]
    public struct ScoredSpawnCondition
    {
        public GameObject enemy;
        public float minScore;
        public float maxScore;
    }

    [Serializable]
    public struct SkillScoreWeight
    {
        public float weight;
        public SkillKey skill;
    }

    [Serializable]
    public struct TreasureWeight
    {
        public int amountOfTreasureObtained;
        public float weight;
    }

    public class ScoredSpawner : MonoBehaviour
    {
        public List<ScoredSpawnCondition> spawnConditions;
        public List<SkillScoreWeight> skillScoreWeights;
        public List<TreasureWeight> treasureWeights;
        public bool useFallback;
        public GameObject fallbackEnemy;

        private void Start()
        {
            var difficultyScore = CalculateDifficultyScore();
            if (difficultyScore < 0)
            {
                Debug.Log("DataManager not found");
                if (useFallback) SpawnEnemy(fallbackEnemy);
            }

            foreach (var spawnCondition in spawnConditions)
            {
                // Evaluate from top to bottom
                if (difficultyScore > spawnCondition.minScore && difficultyScore <= spawnCondition.maxScore)
                {
                    SpawnEnemy(spawnCondition.enemy);
                }
            }
        }

        private void SpawnEnemy(GameObject enemy)
        {
            var transform1 = transform;
            Instantiate(enemy, transform1.position, transform1.rotation);
            // Don't destroy so it is possible to inspect the problem.
            // Destroy(gameObject);
        }

        private float CalculateDifficultyScore()
        {
            var dataManager = DataManager.Instance;
            // Fallback 
            if (dataManager == null) return -1;


            var playerData = dataManager.PlayerData;
            var gameData = dataManager.GameData;
            var currentScore = 0f;

            // skill score weight;
            foreach (var skillScoreWeight in skillScoreWeights)
            {
                if (playerData.HasSkill(skillScoreWeight.skill))
                {
                    currentScore += skillScoreWeight.weight;
                }
            }

            float treasureWeightScore = 0f;
            foreach (var treasureWeight in treasureWeights)
            {
                if (gameData.TreasureCount >= treasureWeight.amountOfTreasureObtained)
                {
                    treasureWeightScore = treasureWeight.weight;
                }
            }

            currentScore += treasureWeightScore;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"Difficulty score is {currentScore}");
#endif

            // TODO: Add map score
            return currentScore;
        }
    }
}