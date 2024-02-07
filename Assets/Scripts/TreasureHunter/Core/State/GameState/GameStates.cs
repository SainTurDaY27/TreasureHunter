namespace TreasureHunter.Core.State.GameState
{
    public class GameStates : States
    {
        public enum State
        {
            Menu,
            SkillSelection,
            Game,
            End,
            SkillPickup,
            LoadGame
        }
        
        private StateModel[] _states;

        public override StateModel[] GetGameStateModels()
        {
            _states ??= new StateModel[]
            {
                new MenuStateModel(),
                new AbilitySelectionStateModel(),
                new GameStateModel(),
                new EndStateModel(),
                new AbilityPickupStateModel(),
                new LoadGameStateModel()
            };
            return _states;
        }
    }
}