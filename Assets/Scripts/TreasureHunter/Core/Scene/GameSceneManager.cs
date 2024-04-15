using System;
using TreasureHunter.Utilities;
using UnityEngine;

namespace TreasureHunter.Core.Scene
{
    public class GameSceneManager : MonoSingleton<GameSceneManager>
    {
        [SerializeField] private SceneLoader _sceneLoader;

        public void GoToScene(string scene, Action callback = null)
        {
            switch (scene)
            {
                case SceneKey.MENU:
                    _sceneLoader.LoadSceneByName(SceneKey.MENU, callback);
                    break;

                case SceneKey.THE_ENTRANCE:
                    _sceneLoader.LoadSceneByName(SceneKey.THE_ENTRANCE, callback);
                    break;

                case SceneKey.ABNORMAL_CAVE:
                    _sceneLoader.LoadSceneByName(SceneKey.ABNORMAL_CAVE, callback);
                    break;

                case SceneKey.NORMAL_CAVE:
                    _sceneLoader.LoadSceneByName(SceneKey.NORMAL_CAVE, callback);
                    break;
                case SceneKey.WEIRD_SPACE:
                    _sceneLoader.LoadSceneByName(SceneKey.WEIRD_SPACE, callback);
                    break;

                case SceneKey.SCORPION_CAVE:
                    _sceneLoader.LoadSceneByName(SceneKey.SCORPION_CAVE, callback);
                    break;

                case SceneKey.DANGER_DEN:
                    _sceneLoader.LoadSceneByName(SceneKey.DANGER_DEN, callback);
                    break;
                
                case SceneKey.SPEEDY_CAVE:
                    _sceneLoader.LoadSceneByName(SceneKey.SPEEDY_CAVE, callback);
                    break;
                
                case SceneKey.SHRINKING_GROUND:
                    _sceneLoader.LoadSceneByName(SceneKey.SHRINKING_GROUND, callback);
                    break;
                
                case SceneKey.INTERIOR_PEAK:
                    _sceneLoader.LoadSceneByName(SceneKey.INTERIOR_PEAK, callback);
                    break;
                
                case SceneKey.CAVE_OF_SMALL_PEOPLE:
                    _sceneLoader.LoadSceneByName(SceneKey.CAVE_OF_SMALL_PEOPLE, callback);
                    break;
                
                case SceneKey.FORGOTTEN_PASSAGE:
                    _sceneLoader.LoadSceneByName(SceneKey.FORGOTTEN_PASSAGE, callback);
                    break;
                
                case SceneKey.FORGOTTEN_PLACE:
                    _sceneLoader.LoadSceneByName(SceneKey.FORGOTTEN_PLACE, callback);
                    break;
                
                case SceneKey.THE_SURFACE:
                    _sceneLoader.LoadSceneByName(SceneKey.THE_SURFACE, callback);
                    break;
                
                default:
                    Debug.LogError("Scene not found");
                    break;
            }
        }
    }
}