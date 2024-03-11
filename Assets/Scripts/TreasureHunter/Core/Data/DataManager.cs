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
        public SkillIconVisualData SkillIconVisualData => _skillIconVisualData;
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