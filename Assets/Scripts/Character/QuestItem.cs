using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour, Interactable
{
    public bool itemCollected;
    [SerializeField] Dialogue dialogue;
    public void Interact()
    {
        itemCollected = true;
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
