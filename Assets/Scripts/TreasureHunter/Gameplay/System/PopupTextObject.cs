using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class PopupTextObject : MonoBehaviour
    {
        [SerializeField]
        private GameObject popupText;

        private void Start()
        {
            popupText.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            popupText.SetActive(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            popupText.SetActive(false);
        }
    }
}