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
        private const int AnimationScale = 3;
        private const float AnimationTime = 0.5f;

        public TreasureGetStateModel() : base((int)GameStates.State.TreasureGet, nameof(TreasureGetStateModel))
        {
            GameStateManager.Instance.AddTransition(
                new StateTransition(fromState: StateID, toState: (int)GameStates.State.Game));
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn(args);

            // TODO: Show treasure
            _treasureGetPanel = (TreasureGetPanel)UIManager.Instance.Show(UIKey.TreasureGet);
            _treasureGetPanel.continueButton.onClick.AddListener(Continue);

            var treasureCount = DataManager.Instance.GameData.TreasureCount;
            var maxTreasure = GameData.MaxTreasure;
            for (int i = treasureCount; i < 3; i++)
            {
                _treasureGetPanel.treasureImages[i].color = Color.black;
            }

            // Animate
            var animatedImageTransform = _treasureGetPanel.treasureImages[treasureCount - 1].rectTransform;
            var localScale = animatedImageTransform.localScale;
            localScale = new Vector3(localScale.x * AnimationScale,
                localScale.y * AnimationScale);
            animatedImageTransform.localScale = localScale;
            LeanTween.scale(animatedImageTransform,
                    new Vector3(localScale.x / AnimationScale, localScale.y / AnimationScale), AnimationTime)
                .setIgnoreTimeScale(true).setEaseInSine();


            // Pause the game
            Time.timeScale = 0;

            Debug.Log($"Player now has {DataManager.Instance.GameData.TreasureCount} treasure(s).");

            // Would be nice if there is any animation
        }

        private void Continue()
        {
            // TODO: End the game if all treasures are acquired.
            GameStateManager.Instance.GoToState((int)GameStates.State.Game);
        }

        public override void OnStateOut()
        {
            base.OnStateOut();

            // Unpause the game
            Time.timeScale = 1f;
            _treasureGetPanel.continueButton.onClick.RemoveListener(Continue);
            foreach (var image in _treasureGetPanel.treasureImages)
            {
                image.color = Color.white;
            }

            UIManager.Instance.Hide(UIKey.TreasureGet);
        }
    }
}