using System;
using TreasureHunter.Core.Scene;
using TreasureHunter.Core.State.GameState;
using TreasureHunter.Gameplay.Player;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    [RequireComponent(typeof(Collider2D))]
    public class LevelExit : MonoBehaviour
    {
        public float exitX, exitY;
        public string sceneName;
        public bool willFacingRight = true;


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var oldDamageable = other.GetComponent<Damageable>();
                var oldHealth = oldDamageable.Health;
                GameSceneManager.Instance.GoToScene(sceneName, () =>
                {
                    var player = FindObjectOfType<PlayerController>();
                    player.transform.position = new Vector2(exitX, exitY);
                    if (player.IsFacingRight && !willFacingRight)
                    {
                        player.IsFacingRight = false;
                    }

                    var newDamageable = player.GetComponent<Damageable>();
                    newDamageable.Health = oldHealth;

                    // assume can only exit using game state model.
                    var gameState = GameStateManager.Instance.CurrentState;
                    gameState.OnStateIn();
                });
            }
        }
    }
}