using System;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TreasureHunter.Gameplay.UI
{
    public class MapMarker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<MapMarkerData> OnMapMarkerRemoved;
        private bool _isMouseOver = false;
        private DataManager _dataManager;
        private MapPanel _mapPanel;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isMouseOver = true;
            _dataManager.GameData.SetMouseOverMapMarker(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isMouseOver = false;
            _dataManager.GameData.SetMouseOverMapMarker(false);
        }

        private void Awake()
        {
            _dataManager = DataManager.Instance;
        }

        private void Update()
        {
            if (_isMouseOver)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var position = gameObject.transform.position;

                    if (UIManager.Instance.TryGetUIByKey(UIKey.Map, out IBaseUI ui) && (ui is MapPanel panel))
                    {
                        _mapPanel = panel;
                    }

                    OnMapMarkerRemoved?.Invoke(new MapMarkerData(gameObject.transform.position, _mapPanel.CurrentViewDetailedMap));
                    Destroy(gameObject);
                    _dataManager.GameData.SetMouseOverMapMarker(false);
                }
            }
        }
    }
}