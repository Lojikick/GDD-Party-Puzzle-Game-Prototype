using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookbookMenuUI : MonoBehaviour
{
    private enum CookbookState { Opening, Open, Closing, Closed }

    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button initalButton;

    [Header("Settings")]
    [SerializeField] private float fadeOffset;
    [SerializeField] private float fadeDuration;

    [Header("Debugging")]
    [SerializeField] private CookbookState state;
    [SerializeField] private DialogueUI ui;
    [SerializeField] private Dialogue willMonologue;

    private Vector3 initalPosition;
    private bool isVisible;
    private Coroutine routine;

    public static CookbookMenuUI instance;
    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        
        //ui = DialogueUI.instance;
        initalPosition = transform.localPosition;
        state = CookbookState.Closed;
    }

    public bool IsOpen() => state == CookbookState.Open;
    public bool IsOpening() => state == CookbookState.Opening;

    public void Show()
    {
        // Don't do anything if in motion
        if (routine != null) return;

        // Change state
        //if(GameManager.instance.GetNumPuzzles() == 0){
            //ui.Open(willMonologue);
        //}
        state = CookbookState.Opening;
        

        // Fade in UI
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FadeIn(fadeDuration));
    }

    public void Hide()
    {
        // Do nothing if already not visible
        if (routine != null) return;

        // Change state
        state = CookbookState.Closing;

        // Fade out UI
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FadeOut(fadeDuration));
    }

    public bool Toggle()
    {
        bool sucess;
        switch (state)
        {
            case CookbookState.Opening:
                // Do nothing
                sucess = true;
                break;
            case CookbookState.Open:
                Hide();
                sucess = false;
                break;
            case CookbookState.Closing:
                // Do nothing
                sucess = false;
                break;
            case CookbookState.Closed:
                Show();
                sucess = true;
                break;
            default:
                sucess = false;
                break;
        }

        return sucess;
    }

    public void StartLevel(int value)
    {
        // Change scenes
        TransitionManager.instance.LoadSelectedScene(value);
    }

    private IEnumerator FadeIn(float duration)
    {
        Vector3 startPos = initalPosition + Vector3.up * fadeOffset;
        Vector3 endPos = initalPosition;
        canvasGroup.alpha = 0f;

        float elapsed = 0;
        while (elapsed < duration)
        {
            // Lerp alpha
            canvasGroup.alpha = elapsed / duration;

            // Lerp position
            transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / duration);

            // Increment time
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final values
        transform.localPosition = endPos;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Focus on button
        initalButton.Select();

        // Change state
        state = CookbookState.Open;

        routine = null;
    }

    private IEnumerator FadeOut(float duration)
    {
        Vector3 startPos = initalPosition;
        Vector3 endPos = initalPosition + Vector3.up * fadeOffset;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float elapsed = 0;
        while (elapsed < duration)
        {
            // Lerp alpha
            canvasGroup.alpha = 1 - elapsed / duration;

            // Lerp position
            transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / duration);

            // Increment time
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final values
        transform.localPosition = endPos;
        canvasGroup.alpha = 0f;

        // Change state
        state = CookbookState.Closed;

        routine = null;
    }
}
