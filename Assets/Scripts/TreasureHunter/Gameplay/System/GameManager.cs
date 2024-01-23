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
        public override void Awake()
        {
            base.Awake();
            //GameStateManager.Instance.GoToState((int)GameStates.State.Menu);
            //GameSceneManager.Instance.GoToScene(SceneKey.MENU);
        }
    }
}