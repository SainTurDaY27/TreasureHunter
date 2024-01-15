using System.Collections;
using System.Collections.Generic;
using TreasureHunter.Core.Scene;
using TreasureHunter.Utilities;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public override void Awake()
        {
            base.Awake();

            // TODO: Should be menu scene
            GameSceneManager.Instance.GoToScene(SceneKey.THE_ENTRANCE);
        }
    }
}