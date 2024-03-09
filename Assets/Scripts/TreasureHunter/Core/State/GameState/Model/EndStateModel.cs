using TreasureHunter.Core.Data;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.UI;

namespace TreasureHunter.Core.State.GameState
{
    public class EndStateModel : StateModel
    {
        private EndGamePanel _endGamePanel;
        private DataManager _dataManager;

        public EndStateModel() : base((int)GameStates.State.End, nameof(GameStateModel))
        {
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.Menu));
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.Game));
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn();
            _endGamePanel = (EndGamePanel)UIManager.Instance.Show(UIKey.EndGame);
            _dataManager = DataManager.Instance;
            _endGamePanel.OnMainMenuButtonClicked += MainMenu;
            _dataManager.ResetData();
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            UIManager.Instance.Hide(UIKey.EndGame);
            _endGamePanel.OnMainMenuButtonClicked -= MainMenu;
        }

        private void MainMenu()
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.Menu);
        }
    }
}