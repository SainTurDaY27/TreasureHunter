using System;
using System.Collections.Generic;
using System.Linq;
using TreasureHunter.Gameplay.Player;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class SpikeRecover : MonoBehaviour
    {
        public Transform CurrentRespawnPoint { private get; set; }
        [SerializeField] private List<Transform> _respawnPoints = new();

        private void Awake()
        {
            // Only spawn point
            foreach (var child in GetComponentsInChildren<SpikeRecoverPoint>())
            {
                _respawnPoints.Add(child.transform);
            }
        }

        public void RecoverPlayer()
        {
            var player = GameObject.FindWithTag("Player");
            if (CurrentRespawnPoint)
            {
                // Respawn to the current recover point
                player.transform.position = CurrentRespawnPoint.position;
            }
            else
            {
                if (_respawnPoints.Count == 0)
                {
                    Debug.LogError("No recover point found. You must add one if there is a spike or hazard.");
                }
                else
                {
                    // Find the closest recover point
                    var closestPoint = _respawnPoints[0];
                    var playerPosition = player.transform.position;
                    var closestDistance = Vector2.Distance(closestPoint.position, playerPosition);
                    foreach (var point in _respawnPoints)
                    {
                        var currentDistance = Vector2.Distance(point.position, playerPosition);
                        if (currentDistance >= closestDistance) continue;
                        closestDistance = currentDistance;
                        closestPoint = point;
                    }

                    player.transform.position = closestPoint.position;
                }
            }
        }
    }
}