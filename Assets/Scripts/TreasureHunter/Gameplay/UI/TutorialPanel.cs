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

        public event Action OnMainMenuButtonClicked;
        public event Action OnBackToGameButtonClicked;

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

        private void Awake()
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClickedHandler);
            _backToGameButton.onClick.AddListener(OnBackToGameButtonClickedHandler);
        }

        private void OnDestroy()
        {
            _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClickedHandler);
            _backToGameButton.onClick.RemoveListener(OnBackToGameButtonClickedHandler);
        }
    }
}