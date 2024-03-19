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
        public List<SkillKey> obtainedSkill;
        public long lastPlayedTime;

        // HashSet is not serializable.
        public List<string> collectTreasures = new();

        public List<MapAreaKey> exploredMapArea = new();
        public List<Vector2> mapMarkerData = new();
        public int remainingMapMarker;

        public MapAreaKey GetMapAreaKey()
        {
            return currentMapArea;
        }

        public List<SkillKey> GetObtainedSkills()
        {
            // debug all elements in the list
            //foreach (var skill in ObtainedSkill)
            //{
            //    Debug.Log("Obtained skill: " + skill);
            //}
            return obtainedSkill;
        }

        public long GetLastPlayedTime()
        {
            return lastPlayedTime;
        }

        public List<string> GetCollectedTreasures()
        {
            foreach (var treasure in collectTreasures)
            {
                Debug.Log("Treasure collected: " + treasure);
            }

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

            //foreach (var markerData in MapMarkerData)
            //{
            //    if (markerData != null)
            //    {
            //        Debug.Log("Map marker data: " + markerData);
            //    }
            //}
            return mapMarkerData;
        }

        public int GetRemainingMapMarker()
        {
            return remainingMapMarker;
        }
    }
}