using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    public static void Init()
    {
        foreach (var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionId;
        }
    }
    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.brn,
            new Condition()
            {
                Name = "Burn",
                StartMessage = "has been burned",
                OnAfterTurn = (Enemy enemy) =>
                {
                    enemy.UpdateHP(enemy.MaxHp / 16);
                    enemy.StatusChanges.Enqueue($"{enemy.Base.Name} hurt itself due to burn");
                }
            }
        },
        
        {
            ConditionID.cvd,
            new Condition()
            {
                Name = "COVID",
                StartMessage = "has been infected with COVID-19!",
                OnAfterTurn = (Enemy enemy) =>
                {
                    enemy.UpdateHP(enemy.MaxHp / 8);
                    enemy.StatusChanges.Enqueue($"{enemy.Base.Name} was hurt by COVID-19");
                }
            }
        },
        
        {
            ConditionID.frz,
            new Condition()
            {
                Name = "Freeze",
                StartMessage = "has been frozen",
                OnBeforeMove = (Enemy enemy) =>
                {
                    if  (Random.Range(1, 5) == 1)
                    {
                        enemy.CureStatus();
                        enemy.StatusChanges.Enqueue($"{enemy.Base.Name} is not frozen anymore");
                        return true;
                    }

                    return false;
                }
            }
        },
        //Can Implement Volatile Statuses Here (Only lasts for the current battle)
    };
}

public enum ConditionID
{
    none, brn, cvd, frz,
}