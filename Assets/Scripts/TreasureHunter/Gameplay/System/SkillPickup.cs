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

        private DataManager _dataManager;
        private Sprite _skillSprite;

        private void Awake()
        {
            _dataManager = DataManager.Instance;
            _skillSprite = GetComponent<SpriteRenderer>().sprite;
        }

        private void Start()
        {
            if (_dataManager.PlayerData.HasSkill(skill))
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