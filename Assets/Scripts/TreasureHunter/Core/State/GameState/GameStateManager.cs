using System.Diagnostics;

namespace TreasureHunter.Core.State.GameState
{
    public class GameStateManager : StateManager<GameStateManager>
    {
        public GameStates.State initialState = GameStates.State.Game;
        public override void Awake()
        {
            base.Awake();
            GameStates gameStates = new GameStates();
            Initialize((int)initialState, gameStates.GetGameStateModels());
        }
    }
}