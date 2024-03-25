using TMPro;
using TreasureHunter.Core.Data;
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

        [SerializeField]
        private SaveGameSlot _saveGameSlot;

        [SerializeField]
        private Button _deleteSaveButton;

        public Image[] SkillSlotImages => _skillSlotImages;

        public void SetSavedLevelName(string levelName)
        {
            _savedLevelNameText.text = levelName;
        }

        public void SetLastPlayDate(string lastPlayDate)
        {
            _lastPlayDateText.text = lastPlayDate;
        }

        //public void SetPlayTime(string playTime)
        //{
        //    _playTimeText.text = playTime;
        //}

        public void SetSkillSlotImage(int index, Sprite skillSprite)
        {
            _skillSlotImages[index].sprite = skillSprite;
        }
        
        public void SetSkillSlotImageActive(int index, bool isActive)
        {
            _skillSlotImages[index].gameObject.SetActive(isActive);
        }

        public void SetDeleteButtonActive(bool isActive)
        {
            _deleteSaveButton.gameObject.SetActive(isActive);
        }

        public SaveGameSlot GetSaveGameSlot()
        {
            return _saveGameSlot;
        }

        private void Awake()
        {
            _deleteSaveButton.onClick.AddListener(DeleteSaveSlot);
        }

        private void OnDestroy()
        {
            _deleteSaveButton.onClick.RemoveListener(DeleteSaveSlot);
        }

        private void DeleteSaveSlot()
        {
            DataManager.Instance.GameSaveManager.DeleteSaveGame(_saveGameSlot);
        }
    }
}