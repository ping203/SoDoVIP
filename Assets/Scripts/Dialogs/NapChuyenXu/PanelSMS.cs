using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelSMS : PanelGame {
    public UILabel lb20, lb30;
    // Use this for initialization
    void Start () {
        if(lb20 != null || lb30 != null) {
            lb20.text = BaseInfo.formatMoneyDetailDot (BaseInfo.gI ().sms10) + " " + Res.MONEY_FREE;
            lb30.text = BaseInfo.formatMoneyDetailDot (BaseInfo.gI ().sms15) + " " + Res.MONEY_FREE;
        }
    }

    // Update is called once per frame
    void Update () {

    }

    public void NhanTin (string sms, string dauso, string thongbao) {
        GameControl.instance.panelYesNo.onShow (thongbao, delegate {
#if UNITY_EDITOR
            GameControl.instance.panelThongBao.onShow ("Soạn tin theo cú pháp: " + sms + " gửi đến " + dauso, delegate { });
#else
			GameControl.sendSMS(dauso, sms);
#endif
        });
    }

    public void onClick10 () {
        GameControl.instance.sound.startClickButtonAudio ();
        string tb = "Nhắn tin để nạp " + Res.MONEY_VIP_UPPERCASE + " " + BaseInfo.formatMoneyDetailDot (BaseInfo.gI ().sms10) + " (phí 10k)?";
        string sms = BaseInfo.gI ().syntax10 + " " + BaseInfo.gI ().mainInfo.userid;
        string ds = BaseInfo.gI ().port10;
        this.NhanTin (sms, ds, tb);
    }

    public void onClick15 () {
        GameControl.instance.sound.startClickButtonAudio ();
        string tb = "Nhắn tin để nạp " + Res.MONEY_VIP_UPPERCASE + " " + BaseInfo.formatMoneyDetailDot (BaseInfo.gI ().sms15) + " (phí 15k)?";
        string sms = BaseInfo.gI ().syntax15 + " " + BaseInfo.gI ().mainInfo.userid;
        string ds = BaseInfo.gI ().port15;
        this.NhanTin (sms, ds, tb);
    }
}
