using System;

namespace TreasureHunter.Gameplay.System.DynamicDifficulty
{
    [Serializable]
    public struct SkillScoreWeight
    {
        public float weight;
        public SkillKey skill;
    }
}