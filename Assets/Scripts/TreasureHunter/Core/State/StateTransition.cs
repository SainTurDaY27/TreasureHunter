using System;

namespace TreasureHunter.Core.State
{
    public class StateTransition
    {
        public static readonly StateTransition InvalidTransition =
            new StateTransition(States.INVALID_STATE, States.INVALID_STATE, () => false);

        public GameStateTransitionKey Key { get; private set; }
        public int FromStateID { get; private set; }
        public int ToStateID { get; private set; }

        private readonly Func<bool> m_ValidationFunction;

        public StateTransition(int fromState, int toState, Func<bool> validationFunction = null)
        {
            Key = new GameStateTransitionKey(fromState, toState);
            FromStateID = fromState;
            ToStateID = toState;
            m_ValidationFunction = validationFunction;
        }

        public bool IsValid()
        {
            return m_ValidationFunction == null || m_ValidationFunction();
        }
    }

    public class GameStateTransitionKey : Tuple<int, int>
    {
        public int FromStateID => Item1;
        public int ToStateID => Item2;
        public GameStateTransitionKey(int fromStateID, int toStateID) : base(fromStateID, toStateID) { }
    }
}