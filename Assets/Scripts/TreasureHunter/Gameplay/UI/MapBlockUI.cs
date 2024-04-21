using TMPro;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.Map;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.UI
{
    public class MapBlockUI : MonoBehaviour
    {
        [SerializeField]
        private MapAreaKey _mapAreaKey;

        [SerializeField]
        private TextMeshProUGUI _mapNameText;

        [SerializeField]
        private Button _mapBlockButton;

        [SerializeField]
        private GameObject _mapMarkerImage;

        private MapPanel _mapPanel;

        public MapAreaKey MapAreaKey => _mapAreaKey;

        public void SetActiveMapBlockUI(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetMapMarkerImageActive(bool isActive)
        {
            _mapMarkerImage.SetActive(isActive);
        }

        public void ChangeMapNameTextColor(Color color)
        {
            _mapNameText.color = color;
        }

        private void Awake()
        {
            _mapBlockButton.onClick.AddListener(UpdateDetailedMap);
        }

        private void OnDestroy()
        {
            _mapBlockButton.onClick.RemoveListener(UpdateDetailedMap);
        }

        private void UpdateDetailedMap()
        {
            if (UIManager.Instance.TryGetUIByKey(UIKey.Map, out IBaseUI ui) && (ui is MapPanel panel))
            {
                _mapPanel = panel;
            }
            _mapPanel.OpenDetailedMapPanel(_mapAreaKey);
        }
    }
}