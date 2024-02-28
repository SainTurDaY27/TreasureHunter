using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.Map;
using UnityEngine;

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

        // TODO: Treasure UI panel

        private GameObject[] _mapMarkerPrefabs;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetMapMarkerRemaining(int remaining)
        {
            // TODO: move limit number to other classes - maybe constant in data class
            _markerRemainingText.text = remaining.ToString() + " / 6";
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
            Instantiate(_mapMarkerPrefab, position, Quaternion.identity);
            // TODO: decrease remaining marker
        }

        private void RemoveMapMarker()
        {
            
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
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
                        Debug.Log("Marker removed");
                    }
                    else
                    {
                        PlaceMapMarker(hit.point);
                        Debug.Log("Marker placed");
                    }
                }
            }
        }
    }
}