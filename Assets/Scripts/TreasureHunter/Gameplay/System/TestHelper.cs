using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreasureHunter.Gameplay.Enemies;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is basically the helper for testing scene.
 */
namespace TreasureHunter.Gameplay.System
{
    public class TestHelper : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _tempHealthTextUI;
        private GameObject player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            // TODO: For testing purposes only
            SetTempHealthTextUI();
            player.GetComponent<Damageable>().healthChange.AddListener((health, maxHealth) =>
            {
                SetTempHealthTextUI();
            });
        }

        private void Update()
        {
            // Actually, should not be called every frame but testing is probably fine.
            if (player.transform.position.y <= -60)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public void SetTempHealthTextUI()
        {
            _tempHealthTextUI.text = $"Health: {player.GetComponent<Damageable>().Health}";
        }
    }
}