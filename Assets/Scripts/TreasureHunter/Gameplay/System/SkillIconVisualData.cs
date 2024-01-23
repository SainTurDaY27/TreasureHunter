using System;
using System.Linq;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    [CreateAssetMenu(fileName = "SkillIconVisualData", menuName = "TreasureHunter/SkillIconVisualData")]
    public class SkillIconVisualData : ScriptableObject
    {
        [SerializeField]
        private SkillIcons[] _skillIconList;

        [Serializable]
        protected class SkillIcons
        {
            [SerializeField]
            private SkillKey _key;

            [SerializeField]
            private Sprite _imageSprite;

            public SkillKey Key => _key;
            public Sprite ImageSprite => _imageSprite;
        }

        public Sprite GetSkillIcon(SkillKey skillKey)
        {
            var skillIcon = _skillIconList.FirstOrDefault(skill => skill.Key == skillKey);
            if (skillIcon == null)
            {
                Debug.LogError($"Skill icon for {skillKey} not found");
                return null;
            }
            return skillIcon.ImageSprite;
            
        }
    }
}