using System.Collections;
using System.Collections.Generic;
using TreasureHunter.Core.UI;
using UnityEngine;

namespace TreasureHunter.Gameplay.UI
{
    public class MapPanel : MonoBehaviour, IBaseUI
    {
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}