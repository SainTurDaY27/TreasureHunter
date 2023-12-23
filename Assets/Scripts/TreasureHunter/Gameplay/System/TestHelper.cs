using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is basically the helper for testing scene.
 */
namespace TreasureHunter.Gameplay.System
{
    public class TestHelper : MonoBehaviour
    {
        private GameObject player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            // Actually, should not be called every frame but testing is probably fine.
            if (player.transform.position.y <= -60)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}