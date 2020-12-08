using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] Dialogue questDialogue;
    public void Interact()
    {
        if (FindObjectOfType<PlayerController>().questActive)
        {
            DialogueManager.Instance.StartDialogue(questDialogue);
            var targetPos = transform.position;
            targetPos.y = 60;
            transform.position = targetPos;
            FindObjectOfType<PlayerController>().questActive = false;
        }
        else
            DialogueManager.Instance.StartDialogue(dialogue);
    }
}
