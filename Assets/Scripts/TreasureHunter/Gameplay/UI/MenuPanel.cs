using System;
using System.Collections;
using System.Collections.Generic;
using TreasureHunter.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.UI
{
    public class MenuPanel : MonoBehaviour, IBaseUI
    {
        [SerializeField]
        private Button _playButton;

        [SerializeField] 
        private Button _loadButton;

        [SerializeField]
        private Button _exitButton;

        public event Action OnPlayButtonClicked;
        public event Action OnLoadButtonClicked;
        public event Action OnExitButtonClicked;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void Awake()
        {
            _playButton.onClick.AddListener(OnPlayButtonClickedHandler);
            _loadButton.onClick.AddListener(OnLoadButtonClickedHandler);
            _exitButton.onClick.AddListener(OnExitButtonClickedHandler);
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClickedHandler);
            _loadButton.onClick.RemoveListener(OnLoadButtonClickedHandler);
            _exitButton.onClick.RemoveListener(OnExitButtonClickedHandler);
        }
        
        private void OnPlayButtonClickedHandler()
        {
            OnPlayButtonClicked?.Invoke();
        }

        private void OnLoadButtonClickedHandler()
        {
            OnLoadButtonClicked?.Invoke();
        }

        private void OnExitButtonClickedHandler()
        {
            OnExitButtonClicked?.Invoke();
        }
    }
}