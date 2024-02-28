using System;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.State.GameState;
using UnityEngine;
using UnityEngine.Serialization;

namespace TreasureHunter.Gameplay.System
{
    public class Treasure : MonoBehaviour
    {
        public string treasureId = "CHANGE THIS THING!";


        private DataManager _dataManager;


        void Awake()
        {
            _dataManager = FindObjectOfType<DataManager>();
            if (_dataManager.GameData.IsTreasureCollected(treasureId)) Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _dataManager.GameData.CollectTreasure(treasureId);
            GameStateManager.Instance.GoToState((int)GameStates.State.TreasureGet);
            Destroy(gameObject);
        }
    }
}