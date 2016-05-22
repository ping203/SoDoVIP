using UnityEngine;
using System.Collections;
using System;

public class PanelTheCao : PanelGame {
    public UIInput ip_masothe, ip_serithe;
    public UILabel lb_menh_gia_the;

    // Update is called once per frame
    void Update () {
    }

    void OnEnable () {
        infoTygia ();
    }

    int typeCard = 2;

    public void setTypeCard (GameObject obj) {
        GameControl.instance.sound.startClickButtonAudio ();
        switch(obj.name) {
            case "Mobiphone":
                typeCard = 0;
                break;
            case "Vinaphone":
                typeCard = 1;
                break;
            case "Viettel":
                typeCard = 2;
                break;
        }
    }
    public void clickNapTheCao () {
        GameControl.instance.sound.startClickButtonAudio ();

        if(ip_masothe.value == null
            || ip_masothe.value.Trim ().Equals ("")
            || ip_masothe.value.Length > 15) {
            GameControl.instance.panelThongBao
                    .onShow ("Mã số thẻ không hợp lệ!", delegate { });
            return;
        }

        if(/*typeCard != 4 &&*/ (ip_serithe.value.Trim ().Equals (""))) {
            GameControl.instance.panelThongBao
                    .onShow ("Bạn hãy nhập vào số Serial", delegate { });
            return;
        }
        doRequestChargeMoneySimCard (BaseInfo.gI ().mainInfo.nick, typeCard, ip_masothe.value, ip_serithe.value);
        GameControl.instance.panelThongBao
                .onShow ("Hệ thống đang xử lý!", delegate { });
    }

    public void TuChoi () {
        GameControl.instance.sound.startClickButtonAudio ();
        ip_masothe.value = "";
        ip_serithe.value = "";
    }

    private void doRequestChargeMoneySimCard (string userName, int type,
            string cardCode, string series) {
        Message m;
        m = new Message (CMDClient.CMD_PAYCARD_SODO);
        try {
            m.writer ().WriteUTF (userName);
            m.writer ().WriteShort ((short) type);
            m.writer ().WriteUTF (cardCode);
            m.writer ().WriteUTF (series);
            NetworkUtil.GI ().sendMessage (m);
        } catch(Exception e) {
            Debug.LogException (e);
        }
    }

    public void infoTygia () {
        lb_menh_gia_the.text = "";
        for(int i = 0; i < BaseInfo.gI ().list_tygia.Count; i++) {
            TyGia tg = (TyGia) BaseInfo.gI ().list_tygia[i];
            lb_menh_gia_the.text += BaseInfo.formatMoneyDetailDot (tg.menhgia) + " vnđ = " + BaseInfo.formatMoneyDetailDot (tg.xu) + " " + Res.MONEY_FREE + "\n";
        }
    }
}
