using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour, Interactable
{
    public GameObject thePlayer;
    public Transform TeleportGoal;
    /*void OnTriggerEnter(Collider other)
    {
        thePlayer.transform.position = TeleportGoal.transform.position;
    }*/

    public void Interact()
    {
        thePlayer.transform.position = TeleportGoal.transform.position;
    }
}
