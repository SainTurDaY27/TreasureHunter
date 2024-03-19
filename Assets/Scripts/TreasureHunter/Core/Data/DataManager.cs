using System.Collections;
using System.Collections.Generic;
using TreasureHunter.Gameplay.Player;
using TreasureHunter.Gameplay.System;
using TreasureHunter.Utilities;
using UnityEngine;

namespace TreasureHunter.Core.Data
{
    public class DataManager : MonoSingleton<DataManager>
    {
        [SerializeField] private SkillIconVisualData _skillIconVisualData;

        private PlayerData _playerData = new PlayerData();
        private GameData _gameData = new GameData();
        private GameSaveManager _gameSaveManager = new GameSaveManager();
        public SkillIconVisualData SkillIconVisualData => _skillIconVisualData;
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
            _gameSaveManager.SaveGameData(saveGameSlot);
        }

        public void LoadSavedGame(SaveGameSlot saveGameSlot)
        {
            var saveGameData = _gameSaveManager.LoadGameData(saveGameSlot);
            //ResetData();

            _playerData.LoadData(saveGameData);
            _gameData.LoadData(saveGameData);

        }
    }
}