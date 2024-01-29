using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.UI;
using UnityEngine;

namespace TreasureHunter.Core.State.GameState
{
    public class AbilityPickupStateModel : StateModel

    {

        private AbilityGetPanel _abilityGetPanel;
        public AbilityPickupStateModel(int stateID, string stateName) : base((int)GameStates.State.AbilityPickup, nameof(AbilityPickupStateModel))
        {
            GameStateManager.Instance.AddTransition(new StateTransition(
            fromState: StateID,
            toState: (int)GameStates.State.Game));
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn(args);
            
            // Pause the game.
            Time.timeScale = 0;
            // TODO: Set text and image
            _abilityGetPanel = (AbilityGetPanel) UIManager.Instance.Show(UIKey.AbilityGet);
            
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            
            // Unpause the game
            // There is no slow down or speed up in this game.
            Time.timeScale = 1f;

            UIManager.Instance.Hide(UIKey.AbilityGet);
        }

        public void Continue()
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.Game);
        }
    }
}