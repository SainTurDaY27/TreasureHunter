using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.System;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.UI
{
    public class AbilitySelectionPanel : MonoBehaviour, IBaseUI
    {
        [SerializeField]
        private Button[] _skillSelectionButtons;

        [SerializeField] 
        private Color unselectedColor, selectedColor;

        [SerializeField]
        private Button _playButton;
        
        private Button[] selectedButtons = {};

        [SerializeField]
        private SkillButtons[] _skillButtonList;

        [Serializable]
        protected class SkillButtons
        {
            [SerializeField]
            private SkillKey _key;

            [SerializeField]
            private Button _button;

            public SkillKey Key => _key;
            public Button SkillButton => _button;
        }

        public event Action OnPlayButtonClicked;
        public event Action<SkillKey> OnSkillSelected;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void GetSelectedSkill()
        {
            foreach (var button in selectedButtons)
            {
                var skillKey = GetSkillKeyFromButton(button);
                OnSkillSelected?.Invoke(skillKey);
            }
        }

        private void OnPlayButtonClickedHandler()
        {
            GetSelectedSkill();
            OnPlayButtonClicked?.Invoke();
        }

        private void Awake()
        {
            _playButton.onClick.AddListener(OnPlayButtonClickedHandler);
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClickedHandler);
        }

        private void Update()
        {
            if (selectedButtons.Count() == 2)
            {
                _playButton.interactable = true;
            }
            else
            {
                _playButton.interactable = false;
            }
        }

        private SkillKey GetSkillKeyFromButton(Button button)
        {
            var skillButton = _skillButtonList.FirstOrDefault(skill => skill.SkillButton == button);
            if (skillButton == null)
            {
                Debug.LogError($"Skill icon for {button} not found");
            }
            return skillButton.Key;
        }

        private void SelectButton(Button button)
        {
            selectedButtons = selectedButtons.Append(button).ToArray();
            var image = button.GetComponent<Image>();
            image.color = selectedColor;
            var outline = button.GetComponent<Outline>();
            outline.enabled = true;
        }

        private void DeselectButton(Button button)
        {
            selectedButtons = selectedButtons.Where(b => b != button).ToArray();
            var image = button.GetComponent<Image>();
            image.color = unselectedColor;
            var outline = button.GetComponent<Outline>();
            outline.enabled = false;
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