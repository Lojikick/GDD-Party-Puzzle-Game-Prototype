using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image image;

    [Header("Settings")]
    [SerializeField] private float offset;
    [SerializeField] private float duration;

    private void Start()
    {
        LeanTween.moveLocalY(image.gameObject, offset, duration).setEaseInOutQuad().setLoopPingPong();
    }
}
