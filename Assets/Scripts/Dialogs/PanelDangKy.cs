using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelDangKy : PanelGame {
    public UIInput ip_name, ip_pass, ip_confim_pass;
   // public UIToggle tg_sex;

    //public UISprite avata;

   // public PanelChangeAvata panelChangeAvata;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    //public void onRefresh () {
    //    ip_name.value = "";
    //    ip_phone.value = "";
    //    ip_gift.value = "";
    //}

    public void clickChangeAvata () {
        GameControl.instance.sound.startClickButtonAudio ();
        //panelChangeAvata.loadAva ();
        //panelChangeAvata.onShow ();
    }

    public void onClickDongY () {
        if(ip_name.value.Length <= 0) {
            GameControl.instance.panelThongBao.onShow ("Tên đăng nhập không được để trống!", delegate{});
            return;
        }
        if(ip_pass.value.Length <= 6) {
            GameControl.instance.panelThongBao.onShow ("Mật khẩu phải lớn hơn 6 kí tự!", delegate { });
            return;
        }

        if(ip_pass.value != ip_confim_pass.value) {
            GameControl.instance.panelThongBao.onShow ("Mật khẩu không khớp!", delegate { });
            return;
        }
        //if(checkSDT (ip_pass.value) == -1) {
        //    GameControl.instance.panelThongBao.onShow ("Sai định dạng số điện thoại!", delegate { });
        //    return;
        //}

        //if(checkSDT (ip_pass.value) == -3) {
        //    GameControl.instance.panelThongBao.onShow ("Số điện thoại phải nhiều hơn 9 và ít hơn 12 ký tự!", delegate { });
        //    return;
        //}
        //sbyte sex = tg_sex.value ? (sbyte) 1 : (sbyte) 0;
        //SendData.onLoginfirst (ip_pass.value, ip_name.value, sex, ip_gift.value, BaseInfo.gI().mainInfo.idAvata);
        SendData.onRegister (ip_name.value, ip_pass.value, SystemInfo.deviceUniqueIdentifier);
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

    private int checkName (string username) {
        if(username.Length > 10 || username.Length < 4)
            return -3;

        for(int i = 0; i < username.Length; i++) {
            char c = username[i];
            if((('0' > c) || (c > '9')) && (('A' > c) || (c > 'Z'))
                && (('a' > c) || (c > 'z'))) {
                return -1;
            }
        }
        bool isTrung = true;
        for(int i = 0; i < username.Length - 1; i++) {
            char c1 = username[i];
            char c2 = username[i + 1];
            if(c1 != c2) {
                isTrung = false;
                break;
            }
        }
        if(isTrung) {
            return -4;
        }
        bool isLT = false;
        if(username[0] == '0' || username[0] == '1') {
            isLT = true;
            for(int i = 0; i < username.Length - 1; i++) {
                char c1 = username[i];
                char c2 = username[i + 1];
                if(('0' <= c1) && (c1 <= '9')) {

                } else {
                    isLT = false;
                    break;
                }
                if(('0' <= c2) && (c2 <= '9')) {

                } else {
                    isLT = false;
                    break;
                }
                if(int.Parse (c1 + "") != int.Parse (c2 + "") - 1) {
                    isLT = false;
                    break;
                }
            }
        }
        if(isLT) {
            return -4;
        }
        return 1;
    }
}
