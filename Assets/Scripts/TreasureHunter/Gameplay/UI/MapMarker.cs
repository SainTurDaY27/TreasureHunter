using TreasureHunter.Core.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TreasureHunter.Gameplay.UI
{
    public class MapMarker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool _isMouseOver = false;
        private DataManager _dataManager;

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
                    Destroy(gameObject);
                    _dataManager.GameData.SetMouseOverMapMarker(false);
                    _dataManager.GameData.GainMapMarker();
                }
            }
        }
    }
}