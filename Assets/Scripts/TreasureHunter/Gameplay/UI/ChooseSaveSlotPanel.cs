using System;
using System.Linq;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.UI
{
    public class ChooseSaveSlotPanel : MonoBehaviour, IBaseUI
    {
        [SerializeField]
        private SaveGameSlotUI[] _saveGameSlotUIs;

        [SerializeField]
        private Button[] _savedGameSelectionButtons;

        [SerializeField]
        private Color unselectedColor, selectedColor;

        [SerializeField]
        private Button _saveButton;

        [SerializeField]
        private Button _backButton;

        private Button[] selectedButtons = {};

        public event Action OnSaveButtonClicked;
        public event Action OnBackButtonClicked;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnSaveButtonClickedHandler()
        {
            OnSaveButtonClicked?.Invoke();
        }

        public void OnBackButtonClickedHandler()
        {
            OnBackButtonClicked?.Invoke();
        }

        private void Awake()
        {
            _saveButton.onClick.AddListener(OnSaveButtonClickedHandler);
            _backButton.onClick.AddListener(OnBackButtonClickedHandler);
        }

        private void OnDestroy()
        {
            _saveButton.onClick.RemoveListener(OnSaveButtonClickedHandler);
            _backButton.onClick.RemoveListener(OnBackButtonClickedHandler);
        }

        private void Update()
        {
            if (selectedButtons.Count() == 1)
            {
                _saveButton.interactable = true;
            }
            else
            {
                _saveButton.interactable = false;
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

        public void SetSavedGameDataUI()
        {
            // TODO: Implement save method first, then implement this method.
            // - Get saved game data from the save method.
            // - Set the text (stage, time, time played) and ability images of the buttons from the saved game data.
            Debug.Log("Set Saved Game Data Soon!");
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
            return (SaveGameSlot)buttonIndex;
        }
    }
}