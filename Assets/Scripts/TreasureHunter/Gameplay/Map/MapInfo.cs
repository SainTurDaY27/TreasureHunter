using System;
using TreasureHunter.Core.Data;
using UnityEngine;

namespace TreasureHunter.Gameplay.Map
{
    public class MapInfo : MonoBehaviour
    {
        public MapAreaKey mapArea;

        private void Start()
        {
            var dataManager = DataManager.Instance;
            
            dataManager.GameData.ExploreNewMapArea(mapArea);
            dataManager.GameData.SetCurrentMapArea(mapArea);
        }
    }
}