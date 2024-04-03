using System.Linq;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.Scene;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.Map;
using TreasureHunter.Gameplay.Player;
using TreasureHunter.Gameplay.System;
using TreasureHunter.Gameplay.UI;
using TreasureHunter.Gameplay.Utilities;
using UnityEngine;

namespace TreasureHunter.Core.State.GameState
{
    public class GameStateModel : StateModel
    {
        private GameHUDPanel _gameHUDPanel;
        private Damageable _player;
        private GameManager _gameManager;
        private DataManager _dataManager;
        private SkillTestHelper _skillTestHelper;
        private MapPanel _mapPanel;
        private PlayerController _playerController;
        private BackToGameMethod _backToGameMethod;

        public GameStateModel() : base((int)GameStates.State.Game, nameof(GameStateModel))
        {
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.Menu));
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.End));
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.SkillPickup));
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.TreasureGet));
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.Map));
            GameStateManager.Instance.AddTransition(new StateTransition(
                fromState: StateID,
                toState: (int)GameStates.State.Tutorial));
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn();
            _backToGameMethod = BackToGameMethod.ContinueGame;
            if (args.Length > 0)
            {
                _backToGameMethod = (BackToGameMethod)args[0];
            }
            _gameManager = GameManager.Instance;
            _dataManager = DataManager.Instance;
            _skillTestHelper = SkillTestHelper.Instance;

            switch (_backToGameMethod)
            {
                case BackToGameMethod.NewGame:
                    GameSceneManager.Instance.GoToScene(SceneKey.THE_ENTRANCE, () =>
                    {
                        LoadPlayer();
                        LoadGameHUD();

                        // TODO: Move this to a better place later
                        _dataManager.GameData.ExploreNewMapArea(MapAreaKey.TheSurface);
                        _dataManager.GameData.ExploreNewMapArea(MapAreaKey.TheEntrance);

                        Debug.Log("Start new game");
                    });
                    break;

                case BackToGameMethod.LoadGame:
                    DataManager.Instance.LoadSavedGame(DataManager.Instance.GameData.GetCurrentSaveGameSlot());
                    DataManager.Instance.GameSaveManager.OnSaveGameDataChangedHandler();
                    var currentMapArea = DataManager.Instance.GameData.CurrentMapArea;
                    GameSceneManager.Instance.GoToScene(GetSceneKeyByMapAreaKey(currentMapArea), () =>
                    {
                        LoadPlayer();
                        LoadGameHUD();
                    });
                    break;

                case BackToGameMethod.ContinueGame:
                    LoadPlayer();
                    LoadGameHUD();
                    break;

                default:
                    LoadPlayer();
                    LoadGameHUD();
                    break;
            }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            _skillTestHelper.OnSkillChanged += LoadGameHUD;
#endif
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            UIManager.Instance.Hide(UIKey.GameHUD);
            _player.healthChange.RemoveListener(OnPlayerHealthChange);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            _skillTestHelper.OnSkillChanged -= LoadGameHUD;
#endif
        }

        private void OnPlayerHealthChange(int health, int maxHealth)
        {
            _gameHUDPanel.UpdateHealth(health);
            if (health <= 0)
            {
                _gameHUDPanel.SetActiveAnnounceText(true);
                _gameManager.StartEndGameDelay();
            }
        }

        private void LoadGameHUD()
        {
            if (UIManager.Instance.TryGetUIByKey(UIKey.GameHUD, out IBaseUI ui) && (ui is GameHUDPanel panel))
            {
                _gameHUDPanel = panel;
            }

            _gameHUDPanel.UpdateSkillSlot();
            _gameHUDPanel.UpdateHealth(_player.Health);
            _gameHUDPanel.SetActiveAnnounceText(false);
            UIManager.Instance.Show(UIKey.GameHUD);
        }

        private void LoadPlayer()
        {
            _player = GameObject.FindObjectsOfType<Damageable>().FirstOrDefault(d => d.CompareTag("Player"));
            _player.healthChange.AddListener(OnPlayerHealthChange);
            if (_backToGameMethod == BackToGameMethod.LoadGame)
            {
                _playerController = GameObject.FindObjectOfType<PlayerController>();
                var playerPosition = DataManager.Instance
                    .GetSavedGameData(DataManager.Instance.GameData.GetCurrentSaveGameSlot()).GetPlayerPosition();
                _playerController.MovePlayer(playerPosition);
            }
        }

        private string GetSceneKeyByMapAreaKey(MapAreaKey mapAreaKey)
        {
            switch (mapAreaKey)
            {
                // The surface is not included in the map area
                case MapAreaKey.TheEntrance:
                    return SceneKey.THE_ENTRANCE;
                case MapAreaKey.ScorpionCave:
                    return SceneKey.SCORPION_CAVE;
                case MapAreaKey.SpeedyCave:
                    return SceneKey.SPEEDY_CAVE;
                case MapAreaKey.ForgottenPassage:
                    return SceneKey.FORGOTTEN_PASSAGE;
                case MapAreaKey.ForgottenPlace:
                    return SceneKey.FORGOTTEN_PLACE;
                case MapAreaKey.DangerDen:
                    return SceneKey.DANGER_DEN;
                case MapAreaKey.InteriorPeak:
                    return SceneKey.INTERIOR_PEAK;
                case MapAreaKey.NormalCave:
                    return SceneKey.NORMAL_CAVE;
                case MapAreaKey.WeirdSpace:
                    return SceneKey.WEIRD_SPACE;
                case MapAreaKey.ShrinkingGround:
                    return SceneKey.SHRINKING_GROUND;
                case MapAreaKey.CaveOfSmallPeople:
                    return SceneKey.CAVE_OF_SMALL_PEOPLE;
                case MapAreaKey.AbnormalCave:
                    return SceneKey.ABNORMAL_CAVE;
                default:
                    return SceneKey.THE_ENTRANCE;
            }
        }
    }

    public enum BackToGameMethod
    {
        NewGame,
        LoadGame,
        ContinueGame
    }
}