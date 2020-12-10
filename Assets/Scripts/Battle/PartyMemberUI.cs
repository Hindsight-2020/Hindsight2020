using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    [SerializeField] Color highlightedColor;
    
    Enemy _enemy;

    public void SetData(Enemy enemy)
    {
        _enemy = enemy;

        nameText.text = enemy.Base.Name;
        levelText.text = "Lvl " + enemy.Level;
        hpBar.SetHP((float)enemy.HP / enemy.MaxHp);
    }
    
    public void SetSelected(bool selected)
    {
        if (selected)
            nameText.color = highlightedColor;
        else
            nameText.color = Color.black;
    }
}
