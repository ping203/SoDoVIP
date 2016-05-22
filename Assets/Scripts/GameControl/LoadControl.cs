using UnityEngine;
using System.Collections;

public class LoadControl : StageControl {

    public UISprite loadFrame;
    // public UILabel percent;
    // public ParticleSystem khoi;

    private string username;
    private string password;

    void Awake() {
        username = PlayerPrefs.GetString("username");
        password = PlayerPrefs.GetString("password");
        //Debug.Log(username + " zzzzzzzzzzzzzzzzzzzzzzzzzzzz " + password);
        StartCoroutine(LoadProgress());
        //if (khoi != null) {
        //    khoi.Play();
        //}
    }
    float time = 0;
    bool isLogin = false;
    private IEnumerator LoadProgress() {
        yield return new WaitForSeconds(1.0f);

        float x = -239.0f;
        float y = 244.0f;

        Transform rTrans = loadFrame.gameObject.GetComponent<Transform>();
        if (loadFrame != null) {
            int sizeWidt = (int)y - (int)x;
            float rate = 1.0f / sizeWidt;
            float pCent = 0;
            loadFrame.fillAmount = rate;

            int loop = sizeWidt / 2;
            for (int i = 0; i < loop; i++) {
                rTrans.localPosition = new Vector3(rTrans.localPosition.x + sizeWidt * rate * 2, rTrans.localPosition.y, 0.0f);
                pCent = rate * 100;
                //if (percent != null) {
                //    percent.text = pCent.ToString("0") + "%";
                //}
                yield return new WaitForEndOfFrame();
            }
        }

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) {
            gameControl.setStage(gameControl.login);
        } else {
            string imei = SystemInfo.deviceUniqueIdentifier;
            login(4, username, password, imei, "", 0, username, "", "");
        }
    }

    void Update() {
        if (isLogin && gameObject.activeInHierarchy) {
            time += Time.deltaTime;
            if (time >= 14) {
                gameControl.setStage(gameControl.login);
                isLogin = false;
            }
        }
    }

    public void login(sbyte type, string username, string pass,
                           string imei, string link_avatar, sbyte tudangky, string displayName,
                           string accessToken, string regPhone) {
        /*imei = "";
        for(int i = 0; i < 40; i++) {
            imei += "9";
        }*/
        //imei = "443814732034443";
        BaseInfo.gI().isPurchase = false;
        if (checkNetWork()) {
            if (NetworkUtil.GI().isConnected()) {
                NetworkUtil.GI().close();
            }
            gameControl.panelWaiting.onShow();
            isLogin = true;
            Message msg = new Message(CMDClient.CMD_LOGIN_NEW);
            try {
                msg.writer().WriteByte(type);
                msg.writer().WriteUTF(username);
                msg.writer().WriteUTF(pass);
                msg.writer().WriteUTF(Res.version);
                msg.writer().WriteByte(CMDClient.PROVIDER_ID);
                msg.writer().WriteUTF(imei);
                msg.writer().WriteUTF(link_avatar);
                msg.writer().WriteByte(tudangky);
                msg.writer().WriteUTF(displayName);
                msg.writer().WriteUTF(accessToken);
                msg.writer().WriteUTF(regPhone);
            } catch (System.Exception ex) {
                Debug.LogException(ex);
            }
            SendData.isLogin = true;
            NetworkUtil.GI().connect(msg);
            BaseInfo.gI().username = username;
            BaseInfo.gI().pass = pass;
        } else {
            gameControl.panelThongBao.onShow("Vui lòng kiểm tra kết nối mạng!", delegate {
                //gameControl.setStage(gameControl.login);
            });
        }
    }

    private bool checkNetWork() {
        return true;
    }
}
