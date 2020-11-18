using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public string c_name = "Template";
    public float hat_R = -1;
    public float hat_G = -1;
    public float hat_B = -1;

    public void c_b(float b)
    {
        hat_B = b;
    }

    public void c_r(float r)
    {
        hat_R = r;
    }

    public void c_g(float g)
    {
        hat_G = g;
    }

    public void c_n (string n)
    {
        c_name = n;
    }

    public void saveC()
    {
        saveLoad.SaveData(this); 
    }
}