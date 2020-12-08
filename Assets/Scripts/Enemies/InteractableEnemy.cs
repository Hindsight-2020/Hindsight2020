using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableEnemy : MonoBehaviour, Interactable
{
    [SerializeField] public Enemy enemyType;
    [SerializeField] int level;
    public void Interact()
    {
        Debug.Log("Encounter Started.");
        var targetPos = transform.position;
        targetPos.y = 60;
        transform.position = targetPos;
        FindObjectOfType<PlayerController>().questActive = false;
    }
}
