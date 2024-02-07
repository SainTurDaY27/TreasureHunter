using System;
using TreasureHunter.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.UI
{
    public class EndGamePanel : MonoBehaviour, IBaseUI
    {
        [SerializeField]
        private Button _backToMenuButton;

        public event Action OnMainMenuButtonClicked;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnMainMenuButtonClickedHandler()
        {
            OnMainMenuButtonClicked?.Invoke();
        }

        private void Awake()
        {
            _backToMenuButton.onClick.AddListener(OnMainMenuButtonClickedHandler);
        }

        private void OnDestroy()
        {
            _backToMenuButton.onClick.RemoveListener(OnMainMenuButtonClickedHandler);
        }
    }
}