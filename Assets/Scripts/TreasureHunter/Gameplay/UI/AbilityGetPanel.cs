using TMPro;
using TreasureHunter.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.UI
{
    public class AbilityGetPanel : MonoBehaviour, IBaseUI
    {
        public string abilityName, abilityToolTip;
        public Sprite abilitySprite;

        public TMP_Text uiText, uiToolTip;
        public Image uiImage;
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
            // Set UI
            uiText.text = abilityName;
            uiToolTip.text = abilityToolTip;
            uiImage.sprite = abilitySprite;
        }
    }
}