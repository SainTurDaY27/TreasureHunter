namespace TreasureHunter.Core.State.GameState
{
    public class GameStates : States
    {
        public enum State
        {
            Menu,
            Game,
            End
        }

        private StateModel[] _states;

        public override StateModel[] GetGameStateModels()
        {
            _states ??= new StateModel[]
            {
                new MenuStateModel(),
                new GameStateModel(),
                new EndStateModel()
            };
            return _states;
        }
    }
}