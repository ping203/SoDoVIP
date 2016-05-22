using UnityEngine;
using System.Collections;

public class HasMasterCasino : BaseCasino {
    public UIButton btn_batdau;
    public UIButton btn_sansang;
    //public UILabel lb_sansang_name;
    public UIButton btn_datcuoc;
    public UILabel lb_Btn_sansang;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public override void onStartForView(string[] playingName) {
        base.onStartForView(playingName);
        if (BaseInfo.gI().isView) {
            disableAllBtnTable();
        }
    }
    public override void changBetMoney(long betMoney, string info) {
        base.changBetMoney(betMoney, info);
        if (!BaseInfo.gI().isView && !players[0].isMaster()) {
            btn_sansang.gameObject.SetActive(true);
        }
    }
    public override void onJoinView(Message message) {
        base.onJoinView(message);
        disableAllBtnTable();
    }

    public override void startTableOk(int[] cardHand, Message msg, string[] nickPlay) {
        base.startTableOk(cardHand, msg, nickPlay);

        if (BaseInfo.gI().isView) {
            disableAllBtnTable();
        }
        if(isPlaying) {
            lb_Btn_sansang.text = Res.TXT_XINCHO;
        } else {
            lb_Btn_sansang.text = Res.TXT_SANSANG;
        }
    }
    public override void onJoinTableSuccess(string master) {
        if (BaseInfo.gI().isView) {
            btn_batdau.gameObject.SetActive(false);
            btn_sansang.gameObject.SetActive(false);
            btn_datcuoc.gameObject.SetActive(false);
            toggleLock.gameObject.SetActive(false);
            return;
        }
        if (BaseInfo.gI().mainInfo.nick.Equals(master)) {
            btn_batdau.gameObject.SetActive(true);
            btn_sansang.gameObject.SetActive(false);
            btn_datcuoc.gameObject.SetActive(true);
            toggleLock.gameObject.SetActive(true);
        }
        else {
            btn_batdau.gameObject.SetActive(false);
            btn_sansang.gameObject.SetActive(true);
            btn_datcuoc.gameObject.SetActive(false);
            toggleLock.gameObject.SetActive(false);
        }
        if (isPlaying) {
            lb_Btn_sansang.text = Res.TXT_XINCHO;
        }
        else {
            lb_Btn_sansang.text = Res.TXT_SANSANG;
        }

    }

    public override void setMasterSecond(string master) {
        if (!isStart) {
            if (master.Equals(BaseInfo.gI().mainInfo.nick)) {
                btn_batdau.gameObject.SetActive(true);
                btn_sansang.gameObject.SetActive(false);
                btn_datcuoc.gameObject.SetActive(true);
                //groupKhoa.gameObject.SetActive(false);
                toggleLock.gameObject.SetActive(false);
            }
            else {
                btn_batdau.gameObject.SetActive(false);
                btn_datcuoc.gameObject.SetActive(false);
                if (players[0].isReady()) {
                    btn_sansang.gameObject.SetActive(false);
                }
                else {
                    btn_sansang.gameObject.SetActive(true);
                }

            }
        }

        if (master.Equals(BaseInfo.gI().mainInfo.nick)) {
            toggleLock.gameObject.SetActive(true);
        }
        else {
            toggleLock.gameObject.SetActive(false);
        }

        if (BaseInfo.gI().isView) {
            btn_batdau.gameObject.SetActive(false);
            btn_sansang.gameObject.SetActive(false);
            btn_datcuoc.gameObject.SetActive(false);
            toggleLock.gameObject.SetActive(false);
        }
        for (int i = 0; i < nUsers; i++) {
            if (!players[i].isSit()) {
                players[i].setInvite(true);
            }
            else {
                players[i].setInvite(false);
            }
        }
    }
    public void clickReady() {
        SendData.onReady(1);// san sang
        btn_sansang.gameObject.SetActive(false);
    }

    public void clickButtonDatcuoc() {
        gameControl.panelDatCuoc.onShow();
    }
    public void onClickButtonStart() {

        if (getTotalPlayerReady() > 1) {
            if (getTotalPlayerReady() < getTotalPlayer()) {
                gameControl.panelYesNo
                        .onShow("Còn người chưa sẳn sàng,\nBạn có muốn bắt đầu không?", delegate {
                    btn_batdau.gameObject.SetActive(false);
                    SendData.onStartGame();
                    for (int i = 0; i < nUsers; i++) {
                        players[i].setVisibleRank(false);
                    }
                });
            }
            else {
                SendData.onStartGame();
                btn_batdau.gameObject.SetActive(false);
                btn_datcuoc.gameObject.SetActive(false);
            }

        }
        else {
            gameControl.toast.showToast("Chưa đủ người chơi!");
        }

    }
    public override void disableAllBtnTable() {
        btn_batdau.gameObject.SetActive(false);
        btn_sansang.gameObject.SetActive(false);
        btn_datcuoc.gameObject.SetActive(false);
    }
}
