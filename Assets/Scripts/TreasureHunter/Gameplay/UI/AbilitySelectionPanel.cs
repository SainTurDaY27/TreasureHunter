using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreasureHunter.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.UI
{
    public class AbilitySelectionPanel : MonoBehaviour, IBaseUI
    {
        [SerializeField]
        private Button[] _skillSelectionButtons;
        
        private Button[] selectedButtons = {};

        [SerializeField] private Color unselectedColor, selectedColor;


        //[SerializeField]
        //private Button _dashSkillSelectionButton;

        //[SerializeField]
        //private Button _doubleJumpSkillSelectionButton;

        //[SerializeField]
        //private Button _fireballSkillSelectionButton;

        //[SerializeField]
        //private Button _wallJumpSkillSelectionButton;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void SelectButton(Button button)
        {
            selectedButtons = selectedButtons.Append(button).ToArray();
            var image = button.GetComponent<Image>();
            image.color = selectedColor;
        }

        private void DeselectButton(Button button)
        {
            selectedButtons = selectedButtons.Where(b => b != button).ToArray();
            var image = button.GetComponent<Image>();
            image.color = unselectedColor;
        }

        public void OnButtonClicked(Button clickedButton)
        {

            int buttonIndex = Array.IndexOf(_skillSelectionButtons, clickedButton);

            if (buttonIndex == -1)
            {
                return;
            }

            if (selectedButtons.Contains(clickedButton))
            {
                DeselectButton(clickedButton);
            }
            else
            {
                if (selectedButtons.Count() >= 2) return;
                SelectButton(clickedButton);
            }
        }
    }
}