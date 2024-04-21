using System.Collections;
using System.Collections.Generic;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.Player;
using TreasureHunter.Gameplay.System;
using TreasureHunter.Gameplay.UI;
using TreasureHunter.Utilities;
using UnityEngine;

namespace TreasureHunter.Core.Data
{
    public class DataManager : MonoSingleton<DataManager>
    {
        [SerializeField] private SkillIconVisualData _skillIconVisualData;
        [SerializeField] private MapVisualData _mapVisualData;

        private PlayerData _playerData = new PlayerData();
        private GameData _gameData = new GameData();
        private GameSaveManager _gameSaveManager = new GameSaveManager();
        public SkillIconVisualData SkillIconVisualData => _skillIconVisualData;
        public MapVisualData MapVisualData => _mapVisualData;
        public PlayerData PlayerData => _playerData;
        public GameData GameData => _gameData;
        public GameSaveManager GameSaveManager => _gameSaveManager;

        public override void Awake()
        {
            base.Awake();
            _playerData = new PlayerData();
            _gameData = new GameData();
            _gameSaveManager = new GameSaveManager();
        }

        public void ResetData()
        {
            _playerData.ResetData();
            _gameData.ResetData();
        }

        public void SaveGame(SaveGameSlot saveGameSlot)
        {
            var player = GameObject.FindObjectOfType<PlayerController>();
            _playerData.SetPlayerPosition(player.transform.position);
            _gameSaveManager.SaveGameData(saveGameSlot);
            // Display UI text if possible
            IBaseUI gameHUD;
            if (UIManager.Instance.TryGetUIByKey(UIKey.GameHUD, out gameHUD))
            {
                if (gameHUD is GameHUDPanel gameHUDPanel)
                {
                    StartCoroutine(gameHUDPanel.ShowSavingStatus());
                }
            }
        }

        public void LoadSavedGame(SaveGameSlot saveGameSlot)
        {
            var saveGameData = _gameSaveManager.LoadGameData(saveGameSlot);
            _playerData.LoadData(saveGameData);
            _gameData.LoadData(saveGameData);
        }

        public SaveGameData GetSavedGameData(SaveGameSlot saveGameSlot)
        {
            return _gameSaveManager.LoadGameData(saveGameSlot);
        }
    }
}