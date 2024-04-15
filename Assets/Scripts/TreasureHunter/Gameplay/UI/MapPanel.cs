using System;
using System.Collections.Generic;
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
        [SerializeField] private GameObject _mapAreaPanel;

        [SerializeField] private MapBlockUI[] _mapBlockUIs;

        [SerializeField] private MapArrowUI[] _mapArrowUIs;

        [SerializeField] private GameObject _mapMarkerPrefab;

        [SerializeField] private Color _normalMapBlockTextColor;

        [SerializeField] private Color _currentMapBlockTextColor;

        [SerializeField] private TextMeshProUGUI _markerRemainingText;

        [SerializeField] private Image[] _treasureImageUIs;

        [Serializable]
        protected class MapArrowUI
        {
            [SerializeField] private MapAreaKey _mapAreaKey;

            [SerializeField] private GameObject _arrowUI;

            public MapAreaKey MapAreaKey => _mapAreaKey;
            public GameObject ArrowUI => _arrowUI;
        }

        private List<GameObject> _mapMarkers = new();
        private DataManager _dataManager;
        public event Action<Vector2> OnMapMarkerPlaced;
        public event Action<Vector2> OnMapMarkerRemoved;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void UpdateMapMarkerData()
        {
            DataManager.Instance.GameData.ClearMapMarkerData();
            foreach (var mapMarker in _mapMarkers)
            {
                if (mapMarker != null)
                {
                    Debug.Log("MapMarker: " + mapMarker);
                    // TODO: Add map marker data.
                    // _dataManager.GameData.AddMapMarkerData(mapMarker, mapMarker.transform.position);
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
                PlaceMapMarker(mapMarkerData, modifyData: false);
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
                mapBlockUI.ChangeMapNameTextColor(_normalMapBlockTextColor);
            }

            foreach (var mapArrowUI in _mapArrowUIs)
            {
                mapArrowUI.ArrowUI.SetActive(false);
            }
        }

        public void ResetAllMarkers()
        {
            // Destroy all GUI map marker
            _mapMarkers.ForEach(Destroy);
            _mapMarkers.Clear();
        }

        public void UpdateMapUI(List<MapAreaKey> exploredMapAreas)
        {
            ResetMapUI();
            for (int i = 0; i < _mapBlockUIs.Length; i++)
            {
                if (exploredMapAreas.Contains(_mapBlockUIs[i].MapAreaKey))
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
            for (int i = 0; i < _mapMarkers.Count; i++)
            {
                if (_mapMarkers[i] != null)
                {
                    Destroy(_mapMarkers[i]);
                    _dataManager.GameData.GainMapMarker(_mapMarkers[i].transform.position);
                }
            }
        }

        private void PlaceMapMarker(Vector3 position, bool modifyData = true)
        {
            var mapMarker = Instantiate(_mapMarkerPrefab, _mapAreaPanel.transform);
            mapMarker.transform.position = position;
            // GUI
            _mapMarkers.Add(mapMarker);
            var component = mapMarker.GetComponent<MapMarker>();
            component.OnMapMarkerRemoved += OnMapMarkerRemoved;
            // Actual data
            if (modifyData) OnMapMarkerPlaced?.Invoke(position);
            SetMapMarkerRemaining(_dataManager.GameData.RemainingMapMarker);
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