using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoginControl : StageControl {
    public UILabel lb_version;
    public UIInput input_username;
    public UIInput input_passsword;
    public UILabel cskh;
    //public UIInput input_repass;
    public FacebookManager fbM;

    //public GameObject loginGroup;
    //private bool isLogin;

    //public GameObject go;
    //public GameObject back;
    //public GameObject group;

    // Use this for initialization
    void Start () {
        lb_version.text = "Ver " + Res.version;
    }

    // Update is called once per frame
    void Update () {
        if(gameObject.activeInHierarchy && Input.GetKeyDown (KeyCode.Escape)) {
            onBack ();
        }
    }

    void OnEnable () {
        // isLogin = true;
        //BaseInfo.gI ().isLogin = false;
        input_username.value = PlayerPrefs.GetString ("username");
        input_passsword.value = PlayerPrefs.GetString ("password");
        //PlayerPrefs.Save();
        //BaseInfo.gI().isNhanLoiMoiChoi = Preference.getInstance().dataGame.isNhanLoiMoiChoi;
        //OnSubmit ();
        cskh.text = PlayerPrefs.GetString ("CSKH");
    }

    private bool checkNetWork () {
        return true;
    }
    /**
 * 
 * @param username
 * @param pass
 * @param type
 *            : 1-facebook 2-choingay 3-gmail 4-login normal
 * @param imei
 * @param link_avatar
 * @param tudangky
 *            : 1 la tu dang ky, 0
 * @param displayName
 * @param accessToken
 * @param regPhone
 */
    public void login (sbyte type, string username, string pass,
                               string imei, string link_avatar, sbyte tudangky, string displayName,
                               string accessToken, string regPhone) {
        /*imei = "";
        for(int i = 0; i < 40; i++) {
            imei += "9";
        }*/
        //imei = "443814732034443";
        BaseInfo.gI ().isPurchase = false;
        if(checkNetWork ()) {
            if(NetworkUtil.GI ().isConnected ()) {
                NetworkUtil.GI ().close ();
            }
            gameControl.panelWaiting.onShow ();
            Message msg = new Message (CMDClient.CMD_LOGIN_NEW);
            try {
                msg.writer ().WriteByte (type);
                msg.writer ().WriteUTF (username);
                msg.writer ().WriteUTF (pass);
                msg.writer ().WriteUTF (Res.version);
                msg.writer ().WriteByte (CMDClient.PROVIDER_ID);
                msg.writer ().WriteUTF (imei);
                msg.writer ().WriteUTF (link_avatar);
                msg.writer ().WriteByte (tudangky);
                msg.writer ().WriteUTF (displayName);
                msg.writer ().WriteUTF (accessToken);
                msg.writer ().WriteUTF (regPhone);
            } catch(Exception ex) {
                Debug.LogException (ex);
            }
            SendData.isLogin = true;
            NetworkUtil.GI ().connect (msg);
            BaseInfo.gI ().username = username;
            BaseInfo.gI ().pass = pass;
        } else {
            gameControl.panelThongBao.onShow ("Vui lòng kiểm tra kết nối mạng!", delegate { });
        }
    }

    public void doLogin (string username, string password) {
        GameControl.instance.sound.startClickButtonAudio ();
        //if(input_username.gameObject.activeInHierarchy && input_passsword.gameObject.activeInHierarchy) {
        string imei = SystemInfo.deviceUniqueIdentifier;
        login (4, username, password, imei, "", 0, username, "", "");
        PlayerPrefs.SetString ("username", username);
        PlayerPrefs.SetString ("password", password);
        PlayerPrefs.Save ();
        //OnHideKeyBoard ();
        //} else {
        //    SetActiveInput (true);
        //}
    }

    void SetActiveInput (bool state) {
        input_username.gameObject.SetActive (state);
        input_passsword.gameObject.SetActive (state);
        //back.SetActive (state);
        //go.SetActive (!state);
    }

    //public void clickBack () {
    //    SetActiveInput (false);
    //    //group.transform.localPosition = new Vector3 (260, 0, 0);
    //}

    public void clickLoginPlayNow () {
        GameControl.instance.sound.startClickButtonAudio ();
        string imei = SystemInfo.deviceUniqueIdentifier;

        login (2, imei, imei, imei, "", 1, "", "", "");
    }

    public void clickOnFacebook () {
        GameControl.instance.sound.startClickButtonAudio ();
            gameControl.panelThongBao.onShow ("Chức năng đang hoàn thành!", delegate { });
        //if(!checkNetWork ()) {
        //    gameControl.panelThongBao.onShow ("Vui lòng kiểm tra kết nối mạng!", delegate { });
        //    return;
        //}
        ////gameControl.toast.showToast ("Chức năng đang được hoàn thành!");
        //bool islogin = FB.IsLoggedIn;
        //string imei = SystemInfo.deviceUniqueIdentifier;
        //if(islogin) {
        //    login (1, "sgc", "sgc", imei, "", 1, "", fbM.getAccessToken (), "");
        //} else {
        //    fbM.loginFB ();
        //    Debug.Log ("++++++++++++++++++++ " + FB.IsLoggedIn);
        //    if(FB.IsLoggedIn) {
        //        login (1, "sgc", "sgc", imei, "", 1, "", fbM.getAccessToken (), "");
        //    }
        //}
    }

    public void clickSetting () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelSetting.onShow ();
        //Handheld.Vibrate ();
    }

    public void clickHelp () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panleHelp.onShow ();
    }

    public void clickCSKH () {
        gameControl.panelYesNo.onShow ("Gọi điện đến tổng đài chăm sóc khách hàng "
            + Res.TXT_PhoneNumber + "?", delegate {
            Application.OpenURL ("tel://" + Res.TXT_PhoneNumber);
        });
    }
    public void clickDangKy () {
        gameControl.panelDangKy.onShow ();
    }

    //public void clickCapNhat () {
    //    if(checkNetWork ()) {
    //        //gameControl.panelWaiting.onShow();
    //        NetworkUtil
    //                .GI ()
    //                .connect (
    //                        SendData.onGetMessageUpdateVersionNew (CMDClient.PROVIDER_ID));
    //    } else {
    //        gameControl.panelThongBao.onShow ("Vui lòng kiểm tra kết nối mạng!", delegate { });
    //    }
    //}

    //public void clickGioiThieuBanChoi () {
    //    if(checkNetWork ()) {
    //        //gameControl.panelWaiting.onShow();
    //        NetworkUtil
    //                .GI ()
    //                .connect (
    //                        SendData.onGetMessageIntroduceFriend (CMDClient.PROVIDER_ID));
    //    } else {
    //        gameControl.panelThongBao.onShow ("Vui lòng kiểm tra kết nối mạng!", delegate { });
    //    }


    //}

    public void clickQuenMK () {
        gameControl.panelInput.onShow ("LẤY LẠI MẬT KHẨU", "Tên đăng nhập:", "", delegate {
            string str = gameControl.panelInput.ip_enter.value;
            if(!str.Equals (""))
                NetworkUtil.GI ().connect (SendData.onGetMessagePass (str));
        });
    }

    public void clickCapNhat () {
        if(int.Parse (Res.version.Replace (".", "")) < BaseInfo.gI ().current_version) {
            gameControl.panelYesNo.onShow ("Đã có phiên bản mới. Bạn có muốn cập nhật không?", delegate {
                NetworkUtil.GI ().close ();
                Application.OpenURL (BaseInfo.gI ().linkdownload);
                //Gdx.app.exit();
                //screen.game.updateType = -1;
            });

        } else {
            gameControl.panelThongBao.onShow ("Bạn đã dùng phiên bản mới nhất!", delegate { });
        }
    }
    public void clickMoiBan () {
        gameControl.panelYesNo.onShow ("Bạn có muốn mời bạn bè vào chơi game Số Đỏ?", delegate {
            //screen.game.ui.onGioithieubanchoi("Mời bạn chơi game Số Đỏ" + "\n" + BaseInfo.gI().linkdownload);
            GameControl.sendSMS ("", "Mời bạn chơi game Số Đỏ" + "\n" + BaseInfo.gI ().linkdownload);
        });
    }



    public void OnSubmit () {
        //this.loginGroup.transform.localPosition = new Vector3(0, 0, 0);
    }

    public override void Appear () {
        base.Appear ();
        OnSubmit ();
    }

    public override void onBack () {
        base.onBack ();
        Application.Quit ();
    }

    //    void FixedUpdate () {
    //#if UNITY_WP8
    //        /*if(Input.GetButton ("Fire1") && (group.transform.localPosition.y == 160)) {
    //            OnHideKeyBoard ();
    //        }*/

    //        //if(Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
    //        //    OnHideKeyBoard ();
    //        //}
    //#endif
    //    }

    //    public void OnShowKeyBoard () {
    //#if UNITY_WP8
    //        if(Input.deviceOrientation != DeviceOrientation.Portrait && Input.deviceOrientation != DeviceOrientation.PortraitUpsideDown)
    //            TweenPosition.Begin (group, 0.1f, new Vector3 (260, 160, 0));
    //#endif
    //    }

    //    public void OnHideKeyBoard () {
    //        TweenPosition.Begin (group, 0.01f, new Vector3 (260, 0, 0));
    //    }
}
