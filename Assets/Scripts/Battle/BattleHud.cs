using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Text statusText;
    [SerializeField] HPBar hpBar;

    [SerializeField] Color brnColor;
    [SerializeField] Color cvdColor;
    [SerializeField] Color frzColor;


    Enemy _enemy;
    Dictionary<ConditionID, Color> statusColors;
    
    public void SetData(Enemy enemy)
    {
        _enemy = enemy;

        nameText.text = enemy.Base.Name;
        levelText.text = "Lvl " + enemy.Level;
        hpBar.SetHP((float) enemy.HP / enemy.MaxHp);
        
        statusColors = new Dictionary<ConditionID, Color>()
        {
            {ConditionID.brn, brnColor},
            {ConditionID.cvd, cvdColor},
            {ConditionID.frz, frzColor},
        };
        
        SetStatusText();
        _enemy.OnStatusChanged += SetStatusText;
    }

    void SetStatusText()
    {
        if (_enemy.Status == null)
        {
            statusText.text = "";
        }
        else
        {
            statusText.text = _enemy.Status.Id.ToString().ToUpper();
            statusText.color = statusColors[_enemy.Status.Id];
        }
    }
    
    public IEnumerator UpdateHP()
    {
        if (_enemy.HpChanged)
        {
            yield return hpBar.SetHPSmooth((float) _enemy.HP / _enemy.MaxHp);
            _enemy.HpChanged = false;
        }
    }
}
