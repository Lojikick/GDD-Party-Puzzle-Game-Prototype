using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamplaySceneManager : MonoBehaviour
{
    [SerializeField] private Dialogue introDialogue;
    [SerializeField] private Dialogue funeralDialogue;
    [SerializeField] private Dialogue garyOrder;
    [SerializeField] private Dialogue magOrder;
    [SerializeField] private Dialogue oliveOrder;
    [SerializeField] private Dialogue ascension;
    [SerializeField] private Dialogue finale;
    [SerializeField] private DialogueUI ui;
    [SerializeField] private int puzzlesComplete;
    [SerializeField] private int pplTalked;
    [SerializeField] private bool gameComplete;
    [SerializeField] private CookbookMenuUI book;

    // Start is called before the first frame update
    public void Start()
    {
        book = CookbookMenuUI.instance;
        ui = DialogueUI.instance;
        puzzlesComplete = GameManager.instance.GetNumPuzzles();
        pplTalked = GameManager.instance.GetPplTalked();
        gameComplete = GameManager.instance.GetCompletionStatus();

        //Tracks if npcs interacted with are less than 4, open Intro scene then
        if (pplTalked < 4){
            ui.Open(introDialogue);
        } else {
            //Tracks which dialouge orders to open with based on puzzles completed
            if(puzzlesComplete == 0){
                ui.Open(garyOrder);
            } else if (puzzlesComplete == 1){
                ui.Open(magOrder);
            } else if (puzzlesComplete == 2){
                ui.Open(oliveOrder);
            } else if (puzzlesComplete == 3){
                GameManager.instance.completeGame();
                ui.Open(ascension);
            //} else if (puzzlesComplete == 3 && gameComplete){
                //ui.Open(finale);
            }
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // (book.IsOpen()){
                //Debug.Log("Is this thing even on? Sequel");
            //}
            if (ui.IsOpen())
            {
                if (ui.IsDone())
                {
                    ui.Close();
                    //if(GameManager.instance.GetPplTalked() > 4){
                        //TransitionManager.instance.LoadSelectedScene(4);
                    //} else { 
                        //TransitionManager.instance.LoadSelectedScene(2);
                    //}
                }
                else
                {
                    // Increment message
                    ui.NextMessage();
                }
            }
        }
    }
}

