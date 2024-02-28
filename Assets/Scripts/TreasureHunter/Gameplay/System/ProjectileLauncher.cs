using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class ProjectileLauncher : MonoBehaviour
    {
        public Transform launchPoint;
        public GameObject projectilePrefab;

        public void FireProjectile()
        {
            GameObject projectile =
                Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
            Vector3 origScale = projectile.transform.localScale;
            // Projectile based on player's transform
            float sign = Mathf.Sign(transform.localScale.x);
            projectile.transform.localScale = new Vector3(
                origScale.x * sign,
                origScale.y,
                origScale.z
            );
        }
    }
}