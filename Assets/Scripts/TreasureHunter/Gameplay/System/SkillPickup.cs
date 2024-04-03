using System;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.State.GameState;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class SkillPickup : MonoBehaviour
    {
        public SkillKey skill;
        public string skillName, skillToolTip;
        public GameObject treasurePrefab;

        private DataManager _dataManager;
        private Sprite _skillSprite;

        private void Awake()
        {
            _dataManager = DataManager.Instance;
            _skillSprite = GetComponent<SpriteRenderer>().sprite;
        }

        private void Start()
        {
            // Replace starting skill with treasure
            if (_dataManager.PlayerData.HasSkill(skill, startingSkill: true))
            {
                Debug.Log("Already has skill. Spawn treasure");
                var transform1 = transform;
                var treasureGameObject = GameObject.Instantiate(treasurePrefab, transform1.position, transform1.rotation);
                var treasure = treasureGameObject.GetComponent<Treasure>();
                if (treasure == null)
                {
                    Debug.LogError("Invalid treasure prefab");
                    return;
                }
                treasure.treasureId = skillName;
                treasure.DestroyIfAlreadyObtained();
                Destroy(gameObject);
            }
            // Already has skill => destroy
            if (_dataManager.PlayerData.HasSkill(skill, startingSkill: false))
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            GameStateManager.Instance.GoToState((int)GameStates.State.SkillPickup,
                skillName,
                skillToolTip,
                _skillSprite
            );
            _dataManager.PlayerData.ObtainSkill(skill);
            Destroy(gameObject);
        }
    }
}