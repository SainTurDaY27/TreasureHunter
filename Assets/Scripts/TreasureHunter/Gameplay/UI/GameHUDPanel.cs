using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreasureHunter.Core.Data;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.System;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.UI
{
    public class GameHUDPanel : MonoBehaviour, IBaseUI
    {
        [SerializeField]
        private GameObject[] _health;

        [SerializeField]
        private GameObject[] _skillSlots;

        [SerializeField]
        private TextMeshProUGUI _mapNameText;

        [SerializeField]
        private TextMeshProUGUI _announceText;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void UpdateHealth(int health)
        {
            if (health < 0 || health > _health.Length)
            {
                Debug.LogError("Invalid health value");
                return;
            }
            for (int i = 0; i < _health.Length; i++)
            {
                _health[i].SetActive(i < health);
            }
        }

        public void UpdateSkillSlot()
        {
            foreach (var slot in _skillSlots)
            {
                slot.SetActive(false);
            }
            var obtainedSkills = DataManager.Instance.PlayerData.GetObtainedSkills();
            for (int i = 0; i < obtainedSkills.Count; i++)
            {
                if (obtainedSkills[i] != SkillKey.GroundSlam)
                {
                    _skillSlots[i].SetActive(i < obtainedSkills.Count);
                    var skillIcon = DataManager.Instance.SkillIconVisualData.GetSkillIcon(obtainedSkills[i]);
                    _skillSlots[i].GetComponent<Image>().sprite = skillIcon;
                }
            }
        }

        public void UpdateMapName(string mapName)
        {
            _mapNameText.text = mapName;
        }

        public void UpdateAnnounceText(string announceText)
        {
            _announceText.text = announceText;
        }

        public void SetActiveAnnounceText(bool isActive)
        {
            _announceText.gameObject.SetActive(isActive);
        }

        private void Update()
        {
            // TODO: For testing purposes only
            if (Input.GetKeyDown(KeyCode.U))
            {
                UpdateSkillSlot();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                UpdateMapName("Map 2");
            }
        }
    }
}