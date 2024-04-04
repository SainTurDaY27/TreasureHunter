using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TreasureHunter.Core.Data;
using TreasureHunter.Gameplay.System;
using UnityEngine;

namespace TreasureHunter.Gameplay.Player
{
    public class PlayerData
    {
        private List<SkillKey> _obtainedSkills;
        private List<SkillKey> _startingSkills;
        private Vector2 _playerPosition;
        private PlayerController _playerController;
        public event Action OnSkillObtained;

        public PlayerData()
        {
            _obtainedSkills = new List<SkillKey>();
            _startingSkills = new List<SkillKey>();
            {
                // GroudSlam is the default skill player obtains
                //SkillKey.GroundSlam
            }
            ;
        }

        public void ResetData()
        {
            _obtainedSkills.Clear();
            _startingSkills.Clear();
        }

        public void LoadData(SaveGameData saveGameData)
        {
            // Load obtained skills
            var playerPosition = saveGameData.GetPlayerPosition();
            //_playerController = GameObject.FindObjectOfType<PlayerController>();
            //_playerController.MovePlayer(playerPosition);
            var startingSkills = saveGameData.GetStartingSkills();
            foreach (var skill in startingSkills)
            {
                ObtainSkill(skill, startingSkill: true);
            }

            var obtainedSkills = saveGameData.GetObtainedSkills();
            foreach (var skill in obtainedSkills)
            {
                ObtainSkill(skill);
            }
        }

        public void ObtainSkill(SkillKey skillKey, bool startingSkill = false)
        {
            if (_obtainedSkills.Contains(skillKey))
            {
                return;
            }

            if (startingSkill)
            {
                _startingSkills.Add(skillKey);
            }

            _obtainedSkills.Add(skillKey);
            OnSkillObtainedHandler();
        }

        public void RemoveSkill(SkillKey skillKey)
        {
            _obtainedSkills.Remove(skillKey);
            OnSkillObtainedHandler();
        }

        public bool ToggleSkill(SkillKey skillKey)
        {
            if (!_obtainedSkills.Contains(skillKey))
            {
                ObtainSkill(skillKey);
                return true;
            }
            else
            {
                RemoveSkill(skillKey);
                return false;
            }
        }

        public bool HasSkill(SkillKey skillKey, bool startingSkill = false)
        {
            return startingSkill ? _startingSkills.Contains(skillKey) : _obtainedSkills.Contains(skillKey);
        }

        public List<SkillKey> GetObtainedSkills()
        {
            return _obtainedSkills;
        }

        public List<SkillKey> GetStartingSkills()
        {
            return _startingSkills;
        }

        public void SetPlayerPosition(Vector2 position)
        {
            _playerPosition = position;
        }

        public Vector2 GetPlayerPosition()
        {
            return _playerPosition;
        }

        private void OnSkillObtainedHandler()
        {
            OnSkillObtained?.Invoke();
        }
    }
}