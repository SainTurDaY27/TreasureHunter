using System.Collections;
using System.Collections.Generic;
using TreasureHunter.Core.State.GameState;
using TreasureHunter.Utilities;
using UnityEngine;

namespace TreasureHunter.Core.State
{
    public abstract class StateManager<T> : MonoSingleton<T> where T : MonoBehaviour
    {
        public delegate void StateChangedHandler(
            StateModel oldState,
            StateModel newState,
            StateTransition transition,
            params object[] args);

        public StateModel CurrentState { get; private set; }
        public StateModel PreviousState { get; private set; }
        public StateModel NextState { get; private set; }
        public event StateChangedHandler OnStateChanged;
        private Stack<StateModel> _stateStack = new();
        private Dictionary<int, StateModel> States;
        private Dictionary<GameStateTransitionKey, StateTransition> _transitions;
        private bool _isInitialized = false;
        private Coroutine _changeStateCoroutine;

        protected void Initialize(int starterState, StateModel[] gameStateModels, params object[] stateInArgs)
        {
            if (!_isInitialized)
            {
                InitStates(gameStateModels);
                CurrentState = States[starterState];
                CurrentState.OnStateIn(stateInArgs);
                _isInitialized = true;
            }
        }

        public void GoToState(int stateID, params object[] args)
        {
            if (States.TryGetValue(stateID, out StateModel nextState))
            {
                StateTransition transition = GetTransitionFromCurrentState(nextState);
                PreviousState = CurrentState;
                NextState = nextState;
                if (transition.IsValid())
                {
                    Debug.Log($"[{typeof(T).Name}] Change game state from {CurrentState.StateName} to {nextState.StateName}");
                    MainThreadDispatcher.Instance.RunOnMainThread(() =>
                    {
                        SetState(nextState, transition, args);
                    });
                }
                else
                {
                    Debug.LogWarning($"[{typeof(T).Name}] Invalid transition from {CurrentState.StateName} to {nextState.StateName} state");
                }
            }
            else
            {
                Debug.LogWarning($"[{typeof(T).Name}] There is no state id: {stateID} in {nameof(GameStates)} game state models list");
            }
        }

        public void Clear()
        {
            OnStateChanged = null;
            States.Clear();
            _isInitialized = false;
        }

        public bool IsState(int state) => CurrentState != null && CurrentState.StateID == state;

        #region GameState

        private void InitStates(StateModel[] gameStateModels)
        {
            States ??= new Dictionary<int, StateModel>();
            States.Clear();

            foreach (StateModel state in gameStateModels)
            {
                States.Add(state.StateID, state);
            }
        }

        private void SetState(StateModel newState, StateTransition transition, params object[] args)
        {
            StateModel oldState = CurrentState;
            CurrentState = newState;
            ChangeState(oldState, CurrentState, args);

            if (_changeStateCoroutine != null)
            {
                StopCoroutine(_changeStateCoroutine);
            }
            _changeStateCoroutine = StartCoroutine(CallStateChanged(oldState, transition, args));
        }

        private void ChangeState(
            StateModel oldStateModel,
            StateModel newStateModel,
            params object[] args)
        {
            oldStateModel?.OnStateOut();
            newStateModel?.OnStateIn(args);
        }

        private IEnumerator CallStateChanged(
            StateModel oldState,
            StateTransition transition,
            params object[] args)
        {

            yield return new WaitForEndOfFrame();
            OnStateChanged?.Invoke(oldState, CurrentState, transition, args);
            _changeStateCoroutine = null;
        }

        #endregion

        #region GameTransition

        /// <summary>
        /// If duplicated, it'll override the existing one
        /// </summary>
        /// <param name="transition"></param>
        public void AddTransition(StateTransition transition)
        {
            _transitions ??= new Dictionary<GameStateTransitionKey, StateTransition>();
            _transitions[transition.Key] = transition;
        }

        private StateTransition GetTransitionFromCurrentState(StateModel toState)
        {
            if (CurrentState.StateID == State.States.INVALID_STATE || CurrentState == toState)
            {
                ShowStateErrorPopup(CurrentState.StateName, toState.StateName);
                return StateTransition.InvalidTransition;
            }

            GameStateTransitionKey key = new GameStateTransitionKey(CurrentState.StateID, toState.StateID);

            if (!_transitions.TryGetValue(key, out StateTransition transition))
            {
                ShowStateErrorPopup(CurrentState.StateName, toState.StateName);
                transition = StateTransition.InvalidTransition;
            }

            return transition;
        }

        private void ShowStateErrorPopup(string fromStateName, string toStateName)
        {
            if (fromStateName == toStateName)
            {
                Debug.LogError($"Game state order is invalid.\n" +
                               $"From {fromStateName} to {toStateName}");
                return;
            }
        }

        #endregion
    }
}