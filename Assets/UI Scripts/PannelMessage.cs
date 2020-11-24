using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PannelMessage : MonoBehaviour
{
    private float clearDelta;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > clearDelta)
        {
            GetComponent<Image>().enabled = false;
            clearDelta += 1000000;
            Debug.Log("Help But Dont");
            
        }    
    }

    public void setText(String text, int time)
    {
        clearDelta = Time.time + time;
        Debug.Log("Help");
        GetComponent<Image>().enabled = true;
    }
}
