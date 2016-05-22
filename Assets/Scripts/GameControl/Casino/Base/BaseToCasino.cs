using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class BaseToCasino : BaseCasino {

    protected int first;
    public UIButton btn_fold, btn_check, btn_call,
            btn_callany;//, btn_check_fold
    public UIButton btn_ruttien;
    protected bool is_fold, is_check, is_call, is_callany;
    //public UIToggle cb_fold, cb_check_fold, cb_check, cb_call, cb_callany;
    public UILabel txt_moneyCuoc;
    public UILabel lb_fold, lb_check, lb_call, lb_callany;//, lb_check_fold
    protected long moneyCuoc = 0;
    long minMoney, maxMoney;
    bool isDongY;
    protected int lanuage = 1;
    public MoneyInpot[] moneyInPot;
    public MoneyInpot moneyTemp;
    public bool isAutoStart;
    protected bool started;

    public UIPanel sliderTo;
    public UISlider slider;
    public UILabel currentMoney;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Awake () {
        lanuage = 1;
        is_fold = false;
        //is_check_fold = false;
        is_check = false;
        is_call = false;
        is_callany = false;
    }
    //public override void calculDiem() {

    //}
    protected void setMoneyCuoc (long moneyCuoc) {
        try {
            long moneycuoc1 = getMaxChips () - players[0].getMoneyChip ();
            if(moneycuoc1 == 0) {
                txt_moneyCuoc.gameObject.SetActive (false);
                is_call = false;
            } else {
                if(moneycuoc1 > players[0].getFolowMoney ()) {
                    lb_call.text = Res.TXT_CALL[lanuage]
                            + " "
                            + BaseInfo.formatMoneyDetailDot (players[0]
                                    .getFolowMoney ());

                } else {
                    lb_call.text = (Res.TXT_CALL[lanuage] + " "
                            + BaseInfo.formatMoneyDetailDot (moneycuoc1));
                }

                is_check = false;
                setDisable (btn_check, true);

                if(this.moneyCuoc != moneycuoc1) {
                    is_call = false;
                }
            }
            if(players[0].getFolowMoney () == 0) {
                enableAllButton (false);
                lb_call.text = (Res.TXT_CALL[lanuage]);
            }
            this.moneyCuoc = moneycuoc1;
        } catch(Exception e) {
            // TODO: handle exception
        }

    }


    protected long getMaxChips () {
        long max = 0;
        for(int i = 0; i < players.Length; i++) {
            if(players[i].getMoneyChip () > max) {
                max = players[i].getMoneyChip ();
            }
        }
        return max;
    }
    public override void onStartForView (string[] playingName) {
        base.onStartForView (playingName);
        hideAllButton ();
    }
    public override void onJoinView (Message message) {
        // TODO Auto-generated method stub
        base.onJoinView (message);
        hideAllButton ();
        //btn_ruttien.gameObject.SetActive(false);
    }
    public override void onJoinTableSuccess (string master) {
        if(BaseInfo.gI ().isView) {
            disableAllBtnTable ();
        }
        for(int i = 0; i < nUsers; i++) {
            if(!players[i].isSit ()) {
                players[i].setInvite (true);
            } else {
                players[i].setInvite (false);
            }
        }
        masterID = "";
        toggleLock.gameObject.SetActive (false);
    }

    public override void setMasterSecond (string master) {
        for(int i = 0; i < nUsers; i++) {
            if(!players[i].isSit ()) {
                players[i].setInvite (true);
            } else {
                players[i].setInvite (false);
            }
        }
        masterID = "";
        toggleLock.gameObject.SetActive (false);
    }

    private void setDisable (UIButton button, bool isDisable) {
        UILabel lb = null;
        if(button == btn_call) {
            lb = lb_call;
        } else if(button == btn_callany) {
            lb = lb_callany;
        } else if(button == btn_check) {
            lb = lb_check;
        } else if(button == btn_fold) {
            lb = lb_fold;
        }

        if(isDisable) {
            if(lb != null) {
                lb.color = Color.gray;
            }
            //button.state = UIButtonColor.State.Disabled;
        } else {
            if(lb != null) {
                lb.color = Color.white;
            }
            //button.state = UIButtonColor.State.Normal;
        }

        button.enabled = !isDisable;
    }

    public override void setTurn (string nick, Message message) {
        base.setTurn (nick, message);
        if(!nick.Equals (BaseInfo.gI ().mainInfo.nick)) {
            hideThanhTo ();
        }
    }

    private void hideThanhTo () {
        sliderTo.gameObject.SetActive (false);
        //currentMoney.gameObject.SetActive(false);
    }

    public override void onNickSkip (String nick, Message msg) {
        // TODO Auto-generated method stub
        if(lanuage == 1) {
            players[getPlayer (nick)].setAction (Res.AC_UPBO);
        } else {
            players[getPlayer (nick)].setAction (Res.AC_FOLD);
        }
        players[getPlayer (nick)].setTurn (false);
        players[getPlayer (nick)].setRank (4);
        players[getPlayer (nick)].cardHand.setAllMo (true);
        players[getPlayer (nick)].setPlaying (false);
        if(getPlayer (nick) == 0) {
            disableAllBtnTable ();
        }
        try {
            setTurn (msg.reader ().ReadUTF (), msg);
        } catch(Exception e) {
            // TODO: handle exception
        }
    }

    public override void onFinishGame (Message message) {
        try {
            isPlaying = false;
            prevPlayer = -1;
            preCard = -1;
            isStart = false;
            int total = message.reader ().ReadByte ();
            BaseInfo.gI ().infoWin.Clear ();

            for(int i = 0; i < total; i++) {
                String nick = message.reader ().ReadUTF ();
                int rank = message.reader ().ReadByte ();
                long money = message.reader ().ReadLong ();

                if(getPlayer (nick) == 0) {
                    BaseInfo.gI ().infoWin.Add (new InfoWin (i + 1 + ". ", nick,
                            money, true));
                } else {
                    BaseInfo.gI ().infoWin.Add (new InfoWin (i + 1 + ". ", nick,
                            money, false));
                }
                // gameControl.dialogKetQua.setInfo(BaseInfo.gI().infoWin);
                nickFire = "";
                for(int j = 0; j < nUsers; j++) {
                    if(players[j].isPlaying ()
                            && players[j].getName ().Equals (nick)) {
                        players[j].setRank (rank);
                        players[j].setReady (false);
                        break;
                    }
                }
            }
            disableAllBtnTable ();
            onJoinTableSuccess (masterID);
            for(int j = 0; j < nUsers; j++) {
                if(players[j].isPlaying ()) {
                    players[j].setPlaying (false);
                }
                players[j].setTurn (false);
            }
            //SoundManager.Get().pauseAudio(SoundManager.AUDIO_TYPE.COUNT_DOWN);
        } catch(Exception ex) {
            Debug.LogException (ex);

        }
    }
    public override void disableAllBtnTable () {
        base.disableAllBtnTable ();
        showAllButton (true, false, false);
    }
    void resetButton () {
        is_fold = false;
        //is_check_fold = false;
        is_check = false;
        is_call = false;
        is_callany = false;

        //cb_fold.value = (false);
        //cb_fold.value = (false);
        //cb_check_fold.value = (false);
        //cb_check.value = (false);
        //cb_call.value = (false);
        //cb_callany.value = (false);
    }
    public override void resetData () {
        base.resetData ();
        if(moneyInPot != null) {
            for(int i = 0; i < moneyInPot.Length; i++) {
                moneyInPot[i].resetData ();
            }
        }
        if(moneyTemp != null) {
            moneyTemp.resetData ();
        }
        hideThanhTo ();
    }

    private void hideAllButton () {
        btn_call.gameObject.SetActive (false);
        btn_callany.gameObject.SetActive (false);
        btn_check.gameObject.SetActive (false);
        btn_fold.gameObject.SetActive (false);
        btn_ruttien.gameObject.SetActive (false);
    }
    protected void enableAllButton (bool isEnable) {
        setDisable (btn_call, !isEnable);
        setDisable (btn_check, !isEnable);
        setDisable (btn_callany, !isEnable);
        setDisable (btn_fold, !isEnable);
    }
    protected void showAllButton (bool isWait, bool isCheck,
            bool isVisible) {
        if(BaseInfo.gI ().isView) {
            hideAllButton ();
            return;
        }
        enableAllButton (true);
        isDongY = false;
        if(isWait && !BaseInfo.gI ().isView) {
            // TODO Auto-generated method stub
            lb_fold.text = (Res.TXT_FOLD[lanuage]);
            //lb_check_fold.text = (Res.TXT_CHECK_FOLD[lanuage]);
            lb_check.text = (Res.TXT_CHECK[lanuage]);
            lb_call.text = (Res.TXT_CALL[lanuage]);
            //lb_callany.text = (Res.TXT_CALL_ANY[lanuage]);
            setDisable (btn_call, true);
            setDisable (btn_callany, true);
            setDisable (btn_check, true);
            setDisable (btn_fold, true);
        } else {
            // TODO Auto-generated method stub
            lb_fold.text = (Res.TXT_FOLD[lanuage]);
            //lb_check_fold.text = (Res.TXT_CHECK_FOLD[lanuage]);
            lb_check.text = (Res.TXT_CHECK[lanuage]);
            lb_call.text = (Res.TXT_CALL[lanuage]);
            lb_callany.text = (Res.TXT_RAISE[lanuage]);
            //setDisable(btn_call, false);
            //setDisable(btn_callany, false);
            //setDisable(btn_check, false);
            setDisable (btn_fold, false);
            //setDisable(btn_check_fold, true);

        }

        if(!isCheck) {
            is_fold = false;
            //is_check_fold = false;
            is_check = false;
            is_call = false;
            is_callany = false;

        } else {

        }

        if(isVisible) {
            btn_fold.gameObject.SetActive (true);
            //btn_check_fold.gameObject.SetActive(true);
            btn_check.gameObject.SetActive (true);
            btn_call.gameObject.SetActive (true);
            btn_callany.gameObject.SetActive (true);
            btn_ruttien.gameObject.SetActive (false);
        } else {
            btn_fold.gameObject.SetActive (false);
            //btn_check_fold.gameObject.SetActive(false);
            btn_check.gameObject.SetActive (false);
            btn_call.gameObject.SetActive (false);
            btn_callany.gameObject.SetActive (false);
            btn_ruttien.gameObject.SetActive (true);
        }
    }
    protected long getMaxMoney (long money) {

        minMoney = getmoneyTong ();
        if(minMoney > BaseInfo.gI ().moneyMinTo) {
            minMoney = BaseInfo.gI ().moneyMinTo;
        }
        if(minMoney < 0) {
            minMoney = 0;
        }

        if(money < minMoney) {
            // return;
            money = minMoney;
        }
        if(money % 10 != 0) {
            money = money - money % 10 + 10;
        }

        if(money > getmoneyTong ()) {
            money = getmoneyTong ();
        }
        long m = money + getMaxChips () - players[0].getMoneyChip ();
        return m;
    }
    protected long getmoneyTong () {
        long money = players[0].getFolowMoney () - getMaxChips ()
                + players[0].getMoneyChip ();
        long max = 0;
        for(int i = 1; i < players.Length; i++) {
            if(players[i].isPlaying ()) {
                if(players[i].getFolowMoney () - getMaxChips ()
                        + players[i].getMoneyChip () >= max) {
                    max = players[i].getFolowMoney () - getMaxChips ()
                            + players[i].getMoneyChip ();
                }
            }
        }
        if(max < money) {
            money = max;
        }
        if(money < 0) {
            money = 0;
        }

        return money;
    }

    protected virtual void infoWinPlayer (InfoWinTo infoWin) {
        for(int i = 0; i < players.Length; i++) {
            players[i].cardHand.setAllMo (true);
            players[i].sp_xoay.gameObject.SetActive (false);
            try {
                players[i].sp_typeCard.gameObject.SetActive (false);
            } catch(Exception e) {
                // TODO: handle exception
            }
            try {
                players[i].cardHand3Cay.setAllMo (true);
            } catch(Exception e) {
                // TODO: handle exception
            }

            players[i].cardHand.reAddAllCard ();
        }

        int poss = getPlayer (infoWin.name);
        if(players[poss].isSit ()) {
            players[poss].sp_xoay.gameObject.SetActive (true);
        }

        players[poss].cardHand.setAllMo (false);
        try {
            players[poss].cardHand3Cay.setAllMo (false);
        } catch(Exception e) {
            // TODO: handle exception
        }
        int rank = infoWin.rank;
        if(infoWin.money > 0) {

            for(int i = 0; i < moneyInPot.Length; i++) {
                if(moneyInPot[i].moneyInPot == 0) {
                    continue;
                }
                int temps = 0;
                for(int j = 0; j < BaseInfo.gI ().infoWinTo.Count; j++) {
                    if(moneyInPot[i]
                            .checkExist (BaseInfo.gI ().infoWinTo[j].name)
                            && (BaseInfo.gI ().infoWinTo[j].rank == rank)) {
                        temps++;
                    }
                }
                if(temps != 0) {
                    long money = moneyInPot[i].moneyInPot / temps;

                    for(int j = 0; j < BaseInfo.gI ().infoWinTo.Count; j++) {
                        if(moneyInPot[i].checkExist (BaseInfo.gI ().infoWinTo
                            [j].name)
                                && (BaseInfo.gI ().infoWinTo[j].rank == rank)) {
                            moneyInPot[i].chiaChipToPlayer (money, players[poss]);

                            break;
                        }
                    }
                }

            }
        }
        for(int i = 0; i < BaseInfo.gI ().infoWinTo.Count; i++) {
            if(infoWin == BaseInfo.gI ().infoWinTo[i]) {
                BaseInfo.gI ().infoWinTo[i].rank = -1;
                break;
            }
        }
    }

    public override void onInfoWinPlayer (List<InfoWinTo> infoWin) {
        // TODO Auto-generated method stub
        // flyMoney();
        BaseInfo.gI ().infoWinTo = infoWin;
        flyMoney ();
        //currentTime = System.currentTimeMillis();

        if(infoWin.Count == 0) {
            return;
        }
        for(int i = 0; i < infoWin.Count; i++) {
            StartCoroutine (actionInfoWin (infoWin));
        }
    }
    IEnumerator actionInfoWin (List<InfoWinTo> infoWin) {
        for(int i = 0; i < infoWin.Count; i++) {
            yield return new WaitForSeconds (1.5f);
            infoWinPlayer (infoWin[i]);
        }

    }
    protected void flyMoney () {

        // TODO Auto-generated method stub
        BaseInfo.gI ().moneyMinTo = BaseInfo.gI ().moneyTable * 2;
        int dem = getBeginMoneyInpot ();
        for(int i = 0; i < nUsers; i++) {
            if(players[i].isPlaying ()) {
                if(players[i].getMoneyChip () > 0) {
                    moneyTemp.addChip (players[i].getMoneyChip (),
                            players[i].getName (), false);
                    players[i].flyMoney ();
                }
            } else if(!players[i].isPlaying ()) {
                if(players[i].getMoneyChip () > 0) {
                    moneyTemp.addChip (players[i].getMoneyChip (),
                            players[i].getName (), true);
                    players[i].flyMoney ();
                }
            }

        }
        moneyTemp.setMoneyInPot (moneyTemp.moneyInPot);
        moneyTemp.clearChipsPlayer ();
        int fuck = 0;
        while(!(moneyTemp.chipsPlayer.Count == 0)) {
            fuck++;
            if(fuck > 20) {
                break;
            }
            Debug.Log ("while ");
            long moneyT = moneyTemp.minIsPlaying ();
            long tongTien = 0;
            for(int i = 0; i < moneyTemp.chipsPlayer.Count; i++) {
                if(moneyTemp.chipsPlayer[i].isSkip) {
                    long moneyTs = moneyT;
                    if(moneyTs > moneyTemp.chipsPlayer[i].money) {
                        moneyTs = moneyTemp.chipsPlayer[i].money;
                    }
                    tongTien = tongTien + moneyTs;
                } else {
                    tongTien = tongTien + moneyT;
                }

            }
            Debug.Log ("dem = " + dem);
            Debug.Log ("moneyInPot.Length = " + moneyInPot.Length);
            moneyTemp.chiaChipToChip (tongTien, moneyInPot[dem]);
            for(int i = 0; i < moneyTemp.chipsPlayer.Count; i++) {
                if(moneyTemp.chipsPlayer[i].isSkip) {
                    long moneyTs = moneyT;
                    if(moneyTs > moneyTemp.chipsPlayer[i].money) {
                        moneyTs = moneyTemp.chipsPlayer[i].money;
                    }

                    moneyInPot[dem].addChip2 (moneyTs,
                            moneyTemp.chipsPlayer[i].name, false);
                    moneyTemp.rutChip (moneyTemp.chipsPlayer[i].name,
                            moneyTs);
                } else {
                    moneyInPot[dem].addChip2 (moneyT,
                            moneyTemp.chipsPlayer[i].name, false);
                    moneyTemp
                            .rutChip (moneyTemp.chipsPlayer[i].name, moneyT);
                }

            }
            moneyTemp.clearChipsPlayer ();
            dem++;
        }
    }

    public int getBeginMoneyInpot () {
        int dem = 0;
        for(int i = 0; i < moneyInPot.Length; i++) {
            if(i == 0) {
                if(moneyInPot[i].moneyInPot == 0) {
                    dem = i;
                    break;
                }
            }
            if(moneyInPot[i].moneyInPot == 0) {
                dem = i - 1;
                break;
            }
            if(i == moneyInPot.Length - 1) {
                if(moneyInPot[i].moneyInPot != 0) {
                    dem = i;
                    break;
                }
            }
        }
        bool isIncreas = false;
        for(int i = 0; i < nUsers; i++) {
            if(players[i].isPlaying ()) {
                if(players[i].getMoneyChip () == 0) {
                    for(int j = 0; j < moneyInPot[dem].chipsPlayer.Count; j++) {
                        if(players[i].getName ().Equals (
                                moneyInPot[dem].chipsPlayer[j].name)) {
                            isIncreas = true;
                            break;
                        }
                    }
                }

            }
        }
        if(isIncreas) {
            dem++;
        }
        return dem;
    }

    protected void baseSetturn (long moneyCuoc) {
        setMoneyCuoc (moneyCuoc);
        if(moneyCuoc <= 0) {
            if(is_check || is_callany || is_call) {
                SendData.onAccepFollow ();
                showAllButton (true, false, true);
            }
            if(is_fold) {
                SendData.onSendSkipTurn ();
                showAllButton (true, false, true);
            } else {
                showAllButton (false, false, true);
                setDisable (btn_call, true);
            }
        } else if(moneyCuoc < players[0].getFolowMoney ()) {
            if(is_fold) {
                SendData.onSendSkipTurn ();
                showAllButton (true, false, true);
            } else if(is_callany || is_call) {
                SendData.onAccepFollow ();
                showAllButton (true, false, true);
            } else {
                showAllButton (false, false, true);
                setDisable (btn_check, true);
                lb_call.text = (Res.TXT_CALL[lanuage] + " "
                        + BaseInfo.formatMoneyDetailDot (moneyCuoc));
            }
        } else {
            if(is_fold) {
                SendData.onSendSkipTurn ();
                showAllButton (true, false, true);
            } else if(is_callany || is_call) {
                SendData.onAccepFollow ();
                showAllButton (true, false, true);
            } else {
                showAllButton (false, false, true);
                setDisable (btn_check, true);
                setDisable (btn_callany, true);
                lb_call.text = (Res.TXT_ALLIN[lanuage]);
            }
        }
        if(getmoneyTong () == 0) {
            setDisable (btn_callany, true);
        }
    }
    public override void onNickCuoc (long moneyInPot, long soTienTo, long moneyBoRa, string nick, Message message) {
        if(BaseInfo.gI ().moneyTable * 2 >= soTienTo) {
            BaseInfo.gI ().moneyMinTo = BaseInfo.gI ().moneyTable * 2;
        } else {
            BaseInfo.gI ().moneyMinTo = soTienTo;
        }

        for(int i = 0; i < nUsers; i++) {
            if(players[i].isPlaying ()) {
                if(players[i].getName ().Equals (nick)) {
                    first++;
                    if(lanuage == 1) {
                        players[i].setAction (Res.AC_TO);
                    } else {
                        players[i].setAction (Res.AC_RAISE);
                    }

                    players[i].setMoneyChip (moneyBoRa
                            + players[i].getMoneyChip ());
                    gameControl.sound.startToAudio ();
                    break;
                }
            }
        }
        try {
            setTurn (message.reader ().ReadUTF (), message);
        } catch(Exception e) {
            // TODO: handle exception
        }

    }
    public override void onHaveNickTheo (long money, string nick, Message message) {
        if(money == 0) {
            if(lanuage == 1) {
                players[getPlayer (nick)].setAction (Res.AC_XEMBAI);
            } else {
                players[getPlayer (nick)].setAction (Res.AC_CHECK);
            }

        } else {
            if(lanuage == 1) {
                players[getPlayer (nick)].setAction (Res.AC_THEO);
            } else {
                players[getPlayer (nick)].setAction (Res.AC_CALL);
            }
        }
        players[getPlayer (nick)].setMoneyChip (money
                + players[getPlayer (nick)].getMoneyChip ());
        try {
            setTurn (message.reader ().ReadUTF (), message);
        } catch(Exception e) {
            // TODO: handle exception
            Debug.LogException (e);
        }
    }
    public void clickButtonFold () {
        //if (cb_fold.gameObject.activeInHierarchy) {
        //    if (is_fold) {
        //        resetButton();
        //        is_fold = false;
        //        cb_fold.value = false;
        //    }
        //    else {
        //        resetButton();
        //        is_fold = true;
        //        cb_fold.value = (true);
        //    }
        //}
        //else {
        SendData.onSendSkipTurn ();
        showAllButton (true, false, false);
        isDongY = false;
        players[0].setTurn (false);
        //}
    }
    public void clickButtonCheckFold () {
        //if (cb_check_fold.gameObject.activeInHierarchy) {
        //    if (is_check_fold) {
        //        resetButton();
        //        cb_check_fold.value = (false);
        //    }
        //    else {
        //        resetButton();
        //        is_check_fold = true;
        //        cb_check_fold.value = (true);
        //    }
        //}
        //else {
        SendData.onAccepFollow ();
        showAllButton (true, false, true);
        isDongY = false;
        players[0].setTurn (false);
        //}
    }
    public void clickButtonCheck () {
        //if (cb_check.gameObject.activeInHierarchy) {
        //    if (is_check) {
        //        resetButton();
        //        cb_check.value = (false);
        //    }
        //    else {
        //        resetButton();
        //        is_check_fold = true;
        //        cb_check.value = (true);
        //    }
        //}
        //else {
        SendData.onAccepFollow ();
        showAllButton (true, false, true);
        isDongY = false;
        players[0].setTurn (false);
        //}
    }
    public void clickButtonCall () {
        //if (cb_call.gameObject.activeInHierarchy) {
        //    if (is_call) {
        //        resetButton();
        //        is_call = false;
        //        cb_call.value = (false);
        //    }
        //    else {
        //        resetButton();
        //        is_call = true;
        //        cb_call.value = (true);
        //    }
        //}
        //else {
        SendData.onAccepFollow ();
        showAllButton (true, false, true);
        isDongY = false;
        players[0].setTurn (false);
        //}
    }
    public void clickButtonCallAny () {
        //if (cb_callany.gameObject.activeInHierarchy) {
        //    if (is_callany) {
        //        resetButton();
        //        is_callany = false;
        //        cb_callany.value = (false);
        //    }
        //    else {
        //        resetButton();
        //        is_callany = true;
        //        cb_callany.value = (true);
        //    }
        //}
        //else {
        if(isDongY) {
            players[0].setTurn (false);
            SendData.onCuocXT (-99, BaseInfo.gI ().moneyto);
            showAllButton (true, false, true);
            hideThanhTo ();
            setDisable (btn_callany, false);
        } else {
            minMoney = getmoneyTong ();
            if(minMoney > BaseInfo.gI ().moneyMinTo) {
                minMoney = BaseInfo.gI ().moneyMinTo;
            }
            if(minMoney < 0) {
                minMoney = 0;
            }
            if(minMoney > getmoneyTong ()) {
                minMoney = getmoneyTong ();
            }
            setMoneyTruot (minMoney);
            showThanhTo (
                    minMoney + getMaxChips ()
                            - players[0].getMoneyChip (),
                    getmoneyTong () + getMaxChips ()
                            - players[0].getMoneyChip ());
            //lb_callany.text = (Res.TXT_DONGY[lanuage]);

            setDisable (btn_callany, true);
        }
        isDongY = !isDongY;

        //}
    }
    public void clickButtnRutTien () {
        long temp = 0;
        if(RoomControl.roomType == 1) {
            temp = BaseInfo.gI ().mainInfo.moneyChip;
        } else {
            temp = BaseInfo.gI ().mainInfo.moneyXu;
        }
        if(temp < BaseInfo.gI ().moneyNeedTable) {
            gameControl.panelYesNo.onShow ("Không đủ tiền để rút, bạn có muốn nạp thêm? ", delegate {

            });

        } else {
            if(players[0].getFolowMoney () < BaseInfo.gI ().currentMinMoney) {
                gameControl.panelRutTien.show (BaseInfo.gI ().currentMinMoney,
                        BaseInfo.gI ().currentMaxMoney, 2, 0, 0, 0, RoomControl.roomType);

            } else {
                gameControl.panelRutTien.show (BaseInfo.gI ().currentMinMoney,
                      BaseInfo.gI ().currentMaxMoney, 3, 0, 0, 0, RoomControl.roomType);

            }

        }
    }
    private void showThanhTo (float min, float maxMoney) {
        slider.value = 0;
        sliderTo.gameObject.SetActive (true);
        currentMoney.gameObject.SetActive (true);

        currentMoney.text = BaseInfo.formatMoneyDetailDot ((int) min);
        minMoney = getmoneyTong ();

        if(minMoney > BaseInfo.gI ().moneyMinTo) {
            minMoney = BaseInfo.gI ().moneyMinTo;
        }
        this.maxMoney = (long) maxMoney;
        setMoneyTruot (minMoney);
    }
    long money;
    private void setMoneyTruot (long money) {
        minMoney = getmoneyTong ();
        if(minMoney > BaseInfo.gI ().moneyMinTo) {
            minMoney = BaseInfo.gI ().moneyMinTo;
        }
        if(minMoney < 0) {
            minMoney = 0;
        }

        if(money < minMoney) {
            // return;
            money = minMoney;
        }
        if(money % 10 != 0) {
            money = money - money % 10 + 10;
        }

        if(money > getmoneyTong ()) {
            money = getmoneyTong ();
        }
        this.money = money;

        BaseInfo.gI ().moneyto = money;
        currentMoney.text = (BaseInfo.formatMoneyDetailDot ((int) (BaseInfo
                        .gI ().moneyto + getMaxChips () - players[0]
                        .getMoneyChip ())));

    }
    public void OnSliderChange () {
        setMoneyTruot ((int) (slider.value * maxMoney));
    }
    public override void onTimeAuToStart (sbyte p) {
        base.onTimeAuToStart (p);
        isAutoStart = true;
        showAllButton (true, false, false);
    }
    public void clickButtonOkTo () {
        clickButtonCallAny ();
    }
    public void clickButtonCancelTo () {
        hideThanhTo ();
        setDisable (btn_callany, false);
        isDongY = false;
    }
}
