using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dialogue : ScriptableObject
{
    public Sprite initalLeftCharacterSprite;
    public Sprite initalRightCharacterSprite;
    public DialogueMessage[] messages;
    public int length { get { return messages.Length; } }

    // Allow indexing dialogue as if it were an arry
    public DialogueMessage this[int index]
    {
        get
        {
            return messages[index];
        }
        set
        {
            messages[index] = value;
        }
    }
}

[System.Serializable]
public class DialogueMessage
{
    public string name;
    public Sprite characterSprite;
    public bool isRightCharacter;
    public bool italicized;
    public string sfx;
    public bool bold;
    [TextArea(4, 10)]
    public string text;
}
