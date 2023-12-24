using System;
using System.Collections;
using System.Collections.Generic;
using TreasureHunter.Gameplay.System;
using UnityEngine;

namespace TreasureHunter.Gameplay.Player
{
    public class PlayerData
    {
        private List<SkillKey> _obtainedSkills;

        public PlayerData()
        {
            _obtainedSkills = new List<SkillKey>
            {
                // GroudSlam is the default skill player obtains
                SkillKey.GroundSlam
            };

            // TODO: For testing purposes only
            ObtainSkill(SkillKey.DoubleJump);
        }

        public void ObtainSkill(SkillKey skillKey)
        {
            _obtainedSkills.Add(skillKey);
        }

        public bool HasSkill(SkillKey skillKey)
        {
            Debug.Log($"Checking if player has skill {skillKey} : {_obtainedSkills.Contains(skillKey)}");
            return _obtainedSkills.Contains(skillKey);
        }
    }
}