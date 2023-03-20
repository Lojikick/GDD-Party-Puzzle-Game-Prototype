using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHandler : MonoBehaviour
{
    [Header("Components")]
    // TODO

    [Header("Data")]
    [SerializeField] private Dialogue dialogue;

    public Dialogue GetDialogue()
    {
        return dialogue;
    }
}
