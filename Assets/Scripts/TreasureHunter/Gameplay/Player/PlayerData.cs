using System;
using System.Collections;
using System.Collections.Generic;
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

            // TODO: For testing purposes only
            ObtainSkill(SkillKey.DoubleJump);
            ObtainSkill(SkillKey.Dash);
            ObtainSkill(SkillKey.WallJump);
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

        public bool HasSkill(SkillKey skillKey)
        {
            // Debug.Log($"Checking if player has skill {skillKey} : {_obtainedSkills.Contains(skillKey)}");
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