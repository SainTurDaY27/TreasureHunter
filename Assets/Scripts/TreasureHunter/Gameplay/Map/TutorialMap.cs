using System;
using System.Collections.Generic;
using TreasureHunter.Core.Data;
using TreasureHunter.Gameplay.System;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.Map
{
    public class TutorialMap : MonoBehaviour
    {
        public List<DestinationSetting> destinationSettings;
        public List<SourceSetting> sourceSettings;
        public SkillIconVisualData skillIconVisualData;

        private void Start()
        {
            var playerData = DataManager.Instance.PlayerData;
            var startingSkills = playerData.GetStartingSkills();
            if (startingSkills.Count != sourceSettings.Count)
            {
                Debug.LogWarning(
                    "Number of starting skills and source teleporters do not match. Aborting.");
                return;
            }

            for (int i = 0; i < startingSkills.Count; i++)
            {
                var skillKey = startingSkills[i];
                var sourceSetting = sourceSettings[i];
                var destinationSetting = destinationSettings.Find(dest => dest.skill == skillKey);
                
                // Set teleporter
                var sourceTeleporter = sourceSetting.sourceTeleporter;
                var sourceImage = sourceSetting.skillImage;
                var destinationTeleporter = destinationSetting.destinationTeleporter;
                sourceTeleporter.teleportDestination = destinationTeleporter.transform.position;
                sourceImage.sprite = skillIconVisualData.GetSkillIcon(skillKey);
                destinationTeleporter.teleportDestination = sourceTeleporter.transform.position;
            }
        }

        [Serializable]
        public struct DestinationSetting
        {
            public SkillKey skill;
            public Teleporter destinationTeleporter;
        }

        [Serializable]
        public struct SourceSetting
        {
            public Image skillImage;
            public Teleporter sourceTeleporter;
        }
    }
}