using System;
using TreasureHunter.Core.Scene;
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
                GameSceneManager.Instance.GoToScene(sceneName, () =>
                {
                    var player = FindObjectOfType<PlayerController>();
                    player.transform.position = new Vector2(exitX, exitY);
                    if (player.IsFacingRight && !willFacingRight)
                    {
                        player.IsFacingRight = false;
                    }
                });
            }
        }
    }
}