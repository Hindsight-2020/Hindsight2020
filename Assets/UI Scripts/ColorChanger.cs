using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    private GameObject desatHat_0;
    private GameObject desatHat_4;
    void Start()
    {
        desatHat_0 = GameObject.Find("desatHat_0");
        desatHat_4 = GameObject.Find("desatHat_4");

        desatHat_0.GetComponent<SpriteRenderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
        desatHat_4.GetComponent<SpriteRenderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
    }

    void Update()
    {
        
    }

    

    public void updateHatBlue(float value)
    {
        Color c = desatHat_0.GetComponent<SpriteRenderer>().material.color;
        desatHat_0.GetComponent<SpriteRenderer>().material.color = new Color(c.r, c.g, value);
        desatHat_4.GetComponent<SpriteRenderer>().material.color = new Color(c.r, c.g, value);
    }
    
    public void updateHatRed(float value)
    {
        Color c = desatHat_0.GetComponent<SpriteRenderer>().material.color;
        desatHat_0.GetComponent<SpriteRenderer>().material.color = new Color(value, c.g, c.b);
        desatHat_4.GetComponent<SpriteRenderer>().material.color = new Color(value, c.g, c.b);
    }
    
    public void updateHatGreem(float value)
    {
        Color c = desatHat_0.GetComponent<SpriteRenderer>().material.color;
        desatHat_0.GetComponent<SpriteRenderer>().material.color = new Color(c.r, value, c.b);
        desatHat_4.GetComponent<SpriteRenderer>().material.color = new Color(c.r, value, c.b);
    }
}
