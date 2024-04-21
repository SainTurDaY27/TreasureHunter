using System;
using System.Collections.Generic;
using TMPro;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.Map;
using TreasureHunter.Gameplay.Player;
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

        [SerializeField] private GameObject _detailedMapPanel;

        [SerializeField] private Image _detailedMapImage;

        [SerializeField] private TextMeshProUGUI _detailedMapNameText;

        [SerializeField] private GameObject _playerIconPrefab;

        private bool _isMouseOverDetailedMap;
        private GameObject _playerIcon;

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
        private MapAreaKey _currentViewDetailedMap;
        public MapAreaKey CurrentViewDetailedMap => _currentViewDetailedMap;
        public event Action<MapMarkerData> OnMapMarkerPlaced;
        public event Action<MapMarkerData> OnMapMarkerRemoved;

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
                }
            }
        }

        public void LoadMapMarkerFromData()
        {
            var _mapMarkerData = _dataManager.GameData.GetMapMarkerData();
            foreach (var mapMarkerData in _mapMarkerData)
            {
                if (mapMarkerData.mapAreaKey == _currentViewDetailedMap)
                {
                    PlaceMapMarker(mapMarkerData.position, modifyData: false);
                    Debug.Log("Place map marker to " + _currentViewDetailedMap.ToString());
                }
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
            //_mapMarkers.Clear();
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
                    _dataManager.GameData.GainMapMarker(new MapMarkerData(_mapMarkers[i].transform.position, _currentViewDetailedMap));
                }
            }
        }

        public void OpenDetailedMapPanel(MapAreaKey mapAreaKey)
        {
            _currentViewDetailedMap = mapAreaKey;
            _mapAreaPanel.SetActive(false);
            SetDetailedMapImage(_currentViewDetailedMap);
            _detailedMapPanel.SetActive(true);
            _detailedMapNameText.text = _currentViewDetailedMap.ToString();
            RenderPlayerLocationOnMap();
        }

        public void BackToMainMapPanel()
        {
            _detailedMapPanel.SetActive(false);
            _mapAreaPanel.SetActive(true);
            ResetAllMarkers();
            RemovePlayerIcon();
        }

        public void RemovePlayerIcon()
        {
            if (_playerIcon != null)
            {
                Destroy(_playerIcon);
            }
        }

        public void SetIsMouseOverDetailedMap(bool isMouseOverDetailedMap)
        {
            _isMouseOverDetailedMap = isMouseOverDetailedMap;
        }

        private void SetDetailedMapImage(MapAreaKey mapAreaKey)
        {
            _detailedMapImage.sprite = _dataManager.MapVisualData.GetMapImage(mapAreaKey);
        }

        public void RenderPlayerLocationOnMap()
        {
            RemovePlayerIcon();
            if (_dataManager.GameData.CurrentMapArea == _currentViewDetailedMap)
            {
                var mapBorder = GameObject.FindGameObjectWithTag("MapBorder");
                if (mapBorder == null)
                {
                    Debug.Log("Map border not found!");
                    return;
                }
                var player = GameObject.FindObjectOfType<PlayerController>();
                // Player position based on Unity engine
                Vector2 playerPosition = player.transform.position;
                var boxCollider2D = mapBorder.GetComponent<BoxCollider2D>();
                Vector2 mapSize = boxCollider2D.size;
                Vector2 detailedMapSize = _detailedMapImage.rectTransform.sizeDelta;
                // Map position = center of the map
                Vector2 mapPosition = (Vector2) mapBorder.transform.position + boxCollider2D.offset;
                Vector2 detailedMapPosition = _detailedMapImage.rectTransform.anchoredPosition;
                Vector2 relativeMapPosition = playerPosition - mapPosition; 
                var convertedRatio = new Vector2(detailedMapSize.x / mapSize.x, detailedMapSize.y / mapSize.y);
                var playerIconPosition = new Vector2((relativeMapPosition.x ) * convertedRatio.x, (relativeMapPosition.y ) * convertedRatio.y);
                playerIconPosition += _detailedMapImage.rectTransform.anchoredPosition;
                _playerIcon = Instantiate(_playerIconPrefab, _detailedMapPanel.transform);
                // set player icon position on detailed map's image
                _playerIcon.transform.localPosition = playerIconPosition;
            }
            LoadMapMarkerFromData();
        }

        private void PlaceMapMarker(Vector3 position, bool modifyData = true)
        {
            var mapMarker = Instantiate(_mapMarkerPrefab, _detailedMapPanel.transform);
            mapMarker.transform.position = position;
            // GUI
            _mapMarkers.Add(mapMarker);
            var component = mapMarker.GetComponent<MapMarker>();
            component.OnMapMarkerRemoved += OnMapMarkerRemoved;
            // Actual data
            if (modifyData) OnMapMarkerPlaced?.Invoke(new MapMarkerData(position, _currentViewDetailedMap));
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
                
                if (!_isMouseOverDetailedMap)
                {
                    return;
                }

                if (_dataManager.GameData.CheckMapMarkerAvailable() && _detailedMapPanel.activeSelf)
                {
                    PlaceMapMarker(Input.mousePosition);
                }
            }
        }
    }
}