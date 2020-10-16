using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class startGameB : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 _pos;
    private GameObject button;
    private GameObject[] clouds;
    private GameObject[] mountains;

    private float[] mxs;
    
    void Start()
    {
        Debug.Log("Hello");
        _pos = GameObject.Find("startGame").transform.position;
        button = GameObject.Find("startGame");

        clouds = new GameObject[5];
        mountains = new GameObject[3];
        mxs = new float[3];
        
        clouds[0] = GameObject.Find("c1");
        clouds[1] = GameObject.Find("c2");
        clouds[2] = GameObject.Find("c3");
        clouds[3] = GameObject.Find("c4");
        clouds[4] = GameObject.Find("c5");

        mountains[0] = GameObject.Find("mountainFront");
        mountains[1] = GameObject.Find("mountainLeft");
        mountains[2] = GameObject.Find("mountainRight");
        
        mxs[0] = mountains[0].transform.position.x;
        mxs[1] = mountains[1].transform.position.x;
        mxs[2] = mountains[2].transform.position.x;
        
    }

    // Update is called once per frame
    void Update()
    {
        var frontParrallax = Input.mousePosition.x/600 * 10 - 5;
        var backParrallax = Input.mousePosition.x/600 * 2 - 1;
        
            mountains[0].transform.position = new Vector3(
                mxs[0]+frontParrallax,
                mountains[0].transform.position.y,
                mountains[0].transform.position.z
            );
            mountains[1].transform.position = new Vector3(
                mxs[1]+backParrallax,
                mountains[1].transform.position.y,
                mountains[1].transform.position.z
            );
            mountains[2].transform.position = new Vector3(
                mxs[2]+backParrallax,
                mountains[2].transform.position.y,
                mountains[2].transform.position.z
            );
        
        
        foreach (var cloud in clouds)
        {
            var p = cloud.transform.position;
            cloud.transform.position = new Vector3(
                    p.x+Time.fixedDeltaTime*(Random.value*5),
                    p.y,
                    p.z);

            if (p.x > -160)
                cloud.transform.position = new Vector3(
                    -875,
                    p.y,
                    p.z);
        }
        
    }

    public void OnClick()
    {
        Debug.Log("Does this work now");
        //Load next screen aka character selection
    }
}
