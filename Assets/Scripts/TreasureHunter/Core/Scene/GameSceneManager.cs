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
                case SceneKey.THE_ENTRANCE:
                    _sceneLoader.LoadSceneByName(SceneKey.THE_ENTRANCE, callback);
                    break;

                case SceneKey.ABNORMAL_CAVE:
                    _sceneLoader.LoadSceneByName(SceneKey.ABNORMAL_CAVE, callback);
                    break;

                default:
                    Debug.LogError("Scene not found");
                    break;
            }
        }
    }
}