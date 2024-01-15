using System;
using System.Collections;
using System.Collections.Generic;
using TreasureHunter.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.UI
{
    public class AbilitySelectionPanel : MonoBehaviour, IBaseUI
    {
        [SerializeField]
        private Button[] _skillSelectionButtons;

        private int _buttonClickedCount = 0;

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

        public void SetAllButtonsInteractable()
        {
            foreach (Button button in _skillSelectionButtons)
            {
                button.interactable = true;
            }
        }

        public void OnButtonClicked(Button clickedButton)
        {
            //int buttonClickedCount = 0;
            // if clickedButton is not interactable, make it interactable
            //if (!clickedButton.interactable)
            //{
            //    clickedButton.interactable = true;
            //    return;
            //}


            int buttonIndex = Array.IndexOf(_skillSelectionButtons, clickedButton);

            if (buttonIndex == -1)
            {
                return;
            }
            _buttonClickedCount++;
            clickedButton.interactable = false;

            if (_buttonClickedCount > 2)
            {
                SetAllButtonsInteractable();
                _buttonClickedCount = 0;
            }
        }
    }
}