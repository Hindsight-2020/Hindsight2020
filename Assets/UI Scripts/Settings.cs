using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject mF, mR, mL;
    private bool mov;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var f = mF.transform.position;
        var r = mR.transform.position;
        var l = mL.transform.position;

        if (mov)
        {
            mF.transform.position = f - new Vector3(100 * Time.deltaTime, 100*Time.deltaTime, 0);
            mR.transform.position = r + new Vector3(100 * Time.deltaTime,  -100*Time.deltaTime, 0);
            mL.transform.position = l - new Vector3(100 * Time.deltaTime, 100*Time.deltaTime, 0);

            if (mF.transform.localPosition.y < -200)
                mov = false;
        }
        
    }

    public void onClick()
    {
        mov = true;
    }
}
