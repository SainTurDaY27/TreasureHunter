using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.UI;

namespace TreasureHunter.Core.State.GameState
{
    public class LoadGameStateModel : StateModel
    {
        private LoadGamePanel _loadGamePanel;

        public LoadGameStateModel() : base((int)GameStates.State.LoadGame, nameof(LoadGameStateModel))
        {
            GameStateManager.Instance.AddTransition(new StateTransition(
            fromState: StateID,
            toState: (int)GameStates.State.Game));
            GameStateManager.Instance.AddTransition(new StateTransition(
            fromState: StateID,
            toState: (int)GameStates.State.Menu));
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn(args);
            _loadGamePanel = (LoadGamePanel)UIManager.Instance.Show(UIKey.LoadGame);
            _loadGamePanel.SetSavedGameDataUI();
            _loadGamePanel.OnPlayButtonClicked += PlayLoadGame;
            _loadGamePanel.OnBackButtonClicked += BackToMenu;
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            UIManager.Instance.Hide(UIKey.LoadGame);
            _loadGamePanel.OnPlayButtonClicked -= PlayLoadGame;
            _loadGamePanel.OnBackButtonClicked -= BackToMenu;
        }

        private void PlayLoadGame()
        {
            // TODO: Implement load game functionality - Maybe in GameStateModel
            GameStateManager.Instance.GoToState((int)GameStates.State.Game, false);
        }

        private void BackToMenu()
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.Menu);
        }
    }
}