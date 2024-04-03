using System;
using TreasureHunter.Core.Scene;
using TreasureHunter.Core.State.GameState;
using TreasureHunter.Core.UI;
using TreasureHunter.Gameplay.Player;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    [RequireComponent(typeof(Collider2D))]
    public class Teleporter : MonoBehaviour
    {
        public bool isTeleportToOtherMap;
        public string sceneName;
        public Vector2 teleportDestination = Vector2.zero;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isTeleportToOtherMap)
            {
                if (!other.CompareTag("Player")) return;
                var playerController = GetPlayerController(other);
                if (playerController == null) return;
                playerController.SetTeleportDestination(teleportDestination);
            }
            else if (isTeleportToOtherMap)
            {
                var oldDamageable = other.GetComponent<Damageable>();
                var oldPlayerController = other.GetComponent<PlayerController>();

                // lock player movement
                oldDamageable.LockVelocity = true;
                oldDamageable.BecomeInvincible();
                oldPlayerController.ZeroGravity = true;
                oldPlayerController.ResetVelocity();

                UIManager.Instance.FadeSceneTransition(true, () =>
                {
                    GameManager.Instance.PauseGame(true);
                    var oldHealth = oldDamageable.Health;
                    GameSceneManager.Instance.GoToScene(sceneName, () =>
                    {
                        GameManager.Instance.PauseGame(false);
                        UIManager.Instance.FadeSceneTransition(false);
                        var player = FindObjectOfType<PlayerController>();
                        player.transform.position = teleportDestination;

                        var newDamageable = player.GetComponent<Damageable>();
                        newDamageable.Health = oldHealth;

                        var gameState = GameStateManager.Instance.CurrentState;
                        gameState.OnStateIn();
                        newDamageable.LockVelocity = false;
                    });
                });
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            var playerController = GetPlayerController(other);
            if (playerController == null) return;
            playerController.RemoveTeleportDestination();
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