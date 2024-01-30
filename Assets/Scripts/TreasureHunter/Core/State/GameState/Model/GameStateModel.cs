using System.Linq;
using TreasureHunter.Core.Scene;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.UI;

namespace TreasureHunter.Core.State.GameState
{
    public class GameStateModel : StateModel
    {
        private GameHUDPanel _gameHUDPanel;

        public GameStateModel() : base((int)GameStates.State.Game, nameof(GameStateModel))
        {
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.Menu));
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.End));
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.SkillPickup));
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn();
            var startNewGame = false;
            if (args.Length > 0)
            {
                startNewGame = (bool)args[0];
            }

            // TODO: Handle load game
            if (startNewGame)
            {
                GameSceneManager.Instance.GoToScene(SceneKey.THE_ENTRANCE, () =>
                {
                    LoadGameHUD();
                });
            }
            else
            {
                // if not starting a new game. just load UI instance
                LoadGameHUD();
            }
        }

        private void LoadGameHUD()
        {
            if (UIManager.Instance.TryGetUIByKey(UIKey.GameHUD, out IBaseUI ui) && (ui is GameHUDPanel panel))
            {
                _gameHUDPanel = panel;
            }

            _gameHUDPanel.UpdateSkillSlot();

            //_gameHUDPanel = (GameHUDPanel)UIManager.Instance.Show(UIKey.GameHUD);
            UIManager.Instance.Show(UIKey.GameHUD);
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            UIManager.Instance.Hide(UIKey.GameHUD);
        }
    }
}