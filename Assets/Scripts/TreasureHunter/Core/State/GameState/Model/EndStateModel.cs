using TreasureHunter.Core.Data;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.UI;

namespace TreasureHunter.Core.State.GameState
{
    public class EndStateModel : StateModel
    {
        private EndGamePanel _endGamePanel;
        private MapPanel _mapPanel;
        private DataManager _dataManager;
        private EndGameType _endGameType;

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
            _endGameType = EndGameType.Lose;
            if (args.Length > 0)
            {
                _endGameType = (EndGameType)args[0];
            }

            _endGamePanel = (EndGamePanel)UIManager.Instance.Show(UIKey.EndGame);
            if (UIManager.Instance.TryGetUIByKey(UIKey.Map, out IBaseUI ui) && (ui is MapPanel panel))
            {
                _mapPanel = panel;
            }
            _dataManager = DataManager.Instance;
            _endGamePanel.OnMainMenuButtonClicked += MainMenu;
            SetEndGameDisplayText();

            // TODO: Change remove map marker logic when save system was implemented
            _mapPanel.RemoveAllMapMarker();

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

        private void SetEndGameDisplayText()
        {
            if (_endGameType == EndGameType.Win)
            {
                _endGamePanel.SetDisplayText("Congratulations! You are now so rich.");
            }
            else
            {
                _endGamePanel.SetDisplayText("Game Over. You are dead.");
            }
        }
    }

    public enum EndGameType
    {
        Win,
        Lose
    }
}