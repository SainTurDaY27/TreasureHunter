using System;
using TreasureHunter.Gameplay.Player;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class SavePoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            var playerController = other.GetComponent<PlayerController>();
            if (playerController == null)
            {
                Debug.LogError("The player does not have player controller");
                return;
            }

            playerController.canSave = true;

        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            var playerController = other.GetComponent<PlayerController>();
            if (playerController == null)
            {
                Debug.LogError("The player does not have player controller");
                return;
            }

            playerController.canSave = false;
        }
    }
}