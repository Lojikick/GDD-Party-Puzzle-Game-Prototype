using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int numberOfPuzzlesComplete;
    //Setup for when player talks to four NPCs, funeral cutscene plays (Did some weird work arounds for edgecases, explained below)
    [SerializeField] private int npcsTalkedTo;
    [SerializeField] private bool gameComplete;


    public static GameManager instance;

    public GameState State;

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

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.IntroSequence:
                break;
            case GameState.GreetingSequence:
                //Debug.Log(State);
                //Upon Switching Gamestate, switch to funeral scene 
                break;
            case GameState.FuneralSequence:
                TransitionManager.instance.LoadSelectedScene(3);
                //Debug.Log(State);
                //Upon Switching Gamestate, switch to funeral scene 
                break;
            case GameState.GarySequence:
                break;
            case GameState.MagnoliaSequence:
                break;
            case GameState.OliveSequence:
                break;
            case GameState.EndGameSequence:
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        // Initalize
        numberOfPuzzlesComplete = 0;
        npcsTalkedTo = 0;
        gameComplete = false;
        UpdateGameState(GameState.IntroSequence);

    }

    private void Update()
    {

    }

    public void Reset()
    {
        numberOfPuzzlesComplete = 0;
        npcsTalkedTo = 0;
        gameComplete = false;
        UpdateGameState(GameState.IntroSequence);
    }


    public void CompletePuzzle()
    {
        // Increment score
        numberOfPuzzlesComplete++;

        // Change scene
        //Load memory world
        TransitionManager.instance.LoadSelectedScene(1);
    }

    //This was for ending cutscnes, dont worry abt that now lol
    public void completeGame()
    {
        gameComplete = true;
    }

    public void personTalked()
    {
        // Increment score
        npcsTalkedTo++;
        //Checks if the NPCs that were talked to are = to 5 (Really 4, but I called this func again so that the Gamestate would change and trigger the cutscene once the player talks to a fourth person)
        if (npcsTalkedTo == 5)
        {
            UpdateGameState(GameState.FuneralSequence);

        }
        //Debug.Log(npcsTalkedTo);
    }

    public int GetPplTalked()
    {
        // Increment score
        return npcsTalkedTo;
    }
    public int GetNumPuzzles()
    {
        return numberOfPuzzlesComplete;
    }

    public bool GetCompletionStatus()
    {
        return gameComplete;
    }

    public enum GameState
    {
        IntroSequence,
        GreetingSequence,
        FuneralSequence,
        GarySequence,
        MagnoliaSequence,
        OliveSequence,
        EndGameSequence,
    }

}


