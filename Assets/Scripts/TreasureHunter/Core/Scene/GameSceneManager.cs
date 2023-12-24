using TreasureHunter.Utilities;
using System;
using UnityEngine;

namespace TreasureHunter.Core.Scene
{
    public class GameSceneManager : MonoSingleton<GameSceneManager>
    {
        [SerializeField]
        private SceneLoader _sceneLoader;

        public void GoToScene(string scene, Action callback = null)
        {
            switch (scene)
            {
                // TODO: Add your scene name here, Example:
                //case SceneKey.MENU:
                //    _sceneLoader.LoadSceneByName(SceneKey.MENU, callback);
                //    break;

                default:
                    Debug.LogError("Scene not found");
                    break;
            }
        }
    }
}