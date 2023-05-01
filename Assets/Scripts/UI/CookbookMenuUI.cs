using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookbookMenuUI : MonoBehaviour
{
    private enum CookbookState { Opening, Open, Closing, Closed }

    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Page 1")]
    [SerializeField] private CanvasGroup canvasGroup1;
    [SerializeField] private Button initalButton1;
    [Header("Page 2")]
    [SerializeField] private CanvasGroup canvasGroup2;
    [SerializeField] private Button initalButton2;

    [Header("Settings")]
    [SerializeField] private float fadeOffset;
    [SerializeField] private float fadeDuration;
    [SerializeField] private int maxLevels;

    [Header("Debugging")]
    [SerializeField] private CookbookState state;

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

        initalPosition = transform.localPosition;
        state = CookbookState.Closed;
    }

    public void Show()
    {
        // Don't do anything if in motion
        if (routine != null) return;

        // Change state
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

    private void Update()
    {
        // Make sure book is open
        if (state != CookbookState.Open) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ShowPage(1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ShowPage(2);
        }
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
        // Do nothing
        if (value > maxLevels + 1) return;

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

        // Display page 1
        ShowPage(1);

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

    private void ShowPage(int pageNum)
    {
        if (pageNum == 1)
        {
            canvasGroup1.alpha = 1f;
            canvasGroup1.interactable = true;
            canvasGroup1.blocksRaycasts = true;

            // Focus on button
            initalButton1.Select();

            // Hide other page
            HidePage(2);
        }
        else if (pageNum == 2)
        {
            canvasGroup2.alpha = 1f;
            canvasGroup2.interactable = true;
            canvasGroup2.blocksRaycasts = true;

            // Focus on button
            initalButton2.Select();

            // Hide other page
            HidePage(1);
        }
    }

    private void HidePage(int pageNum)
    {
        if (pageNum == 1)
        {
            canvasGroup1.alpha = 0f;
            canvasGroup1.interactable = false;
            canvasGroup1.blocksRaycasts = false;
        }
        else if (pageNum == 2)
        {
            canvasGroup2.alpha = 0f;
            canvasGroup2.interactable = false;
            canvasGroup2.blocksRaycasts = false;
        }
    }
}
