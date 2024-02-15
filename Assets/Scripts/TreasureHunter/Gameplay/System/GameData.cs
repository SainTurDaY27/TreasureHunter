using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class GameData
    {

        public static readonly int MaxTreasure = 3;

        // Collected treasure will have unique ID
        private HashSet<string> collectedTreasures = new ();

        public int TreasureCount => collectedTreasures.Count;

        public void CollectTreasure(string treasureId)
        {
            collectedTreasures.Add(treasureId);
        }

        public bool IsTreasureCollected(string treasureId)
        {
            return collectedTreasures.Contains(treasureId);
        }
        
    }
}