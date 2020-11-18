using System;
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
    
    private GameObject desatHat_0;
    private GameObject desatHat_4;

    private GameObject mainRL_0;

    public Animator ani;
    void Start()
    {
        desatHat_0 = GameObject.Find("desatHat_0");
        desatHat_4 = GameObject.Find("desatHat_4");

        mainRL_0 = GameObject.Find("mainRL_0");
        
        CharacterData l = saveLoad.LoadData();
        if (l != null)
        {
            hat_R = l.hat_R;
            hat_B = l.hat_B;
            hat_G = l.hat_G;

            c_name = l.name;
            
            desatHat_0.GetComponent<SpriteRenderer>().material.color = new Color(hat_R,hat_G,hat_B);
            desatHat_4.GetComponent<SpriteRenderer>().material.color = new Color(hat_R,hat_G,hat_B);
        }
    }

    private void Update()
    {
        ani.SetFloat("Horizontal",Input.GetAxis("Horizontal"));
        ani.SetFloat("Vertical",Input.GetAxis("Vertical"));

        if (ani.GetFloat("Horizontal") < 0)
        {
            
        }
        
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position = transform.position + horizontal * Time.deltaTime;
        
        Vector3 vertical = new Vector3(0f, Input.GetAxis("Vertical"), 0f);
        transform.position = transform.position + vertical * Time.deltaTime;
    }

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