using System.Collections.Generic;
using UnityEngine;

namespace TreasureHunter.Gameplay.Enemies.Spawners
{
    public abstract class Spawner : MonoBehaviour
    {
        public List<Transform> waypoints;

        /// <summary>
        /// This method is for evaluating spawn condition and then spawn enemy
        /// </summary>
        public abstract void SpawnEnemy();

        /// <summary>
        /// This method is for spawning an actual enemy and supply necessary information.
        /// </summary>
        /// <param name="enemyPrefab"></param>
        public void SpawnEnemyIntoScene(GameObject enemyPrefab)
        {
            var enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

            foreach (var behavior in enemy.GetComponents<ISetWaypointable>())
            {
                behavior.SetWaypoints(waypoints);
            }

            // Player target
            var player = GameObject.FindWithTag("Player");
            foreach (var behavior in enemy.GetComponents<ITargetable>())
            {
                behavior.SetTarget(player.transform);
            }
        }
    }
}