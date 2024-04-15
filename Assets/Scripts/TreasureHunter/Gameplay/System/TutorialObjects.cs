using System;
using System.Collections.Generic;
using TreasureHunter.Core.Data;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class TutorialObjects : MonoBehaviour
    {
        public SkillKey skill;
        public List<GameObject> gameObjects;

        private void Start()
        {
            var startingSkills = DataManager.Instance.PlayerData.GetStartingSkills();
            if (startingSkills.Contains(skill))
            {
                foreach (var theObject in gameObjects)
                {
                    Destroy(theObject);
                }
            }
        }
    }
}