using UnityEngine;
using System.Collections;

public class PanelGetMoneyDay : PanelGame {
    public UILabel label_bim;
    long money;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void onShow(long money) {
        this.money = money;
        label_bim.text = "+" + money;
        base.onShow();
    }

    public void onCLickNhanThuong() {
        BaseInfo.gI().mainInfo.moneyXu += money;
        base.onHide();
    }
}
