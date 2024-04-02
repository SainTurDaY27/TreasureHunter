using System;
using System.Collections.Generic;
using UnityEngine;

namespace TreasureHunter.Gameplay.System.DynamicDifficulty
{
    [Serializable]
    public class DynamicCondition
    {
        public bool useSkillCondition = true;
        public bool useTreasureCondition = false;
        public List<SkillKey> skillConditions;
        public int requiredTreasure = 0;
        // public GameObject enemy;
    }
}