using UnityEngine;
using System.Collections;
using System;

public class Bacay : BaseCasino {
    int turntimeBC;
    long timeReceiveTurnBC;
    public TimerLieng timerWaiting;
    //public UILabel lbl_timeBC;

    public UIButton btn_cuoc, btn_bocuoc;
    //public UILabel lb_cuoc1, lb_cuoc2, lb_cuoc3, lb_cuoc4, lb_cuoc5,
    //      lb_cuoc6;
    public long int_cuoc1, int_cuoc2, int_cuoc3, int_cuoc4, int_cuoc5,
            int_cuoc6;
    sbyte time;// tim con lai de bat dau choi
    private long maxCuoc = 0;
    // Use this for initialization
    void Start() {

    }
    void Awake() {
        nUsers = 5;

    }

    public void clickBtnCuoc() {
        if (maxCuoc > BaseInfo.gI().betMoney * 10) {
            showDatcuoc(BaseInfo.gI().betMoney * 2,
                    BaseInfo.gI().betMoney * 10);
        }
        else {
            showDatcuoc(BaseInfo.gI().betMoney * 2, maxCuoc);
        }
    }

    //public void onClickButtonCuoc() {

    //    SendData.onSendCuocBC(int_cuoc1);
    //    gameControl.panelCuoc.onShow();
    //    showButtonCuoc(false);
    //}
    private void showDatcuoc(long min, long max) {
        //slider.setRange(min, max);
        //slider.setValue(0);
        //tiencuoc = 0;
        //Gdx.app.postRunnable(new Runnable() {

        //    @Override
        //    public void run() {
        //         TODO Auto-generated method stub
        //        currentMoney.setText("0");
        //    }
        //});
        //gr_datcuoc.setVisible(true);
        gameControl.panelCuoc.onShow(min, max);
        btn_bocuoc.gameObject.SetActive(false);
        btn_cuoc.gameObject.SetActive(false);
    }
    public void clickBtnBoCuoc() {
        btn_bocuoc.gameObject.SetActive(false);
        btn_cuoc.gameObject.SetActive(false);
        SendData.onSendCuocBC(0);
    }

    public override void onJoinTableSuccess(string master) {
        for (int i = 0; i < nUsers; i++) {
            if (!players[i].isSit()) {
                players[i].setInvite(true);
            }
            else {
                players[i].setInvite(false);
            }
        }
        masterID = "";
        //groupKhoa.gameObject.SetActive(false);
        toggleLock.gameObject.SetActive(false);
    }

    public override void setMasterSecond(string master) {
        for (int i = 0; i < nUsers; i++) {
            if (!players[i].isSit()) {
                players[i].setInvite(true);
            }
            else {
                players[i].setInvite(false);
            }
        }
        //groupKhoa.gameObject.SetActive(false);
        toggleLock.gameObject.SetActive(false);
    }
    public override void resetData() {
        base.resetData();
        //lbl_timeBC.gameObject.SetActive(false);
    }
    public override void calculDiem() {
        if (players[0].cardHand3Cay.getArrCardObj()[0].getId() != 52) {
            players[0].setInfo(diem3Cay());
        }
    }
    private string diem3Cay() {
        int a = BaseInfo.getScoreFinal(players[0].cardHand3Cay.getArrCardAll());
        if (a < 0) {
            return "";
        }
        int finalDiem = a % 10;
        String diem = "";
        if (finalDiem == 0) {
            diem = "10 điểm";
        }
        else {
            diem = finalDiem + " điểm";
        }
        return diem;
    }
    public override void onFinishGame(Message message) {
        //lbl_timeBC.gameObject.SetActive(false);
        //players[0].cardHand.removeAllCard();
        flyMoney();
        try {
            isPlaying = false;
            prevPlayer = -1;
            preCard = -1;
            isStart = false;
            int total = message.reader().ReadByte();
            BaseInfo.gI().infoWin.Clear();
            for (int i = 0; i < total; i++) {
                String nick = message.reader().ReadUTF();
                int rank = message.reader().ReadByte();
                if (rank != 1 && rank != 5) {
                    // int pl = getPlayer(nick);
                    // players[pl].cardHand.setAllMo(true);
                }
                long money = message.reader().ReadLong();
                int score = message.reader().ReadInt();
                String nicks = "";
                String info = "";
                if (score == 99) {
                    nicks = nick + ": " + "MƯỜI A RÔ" + " ";
                    info = "MƯỜI A RÔ";
                }
                else if (score == 100) {
                    nicks = nick + ": " + "SÁP" + " ";
                    info = "SÁP";
                }
                else {
                    nicks = nick + ": " + score + " điểm";
                    info = score + " điểm";
                }
                players[getPlayer(nick)].setReady(false);
                players[getPlayer(nick)].setInfo(info);
                if (getPlayer(nick) == 0) {
                    BaseInfo.gI().infoWin.Add(new InfoWin(i + 1 + ". ", nicks,
                            money, true));
                }
                else {
                    BaseInfo.gI().infoWin.Add(new InfoWin(i + 1 + ". ", nicks,
                            money, false));
                }
                nickFire = "";
                for (int j = 0; j < nUsers; j++) {
                    if (players[j].isPlaying()
                            && players[j].getName().Equals(nick)) {
                        players[j].setRank(rank);
                        break;
                    }
                }
            }
            disableAllBtnTable();
            for (int j = 0; j < nUsers; j++) {
                if (players[j].isSit()) {
                    players[j].setPlaying(false);
                    //players[j].setReady(false);
                }
                players[j].setTurn(false);
            }
            //SoundManager.Get().pauseAudio(SoundManager.AUDIO_TYPE.COUNT_DOWN);

            // finishTurn = true;
        }
        catch (Exception ex) {
            Debug.LogException(ex);
        }
    }
    public override void startTableOk(int[] cardHand, Message msg, string[] nickPlay) {
        base.startTableOk(cardHand, msg, nickPlay);
        players[0].setCardHand(new int[] { 52, 52, 52 }, true, false, false);
        players[0].cardHand3Cay.setArrCard(cardHand, true, false, false);
        for (int i = 1; i < players.Length; i++) {
            if(players[i].isPlaying () && !players[i].st_name.Equals ("")) {
                players[i].setCardHand(new int[] { 52, 52, 52 }, true, false,
                        false);
            }
        }
        turntime = 0;
        turntimeBC = 10;
        timerWaiting.setActiveNan(turntimeBC);
        timeReceiveTurnBC = GetCurrentMilli();
        //SoundManager.Get().startAudio(SoundManager.AUDIO_TYPE.COUNT_DOWN);
        gameControl.sound.startTineCountAudio ();
    }

