using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dialogue : ScriptableObject
{
    [Header("Editable Attributes")]
    [TextArea(4,10)]
    [SerializeField] private string[] messages;
    [SerializeField] private Sprite portraitSprite;

    [Header("Debugging")]
    [SerializeField] private int currentIndex;

    public void Reset()
    {
        // Set index to 0
        currentIndex = 0;
    }

    public void Next()
    {
        // Increment index up to message length
        currentIndex = Mathf.Min(messages.Length, currentIndex + 1);
    }

    public void Previous()
    {
        // Decrement index until 0
        currentIndex = Mathf.Max(0, currentIndex - 1);
    }

    public string GetMessage()
    {
        return messages[currentIndex];
    }

    public Sprite GetPortrait()
    {
        return portraitSprite;
    }
}
