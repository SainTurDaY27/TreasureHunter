using TreasureHunter.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunter.Gameplay.UI
{
    public class TreasureGetPanel : MonoBehaviour, IBaseUI
    {
        public Image[] treasureImages;
        public Button continueButton;
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}