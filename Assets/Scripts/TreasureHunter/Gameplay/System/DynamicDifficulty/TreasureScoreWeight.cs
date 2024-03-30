using System;

namespace TreasureHunter.Gameplay.System.DynamicDifficulty
{
    [Serializable]
    public struct TreasureScoreWeight
    {
        public int amountOfTreasureObtained;
        public float weight;
    }
}