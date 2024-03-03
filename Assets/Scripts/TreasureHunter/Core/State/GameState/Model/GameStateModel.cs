using System.Linq;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.Scene;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.Map;
using TreasureHunter.Gameplay.System;
using TreasureHunter.Gameplay.UI;
using UnityEngine;

namespace TreasureHunter.Core.State.GameState
{
    public class GameStateModel : StateModel
    {
        private GameHUDPanel _gameHUDPanel;
        private Damageable _player;
        private GameManager _gameManager;
        private DataManager _dataManager;

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
        }

        public override void OnStateIn(params object[] args)
        {
            base.OnStateIn();
            var backToGameMethod = BackToGameMethod.ContinueGame;
            if (args.Length > 0)
            {
                backToGameMethod = (BackToGameMethod)args[0];
            }
            _gameManager = GameManager.Instance;
            _dataManager = DataManager.Instance;

            switch (backToGameMethod)
            {
                case BackToGameMethod.NewGame:
                    GameSceneManager.Instance.GoToScene(SceneKey.THE_ENTRANCE, () =>
                    {
                        LoadPlayer();
                        LoadGameHUD();

                        // TODO: Move this to a better place later
                        _dataManager.GameData.ExploreNewMapArea(MapAreaKey.TheSurface);
                        _dataManager.GameData.ExploreNewMapArea(MapAreaKey.TheEntrace);

                        Debug.Log("Start new game");
                    });
                    break;

                case BackToGameMethod.LoadGame:
                    // TODO: Implement load game later
                    LoadPlayer();
                    LoadGameHUD();
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
        }

        public override void OnStateOut()
        {
            base.OnStateOut();
            UIManager.Instance.Hide(UIKey.GameHUD);
            _player.healthChange.RemoveListener(OnPlayerHealthChange);
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
        }
    }

    public enum BackToGameMethod
    {
        NewGame,
        LoadGame,
        ContinueGame
    }
}