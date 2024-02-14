using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class GameData
    {
        // Implement later
        private const int MaxTreasure = 3;

        public int TreasureCount { get; private set; } = 0;

        public void ObtainTreasure()
        {
            if (TreasureCount >= MaxTreasure) return;
            TreasureCount++;
        }
    }
}