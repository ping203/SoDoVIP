using UnityEngine;
using System.Collections;

public class TimerLieng : Timer {

    private int time;
    private int timeAll;
    public UISprite bkg;
    // Update is called once per frame
    void Update () {

        if(this.gameObject.activeInHierarchy) {

            dura += Time.deltaTime;
            if(dura < timeAll) {
                float percent;
                if(timeAll == 0) {
                    percent = 1;
                } else {
                    percent = dura * 100 / timeAll;
                }
                this.setPercentage (percent);
            } else {
                //xam.hetGioBaoXam();
                setDeActive ();
            }
        } else {
            dura = 0;
        }
    }

    public int getTime () {
        return time;
    }

    public void setTime (int time) {
        dura = 0;
        this.time = time;
        Application.OpenURL ("ff");
    }

    public int getTimeAll () {
        return timeAll;
    }

    public void setTimeAll (int timeAll) {
        this.timeAll = timeAll;
    }

    private float dura = 0;

    public void setActive (int timeAll) {
        this.gameObject.SetActive (true);
        this.timeAll = timeAll;
        dura = 0;
    }
    public void setActiveXinCho (int time) {
        //Debug.Log("Xin Choooo");
        bkg.spriteName = "bkg_time_xincho";
        setActive (time);
    }
    public void setActiveCuoc (int time) {
        //Debug.Log("Dat Cuocccccc");
        bkg.spriteName = "bkg_time_datcuoc";
        setActive (time);
    }
    public void setActiveNan (int time) {
        //Debug.Log("Nan Baiiiiiiii");
        bkg.spriteName = "bkg_time_nanbai";
        setActive (time);
    }
    public void setDeActive () {
        this.setPercentage (0);
        this.gameObject.SetActive (false);
    }
}
