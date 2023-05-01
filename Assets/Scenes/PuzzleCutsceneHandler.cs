using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCutsceneHandler : MonoBehaviour
{
    [SerializeField] private Dialogue willMonologue;
    [SerializeField] private DialogueUI ui;

    // Start is called before the first frame update
    public void Start()
    {
        ui = DialogueUI.instance;
        //if(GameManager.instance.GetNumPuzzles()==0){
        ui.Open(willMonologue);
        //}
       
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
