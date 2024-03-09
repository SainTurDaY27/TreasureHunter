using System;
using System.Collections.Generic;
using TreasureHunter.Gameplay.Map;

namespace TreasureHunter.Gameplay.System
{
    public class GameData
    {
        public static readonly int MaxTreasure = 3;
        public static readonly int MaxMapMarker = 6;

        // Collected treasure will have unique ID
        private HashSet<string> collectedTreasures = new();
        private Dictionary<string, bool> boolStates = new();
        private MapAreaKey[] _exploredMapAreas;

        // TODO: Set this variable later
        private MapAreaKey _currentMapArea = MapAreaKey.TheEntrace;

        // TODO: Config this later -> move to config data file
        private int remainingMapMarker = 6;

        public int TreasureCount => collectedTreasures.Count;
        public int MapMarker => remainingMapMarker;
        public MapAreaKey[] ExploredMapAreas => _exploredMapAreas;
        public MapAreaKey CurrentMapArea => _currentMapArea;

        public bool IsMouseOverMapMarker = false;
        public event Action OnMapMarkerChanged;
        public event Action OnMapAreaExplored;

        public void ResetData()
        {
            collectedTreasures.Clear();
            boolStates.Clear();
            _exploredMapAreas = null;
            _currentMapArea = MapAreaKey.TheEntrace;
            remainingMapMarker = MaxMapMarker;
        }

        public void ExploreNewMapArea(MapAreaKey mapAreaKey)
        {
            if (_exploredMapAreas == null)
            {
                _exploredMapAreas = new MapAreaKey[1];
                _exploredMapAreas[0] = mapAreaKey;
            }
            else
            {
                if (Array.Exists(_exploredMapAreas, element => element == mapAreaKey))
                {
                    return;
                }
                Array.Resize(ref _exploredMapAreas, _exploredMapAreas.Length + 1);
                _exploredMapAreas[_exploredMapAreas.Length - 1] = mapAreaKey;
            }
            OnMapAreaExplored?.Invoke();
        }

        public void SetCurrentMapArea(MapAreaKey mapAreaKey)
        {
            _currentMapArea = mapAreaKey;
        }

        public void SetMouseOverMapMarker(bool value)
        {
            IsMouseOverMapMarker = value;
        }

        public void CollectTreasure(string treasureId)
        {
            collectedTreasures.Add(treasureId);
        }

        public bool IsTreasureCollected(string treasureId)
        {
            return collectedTreasures.Contains(treasureId);
        }

        public void UseMapMarker()
        {
            if (CheckMapMarkerAvailable())
            {
                remainingMapMarker--;
            }
            OnMapMarkerChangedHandler();
        }

        public void GainMapMarker()
        {
            remainingMapMarker++;
            OnMapMarkerChangedHandler();
        }

        public int GetRemainingMapMarker()
        {
            return remainingMapMarker;
        }

        public bool CheckMapMarkerAvailable()
        {
            return remainingMapMarker > 0;
        }

        public bool GetBoolState(string stateId, out bool result)
        {
            return boolStates.TryGetValue(stateId, out result);
        }

        public void SetBoolState(string stateId, bool value)
        {
            boolStates[stateId] = value;
        }

        private void OnMapMarkerChangedHandler()
        {
            OnMapMarkerChanged?.Invoke();
        }
    }
}