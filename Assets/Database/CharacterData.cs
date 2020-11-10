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

    public CharacterData(Character character)
    {
        name = character.name;
        hat_R = character.hat_R;
        hat_G = character.hat_G;
        hat_B = character.hat_B;
    }
}