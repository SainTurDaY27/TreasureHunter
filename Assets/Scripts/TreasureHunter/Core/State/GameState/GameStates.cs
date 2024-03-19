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
            TreasureGet,
            LoadGame,
            Map,
            SaveGame
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
                new TreasureGetStateModel(),
                new LoadGameStateModel(),
                new MapStateModel(),
                new SaveGameStateModel()
            };
            return _states;
        }
    }
}