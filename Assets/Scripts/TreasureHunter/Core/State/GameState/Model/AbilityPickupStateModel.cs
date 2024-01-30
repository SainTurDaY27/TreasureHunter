using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.UI;
using UnityEngine;

namespace TreasureHunter.Core.State.GameState
{
    public class AbilityPickupStateModel : StateModel

    {

        private AbilityGetPanel _abilityGetPanel;
        public AbilityPickupStateModel() : base((int)GameStates.State.SkillPickup, nameof(AbilityPickupStateModel))
        {
            GameStateManager.Instance.AddTransition(new StateTransition(
            fromState: StateID,
            toState: (int)GameStates.State.Game));
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn(args);
            
            // If args are invalid just do nothing.

            // If there is a better method, please fix.
            var abilityName = (string) args[0];
            var abilityToolTip = (string)args[1];
            var abilitySprite = (Sprite)args[2];
            
            // Pause the game.
            _abilityGetPanel = (AbilityGetPanel) UIManager.Instance.Show(UIKey.AbilityGet);
            _abilityGetPanel.continueButton.onClick.AddListener(Continue);
            _abilityGetPanel.uiText.text = abilityName;
            _abilityGetPanel.uiToolTip.text = abilityToolTip;
            _abilityGetPanel.uiImage.sprite = abilitySprite;
                
            Time.timeScale = 0;
            // TODO: Set text and image
            
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            
            // Unpause the game
            // There is no slow down or speed up in this game.
            Time.timeScale = 1f;
            _abilityGetPanel.continueButton.onClick.RemoveListener(Continue);
            UIManager.Instance.Hide(UIKey.AbilityGet);
        }

        private void Continue()
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.Game);
        }
    }
}