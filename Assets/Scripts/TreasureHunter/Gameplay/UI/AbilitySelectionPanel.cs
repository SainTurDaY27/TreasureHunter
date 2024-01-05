using System.Collections;
using System.Collections.Generic;
using TreasureHunter.Core.UI;
using UnityEngine;

namespace TreasureHunter.Gameplay.UI
{
    public class AbilitySelectionPanel : MonoBehaviour, IBaseUI
    {
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}