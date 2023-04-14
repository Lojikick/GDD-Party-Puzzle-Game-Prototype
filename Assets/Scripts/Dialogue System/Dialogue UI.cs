using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueUI : MonoBehaviour, IPointerClickHandler
{
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
    public Dialogue test;

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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check for left clicking
        if (eventData.button == PointerEventData.InputButton.Left)
            NextMessage();
    }

    // Open UI
    public void Open(Dialogue dialogue)
    {
        // Error checking
        if (dialogue == null) throw new System.Exception("ATTEMPTED TO OPEN NULL DIALOGUE!");

        this.currentDialogue = dialogue;

        // Setup character values
        IntialSetup(currentDialogue);

        // Show characters
        StartCoroutine(FadeUIIn(uiFadeDuration));
    }

    // Close UI
    public void Close()
    {
        // Hide UI
        StartCoroutine(FadeUIOut(uiFadeDuration));

        this.currentDialogue = null;
    }

    // This is called via buttons
    public void FirstMessage()
    {
        currentIndex = 0;

        DisplayCurrentMessage(currentDialogue[currentIndex]);
    }

    // This is called via buttons
    public bool NextMessage()
    {
        // Make sure nothing happens
        if (!dialogueBoxCanvasGroup.interactable) return false;

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
            // Hide arrow
            HideArrow();

            // If we are at the end of dialogue
            if (currentIndex == currentDialogue.length - 1)
            {
                Close();
                return true;
            }
            // Else show next message
            else
            {
                currentIndex++;
                DisplayCurrentMessage(currentDialogue[currentIndex]);
            }
        }

        // Return whether you are done with dialogue
        return false;
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

    public bool IsDone()
    {
        return currentIndex >= currentDialogue.length - 1;
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
    }

    private IEnumerator FadeCharactersIn(float startScale, float finalScale, float duration)
    {
        // Set starting values
        leftCharacterImage.transform.localScale = Vector3.one * startScale;
        var leftColor = leftCharacterImage.color;
        leftColor.a = 0f;
        leftCharacterImage.color = leftColor;

        rightCharacterImage.transform.localScale = Vector3.one * startScale;
        var rightColor = rightCharacterImage.color;
        rightColor.a = 0f;
        rightCharacterImage.color = rightColor;

        float elapsed = 0;
        while (elapsed < duration)
        {
            float ratio = elapsed / duration;

            // Lerp values
            leftCharacterImage.transform.localScale = Vector3.one * Mathf.Lerp(startScale, finalScale, ratio);
            leftColor.a = Mathf.Lerp(0f, characterDimAlpha, ratio);
            leftCharacterImage.color = leftColor;

            rightCharacterImage.transform.localScale = Vector3.one * Mathf.Lerp(startScale, finalScale, ratio);
            rightColor.a = Mathf.Lerp(0f, characterDimAlpha, ratio);
            rightCharacterImage.color = rightColor;

            // Increment time
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final values
        leftCharacterImage.transform.localScale = Vector3.one * finalScale;
        leftColor.a = characterDimAlpha;
        leftCharacterImage.color = leftColor;

        rightCharacterImage.transform.localScale = Vector3.one * finalScale;
        rightColor.a = characterDimAlpha;
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

        // If sprite is empty, the assume dialogue is narration
        if (message.characterSprite == null)
        {
            messsageTextBox.fontStyle = FontStyles.Italic;

            DimCharacter(rightCharacterImage);
            DimCharacter(leftCharacterImage);
        }
        else
        {
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