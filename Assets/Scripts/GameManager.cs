using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int numberOfPuzzlesComplete;

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
        // Keep between scenes
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        // Initalize
        numberOfPuzzlesComplete = 0;
    }

    public void Reset()
    {
        numberOfPuzzlesComplete = 0;
    }

    public void IncrementNumPuzzles()
    {
        numberOfPuzzlesComplete++;
    }

    public int GetNumPuzzles()
    {
        return numberOfPuzzlesComplete;
    }
}
