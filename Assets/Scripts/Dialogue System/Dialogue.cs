using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dialogue : ScriptableObject
{
    public Sprite portraitSprite;
    [Header("Editable Attributes")]
    [TextArea(4,10)]
    public string[] messages;
    public int length { get { return messages.Length; }}

    // Allow indexing dialogue as if it were an arry
    public string this[int index] {
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
