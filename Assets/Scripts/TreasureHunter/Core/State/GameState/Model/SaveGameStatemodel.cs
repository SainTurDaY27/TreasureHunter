using System.Collections;
using System.Collections.Generic;
using TreasureHunter.Core.State;
using TreasureHunter.Core.State.GameState;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.UI;
using UnityEngine;

namespace TreasureHunter.Core.State.GameState
{
    public class SaveGameStateModel : StateModel
    {
        public SaveGameStateModel() : base((int)GameStates.State.SaveGame, nameof(SaveGameStateModel))
        {
            GameStateManager.Instance.AddTransition(new StateTransition(
            fromState: StateID,
            toState: (int)GameStates.State.Game));
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn(args);
            //_saveGamePanel = (LoadGamePanel)UIManager.Instance.Show(UIKey.SaveGame);
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            //UIManager.Instance.Hide(UIKey.SaveGame);
        }
    }
}