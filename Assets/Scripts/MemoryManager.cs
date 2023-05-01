using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For Memory cutscenes if we have time
public class MemoryManager : MonoBehaviour
{
    [SerializeField] private Dialogue garyMemory;
    [SerializeField] private Dialogue magMemory;
    [SerializeField] private Dialogue oliveMemory;
    [SerializeField] private int puzzlesComplete;
    [SerializeField] private int pplTalked;
    [SerializeField] private DialogueUI ui;

    // Start is called before the first frame update
    public void Start()
    {
        ui = DialogueUI.instance;
        puzzlesComplete = GameManager.instance.GetNumPuzzles();
        pplTalked = GameManager.instance.GetPplTalked();
        if(puzzlesComplete == 1){
            ui.Open(garyMemory);
        } else if (puzzlesComplete == 2){
            ui.Open(magMemory);
        } else if (puzzlesComplete == 3){
            ui.Open(oliveMemory);
        }
        

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ui.IsOpen())
            {
                if (ui.IsDone())
                {
                    ui.Close();
                    //if(GameManager.instance.GetPplTalked() > 4){
                        //TransitionManager.instance.LoadSelectedScene(4);
                    //} else { 
                    TransitionManager.instance.LoadSelectedScene(2);
                    //}
                }
                else
                {
                    // Increment message
                    ui.NextMessage();
                }
            }
            // else
            // {
            //     // If UI is FULLY closed
            //     if (ui.IsClosed())
            //     {
            //         // Open dialogue UI
            //         ui.Open(introDialogue);
            //     }
            //     else if (ui.IsOpening())
            //     {

            //     }
            //     else
            //     {

            //     }
            // }

        }
    }
}

