using System;
using TreasureHunter.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.UI
{
    public class TutorialPanel : MonoBehaviour, IBaseUI
    {
        [SerializeField]
        private Button _mainMenuButton;

        [SerializeField]
        private Button _backToGameButton;

        [SerializeField]
        private GameObject _popUpPanel;

        [SerializeField]
        private Button _backPopUpButton;

        [SerializeField]
        private Button _confirmPopUpButton;

        public event Action OnMainMenuButtonClicked;
        public event Action OnBackToGameButtonClicked;
        public event Action OnConfirmPopUpButtonClicked;
        public event Action OnBackPopUpButtonClicked;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnMainMenuButtonClickedHandler()
        {
            OnMainMenuButtonClicked?.Invoke();
        }

        private void OnBackToGameButtonClickedHandler()
        {
            OnBackToGameButtonClicked?.Invoke();
        }

        private void OnConfirmPopUpButtonClickedHandler()
        {
            OnConfirmPopUpButtonClicked?.Invoke();
        }

        private void OnBackPopUpButtonClickedHandler()
        {
            OnBackPopUpButtonClicked?.Invoke();
        }

        private void Awake()
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClickedHandler);
            _backToGameButton.onClick.AddListener(OnBackToGameButtonClickedHandler);
            _confirmPopUpButton.onClick.AddListener(OnConfirmPopUpButtonClickedHandler);
            _backPopUpButton.onClick.AddListener(OnBackPopUpButtonClickedHandler);
        }

        private void OnDestroy()
        {
            _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClickedHandler);
            _backToGameButton.onClick.RemoveListener(OnBackToGameButtonClickedHandler);
            _confirmPopUpButton.onClick.RemoveListener(OnConfirmPopUpButtonClickedHandler);
            _backPopUpButton.onClick.RemoveListener(OnBackPopUpButtonClickedHandler);
        }

        public void SetTutorialPanelButtonInteractable(bool isInteractable)
        {
            _mainMenuButton.interactable = isInteractable;
            _backToGameButton.interactable = isInteractable;
        }

        public void SetPopUpPanelActive(bool isActive)
        {
            _popUpPanel.SetActive(isActive);
        }
    }
}