using UnityEngine;
using System.Collections;

public class PanelDatCuoc : PanelGame {
    public UISlider sliderMoney;
    public UILabel inputMoney;
    private long money;
    float rateVIP, rateFREE;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public void onChangeMoney() {
        bool isOk = false;
        float vl = sliderMoney.value;
        if (RoomControl.roomType == 1) {
            for (int j = 0; j < BaseInfo.gI().listBetMoneysFREE.Count; j++) {
                BetMoney b = BaseInfo.gI().listBetMoneysFREE[j];
                if (BaseInfo.gI().mainInfo.moneyXu < b.maxMoney) {
                    rateFREE = (float)1 / b.listBet.Count;
                    for (int i = 0; i < b.listBet.Count; i++) {
                        if (vl <= i * rateFREE) {
                            inputMoney.text = BaseInfo.formatMoneyDetailDot(b.listBet[i]); ;
                            money = b.listBet[i];
                            isOk = true;
                            break;
                        }
                    }
                }
                if (isOk) break;
            }
        }
        else {
            for (int j = 0; j < BaseInfo.gI().listBetMoneysVIP.Count; j++) {
                BetMoney b = BaseInfo.gI().listBetMoneysVIP[j];
                if (BaseInfo.gI().mainInfo.moneyXu < b.maxMoney) {
                    rateVIP = (float)1 / b.listBet.Count;
                    for (int i = 0; i < b.listBet.Count; i++) {
                        if (vl <= i * rateVIP) {
                            inputMoney.text = BaseInfo.formatMoneyDetailDot(b.listBet[i]); ;
                            money = b.listBet[i];
                            isOk = true;
                            break;
                        }
                    }
                }
                if (isOk) break;
            }
        }
    }

    public void clickOK () {
        GameControl.instance.sound.startClickButtonAudio ();
        SendData.onChangeBetMoney(money);
        this.onHide();
    }
    public void onShow() {
        sliderMoney.value = 0;
        onChangeMoney();
        base.onShow();
    }
}
