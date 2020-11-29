using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasAnimations : MonoBehaviour
{
    // Start is called before the first frame update

    private Transform startButton;
    void Start()
    {
         startButton = gameObject.transform.Find("startGame");
         print("This is loaded");
         
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
