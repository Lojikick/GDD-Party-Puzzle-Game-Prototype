using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Dialogue introDialogue;

    [SerializeField] private DialogueUI ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = DialogueUI.instance;

        // Start monologue
        ui.Open(introDialogue);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ui.IsOpen())
            {
                if (ui.IsDone())
                {
                    // Close UI
                    ui.Close();

                    // Load bakery
                    TransitionManager.instance.LoadSelectedScene(1);
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
