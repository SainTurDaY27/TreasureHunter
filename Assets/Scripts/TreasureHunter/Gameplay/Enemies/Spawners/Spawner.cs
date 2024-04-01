using System.Collections.Generic;
using TreasureHunter.Gameplay.Enemies.Attacks;
using TreasureHunter.Gameplay.Enemies.Movements;
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
            
            // Currently, there is only waypoint movement
            // TODO: Discuss on how to make this more generic
            var waypointMovement = enemy.GetComponent<WaypointsFlyingMovement>();
            if (waypointMovement != null)
            {
                waypointMovement.waypoints = waypoints;
                waypointMovement.ResetWaypoint();
            }
            
            // Player target
            // Currently, there are only circle flying and chase
            // TODO: Discuss on how to make more generic
            // Note: I think interface might be a great choice but I am not sure if it is a good
            // idea to use GetComponents or not.
            // Please review
            var circleFlyingMovement = enemy.GetComponent<CircleFlyingMovement>();
            var player = GameObject.FindWithTag("Player");
            if (circleFlyingMovement != null)
            {
                circleFlyingMovement.target = player.transform;
            }

            var flyingChargeAttack = enemy.GetComponent<FlyingChargeAttack>();
            if (flyingChargeAttack != null)
            {
                flyingChargeAttack.chargingTarget = player.transform;
            }


        }
    }
}