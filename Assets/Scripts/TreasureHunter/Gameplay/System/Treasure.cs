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


        void Awake()
        {
            DestroyIfAlreadyObtained();
        }

        public void DestroyIfAlreadyObtained()
        {
            if (DataManager.Instance.GameData.IsTreasureCollected(treasureId)) Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            DataManager.Instance.GameData.CollectTreasure(treasureId);
            GameStateManager.Instance.GoToState((int)GameStates.State.TreasureGet);
            Destroy(gameObject);
        }
    }
}