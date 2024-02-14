using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.UI;
using UnityEngine;

namespace TreasureHunter.Core.State.GameState
{
    public class TreasureGetStateModel : StateModel
    {
        private TreasureGetPanel _treasureGetPanel;

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
            
            // Pause the game
            Time.timeScale = 0;
            
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
            UIManager.Instance.Hide(UIKey.TreasureGet);
        }
    }
}