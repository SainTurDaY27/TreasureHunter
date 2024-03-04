using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.UI
{
    public class SaveGameSlotUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _savedLevelNameText;

        [SerializeField]
        private TextMeshProUGUI _lastPlayDateText;

        [SerializeField]
        private TextMeshProUGUI _playTimeText;

        [SerializeField]
        private Image[] _skillSlotImages;

        public void SetSavedLevelName(string levelName)
        {
            _savedLevelNameText.text = levelName;
        }

        public void SetLastPlayDate(string lastPlayDate)
        {
            _lastPlayDateText.text = lastPlayDate;
        }

        public void SetPlayTime(string playTime)
        {
            _playTimeText.text = playTime;
        }

        public void SetSkillSlotImage(int index, Sprite skillSprite)
        {
            _skillSlotImages[index].sprite = skillSprite;
        }

        public void SetSkillSlotImageActive(int index, bool isActive)
        {
            _skillSlotImages[index].gameObject.SetActive(isActive);
        }
    }
}