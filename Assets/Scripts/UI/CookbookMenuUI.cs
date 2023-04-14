using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookbookMenuUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Settings")]
    [SerializeField] private float fadeOffset;
    [SerializeField] private float fadeDuration;

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
    }

    public void Show()
    {
        // Don't do anything if in motion
        if (routine != null) return;

        // Fade in UI
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FadeIn(fadeDuration));
    }

    public void Hide()
    {
        // Do nothing if already not visible
        if (routine != null) return;

        // Fade out UI
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FadeOut(fadeDuration));
    }

    public void LoadLevel1()
    {
        // Change scenes
        TransitionManager.instance.LoadSelectedScene(2);
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

        routine = null;
    }
}
