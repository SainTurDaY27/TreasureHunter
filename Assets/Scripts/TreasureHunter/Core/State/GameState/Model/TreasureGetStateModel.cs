using TMPro;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.System;
using TreasureHunter.Gameplay.UI;
using UnityEngine;

namespace TreasureHunter.Core.State.GameState
{
    public class TreasureGetStateModel : StateModel
    {
        private TreasureGetPanel _treasureGetPanel;
        private GameManager _gameManager;
        private int _treasureCount;
        private const int AnimationScale = 3;
        private const float AnimationTime = 0.5f;

        public TreasureGetStateModel() : base((int)GameStates.State.TreasureGet, nameof(TreasureGetStateModel))
        {
            GameStateManager.Instance.AddTransition(
                new StateTransition(fromState: StateID, toState: (int)GameStates.State.Game));
            GameStateManager.Instance.AddTransition(
                new StateTransition(fromState: StateID, toState: (int)GameStates.State.End));
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn(args);
            _gameManager = GameManager.Instance;
            _treasureGetPanel = (TreasureGetPanel)UIManager.Instance.Show(UIKey.TreasureGet);
            _treasureGetPanel.continueButton.onClick.AddListener(Continue);

            _treasureCount = DataManager.Instance.GameData.TreasureCount;
            var maxTreasure = GameData.MaxTreasure;
            if (_treasureCount == maxTreasure)
            {
                _treasureGetPanel.buttonText.text = "End Game";
            }
            for (int i = _treasureCount; i < maxTreasure; i++)
            {
                _treasureGetPanel.treasureImages[i].color = Color.black;
            }

            // Animate
            var animatedImageTransform = _treasureGetPanel.treasureImages[_treasureCount - 1].rectTransform;
            var localScale = animatedImageTransform.localScale;
            localScale = new Vector3(localScale.x * AnimationScale,
                localScale.y * AnimationScale);
            animatedImageTransform.localScale = localScale;
            LeanTween.scale(animatedImageTransform,
                    new Vector3(localScale.x / AnimationScale, localScale.y / AnimationScale), AnimationTime)
                .setIgnoreTimeScale(true).setEaseInSine();


            // Pause the game
            _gameManager.PauseGame(true);


            Debug.Log($"Player now has {DataManager.Instance.GameData.TreasureCount} treasure(s).");
        }

        private void Continue()
        {
            if (_treasureCount == GameData.MaxTreasure)
            {
                GameStateManager.Instance.GoToState((int)GameStates.State.End);
            }
            else
            {
                GameStateManager.Instance.GoToState((int)GameStates.State.Game);
            }
        }

        public override void OnStateOut()
        {
            base.OnStateOut();

            // Unpause the game
            _gameManager.PauseGame(false);
            _treasureGetPanel.continueButton.onClick.RemoveListener(Continue);
            foreach (var image in _treasureGetPanel.treasureImages)
            {
                image.color = Color.white;
            }

            UIManager.Instance.Hide(UIKey.TreasureGet);
        }
    }
}