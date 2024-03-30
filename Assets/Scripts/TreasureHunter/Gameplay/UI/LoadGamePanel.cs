using System;
using System.Linq;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.UI
{
    public class LoadGamePanel : MonoBehaviour, IBaseUI
    {
        [SerializeField]
        private SaveGameSlotUI[] _saveGameSlotUIs;

        [SerializeField]
        private Button[] _savedGameSelectionButtons;

        [SerializeField]
        private Color unselectedColor, selectedColor;

        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private Button _backButton;

        private Button[] selectedButtons = { };

        public event Action OnPlayButtonClicked;
        public event Action OnBackButtonClicked;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnPlayButtonClickedHandler()
        {
            OnPlayButtonClicked?.Invoke();
        }

        public void OnBackButtonClickedHandler()
        {
            OnBackButtonClicked?.Invoke();
        }

        private void Awake()
        {
            _playButton.onClick.AddListener(OnPlayButtonClickedHandler);
            _backButton.onClick.AddListener(OnBackButtonClickedHandler);
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClickedHandler);
            _backButton.onClick.RemoveListener(OnBackButtonClickedHandler);
        }

        private void Update()
        {
            if (selectedButtons.Count() == 1)
            {
                _playButton.interactable = true;
            }
            else
            {
                _playButton.interactable = false;
            }
        }

        private void SelectButton(Button button)
        {
            selectedButtons = selectedButtons.Append(button).ToArray();
            var image = button.GetComponent<Image>();
            image.color = selectedColor;
            var outline = button.GetComponent<Outline>();
            outline.enabled = true;
        }

        private void DeselectButton(Button button)
        {
            selectedButtons = selectedButtons.Where(b => b != button).ToArray();
            var image = button.GetComponent<Image>();
            image.color = unselectedColor;
            var outline = button.GetComponent<Outline>();
            outline.enabled = false;
        }

        public void DeselectAllButton()
        {
            foreach (var button in _savedGameSelectionButtons)
            {
                DeselectButton(button);
            }
        }

        public void UpdateSavedGameDataUI()
        {
            for (int i = 0; i < _saveGameSlotUIs.Length; i++)
            {
                var saveGameSlotUI = _saveGameSlotUIs[i];
                var saveGameSlot = saveGameSlotUI.GetSaveGameSlot();
                _savedGameSelectionButtons[i].interactable = true;

                if (DataManager.Instance.GameSaveManager.IsSaveGameExist(saveGameSlot))
                {
                    var saveGameData = DataManager.Instance.GetSavedGameData(saveGameSlot);
                    saveGameSlotUI.SetSavedLevelName(saveGameData.GetMapAreaKey().ToString());
                    saveGameSlotUI.SetLastPlayDate(DateTime.FromFileTime(saveGameData.GetLastPlayedTime()).ToString());
                    saveGameSlotUI.SetDeleteButtonActive(true);
                    var obtainedSkills = saveGameData.GetObtainedSkills();
                    for (int skillSlotImageIndex = 0; skillSlotImageIndex < saveGameSlotUI.SkillSlotImages.Length; skillSlotImageIndex++)
                    {
                        if (skillSlotImageIndex < obtainedSkills.Count)
                        {
                            var skill = obtainedSkills[skillSlotImageIndex];
                            saveGameSlotUI.SetSkillSlotImage(skillSlotImageIndex, DataManager.Instance.SkillIconVisualData.GetSkillIcon(skill));
                            saveGameSlotUI.SetSkillSlotImageActive(skillSlotImageIndex, true);
                        }
                        else
                        {
                            saveGameSlotUI.SetSkillSlotImageActive(skillSlotImageIndex, false);
                        }
                    }
                }
                else
                {
                    saveGameSlotUI.SetSavedLevelName("Empty Slot " + (i + 1).ToString());
                    saveGameSlotUI.SetLastPlayDate("");
                    for (int skillSlotImageIndex = 0; skillSlotImageIndex < saveGameSlotUI.SkillSlotImages.Length; skillSlotImageIndex++)
                    {
                        saveGameSlotUI.SetSkillSlotImageActive(skillSlotImageIndex, false);
                    }
                    saveGameSlotUI.SetDeleteButtonActive(false);
                    _savedGameSelectionButtons[i].interactable = false;
                }
            }
        }

        public void OnButtonClicked(Button clickedButton)
        {

            int buttonIndex = Array.IndexOf(_savedGameSelectionButtons, clickedButton);

            if (buttonIndex == -1)
            {
                return;
            }

            if (selectedButtons.Contains(clickedButton))
            {
                DeselectButton(clickedButton);
            }
            else
            {
                if (selectedButtons.Count() >= 1) return;
                SelectButton(clickedButton);
            }
        }

        public SaveGameSlot GetSelectedSaveGameSlot()
        {
            int buttonIndex = Array.IndexOf(_savedGameSelectionButtons, selectedButtons[0]);
            return (SaveGameSlot)buttonIndex + 1;
        }
    }
}