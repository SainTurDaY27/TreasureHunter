using System.Collections;
using System.Collections.Generic;
using TreasureHunter.Core.Scene;
using TreasureHunter.Core.State.GameState;
using TreasureHunter.Core.UI;
using TreasureHunter.Utilities;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private Coroutine _endGameDelay;
        private const int _END_GAME_DELAY = 4;

        public void StartEndGameDelay()
        {
            if (_endGameDelay != null)
            {
                StopCoroutine(_endGameDelay);
            }
            _endGameDelay = StartCoroutine(EndGameDelay());
        }

        private IEnumerator EndGameDelay()
        {
            yield return new WaitForSeconds(_END_GAME_DELAY);
            GameStateManager.Instance.GoToState((int)GameStates.State.End);
        }
    }
}