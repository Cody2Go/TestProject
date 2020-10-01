using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Interactable
{
    public Dialogue dialogue;

    public override void Interact()
    {
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
