using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button defaultButton;

    [Header("Data")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private bool isPaused;

    public static PauseManager instance;
    private void Awake()
    {
        // Singleton logic
        if (PauseManager.instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        canvasGroup = GetComponentInChildren<CanvasGroup>();
        defaultButton = GetComponentInChildren<Button>();
        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (!isPaused) Pause();
            else Resume();
        }
    }

    public void Pause()
    {
        isPaused = true;

        // Stop time
        Time.timeScale = 0f;

        // Pause sounds
        AudioListener.pause = true;

        // Select default button
        defaultButton.Select();

        // Make menu visible
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Resume()
    {
        isPaused = false;

        // Start time
        Time.timeScale = 1f;

        // Resume audio
        AudioListener.pause = false;

        // Make menu invisible
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void Restart()
    {
        // Resume first
        Resume();

        // Reload current scene
        TransitionManager.instance.ReloadScene();
    }

    public void MainMenu()
    {
        // Resume first
        Resume();

        // Tell game to return to main menu
        TransitionManager.instance.LoadMainMenuScene();
    }

    public void OpenSettings()
    {
        // Open settings
        // SettingsManager.instance.Open();
    }

    public void Quit()
    {
        // Debug
        Debug.Log("Player quit game.");

        // Quit game
        Application.Quit();
    }

    public bool IsPaused() => isPaused;
}
