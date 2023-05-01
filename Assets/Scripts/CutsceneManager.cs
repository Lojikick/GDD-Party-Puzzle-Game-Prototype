using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Dialogue introDialogue;
    [SerializeField] private Dialogue funeralDialogue;
    [SerializeField] private DialogueUI ui;

    

    // Start is called before the first frame update
    public void Start()
    {
        ui = DialogueUI.instance;

       
    }

    //Cutscene manager Handles loading the dialouge for the IntroCutscene and Funeral Cutscene

    public void StartCutscene()
    {
         // Start monologue
        ui.Open(introDialogue);
    }

    public void funeralCutscene()
    {
         // Start monologue
        ui.Open(funeralDialogue);
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
