using TreasureHunter.Core.Scene;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.UI;
using UnityEngine;

namespace TreasureHunter.Core.State.GameState
{
    public class MenuStateModel : StateModel
    {
        private MenuPanel _menuPanel;

        public MenuStateModel() : base((int)GameStates.State.Menu, nameof(MenuStateModel))
        {
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.AbilitySelection));
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.Game));
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.End));
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn();
            Debug.Log("GameSceneManager : " + GameSceneManager.Instance);
            GameSceneManager.Instance.GoToScene(SceneKey.MENU, () =>
            {
                _menuPanel = (MenuPanel)UIManager.Instance.Show(UIKey.Menu);
                _menuPanel.OnPlayButtonClicked += PlayNewGame;
                _menuPanel.OnLoadButtonClicked += LoadGame;
                _menuPanel.OnExitButtonClicked += ExitGame;
            });
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            UIManager.Instance.Hide(UIKey.Menu);
            _menuPanel.OnPlayButtonClicked -= PlayNewGame;
            _menuPanel.OnLoadButtonClicked -= LoadGame;
            _menuPanel.OnExitButtonClicked -= ExitGame;
        }

        private void PlayNewGame()
        {
            GameStateManager.Instance.GoToState((int)GameStates.State.AbilitySelection);
        }

        private void LoadGame()
        {
            // TODO: Implement later
        }

        private void ExitGame()
        {
            Application.Quit();
        }
    }
}