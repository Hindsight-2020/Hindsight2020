using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text messageText;

    PartyMemberUI[] memberSlots;
    
    List<Enemy> enemies;

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Enemy> enemies)
    {
        this.enemies = enemies;
        
        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < enemies.Count)
                memberSlots[i].SetData(enemies[i]);
            else
                memberSlots[i].gameObject.SetActive(false);
        }

        messageText.text = "Choose a Party Member";
    }
    
    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (i == selectedMember)
                memberSlots[i].SetSelected(true);
            else
                memberSlots[i].SetSelected(false);
        }
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}