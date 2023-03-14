using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Dialogue currentDialogue;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI textbox;
    [SerializeField] private Image portraitImage;
    [SerializeField] private float textSpeed;
    [SerializeField] private int currentIndex;

    [Header("Debugging")]
    public Dialogue test;

    private Coroutine routine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Open(test);
        }
    }

    // Open UI
    public void Open(Dialogue dialogue)
    {
        this.currentDialogue = dialogue;

        // Show UI
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        portraitImage.sprite = dialogue.portraitSprite;

        // Write first message
        FirstMessage();
    }

    // Close UI
    public void Close()
    {
        // Hide UI
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        this.currentDialogue = null;
    }

    public void FirstMessage()
    {
        currentIndex = 0;

        DisplayCurrentMessage();
    }

    public void NextMessage()
    {
        // Check if we are in the middle of the current dialogue
        if (routine != null)
        {
            // Display entire message
            textbox.text = currentDialogue[currentIndex];

            // Stop printing
            StopCoroutine(routine);
            routine = null;
        }
        else 
        {
            // If we are at the end of dialogue
            if (currentIndex == currentDialogue.length - 1)
            {
                Close();
            }
            // Else show next message
            else
            {
                currentIndex++;
                DisplayCurrentMessage();
            }
        }
    }

    public void PreviousMessage()
    {
        // Decrement index until 0
        currentIndex = Mathf.Max(0, currentIndex - 1);

        DisplayCurrentMessage();
    }

    public void LastMessage()
    {
        currentIndex = currentDialogue.length - 1;

        DisplayCurrentMessage();
    }

    private IEnumerator FadeIn(float duration)
    {
        canvasGroup.alpha = 0f;

        float elapsed = 0;
        while (elapsed < duration)
        {
            // Lerp alpha
            canvasGroup.alpha = elapsed / duration;

            // Increment time
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final values
        canvasGroup.alpha = 1f;
    }

    private void DisplayCurrentMessage()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(WriteMessageOverTime(currentDialogue[currentIndex]));
    }

    private IEnumerator WriteMessageOverTime(string message)
    {
        // Clear text
        textbox.text = "";

        // Write one character at a time, waiting between each char
        foreach (char c in message.ToCharArray())
        {
            textbox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        // Clear routine
        routine = null;
    }
}
