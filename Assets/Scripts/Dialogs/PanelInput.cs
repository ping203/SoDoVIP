using UnityEngine;
using System.Collections;

public class PanelInput : PanelGame {
    public UILabel lb_title, lb_display_1, lb_display_2;
    public UIInput ip_enter;

    public UIButton btnOK;
    public delegate void CallBack ();
    public CallBack onClickOK;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void onShow (string title, string display1, string display2, CallBack clickOK) {
        DoOnMainThread.ExecuteOnMainThread.Enqueue (() => {
            lb_title.text = title;
            lb_display_1.text = display1;
            lb_display_2.text = display2;
            if(display2.Trim ().Equals ("SĐT:")) {
                doSetPhoneNumber ();
            }
            btnOK.gameObject.SetActive (true);
            onClickOK = clickOK;
            base.onShow ();
        });
    }

    public void onClickButtonOK () {
        GameControl.instance.sound.startClickButtonAudio ();
        // onHide ();
        onClickOK.Invoke ();
    }

    private void doSetPhoneNumber () {
        string phoneNumber = ip_enter.value;
        string info = "";
        if(phoneNumber.Equals ("")) {
            info = "Nhập vào số điện thoại";
        } else if(checkSDT (phoneNumber) == -1) {
            info = "Sai định dạng số điện thoại!";
        } else if(checkSDT (phoneNumber) == -3) {
            info = "Số điện thoại phải nhiều hơn 9 và ít hơn 12 ký tự";
        } else {
            //mainGame.ui.onLogin (BaseInfo.gI ().username, BaseInfo.gI ().pass, (byte) 1, phoneNumber);
            GameControl.instance.login.login ((sbyte) 4, BaseInfo.gI ().username, BaseInfo.gI ().pass, SystemInfo.deviceUniqueIdentifier, "", (sbyte) 1, BaseInfo.gI ().username, "", phoneNumber);
            SendData.onLoginfirst (phoneNumber, "", (sbyte) 1, "", 0);
        }
        if(info.Length > 0)
            GameControl.instance.panelThongBao.onShow (info, delegate { });
    }

    public int checkSDT (string sdt) {
        if(sdt.Length > 11 || sdt.Length < 10)
            return -3;

        for(int i = 0; i < sdt.Length; i++) {
            char c = sdt[i];
            if(('0' > c) || (c > '9')) {
                return -1;
            }
        }

        return 1;
    }

    void FixedUpdate () {
#if UNITY_WP8
        if(Input.GetButtonDown ("Fire1") && (this.transform.localPosition.y == 160)) {
            OnHideKeyBoard ();
        }

        if(Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
            OnHideKeyBoard ();
        }
#endif
    }

    public void OnShowKeyBoard () {
#if UNITY_WP8
        if(Input.deviceOrientation != DeviceOrientation.Portrait && Input.deviceOrientation != DeviceOrientation.PortraitUpsideDown)
            TweenPosition.Begin (this.gameObject, 0.1f, new Vector3 (0, 160, 0));
#endif
    }

    public void OnHideKeyBoard () {
        TweenPosition.Begin (this.gameObject, 0.01f, new Vector3 (0, 0, 0));
    }
}
