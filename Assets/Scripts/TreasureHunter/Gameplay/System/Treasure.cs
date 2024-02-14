using System;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.State.GameState;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class Treasure : MonoBehaviour
    {
        private DataManager _dataManager;
        void Awake()
        {
            _dataManager = FindObjectOfType<DataManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _dataManager.GameData.ObtainTreasure();
            GameStateManager.Instance.GoToState((int)GameStates.State.TreasureGet);
            // TODO: Mutate treasure data
            Destroy(gameObject);
        }
    }
}