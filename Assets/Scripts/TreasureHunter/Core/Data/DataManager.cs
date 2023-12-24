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
        private PlayerData _playerData;
        private GameData _gameData;
        public PlayerData PlayerData => _playerData;
        public GameData GameData => _gameData;

        public override void Awake()
        {
            base.Awake();
            _playerData = new PlayerData();
            _gameData = new GameData();
        }
    }
}