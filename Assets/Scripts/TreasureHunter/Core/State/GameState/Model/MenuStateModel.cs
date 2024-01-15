using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.UI;

namespace TreasureHunter.Core.State.GameState
{
    public class MenuStateModel : StateModel
    {
        private MenuPanel _menuPanel;

        public MenuStateModel() : base((int)GameStates.State.Menu, nameof(GameStateModel))
        {
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
            _menuPanel = (MenuPanel)UIManager.Instance.Show(UIKey.Menu);
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            UIManager.Instance.Hide(UIKey.Menu);
        }
    }
}