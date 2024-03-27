using System.Collections.Generic;
using TreasureHunter.Gameplay.System;
using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies.Spawners
{
    /// <summary>
    /// This is a spawn condition used by spawner.
    ///
    /// Unlike self-dynamic spawn, the object should not be spawned first
    /// and then destroyed. The spawner should only spawn the object if the game
    /// satisfies the condition.
    /// </summary>
    [CreateAssetMenu(fileName = "SpawnCondition", menuName = "TreasureHunter/SpawnCondition", order = 0)]
    public class SpawnCondition : ScriptableObject
    {
        public bool useSkillCondition = true;
        public bool useTreasureCondition = false;
        public List<SkillKey> skillConditions;
        public int requiredTreasure = 0;
        public GameObject enemy;
    }
}