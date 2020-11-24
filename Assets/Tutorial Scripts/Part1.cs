using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part1 : MonoBehaviour
{
    public PannelMessage mainText;
    // Start is called before the first frame update
    void Start()
    {
        mainText.setText("Hello",15);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
