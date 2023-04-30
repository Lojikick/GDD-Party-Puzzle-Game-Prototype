using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button defaultButton;

    public static MainMenuManager instance;
    private void Awake()
    {
        // Singleton Logic
        if (MainMenuManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        defaultButton = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        // Open scene in center
        TransitionManager.instance.OpenScene();
        AudioManager.instance.PlayMusic("Title Screen Theme");

        // Highlight button
        defaultButton.Select();
    }

    public void StartGame()
    {
        // Debug
        print("Start Game");

        // Load next scene
        TransitionManager.instance.LoadNextScene();
        AudioManager.instance.StopMusic("Title Screen Theme");
    }

    public void HowToPlay()
    {
        // Debug
        print("Open Help Screen");
    }

    public void QuitGame()
    {
        // Debug
        print("Quit Game");

        // Close application
        Application.Quit();
    }

}
