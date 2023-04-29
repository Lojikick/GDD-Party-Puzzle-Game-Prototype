using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableHandler : MonoBehaviour
{
    [SerializeField] private GameObject interactionIndicatorGameObject;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private float offset;
    [SerializeField] private float duration;

    private void Start()
    {
        // Apply effect
        LeanTween.moveLocalY(interactionIndicatorGameObject, offset, duration).setEaseInOutQuad().setLoopPingPong();

        // Hide it for now
        Hide();
    }

    public void Show()
    {
        canvasGroup.alpha = 0.5f;
        LeanTween.resume(interactionIndicatorGameObject);
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        LeanTween.pause(interactionIndicatorGameObject);
    }

}
