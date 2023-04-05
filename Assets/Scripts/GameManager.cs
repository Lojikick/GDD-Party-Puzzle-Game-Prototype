using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        // Singleton Logic
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Set the only instance to this
        instance = this;
    }

    private void Start()
    {
        // set start screen to visible
        SceneManager.LoadScene("Assets/Scenes/Title screen.unity");
    }
}
