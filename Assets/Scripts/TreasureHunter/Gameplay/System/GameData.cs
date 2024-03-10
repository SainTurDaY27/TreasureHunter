using System;
using System.Collections.Generic;
using TreasureHunter.Gameplay.Map;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace TreasureHunter.Gameplay.System
{
    public class GameData
    {
        public static readonly int MaxTreasure = 3;
        public static readonly int MaxMapMarker = 6;

        // Collected treasure will have unique ID
        private HashSet<string> _collectedTreasures = new();
        private Dictionary<string, bool> _boolStates = new();
        private MapAreaKey[] _exploredMapAreas;
        private List<MapMarkerData> _mapMarkerDatas = new();

        public class MapMarkerData
        {
            private GameObject _mapMarkerGameObject { get; set; }
            private Vector3 _position { get; set; }

            public GameObject GetMapMarkerGameObject()
            {
                return _mapMarkerGameObject;
            }

            public Vector3 GetPosition()
            {
                return _position;
            }

            public void SetMapMarkerGameObject(GameObject gameObject)
            {
                _mapMarkerGameObject = gameObject;
            }

            public void SetPosition(Vector3 position)
            {
                _position = position;
            }
        }

        // TODO: Set this variable later
        private MapAreaKey _currentMapArea = MapAreaKey.TheEntrace;

        // TODO: Config this later -> move to config data file
        private int remainingMapMarker = 6;

        public int TreasureCount => _collectedTreasures.Count;
        public int MapMarker => remainingMapMarker;
        public MapAreaKey[] ExploredMapAreas => _exploredMapAreas;
        public MapAreaKey CurrentMapArea => _currentMapArea;

        public bool IsMouseOverMapMarker = false;
        public event Action OnMapMarkerChanged;
        public event Action OnMapAreaExplored;

        public void ResetData()
        {
            _collectedTreasures.Clear();
            _boolStates.Clear();
            _exploredMapAreas = null;
            _currentMapArea = MapAreaKey.TheEntrace;
            remainingMapMarker = MaxMapMarker;
            _mapMarkerDatas.Clear();
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

        public void AddMapMarkerData(GameObject mapMarkerGO, Vector3 position)
        {
            for (int i = 0; i < _mapMarkerDatas.Count; i++)
            {
                if (_mapMarkerDatas[i] == null)
                {
                    _mapMarkerDatas[i].SetMapMarkerGameObject(mapMarkerGO);
                    _mapMarkerDatas[i].SetPosition(position);
                }
            }
        }

        public List<MapMarkerData> GetMapMarkerData()
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

        private void OnMapMarkerChangedHandler()
        {
            OnMapMarkerChanged?.Invoke();
        }
    }
}