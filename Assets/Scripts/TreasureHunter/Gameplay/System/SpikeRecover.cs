using System;
using System.Collections.Generic;
using System.Linq;
using TreasureHunter.Gameplay.Player;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class SpikeRecover : MonoBehaviour
    {
        public Transform CurrentRecoverPoint { private get; set; }
        private readonly List<Transform> _recoverPoints = new();

        private void Awake()
        {
            // Only spawn point
            foreach (var child in GetComponentsInChildren<SpikeRecoverPoint>())
            {
                _recoverPoints.Add(child.transform);
            }
        }

        public void RecoverPlayer()
        {
            var player = GameObject.FindWithTag("Player");
            if (CurrentRecoverPoint)
            {
                // Respawn to the current recover point
                player.transform.position = CurrentRecoverPoint.position;
            }
            else
            {
                if (_recoverPoints.Count == 0)
                {
                    Debug.LogError("No recover point found. You must add one if there is a spike or hazard.");
                }
                else
                {
                    // Find the closest recover point
                    var closestPoint = _recoverPoints[0];
                    var playerPosition = player.transform.position;
                    var closestDistance = Vector2.Distance(closestPoint.position, playerPosition);
                    foreach (var point in _recoverPoints)
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