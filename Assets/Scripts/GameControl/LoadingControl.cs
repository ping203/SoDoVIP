using UnityEngine;
using System.Collections;

public class LoadingControl : StageControl {
    public UILabel preLoadText;
    public UISprite preload;
    float per = 0;
    float timeMaxLoad = 5;
    float time;
    bool isStateLogin = false;
    public LoginControl login;
    void Awake () {
        //PlayerPrefs.DeleteAll();
        //version.text = "Phiên bản " + Res.fake_version;
        setPercentage (per);
    }
    // Update is called once per frame
    void Update () {
        if(gameObject.activeInHierarchy) {
            if(time < timeMaxLoad) {
                time += Time.deltaTime;
                float p = (100 * time) / timeMaxLoad;
                setPercentage (p);
            } else if(!isStateLogin) {
                autoLogin ();
            }
        }
    }

    public void setPercentage (float percent) {
        preLoadText.text = (int) percent + "%";
        percent = percent / 100;
        preload.fillAmount = percent;
    }

    void autoLogin () {
        NetworkUtil.GI().connect(SendData.onGetPhoneCSKH());
        string u = PlayerPrefs.GetString ("username");
        string p = PlayerPrefs.GetString ("password");
        //Debug.Log("-=-=- " + u + "    " + p);
        //if (!u.Equals("") && !p.Equals("")) {
        //    string imei = SystemInfo.deviceUniqueIdentifier;
        //   login.login(4, u, p, imei, "", 0, u, "", "");
        //} else
        gameControl.setStage (login);

        isStateLogin = true;
    }
}
