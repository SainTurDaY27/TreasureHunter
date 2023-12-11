using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is basically the helper for testing scene.
 */
public class TestHelper : MonoBehaviour
{
    private GameObject player;
    
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Actually, should not be called every frame but testing is probably fine.
        if (player.transform.position.y <= -60)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
