﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/Create new enemy")]
public class EnemyBase : ScriptableObject
{
    [SerializeField] new string name;

    [TextArea] [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] EnemyType type1;
    [SerializeField] EnemyType type2;
    
    //Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    [SerializeField] List<LearnableMove> learnableMoves;

    public string Name {
        get { return name; }
    }

    public string Description {
        get { return description; }
    }

    public Sprite FrontSprite {
        get { return frontSprite; }
    }

    public Sprite BackSprite {
        get { return backSprite; }
    }

    public EnemyType Type1 {
        get { return type1; }
    }

    public EnemyType Type2 {
        get { return type2; }
    }

    public int MaxHp {
        get { return maxHp; }
    }

    public int Attack {
        get { return attack; }
    }

    public int SpAttack {
        get { return spAttack; }
    }

    public int Defense {
        get { return defense; }
    }

    public int SpDefense {
        get { return spDefense; }
    }

    public int Speed {
        get { return speed; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }
}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base
    {
        get { return moveBase; }
    }

    public int Level
    {
        get { return level; }
    }
}
public enum EnemyType
{
    None,
    Normal,
    Fire,
    Water,
    COVID,
    Bug,
}

public class TypeChart
{
    static float[][] chart =
    {
        //                   NOR   FIR   WAT   CVD   BG                
        /*NOR*/ new float[] { 1f,  1f,    1f,  0.5f, 1f },
        /*FIR*/ new float[] { 1f,  1f,  0.5f,  1f,   2f },
        /*WAT*/ new float[] { 1f,  2f,    1f,  1f,   1f },
        /*CVD*/ new float[] { 2f,  0.5f,  1f,  1f,   1f },
        /*BG */ new float[] { 1f,  0.5f,  1f,  2f,   1f }
    };

    public static float GetEffectiveness(EnemyType attackType, EnemyType defenseType)
    {
        if (attackType == EnemyType.None || defenseType == EnemyType.None)
            return 1;

        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;

        return chart[row][col];
    }
}