    public override void onTimeAuToStart(sbyte time) {
        base.onTimeAuToStart(time);
        if (timerWaiting != null) {
            timerWaiting.setActiveXinCho(time);
        }
        disableAllBtnTable();
    }

    public override void onStartForView(string[] playingName) {
        base.onStartForView(playingName);
        if (BaseInfo.gI().isView) {
            disableAllBtnTable();
        }
        //lbl_timeBC.gameObject.SetActive(true);
        for (int i = 0; i < players.Length; i++) {
            if(players[i].isPlaying () && !players[i].st_name.Equals ("")) {
                players[i].setCardHand(new int[] { 52, 52, 52 }, true, false,
                        false);
            }

        }
        //lbl_timeBC.text = ("8");

        turntime = 0;
        turntimeBC = 10;
        timeReceiveTurnBC = GetCurrentMilli();
    }
    public override void setTurn(string nick, Message message) {
        base.setTurn(nick, message);

    }
    public override void InfoCardPlayerInTbl(Message message, string turnName, int time, sbyte numP) {
        base.InfoCardPlayerInTbl(message, turnName, time, numP);
        try {
            for (int i = 0; i < numP; i++) {
                String name = message.reader().ReadUTF();
                long chip = message.reader().ReadLong();
                players[getPlayer(name)].setPlaying(true);
                players[getPlayer(name)].cardHand.setArrCard(new int[] { 52,
						52, 52 }, false, false, false);
                if (chip >= 0) {
                    players[getPlayer(name)].setMoneyChip(chip);
                }
            }
            setTurn(turnName, time);
        }
        catch (Exception e) {
            // TODO: handle exception
        }
    }
    private void showButtonCuoc(bool isShow) {
        btn_cuoc.gameObject.SetActive(isShow);
        btn_bocuoc.gameObject.SetActive(isShow);
        //btn_cuoc3.gameObject.SetActive(isShow);
        //btn_cuoc4.gameObject.SetActive(isShow);
        //btn_cuoc5.gameObject.SetActive(isShow);
        //btn_cuoc6.gameObject.SetActive(isShow);

        long maxCuoc = players[0].getFolowMoney();
        if (maxCuoc > players[getPlayer(masterID)].getFolowMoney()) {
            maxCuoc = players[getPlayer(masterID)].getFolowMoney();
        }
        this.maxCuoc = maxCuoc;
    }
    public override void onBeginRiseBacay(Message message) {
        base.onBeginRiseBacay(message);
        try {
            //progress.gameObject.SetActive(false);

            players[getPlayer(masterID)].setMoneyChip(0);
            //SoundManager.Get().startAudio(SoundManager.AUDIO_TYPE.COUNT_DOWN);
            gameControl.sound.startTineCountAudio ();

            turntimeBC = message.reader().ReadByte();
            for (int i = 0; i < players.Length; i++) {
                players[i].resetData();
            }
            if (turntimeBC == -1) {
                turntimeBC = message.reader().ReadByte();
                //lbl_timeBC.text = ("Bàn đang bắt đầu đặt cược!");
                //lbl_timeBC.gameObject.SetActive(true);
                players[0].setPlaying(false);
                showButtonCuoc(false);
            }
            else {
                players[0].setPlaying(true);
                if (players[0].isMaster()) {
                    //lbl_timeBC.text = ("Thời gian nhận cược còn lại: ");
                }
                else {
                    //lbl_timeBC.text = ("Thời gian đặt cược còn lại: ");
                }
                //lbl_timeBC.gameObject.SetActive(true);
                timeReceiveTurnBC = GetCurrentMilli();
                if (!players[0].isMaster()) {
                    showButtonCuoc(true);
                }

            }
            timerWaiting.setActiveCuoc(turntimeBC);
        }
        catch (Exception e) {
            // TODO: handle exception
            Debug.LogException(e);
        }
    }
    public override void disableAllBtnTable() {
        base.disableAllBtnTable();
        btn_cuoc.gameObject.SetActive(false);
        btn_bocuoc.gameObject.SetActive(false);
        //btn_cuoc3.gameObject.SetActive(false);
        //btn_cuoc4.gameObject.SetActive(false);
        //btn_cuoc5.gameObject.SetActive(false);
        //btn_cuoc6.gameObject.SetActive(false);
    }
    public override void onCuoc3Cay(Message message) {
        base.onCuoc3Cay(message);
        try {
            if (message.reader().ReadByte() == 1) {
                String nickCuoc = message.reader().ReadUTF();
                long moneyCuoc = message.reader().ReadLong();
                players[getPlayer(nickCuoc)].setMoneyChip(moneyCuoc);
                for (int i = 0; i < players.Length; i++) {
                    if (players[i].isMaster()) {
                        players[i].setMoneyChip(moneyCuoc
                                + players[i].getMoneyChip());
                    }
                }

            }
            else {
                string mess = message.reader().ReadUTF();
                // CasinoActivity.gI().showToast(mess);
                showButtonCuoc(true);
            }

        }
        catch (Exception e) {
            // TODO: handle exception
        }
    }
    public override void onInfome(Message message) {
        base.onInfome(message);
        try {
            isStart = true;
            players[0].setPlaying(true);
            if (message.reader().ReadByte() == 1) {
                turntimeBC = message.reader().ReadByte();
                long chip = message.reader().ReadLong();
                if (chip >= 0) {
                    players[0].setMoneyChip(chip);
                }
                //if (players[0].isMaster()) {
                //lbl_timeBC.text = ("Thời gian nhận cược còn lại: ");
                //}
                //else {
                //lbl_timeBC.text = ("Thời gian đặt cược còn lại: ");
                //}
                //lbl_timeBC.gameObject.SetActive(true);

                timeReceiveTurnBC = GetCurrentMilli();
                if (!players[0].isMaster()) {
                    showButtonCuoc(true);
                }
            }
            else {
                turntimeBC = message.reader().ReadByte();
                timeReceiveTurnBC = GetCurrentMilli();
                //lbl_timeBC.gameObject.SetActive(true);
                sbyte len = message.reader().ReadByte();
                int[] cardHand = new int[len];

                for (int i = 0; i < cardHand.Length; i++) {
                    cardHand[i] = message.reader().ReadByte();
                }
                players[0].setCardHand(new int[] { 52, 52, 52 }, true, false,
                        false);
                players[0].cardHand3Cay
                        .setArrCard(cardHand, true, false, false);
            }
            timerWaiting.setActiveCuoc(turntimeBC);
        }
        catch (Exception e) {
            // TODO: handle exception
        }
    }
    public override void onJoinTableSuccess(Message message) {
        base.onJoinTableSuccess(message);
        int_cuoc1 = 0;
        int_cuoc2 = BaseInfo.gI().moneyTable * 2;
        int_cuoc3 = BaseInfo.gI().moneyTable * 3;
        int_cuoc4 = BaseInfo.gI().moneyTable * 4;
        int_cuoc5 = BaseInfo.gI().moneyTable * 5;
        int_cuoc6 = BaseInfo.gI().moneyTable * 10;


        //lb_cuoc1.text = ("Bỏ Cược");
        //lb_cuoc2.text = (Res.TXT_DAT[1] + " $"
        //        + BaseInfo.formatMoney(int_cuoc2));
        //lb_cuoc3.text = (Res.TXT_DAT[1] + " $"
        //        + BaseInfo.formatMoney(int_cuoc3));
        //lb_cuoc4.text = (Res.TXT_DAT[1] + " $"
        //        + BaseInfo.formatMoney(int_cuoc4));
        //lb_cuoc5.text = (Res.TXT_DAT[1] + " $"
        //        + BaseInfo.formatMoney(int_cuoc5));
        //lb_cuoc6.text = (Res.TXT_DAT[1] + " $"
        //        + BaseInfo.formatMoney(int_cuoc6));

    }
}
