using System.Collections;
using TreasureHunter.Core.State.GameState;
using TreasureHunter.Utilities;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private Coroutine _endGameDelay;
        private const int _END_GAME_DELAY = 4;
        public bool IsGamePaused { get; private set; }

        public override void Awake()
        {
            base.Awake();
            IsGamePaused = false;
        }

        public void PauseGame(bool pauseGameStatus)
        {
            IsGamePaused = pauseGameStatus;
            Time.timeScale = pauseGameStatus ? 0 : 1;
        }

        public void StartEndGameDelay()
        {
            if (_endGameDelay != null)
            {
                StopCoroutine(_endGameDelay);
            }
            _endGameDelay = StartCoroutine(EndGameDelay());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // TODO: Use better way to check current state
                if (GameStateManager.Instance.CurrentState.StateID == (int)GameStates.State.Game)
                {
                    GameStateManager.Instance.GoToState((int)GameStates.State.Map);
                
                }
                else if (GameStateManager.Instance.CurrentState.StateID == (int)GameStates.State.Map)
                {
                    GameStateManager.Instance.GoToState((int)GameStates.State.Game);
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // TODO: Use better way to check current state
                if (GameStateManager.Instance.CurrentState.StateID == (int)GameStates.State.Game)
                {
                    GameStateManager.Instance.GoToState((int)GameStates.State.Tutorial);

                }
                else if (GameStateManager.Instance.CurrentState.StateID == (int)GameStates.State.Tutorial)
                {
                    GameStateManager.Instance.GoToState((int)GameStates.State.Game);
                }
            }
        }

        private IEnumerator EndGameDelay()
        {
            yield return new WaitForSeconds(_END_GAME_DELAY);
            GameStateManager.Instance.GoToState((int)GameStates.State.End, EndGameType.Lose);
        }
    }
}