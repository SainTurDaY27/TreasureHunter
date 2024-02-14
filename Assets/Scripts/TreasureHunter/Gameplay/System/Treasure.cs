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
        public float oscillationValue = 0.75f;


        private DataManager _dataManager;
        private Vector2 originalPosition;


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

        private void Start()
        {
            originalPosition = transform.position;
        }

        private void Update()
        {
            transform.position = new Vector2(transform.position.x,
                originalPosition.y + Mathf.Sin(Time.time) * oscillationValue);
        }
    }
}