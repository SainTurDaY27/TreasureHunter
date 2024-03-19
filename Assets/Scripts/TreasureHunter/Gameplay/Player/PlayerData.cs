using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TreasureHunter.Core.Data;
using TreasureHunter.Gameplay.System;

namespace TreasureHunter.Gameplay.Player
{
    public class PlayerData
    {
        private List<SkillKey> _obtainedSkills;
        public event Action OnSkillObtained;
        public PlayerData()
        {
            _obtainedSkills = new List<SkillKey>
            {
                // GroudSlam is the default skill player obtains
                //SkillKey.GroundSlam
            };
        }

        public void ResetData()
        {
            _obtainedSkills.Clear();
        }

        public void LoadData(SaveGameData saveGameData)
        {
            // Load obtained skills
            var obtainedSkills = saveGameData.GetObtainedSkills();
            foreach (var skill in obtainedSkills)
            {
                ObtainSkill(skill);
            }
        }

        public void ObtainSkill(SkillKey skillKey)
        {
            if (_obtainedSkills.Contains(skillKey))
            {
                return;
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

        public bool HasSkill(SkillKey skillKey)
        {
            return _obtainedSkills.Contains(skillKey);
        }

        public List<SkillKey> GetObtainedSkills()
        {
            return _obtainedSkills;
        }

        private void OnSkillObtainedHandler()
        {
            OnSkillObtained?.Invoke();
        }
    }
}