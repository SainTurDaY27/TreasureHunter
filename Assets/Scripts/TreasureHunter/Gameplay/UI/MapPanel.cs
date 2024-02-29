using System;
using System.Collections;
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

        private GameObject[] _mapMarkerPrefabs;
        
        // TODO: create event for changing of map areas
        public event Action OnMarkerPlaced;
        public event Action OnMarkerRemoved;
        public event Action OnMapAreaChanged;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetMapMarkerRemaining(int remaining)
        {
            // TODO: move limit number to other classes - maybe constant in data class
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

        private void PlaceMapMarker(Vector3 position)
        {
            // Instantiate map marker prefab as child of map area panel
            // Instantiate(_mapMarkerPrefab, position, Quaternion.identity);
            var mapMarker = Instantiate(_mapMarkerPrefab, _mapAreaPanel.transform);
            mapMarker.transform.position = position;
        }

        private void RemoveMapMarker()
        {
            
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse clicked");
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // if hit object with tag "Marker"
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Marker"))
                    {
                        RemoveMapMarker();

                        // TODO: check correctness of the object
                        Destroy(hit.collider.gameObject);
                        DataManager.Instance.GameData.GainMapMarker();
                        Debug.Log("Marker removed");
                    }
                    else
                    {
                        PlaceMapMarker(hit.point);
                        DataManager.Instance.GameData.UseMapMarker();
                        Debug.Log("Marker placed");
                    }
                }
            }
        }
    }
}