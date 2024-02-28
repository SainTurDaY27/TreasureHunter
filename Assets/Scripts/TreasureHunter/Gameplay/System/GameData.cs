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
        private Dictionary<string, bool> boolStates = new();

        public int TreasureCount => collectedTreasures.Count;

        public void CollectTreasure(string treasureId)
        {
            collectedTreasures.Add(treasureId);
        }

        public bool IsTreasureCollected(string treasureId)
        {
            return collectedTreasures.Contains(treasureId);
        }

        public bool GetBoolState(string stateId, out bool result)
        {
            return boolStates.TryGetValue(stateId, out result);
        }

        public void SetBoolState(string stateId, bool value)
        {
            boolStates[stateId] = value;
        }
        
    }
}