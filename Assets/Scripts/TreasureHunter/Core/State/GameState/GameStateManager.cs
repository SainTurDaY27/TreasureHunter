namespace TreasureHunter.Core.State.GameState
{
    public class GameStateManager : StateManager<GameStateManager>
    {
        public override void Awake()
        {
            base.Awake();
            GameStates gameStates = new GameStates();
            Initialize((int)GameStates.State.Menu, gameStates.GetGameStateModels());
            GameStateManager.Instance.PushStateToStack(gameStates.GetGameStateModels()[(int)GameStates.State.Menu]);
        }
    }
}