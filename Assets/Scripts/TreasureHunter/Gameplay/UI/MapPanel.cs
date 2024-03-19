using System;
using TMPro;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.Map;
using TreasureHunter.Gameplay.System;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.UI
{
    public class MapPanel : MonoBehaviour, IBaseUI
    {
        [SerializeField]
        private GameObject _mapAreaPanel;

        [SerializeField]
        private MapBlockUI[] _mapBlockUIs;

        [SerializeField]
        private MapArrowUI[] _mapArrowUIs;

        [SerializeField]
        private GameObject _mapMarkerPrefab;

        [SerializeField]
        private Color _normalMapBlockTextColor;

        [SerializeField]
        private Color _currentMapBlockTextColor;

        [SerializeField]
        private TextMeshProUGUI _markerRemainingText;

        [SerializeField]
        private Image[] _treasureImageUIs;

        [Serializable]
        protected class MapArrowUI
        {
            [SerializeField]
            private MapAreaKey _mapAreaKey;

            [SerializeField]
            private GameObject _arrowUI;

            public MapAreaKey MapAreaKey => _mapAreaKey;
            public GameObject ArrowUI => _arrowUI;
        }

        private GameObject[] _mapMarkers = new GameObject[6];
        private DataManager _dataManager;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void UpdateMapMarkerData()
        {
            _dataManager.GameData.ClearMapMarkerData();
            foreach (var mapMarker in _mapMarkers)
            {
                if (mapMarker != null)
                {
                    Debug.Log("MapMarker: " + mapMarker);
                    _dataManager.GameData.AddMapMarkerData(mapMarker, mapMarker.transform.position);
                }
                //Debug.Log("MapMarker: " + mapMarker);
                //_dataManager.GameData.AddMapMarkerData(mapMarker, mapMarker.transform.position);
            }
        }

        public void LoadMapMarkerFromData()
        {
            var _mapMarkerData = _dataManager.GameData.GetMapMarkerData();
            foreach (var mapMarkerData in _mapMarkerData)
            {
                PlaceMapMarker(mapMarkerData.GetPosition());
            }
        }

        public void SetMapMarkerRemaining(int remaining)
        {
            _markerRemainingText.text = remaining.ToString() + $" / {GameData.MaxMapMarker}";
        }

        public void SetTreasureImage(int collectTreasure)
        {
            for (int i = 0; i < _treasureImageUIs.Length; i++)
            {
                if (i < collectTreasure)
                {
                    _treasureImageUIs[i].color = Color.white;
                }
                else
                {
                    _treasureImageUIs[i].color = Color.black;
                }
            }
        }

        public void ResetMapUI()
        {
            foreach (var mapBlockUI in _mapBlockUIs)
            {
                mapBlockUI.SetActiveMapBlockUI(false);
                if (mapBlockUI.MapAreaKey != MapAreaKey.TheSurface)
                {
                    mapBlockUI.ChangeMapNameTextColor(_normalMapBlockTextColor);
                }
            }
            foreach (var mapArrowUI in _mapArrowUIs)
            {
                mapArrowUI.ArrowUI.SetActive(false);
            }
        }

        public void UpdateMapUI(MapAreaKey[] exploredMapAreas)
        {
            ResetMapUI();
            for (int i = 0; i < _mapBlockUIs.Length; i++)
            {
                if (Array.Exists(exploredMapAreas, element => element == _mapBlockUIs[i].MapAreaKey))
                {
                    SetActiveMapBlockUI(_mapBlockUIs[i].MapAreaKey, true);
                    SetActiveArrowUI(_mapBlockUIs[i].MapAreaKey, true);
                    if (_mapBlockUIs[i].MapAreaKey == _dataManager.GameData.CurrentMapArea)
                    {
                        _mapBlockUIs[i].ChangeMapNameTextColor(_currentMapBlockTextColor);
                    }
                }
            }
        }

        public void SetActiveMapBlockUI(MapAreaKey mapAreaKey, bool isActive)
        {
            foreach (var mapBlockUI in _mapBlockUIs)
            {
                if (mapBlockUI.MapAreaKey == mapAreaKey)
                {
                    mapBlockUI.SetActiveMapBlockUI(isActive);
                }
            }
        }

        public void SetActiveArrowUI(MapAreaKey mapAreaKey, bool isActive)
        {
            foreach (var mapArrowUI in _mapArrowUIs)
            {
                if (mapArrowUI.MapAreaKey == mapAreaKey)
                {
                    mapArrowUI.ArrowUI.SetActive(isActive);
                }
            }
        }

        public void RemoveAllMapMarker()
        {
            for (int i = 0; i < _mapMarkers.Length; i++)
            {
                if (_mapMarkers[i] != null)
                {
                    Destroy(_mapMarkers[i]);
                    _dataManager.GameData.GainMapMarker();
                }
            }
        }

        private void PlaceMapMarker(Vector3 position)
        {
            var mapMarker = Instantiate(_mapMarkerPrefab, _mapAreaPanel.transform);
            mapMarker.transform.position = position;

            for (int i = 0; i < _mapMarkers.Length; i++)
            {
                if (_mapMarkers[i] == null)
                {
                    _mapMarkers[i] = mapMarker;
                    break;
                }
            }
            _dataManager.GameData.UseMapMarker();
        }

        private void Awake()
        {
            _dataManager = DataManager.Instance;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_dataManager.GameData.IsMouseOverMapMarker)
                {
                    return;
                }
                if (_dataManager.GameData.CheckMapMarkerAvailable())
                {
                    PlaceMapMarker(Input.mousePosition);
                    //_dataManager.GameData.UseMapMarker();
                }
            }
        }
    }
}