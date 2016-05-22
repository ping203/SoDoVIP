using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelSMS : PanelGame {
    public PanelNapChuyenXu panel;
    //public GameControl gameControl;
    public UILabel lb20, lb30;

    public GameObject item9022;
    public GameObject[] parent;
    public UILabel info9029;

    public UIToggle tgViettel, tgVina, tgMobi;
    public UILabel lbinfo9029;
    string dauso9029;
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

    public void onClick9029 (GameObject obj) {
        GameControl.instance.sound.startClickButtonAudio ();

        //GameObject obj = UICamera.currentTouch.pressed;

        string tb = "Nhắn tin để nạp " + BaseInfo.formatMoneyDetailDot (obj.GetComponent<Item9029> ().money) + " (phí " + obj.GetComponent<Item9029> ().name + ")?";
        string sms = obj.GetComponent<Item9029> ().sys + " " + BaseInfo.gI ().mainInfo.userid;
        string ds = obj.GetComponent<Item9029> ().port + "";
        this.NhanTin (sms, ds, tb);
    }

    public void add9022 (List<Item9029> list, int indexParent) {
        for(int i = 0; i < list.Count; i++) {
            GameObject btnT = Instantiate (item9022) as GameObject;
            //btnT.transform.parent = tblContaint.transform;
            parent[indexParent].GetComponent<UIGrid> ().AddChild (btnT.transform);
            btnT.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
            Vector3 vt = btnT.transform.localPosition;
            vt.z = -10;
            btnT.transform.localPosition = vt;

            btnT.GetComponent<Item9029> ().setText (list[i].name, list[i].sys, list[i].port, list[i].money);

            //EventDelegate.Set (btnT.GetComponent<UIButton> ().onClick, delegate {
            //   onClick9029 (btnT);
            //});
        }
    }

    public void setInfo () {
        if(tgViettel.value) {

        }
    }

    public void setToggleViettel () {
        for(int i = 0; i < panel.list_viettel.Count; i++) {
            bool isCheck = false;
            if(i == 0) {
                isCheck = true;
            }
            panel.list_viettel[i].GetComponent<UIToggle> ().value = isCheck;
        }

        lbinfo9029.text = panel.list_viettel[0].GetComponent<Item9029> ().sys + " " + BaseInfo.gI ().mainInfo.userid;
    }


    public void setToggleVina () {
        for(int i = 0; i < panel.list_vina.Count; i++) {
            bool isCheck = false;
            if(i == 0) {
                isCheck = true;
            }
            panel.list_vina[i].GetComponent<UIToggle> ().value = isCheck;
        }

        lbinfo9029.text = panel.list_vina[0].GetComponent<Item9029> ().sys + " " + BaseInfo.gI ().mainInfo.userid;
    }

    public void setToggleMobi () {
        for(int i = 0; i < panel.list_mobi.Count; i++) {
            bool isCheck = false;
            if(i == 0) {
                isCheck = true;
            }
            panel.list_mobi[i].GetComponent<UIToggle> ().value = isCheck;
        }

        lbinfo9029.text = panel.list_mobi[0].GetComponent<Item9029> ().sys + " " + BaseInfo.gI ().mainInfo.userid;
    }

    public void OnClickSend () {
        NhanTin ("", "", "");
    }
}
