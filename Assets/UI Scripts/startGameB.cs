using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startGameB : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 _pos;
    private GameObject button;
    private GameObject[] clouds;
    void Start()
    {
        Debug.Log("Hello");
        _pos = GameObject.Find("startGame").transform.position;
        button = GameObject.Find("startGame");

        clouds = new GameObject[5];
        
        clouds[0] = GameObject.Find("c1");
        clouds[1] = GameObject.Find("c2");
        clouds[2] = GameObject.Find("c3");
        clouds[3] = GameObject.Find("c4");
        clouds[4] = GameObject.Find("c5");
    }

    // Update is called once per frame
    void Update()
    {
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
