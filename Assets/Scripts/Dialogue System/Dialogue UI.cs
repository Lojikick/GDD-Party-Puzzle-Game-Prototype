using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueUI : MonoBehaviour
{
    private enum DialogueState { Opening, Open, Closing, Closed }

    [Header("Components")]
    [SerializeField] private Dialogue currentDialogue;
    [SerializeField] private CanvasGroup dialogueBoxCanvasGroup;
    [SerializeField] private Image leftCharacterImage;
    [SerializeField] private Image rightCharacterImage;
    [SerializeField] private Image nextArrowImage;
    [SerializeField] private TextMeshProUGUI nameTextBox;
    [SerializeField] private TextMeshProUGUI messsageTextBox;
    [SerializeField] private Material grayScaleMaterial;

    [Header("Settings")]
    [SerializeField] private float textSpeed;

    [Header("ADjustable Settings")]
    [SerializeField] private float uiFadeDuration;
    [SerializeField] private float characterFadeDuration;
    [SerializeField] private float characterSpriteChangeBouncePower;
    [SerializeField] private float characterSpriteChangeDuration;
    [SerializeField] private float characterDimAlpha;

    [Header("Debugging")]
    [SerializeField] private int currentIndex;
    [SerializeField] private DialogueState state;

    private Coroutine routine;

    public static DialogueUI instance;
    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        state = DialogueState.Closed;
    }

    // Open UI
    public void Open(Dialogue dialogue)
    {
        // Error checking

        if (dialogue == null) throw new System.Exception("CANNOT OPEN NULL DIALOGUE!");
        if (this.currentDialogue == dialogue) return;

        this.currentDialogue = dialogue;

        // Setup character values
        IntialSetup(currentDialogue);

        // Change state
        state = DialogueState.Opening;

        // Show characters
        StartCoroutine(FadeUIIn(uiFadeDuration));
    }

    // Close UI
    public void Close()
    {
        // Change state
        state = DialogueState.Closing;
        // Hide UI
        StartCoroutine(FadeUIOut(uiFadeDuration));
    }

    public bool IsOpen() => state == DialogueState.Open;
    public bool IsOpening() => state == DialogueState.Opening;
    public bool IsClosed() => state == DialogueState.Closed;
    public bool IsDone() => currentIndex >= currentDialogue.length - 1;


    // This is called via buttons
    public void FirstMessage()
    {
        currentIndex = 0;

        DisplayCurrentMessage(currentDialogue[currentIndex]);
    }

    // This is called via buttons
    public void NextMessage()
    {
        // Make sure nothing happens
        if (!dialogueBoxCanvasGroup.interactable) return;

        // Check if we are in the middle of the current dialogue
        if (routine != null)
        {
            // Display entire message
            messsageTextBox.text = currentDialogue[currentIndex].text;

            // Show arrow
            ShowArrow();

            // Stop printing routine
            StopCoroutine(routine);
            routine = null;
        }
        else
        {
            // Fix
            if (state != DialogueState.Open) return;

            // Hide arrow
            HideArrow();

            // If we are at the end of dialogue
            if (currentIndex == currentDialogue.length - 1)
            {
                // Do nothing
            }
            // Else show next message
            else
            {
                currentIndex++;
                DisplayCurrentMessage(currentDialogue[currentIndex]);
            }
        }
    }

    // This is called via buttons
    public void PreviousMessage()
    {
        // Decrement index until 0
        currentIndex = Mathf.Max(0, currentIndex - 1);

        DisplayCurrentMessage(currentDialogue[currentIndex]);
    }

    // This is called via buttons
    public void LastMessage()
    {
        currentIndex = currentDialogue.length - 1;

        DisplayCurrentMessage(currentDialogue[currentIndex]);
    }

    #region Helper Functions

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

        // Fade characters in
        yield return FadeCharactersIn(0.95f, 1f, characterFadeDuration);

        // Allow interactivity
        dialogueBoxCanvasGroup.interactable = true;
        dialogueBoxCanvasGroup.blocksRaycasts = true;

        // Display first message now
        FirstMessage();

        // Change state
        state = DialogueState.Open;
    }

    private IEnumerator FadeUIOut(float duration)
    {
        // Set starting values
        dialogueBoxCanvasGroup.alpha = 1f;
        dialogueBoxCanvasGroup.interactable = false;
        dialogueBoxCanvasGroup.blocksRaycasts = false;

        // Fade characters out first
        yield return FadeCharactersOut(1f, 0.95f, characterFadeDuration);

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

        // Now remove dialogue
        this.currentDialogue = null;

        // Change state
        state = DialogueState.Closed;
    }

    private IEnumerator FadeCharactersIn(float startScale, float finalScale, float duration)
    {
        // Set starting values
        leftCharacterImage.transform.localScale = Vector3.one * startScale;
        leftCharacterImage.color = Color.clear;

        rightCharacterImage.transform.localScale = Vector3.one * startScale;
        rightCharacterImage.color = Color.clear;

        float elapsed = 0;
        while (elapsed < duration)
        {
            float ratio = elapsed / duration;

            // Lerp values
            leftCharacterImage.transform.localScale = Vector3.one * Mathf.Lerp(startScale, finalScale, ratio);
            leftCharacterImage.color = Color.Lerp(Color.clear, Color.white, ratio);

            rightCharacterImage.transform.localScale = Vector3.one * Mathf.Lerp(startScale, finalScale, ratio);
            rightCharacterImage.color = Color.Lerp(Color.clear, Color.white, ratio);

            // Increment time
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final values
        leftCharacterImage.transform.localScale = Vector3.one * finalScale;
        leftCharacterImage.color = Color.white;

        rightCharacterImage.transform.localScale = Vector3.one * finalScale;
        rightCharacterImage.color = Color.white;
    }

    private IEnumerator FadeCharactersOut(float startScale, float finalScale, float duration)
    {
        // Set starting values
        leftCharacterImage.transform.localScale = Vector3.one * startScale;
        var leftColor = leftCharacterImage.color;

        rightCharacterImage.transform.localScale = Vector3.one * startScale;
        var rightColor = rightCharacterImage.color;

        float elapsed = 0;
        while (elapsed < duration)
        {
            float ratio = elapsed / duration;

            // Lerp values
            leftCharacterImage.transform.localScale = Vector3.one * (startScale + (finalScale - startScale) * ratio);
            leftCharacterImage.color = Color.Lerp(leftColor, Color.clear, ratio);

            rightCharacterImage.transform.localScale = Vector3.one * (startScale + (finalScale - startScale) * ratio);
            rightCharacterImage.color = Color.Lerp(rightColor, Color.clear, ratio);

            // Increment time
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final values
        leftCharacterImage.transform.localScale = Vector3.one * finalScale;
        leftCharacterImage.color = Color.clear;

        rightCharacterImage.transform.localScale = Vector3.one * finalScale;
        rightCharacterImage.color = Color.clear;
    }

    private void DisplayCurrentMessage(DialogueMessage message)
    {
        // Change name
        nameTextBox.text = message.name;

        // Play sfx if it exists
        if (message.sfx != "")
        {
            // Play
            AudioManager.instance.PlaySound(message.sfx);
        }

        // If sprite is empty, the assume dialogue is narration
        if (message.characterSprite == null)
        {
            // messsageTextBox.fontStyle = FontStyles.Italic;

            DimCharacter(rightCharacterImage);
            DimCharacter(leftCharacterImage);
        }
        else
        {
            // Set font style
            if (message.italicized)
                messsageTextBox.fontStyle = FontStyles.Italic;
            else if (message.bold)
                messsageTextBox.fontStyle = FontStyles.Bold;
            else
                messsageTextBox.fontStyle = FontStyles.Normal;

            // Change character sprite
            if (message.isRightCharacter)
            {
                // If new sprite 
                if (rightCharacterImage.sprite != message.characterSprite)
                {
                    ApplyBounceEffect(rightCharacterImage.gameObject);
                }

                rightCharacterImage.sprite = message.characterSprite;

                EmphasizeCharacter(rightCharacterImage);
                DimCharacter(leftCharacterImage);
            }
            else
            {
                // If new sprite 
                if (leftCharacterImage.sprite != message.characterSprite)
                {
                    ApplyBounceEffect(leftCharacterImage.gameObject);
                }

                // Update sprite
                leftCharacterImage.sprite = message.characterSprite;

                EmphasizeCharacter(leftCharacterImage);
                DimCharacter(rightCharacterImage);
            }
        }

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(WriteMessageOverTime(message.text));
    }

    private void IntialSetup(Dialogue dialogue)
    {
        // Hide arrow
        HideArrow();

        // Clear any messages
        nameTextBox.text = "";
        messsageTextBox.text = "";

        Color color;

        leftCharacterImage.sprite = dialogue.initalLeftCharacterSprite;
        leftCharacterImage.material = grayScaleMaterial;
        // Changle alpha
        color = leftCharacterImage.color;
        color.a = 0f;
        leftCharacterImage.color = color;

        rightCharacterImage.sprite = dialogue.initalRightCharacterSprite;
        leftCharacterImage.material = grayScaleMaterial;
        // Changle alpha
        color = rightCharacterImage.color;
        color.a = 0f;
        rightCharacterImage.color = color;
    }

    private void ApplyBounceEffect(GameObject gameObject)
    {
        var offset = characterSpriteChangeBouncePower;
        var newY = gameObject.transform.position.y + offset;
        LeanTween.moveY(gameObject, newY, characterSpriteChangeDuration).setEasePunch();
    }

    private void EmphasizeCharacter(Image characterImage)
    {
        // Remove grayscale
        characterImage.material = null;

        // Restore alpha
        var color = characterImage.color;
        color.a = 1f;
        characterImage.color = color;
    }

    private void DimCharacter(Image characterImage)
    {
        // Add grayscale
        characterImage.material = grayScaleMaterial;

        // Reduce alpha
        var color = characterImage.color;
        color.a = characterDimAlpha;
        characterImage.color = color;
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

        // When finished, show arrow
        ShowArrow();

        // Clear routine
        routine = null;
    }

    private void ShowArrow()
    {
        nextArrowImage.enabled = true;

        // If we are at the end of dialogue, show arrow downward
        if (currentIndex >= currentDialogue.length - 1)
            nextArrowImage.transform.parent.localEulerAngles = new Vector3(0, 0, -90);
        else
            nextArrowImage.transform.parent.localEulerAngles = Vector3.zero;

    }

    private void HideArrow()
    {
        nextArrowImage.enabled = false;
    }

    #endregion
}
