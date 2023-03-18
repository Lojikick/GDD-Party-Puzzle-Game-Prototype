using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Dialogue currentDialogue;
    [SerializeField] private CanvasGroup dialogueBoxCanvasGroup;
    [SerializeField] private Image leftCharacterImage;
    [SerializeField] private Image rightCharacterImage;
    [SerializeField] private TextMeshProUGUI nameTextBox;
    [SerializeField] private TextMeshProUGUI messsageTextBox;

    [Header("Settings")]
    [SerializeField] private Material grayScaleMaterial;
    [SerializeField] private float uiFadeDuration;
    [SerializeField] private float characterFadeDuration;
    [SerializeField] private float textSpeed;

    [Header("Debugging")]
    [SerializeField] private int currentIndex;
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
        // Error checking
        if (dialogue == null) throw new System.Exception("ATTEMPTED TO OPEN NULL DIALOGUE!");

        this.currentDialogue = dialogue;

        // Inital setup
        leftCharacterImage.sprite = dialogue.initalLeftCharacterSprite;
        leftCharacterImage.material = grayScaleMaterial;
        rightCharacterImage.sprite = dialogue.initalRightCharacterSprite;
        rightCharacterImage.material = grayScaleMaterial;

        // Show characters
        StartCoroutine(FadeUIIn(uiFadeDuration));

        // Write first message
        FirstMessage();
    }

    // Close UI
    public void Close()
    {
        // Hide UI
        StartCoroutine(FadeUIOut(uiFadeDuration));

        this.currentDialogue = null;
    }

    public void FirstMessage()
    {
        currentIndex = 0;

        DisplayCurrentMessage(currentDialogue[currentIndex]);
    }

    public void NextMessage()
    {
        // Check if we are in the middle of the current dialogue
        if (routine != null)
        {
            // Display entire message
            messsageTextBox.text = currentDialogue[currentIndex].text;

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
                DisplayCurrentMessage(currentDialogue[currentIndex]);
            }
        }
    }

    public void PreviousMessage()
    {
        // Decrement index until 0
        currentIndex = Mathf.Max(0, currentIndex - 1);

        DisplayCurrentMessage(currentDialogue[currentIndex]);
    }

    public void LastMessage()
    {
        currentIndex = currentDialogue.length - 1;

        DisplayCurrentMessage(currentDialogue[currentIndex]);
    }

    private IEnumerator FadeUIIn(float duration)
    {
        // Set starting values
        dialogueBoxCanvasGroup.alpha = 0f;

        float elapsed = 0;
        while (elapsed < duration)
        {
            float ratio = elapsed / duration;

            // Lerp values
            dialogueBoxCanvasGroup.alpha = ratio;

            // Increment time
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final values
        dialogueBoxCanvasGroup.alpha = 1f;
        dialogueBoxCanvasGroup.interactable = true;
        dialogueBoxCanvasGroup.blocksRaycasts = true;

        // Fade characters in
        yield return FadeCharactersIn(0.95f, 1f, characterFadeDuration);
    }

    private IEnumerator FadeUIOut(float duration)
    {
        // Fade characters out first
        yield return FadeCharactersOut(1f, 0.95f, characterFadeDuration);

        // Set starting values
        dialogueBoxCanvasGroup.alpha = 1f;
        dialogueBoxCanvasGroup.interactable = false;
        dialogueBoxCanvasGroup.blocksRaycasts = false;

        float elapsed = 0;
        while (elapsed < duration)
        {
            float ratio = elapsed / duration;

            // Lerp values
            dialogueBoxCanvasGroup.alpha = 1 - ratio;

            // Increment time
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final values
        dialogueBoxCanvasGroup.alpha = 0f;
    }

    private IEnumerator FadeCharactersIn(float startScale, float finalScale, float duration)
    {
        // Set starting values
        leftCharacterImage.transform.localScale = Vector3.one * startScale;
        var leftColor = leftCharacterImage.color;
        leftColor.a = 0f;

        rightCharacterImage.transform.localScale = Vector3.one * startScale;
        var rightColor = rightCharacterImage.color;
        rightColor.a = 0f;

        float elapsed = 0;
        while (elapsed < duration)
        {
            float ratio = elapsed / duration;

            // Lerp values
            leftCharacterImage.transform.localScale = Vector3.one * (startScale + (finalScale - startScale) * ratio);
            leftColor.a = ratio;
            leftCharacterImage.color = leftColor;

            rightCharacterImage.transform.localScale = Vector3.one * (startScale + (finalScale - startScale) * ratio);
            rightColor.a = ratio;
            rightCharacterImage.color = rightColor;

            // Increment time
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final values
        leftCharacterImage.transform.localScale = Vector3.one * finalScale;
        leftColor.a = 1f;
        leftCharacterImage.color = leftColor;

        rightCharacterImage.transform.localScale = Vector3.one * finalScale;
        rightColor.a = 1f;
        rightCharacterImage.color = rightColor;
    }

    private IEnumerator FadeCharactersOut(float startScale, float finalScale, float duration)
    {
        // Set starting values
        leftCharacterImage.transform.localScale = Vector3.one * startScale;
        var leftColor = leftCharacterImage.color;
        leftColor.a = 1f;

        rightCharacterImage.transform.localScale = Vector3.one * startScale;
        var rightColor = rightCharacterImage.color;
        rightColor.a = 1f;

        float elapsed = 0;
        while (elapsed < duration)
        {
            float ratio = elapsed / duration;

            // Lerp values
            leftCharacterImage.transform.localScale = Vector3.one * (startScale + (finalScale - startScale) * ratio);
            leftColor.a = 1 - ratio;
            leftCharacterImage.color = leftColor;

            rightCharacterImage.transform.localScale = Vector3.one * (startScale + (finalScale - startScale) * ratio);
            rightColor.a = 1 - ratio;
            rightCharacterImage.color = rightColor;

            // Increment time
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final values
        leftCharacterImage.transform.localScale = Vector3.one * finalScale;
        leftColor.a = 0f;
        leftCharacterImage.color = leftColor;

        rightCharacterImage.transform.localScale = Vector3.one * finalScale;
        rightColor.a = 0f;
        rightCharacterImage.color = rightColor;
    }

    private void DisplayCurrentMessage(DialogueMessage message)
    {
        // Change name
        nameTextBox.text = message.name;

        // Change character sprite
        if (message.isRightCharacter)
        {
            // Apply bounce effect if new sprite
            if (rightCharacterImage.sprite != message.characterSprite)
            {
                // TODO
                LeanTween.moveLocalY(rightCharacterImage.gameObject, 20f, 0.5f);
                LeanTween.moveLocalY(rightCharacterImage.gameObject, -20f, 0.5f);
            }

            rightCharacterImage.sprite = message.characterSprite;
            rightCharacterImage.material = null;

            // Gray out other character
            leftCharacterImage.material = grayScaleMaterial;
        }
        else 
        {
            // Apply bounce effect if new sprite
            if (leftCharacterImage.sprite != message.characterSprite)
            {
                // TODO
                // LeanTween.moveLocalY(leftCharacterImage.gameObject, 20f, 0.5f).setEaseInOutBounce();
            }

            leftCharacterImage.sprite = message.characterSprite;
            leftCharacterImage.material = null;

            // Gray out other character
            rightCharacterImage.material = grayScaleMaterial;
        }

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(WriteMessageOverTime(message.text));
    }

    private IEnumerator WriteMessageOverTime(string message)
    {
        // Clear text
        messsageTextBox.text = "";

        // Write one character at a time, waiting between each char
        foreach (char c in message.ToCharArray())
        {
            messsageTextBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        // Clear routine
        routine = null;

    }
}
