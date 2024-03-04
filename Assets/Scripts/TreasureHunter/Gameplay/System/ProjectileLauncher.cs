using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    public class ProjectileLauncher : MonoBehaviour
    {
        public Transform launchPoint;
        public GameObject projectilePrefab;

        public bool scaleFromUser = false;
        public bool fireBackward = false;

        public void FireProjectile()
        {
            GameObject projectile =
                Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
            Vector3 originalScale = projectile.transform.localScale;
            Vector3 actualScale;
            if (scaleFromUser)
            {
                actualScale = transform.localScale;
            }
            else
            {
                float sign = Mathf.Sign(transform.localScale.x);
                actualScale = new Vector3(originalScale.x * sign, originalScale.y, originalScale.z);
            }

            if (fireBackward)
            {
                actualScale.x *= -1;
            }

            // Projectile based on the user's transform
            projectile.transform.localScale = actualScale;
        }
    }
}