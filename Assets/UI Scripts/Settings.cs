using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject mF, mR, mL, SG, Title, clusster;
    public Character c;
    private bool mov;
    private Text title;
    
    void Start()
    {
        var c = saveLoad.LoadData();
        var m = GameObject.Find("music");
        var s = GameObject.Find("sound");
        var t = GameObject.Find("Title Screen");
        
        m.GetComponent<Slider>().value = c.music;
        s.GetComponent<Slider>().value = c.sound;

        t.GetComponent<AudioSource>().volume = c.music;


        title = Title.GetComponent<Text>();
    }

    public void updateMusic(float m)
    {
        var t = GameObject.Find("Title Screen");
        t.GetComponent<AudioSource>().volume = c.music;
    }
    
    // Update is called once per frame
    void Update()
    {
        var f = mF.transform.position;
        var r = mR.transform.position;
        var l = mL.transform.position;
        var c = clusster.transform.position;
        
        if (mov)
        {
            if (mF.transform.localPosition.y < -300)
            {
                this.transform.localScale = new Vector3(0,0,0);
                SG.transform.localScale = new Vector3(0,0,0);
                
                title.text = "Settings";
            }
            else
            {
                mF.transform.position = f - new Vector3(100 * Time.deltaTime, 100*Time.deltaTime, 0);
                mR.transform.position = r + new Vector3(100 * Time.deltaTime,  -100*Time.deltaTime, 0);
                mL.transform.position = l - new Vector3(100 * Time.deltaTime, 100*Time.deltaTime, 0);
                clusster.transform.position = c + new Vector3(-210 * Time.deltaTime, 0, 0);
 
            }
                
        }
        else
        {
            if (mF.transform.localPosition.y < 50)
            {
                mF.transform.position = f + new Vector3(100 * Time.deltaTime, 100*Time.deltaTime, 0);
                mR.transform.position = r + new Vector3(-100 * Time.deltaTime,  100*Time.deltaTime, 0);
                mL.transform.position = l + new Vector3(100 * Time.deltaTime, 100*Time.deltaTime, 0);
                clusster.transform.position = c - new Vector3(-210 * Time.deltaTime, 0, 0);
                
                this.transform.localScale = new Vector3(1,1,1);
                SG.transform.localScale = new Vector3(1,1,1);
                
                Title.GetComponent<Text>().text = "HindSight [2020]";
            }
            
        }
        
    }

    public void onClick(bool t)
    {
        mov = t;
        
    }

    public void OnClickApply()
    {
        mov = false;
        saveLoad.SaveData(c);
    }
}
