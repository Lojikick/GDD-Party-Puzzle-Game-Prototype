using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform maskTransform;
    [SerializeField] private Transform backgroundTransform;

    [Header("Data")]
    [SerializeField] private float transitionTime = 1f;
    private Coroutine coroutine;

    public static TransitionManager instance;
    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        OpenScene();
    }

    public int GetSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    private void PlayBackgroundMusic(int buildIndex)
    {
        if (buildIndex == 0)
        {
            // Play title theme
            AudioManager.instance.PlayMusic("Title Screen");
        }
        else if (buildIndex == 1 || buildIndex == 2)
        {
            // Play bakery theme
            AudioManager.instance.PlayMusic("Overworld");
        }
        else if (buildIndex == 3)
        {
            // Play bakery theme
            AudioManager.instance.PlayMusic("Funeral Song");
        }
        else
        {
            // Play puzzle theme
            AudioManager.instance.PlayMusic("Puzzleworld");
        }
    }

    private void StopBackgroundMusic(int buildIndex)
    {
        if (buildIndex == 0)
        {
            // Stop title theme
            AudioManager.instance.StopMusic("Title Screen");
        }
        else if (buildIndex == 1 || buildIndex == 2)
        {
            // Stop bakery theme
            AudioManager.instance.StopMusic("Overworld");
        }
        else if (buildIndex == 3)
        {
            // Play bakery theme
            AudioManager.instance.StopMusic("Funeral Song");
        }
        else
        {
            // Stop puzzle theme
            AudioManager.instance.StopMusic("Puzzleworld");
        }
    }

    public void OpenScene()
    {
        // Play animation
        animator.Play("Transition In");

        // Play appropriate music
        PlayBackgroundMusic(GetSceneIndex());
    }

    public void LoadNextScene()
    {
        // Stop any transition if one was happening
        if (coroutine != null) StopCoroutine(coroutine);

        // Transition to next scene
        coroutine = StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadSelectedScene(int buildIndex)
    {
        // Stop any transition if one was happening
        if (coroutine != null) StopCoroutine(coroutine);

        // Transition to next scene
        coroutine = StartCoroutine(LoadScene(buildIndex));
    }

    public void ReloadScene()
    {
        // Stop any transition if one was happening
        if (coroutine != null) StopCoroutine(coroutine);

        // Transition to same scene
        coroutine = StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadMainMenuScene()
    {
        // Stop any transition if one was happening
        if (coroutine != null) StopCoroutine(coroutine);

        // Transition to main menu, scene 0
        coroutine = StartCoroutine(LoadScene(0));
    }

    private IEnumerator LoadScene(int index)
    {
        // Stop any music
        StopBackgroundMusic(GetSceneIndex());

        // Play animation
        animator.Play("Transition Out");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Check if next scene exists
        int maxCount = SceneManager.sceneCountInBuildSettings;
        if (index < maxCount)
        {
            // Load scene
            SceneManager.LoadScene(index);
        }
        else
        {
            // Debug
            print("Could not find scene " + index);

            // Load scene 0
            SceneManager.LoadScene(0);
        }
    }
}