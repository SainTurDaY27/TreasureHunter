using TreasureHunter.Gameplay.Player;
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
            var playerController = GetPlayerController(other);
            if (playerController == null) return;
            popupText.SetActive(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            popupText.SetActive(false);
        }

        private PlayerController GetPlayerController(Collider2D other)
        {
            var playerController = other.GetComponent<PlayerController>();
            if (playerController == null)
            {
                Debug.LogWarning("The player somehow has no player controller");
                return playerController;
            };
            return playerController;
        }
    }
}