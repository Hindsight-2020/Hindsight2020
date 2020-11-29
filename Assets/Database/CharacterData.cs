using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string name;
    public float hat_R;
    public float hat_G;
    public float hat_B;

    public float music;
    public float sound;

    //True for male false for female
    public Boolean gender;
    
    public CharacterData(Character character)
    {
        name = character.name;
        hat_R = character.hat_R;
        hat_G = character.hat_G;
        hat_B = character.hat_B;

        music = character.music;
        sound = character.sound;

        gender = character.gender;

    }
}