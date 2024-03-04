using TMPro;
using TreasureHunter.Gameplay.Map;
using UnityEngine;

namespace TreasureHunter.Gameplay.UI
{
    public class MapBlockUI : MonoBehaviour
    {
        [SerializeField]
        private MapAreaKey _mapAreaKey;

        [SerializeField]
        private TextMeshProUGUI _mapNameText;

        public MapAreaKey MapAreaKey => _mapAreaKey;

        public void SetActiveMapBlockUI(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void ChangeMapNameTextColor(Color color)
        {
            _mapNameText.color = color;
        }
    }
}