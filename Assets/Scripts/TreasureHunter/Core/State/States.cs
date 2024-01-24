namespace TreasureHunter.Core.State
{
    public abstract class States
    {
        public const int STARTER_STATE = int.MinValue;
        public const int INVALID_STATE = int.MaxValue;

        public abstract StateModel[] GetGameStateModels();
    }
}