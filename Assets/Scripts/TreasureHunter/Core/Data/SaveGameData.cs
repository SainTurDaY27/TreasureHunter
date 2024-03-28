using System.Collections.Generic;
using TreasureHunter.Gameplay.Map;
using TreasureHunter.Gameplay.System;
using UnityEngine;

namespace TreasureHunter.Core.Data
{
    /// <summary>
    /// This file is for JSON save and load system.
    /// </summary>
    public class SaveGameData
    {
        public MapAreaKey currentMapArea;
        public Vector2 playerPosition;
        public List<SkillKey> obtainedSkill;
        public long lastPlayedTime;

        // HashSet is not serializable.
        public List<string> collectTreasures = new();
        // JsonUtility does not support dictionary. I am very sad.
        // At least, storing anything evaluated to true should be enough.
        public List<string> trueBoolStates = new();

        public List<MapAreaKey> exploredMapArea = new();
        public List<Vector2> mapMarkerData = new();
        public int remainingMapMarker;

        public MapAreaKey GetMapAreaKey()
        {
            return currentMapArea;
        }

        public Vector2 GetPlayerPosition()
        {
            return playerPosition;
        }

        public List<SkillKey> GetObtainedSkills()
        {
            return obtainedSkill;
        }

        public long GetLastPlayedTime()
        {
            return lastPlayedTime;
        }

        public List<string> GetCollectedTreasures()
        {
            return collectTreasures;
        }

        public List<MapAreaKey> GetExploredMapAreas()
        {
            foreach (var mapAreaKey in exploredMapArea)
            {
                Debug.Log("Explored map area: " + mapAreaKey);
            }

            return exploredMapArea;
        }

        public List<Vector2> GetMapMarkerData()
        {
            if (mapMarkerData == null)
            {
                return new List<Vector2>();
            }
            return mapMarkerData;
        }


        public int GetRemainingMapMarker()
        {
            return remainingMapMarker;
        }
    }
}