using System;
using System.Collections.Generic;
using System.Linq;
using TreasureHunter.Core.Data;
using TreasureHunter.Gameplay.Map;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class GameData
    {
        public static readonly int MaxTreasure = 3;
        public static readonly int MaxMapMarker = 6;

        // Collected treasure will have unique ID
        private HashSet<string> _collectedTreasures = new();
        private Dictionary<string, bool> _boolStates = new();
        private List<MapAreaKey> _exploredMapAreas = new();
        private List<Vector2> _mapMarkerDatas = new();
        private SaveGameSlot _currentSaveGameSlot;

        // TODO: Set this variable later
        private MapAreaKey _currentMapArea = MapAreaKey.TheEntrance;

        // TODO: Config this later -> move to config data file

        public int TreasureCount => _collectedTreasures.Count;
        public int RemainingMapMarker => MaxMapMarker - (_mapMarkerDatas?.Count ?? 0);
        public List<MapAreaKey> ExploredMapAreas => _exploredMapAreas;
        public MapAreaKey CurrentMapArea => _currentMapArea;

        // date and time when the game is saved
        public DateTime LastPlayedTime;

        public bool IsMouseOverMapMarker = false;
        public event Action OnMapMarkerChanged;
        public event Action OnMapAreaExplored;

        public void ResetData()
        {
            _collectedTreasures.Clear();
            _boolStates.Clear();
            _exploredMapAreas.Clear();
            _currentMapArea = MapAreaKey.TheEntrance;
            _mapMarkerDatas.Clear();
        }

        public void LoadData(SaveGameData saveGameData)
        {
            // Load current map area
            var _currentMapArea = saveGameData.currentMapArea;
            SetCurrentMapArea(_currentMapArea);

            // Load collected treasures
            var collectedTreasures = saveGameData.GetCollectedTreasures();
            foreach (var treasure in collectedTreasures)
            {
                CollectTreasure(treasure);
            }
            
            _boolStates.Clear();
            foreach (var stateId in saveGameData.trueBoolStates)
            {
                _boolStates[stateId] = true;
            }

            // Load explored map areas
            var _exploredMapAreas = saveGameData.GetExploredMapAreas();
            foreach (var mapAreaKey in _exploredMapAreas)
            {
                ExploreNewMapArea(mapAreaKey);
            }

            // Load map marker data
            _mapMarkerDatas = saveGameData.GetMapMarkerData();

            // Load remaining map marker
            SetRemainingMapMarker(RemainingMapMarker);
        }

        public void ExploreNewMapArea(MapAreaKey mapAreaKey)
        {
            if (!_exploredMapAreas.Contains(mapAreaKey))
            {
                _exploredMapAreas.Add(mapAreaKey);
            }

            OnMapAreaExplored?.Invoke();
        }

        public void SetCurrentSaveGameSlot(SaveGameSlot saveGameSlot)
        {
            _currentSaveGameSlot = saveGameSlot;
        }

        public void SetCurrentMapArea(MapAreaKey mapAreaKey)
        {
            _currentMapArea = mapAreaKey;
        }

        public void SetRemainingMapMarker(int value)
        {
        }

        public void SetMouseOverMapMarker(bool value)
        {
            IsMouseOverMapMarker = value;
        }

        public void UpdateLastPlayedTime()
        {
            // update last played time to current real-time
            LastPlayedTime = DateTime.Now;
        }

        public DateTime GetLastPlayedTime()
        {
            UpdateLastPlayedTime();
            return LastPlayedTime;
        }

        public void CollectTreasure(string treasureId)
        {
            _collectedTreasures.Add(treasureId);
        }

        public bool IsTreasureCollected(string treasureId)
        {
            return _collectedTreasures.Contains(treasureId);
        }

        public void UseMapMarker()
        {
            if (CheckMapMarkerAvailable())
            {
            }

            OnMapMarkerChangedHandler();
        }

        public void GainMapMarker(Vector2 mapMarkerLocation)
        {
            RemoveMapMarker(mapMarkerLocation);
            OnMapMarkerChangedHandler();
        }

        public int GetRemainingMapMarker()
        {
            return RemainingMapMarker;
        }

        public SaveGameSlot GetCurrentSaveGameSlot()
        {
            return _currentSaveGameSlot;
        }

        public bool CheckMapMarkerAvailable()
        {
            return RemainingMapMarker > 0;
        }

        public List<Vector2> GetMapMarkerData()
        {
            return _mapMarkerDatas;
        }

        public void ClearMapMarkerData()
        {
            _mapMarkerDatas.Clear();
        }

        public bool GetBoolState(string stateId, out bool result)
        {
            return _boolStates.TryGetValue(stateId, out result);
        }

        public void SetBoolState(string stateId, bool value)
        {
            _boolStates[stateId] = value;
        }

        public List<string> GetTrueBoolStates()
        {
            return _boolStates.Keys.Where(k => _boolStates[k]).ToList();
        }

        private void OnMapMarkerChangedHandler()
        {
            OnMapMarkerChanged?.Invoke();
        }

        public void AddMapMarker(Vector2 mapMarker)
        {
            _mapMarkerDatas.Add(mapMarker);
        }

        public void RemoveMapMarker(Vector2 mapMarker)
        {
            _mapMarkerDatas = _mapMarkerDatas.Where(m => !m.Equals(mapMarker)).ToList();
        }

        public List<string> GetCollectedTreasures()
        {
            return _collectedTreasures.ToList();
        }
    }
}