using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class BaseCasino : StageControl {
    public ArrayCard tableArrCard1, tableArrCard2, cardTable;
    public int[] tableArrCard;
    protected float xPlay = -350;
    public string masterID;
    private sbyte rule;
    public bool isPlaying = false;
    public bool isStart;
    public ABSUser[] players;
    public int nUsers;
    public UILabel lblTableName;
    public UILabel lbInfoTable;
    public UILabel lbInfoMoney;
    public UILabel lb_luatchoi;
    //public GameObject groupKhoa;
    public UIToggle toggleLock;

    protected bool finishTurn;
    protected String nickFire = "";
    public HistoryChat historychat;
    private bool isOpenChat = false;

    //public UIProgressBar progress;
    float valueProgress;
    float velocityProgress;

    //public UISprite strengNetwork;

    // Use this for initialization
    void Awake() {
    }

    void Start() {
        // EventDelegate.Set (toggleLock.onChange, clickButtonKhoa);
    }

    // Update is called once per frame
    //public void act() {

    //    if (progress != null) {
    //        if (progress.gameObject.activeInHierarchy) {
    //            valueProgress = valueProgress - Time.deltaTime * velocityProgress;

    //            if (valueProgress <= 0) {
    //                valueProgress = 0;
    //                progress.gameObject.SetActive(false);
    //            }
    //            progress.value = valueProgress;
    //        }

    //    }
    //}

    void FixedUpdate() {
        //act ();
        if (this.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape) && !gameControl.panelYesNo.isShow) {
            onBack();
        } else if (Input.GetKeyDown(KeyCode.Escape) && gameControl.panelYesNo.isShow) {
            gameControl.panelYesNo.onHide();
        }

        /*if(this.gameObject.activeInHierarchy) {
            getStrengNet ();
        }*/
    }

    void getStrengNet() {
#if UNITY_WP8
        /*if(strengNetwork != null) {
            string name = "network_streng-0";
            int speed = UnityPluginForWindowPhone.Class1.getNetworkInterfaces ();
            if(speed <= 0) {
                name = "network_streng-0";
            } else if(speed <= 10000) {
                name = "network_streng-1";
            } else if(speed <= 20000) {
                name = "network_streng-2";
            } else {
                name = "network_streng-3";
            }

            strengNetwork.spriteName = name;
        }*/

        /*if(strengNetwork != null) {
            string name = "network_streng-0";
            string speed = UnityPluginForWindowPhone.Class1.getStrongSignal ();
            if(speed != null) {
                name = "network_streng-" + speed;
            }
            Debug.Log ("=-=-=-=-=================== " + speed);
            //strengNetwork.spriteName = name;
        }*/
#endif
    }



    public void clickButtonChat() {
        if (!BaseInfo.gI().isView)
            gameControl.panelChat.onShow();
    }

    public void clickKetQua() {
        //gameControl.dialogKetQua.onShow();
    }
    public virtual void calculDiem() {

    }
    public virtual void changBetMoney(long betMoney, string info) {
        setTableName(BaseInfo.gI().nameTale, BaseInfo.gI().idTable, betMoney);
        gameControl.panelThongBao.onShow(info, delegate { });
    }
    public virtual void onJoinView(Message message) {
        try {
            resetData();
            BaseInfo.gI().isView = true;
            nUsers = BaseInfo.gI().numberPlayer;
            for (int i = 0; i < players.Length; i++) {
                players[i].setExit();
            }
            this.rule = message.reader().ReadByte();
            setLuatChoi(rule);
            masterID = message.reader().ReadUTF();
            int len = message.reader().ReadByte();
            BaseInfo.gI().timerTurnTable = message.reader().ReadInt();
            isPlaying = message.reader().ReadBoolean();
            PlayerInfo[] pl = new PlayerInfo[len];
            for (int i = 0; i < len; i++) {
                String name = message.reader().ReadUTF();
                String displayname = message.reader().ReadUTF();
                String link_avatar = message.reader().ReadUTF();
                int idAvata = message.reader().ReadInt();
                int isPos = message.reader().ReadByte();
                long money = message.reader().ReadLong();
                //sbyte genders = message.reader().ReadByte();
                bool ready = message.reader().ReadBoolean();
                long folowMoney = message.reader().ReadLong();
                pl[i] = new PlayerInfo();
                pl[i].name = name;
                pl[i].displayname = displayname;
                pl[i].idAvata = idAvata;
                pl[i].link_avatar = link_avatar;
                pl[i].posServer = isPos;
                pl[i].money = money;
                //pl[i].gender = genders;
                pl[i].folowMoney = folowMoney;
                if (pl[i].name.Equals(masterID)) {
                    pl[i].isMaster = true;

                    if (pl[i].name.Equals(BaseInfo.gI().mainInfo.nick)) {
                        pl[i].isVisibleInvite = true;
                    } else {
                        pl[i].isVisibleInvite = false;
                    }
                } else {
                    pl[i].isReady = ready;
                }

                players[i].CreateInfoPlayer(pl[i]);

            }
            for (int i = 1; i < nUsers; i++) {
                if (!players[i].isSit())
                    players[i].setInvite(true);
            }

            onJoinTableSuccess(masterID);
            setTableName(BaseInfo.gI().nameTale, BaseInfo.gI().idTable,
                    BaseInfo.gI().moneyTable);

            gameControl.panelWaiting.onHide();
            //if(len < BaseInfo.gI ().numberPlayer)
            //    SendData.onJoinTable (BaseInfo.gI ().mainInfo.nick,
            //            BaseInfo.gI ().idTable, "", -1);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    public virtual void onJoinTableSuccess(Message message) {
        try {

            resetData();
            BaseInfo.gI().isView = false;
            gameControl.sound.startVaobanAudio();
            nUsers = BaseInfo.gI().numberPlayer;
            for (int i = 0; i < players.Length; i++) {
                players[i].setExit();
            }
            this.rule = message.reader().ReadByte();
            setLuatChoi(rule);
            masterID = message.reader().ReadUTF();
            int len = message.reader().ReadByte();
            BaseInfo.gI().timerTurnTable = message.reader().ReadInt();
            isPlaying = message.reader().ReadBoolean();
            PlayerInfo[] pl = new PlayerInfo[len];
            int indexmy = 0;
            for (int i = 0; i < len; i++) {
                String name = message.reader().ReadUTF();
                String displayname = message.reader().ReadUTF();
                String link_avatar = message.reader().ReadUTF();
                int idAvata = message.reader().ReadInt();
                int isPos = message.reader().ReadByte();
                long money = message.reader().ReadLong();
                //sbyte genders = message.reader().ReadByte();
                bool ready = message.reader().ReadBoolean();
                long folowMoney = message.reader().ReadLong();
                pl[i] = new PlayerInfo();
                pl[i].name = name;
                pl[i].displayname = displayname;
                pl[i].idAvata = idAvata;
                pl[i].link_avatar = link_avatar;
                pl[i].posServer = isPos;
                pl[i].money = money;
                //pl[i].gender = genders;
                pl[i].folowMoney = folowMoney;
                if (pl[i].name.Equals(masterID)) {
                    pl[i].isMaster = true;

                    toggleLock.value = true;

                    if (pl[i].name.Equals(BaseInfo.gI().mainInfo.nick)) {
                        pl[i].isVisibleInvite = true;
                    } else {
                        pl[i].isVisibleInvite = false;
                    }
                } else {
                    pl[i].isReady = ready;
                }
                if (pl[i].name.Equals(BaseInfo.gI().mainInfo.nick)) {
                    pl[i].isVisibleInvite = false;
                    players[0].CreateInfoPlayer(pl[i]);

                    indexmy = pl[i].posServer;
                }
            }
            for (int i = 1; i < nUsers; i++) {
                if (!players[i].isSit())
                    players[i].setInvite(true);
            }

            for (int i = 0; i < len; i++) {
                if (i != indexmy) {
                    setPlayerInfo(pl[i], indexmy);
                }
            }

            onJoinTableSuccess(masterID);
            setTableName(BaseInfo.gI().nameTale, BaseInfo.gI().idTable,
                    BaseInfo.gI().moneyTable);

            gameControl.panelWaiting.onHide();
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void setTableName(string name, short id, long money) {
        //string typePhong = "";
        //if(BaseInfo.gI().typetableLogin == Res.ROOMFREE) {
        //    typePhong = "FREE";
        //} else {
        //    typePhong = "VIP";
        //}
        lblTableName.text = name;
        lbInfoTable.text = "Bàn: " + id;

        if (lbInfoMoney != null) {
            lbInfoMoney.text = "Mức cược\n" + BaseInfo.formatMoneyDetail(money) + Res.MONEY_FREE;
        }

        setLuatChoi(rule);
    }

    public abstract void onJoinTableSuccess(string master);

    public void setPlayerInfo(PlayerInfo pl, int start) {
        if (players != null) {
            int pos = 0;
            pos = pl.posServer - start;
            if (pos < 0) {
                pos += nUsers;
            }
            players[pos].CreateInfoPlayer(pl);

            players[pos].setInvite(false);
        }
    }

    public virtual void resetData() {
        for (int i = 0; i < players.Length; i++) {
            players[i].setExit();
        }
        disableAllBtnTable();
        //if (progress != null) {
        //    progress.gameObject.SetActive(false);
        //}
    }
    protected String[] luatchoi = new String[] { "TÁI GỬI", "KHÔNG TÁI GỬI" };
    public virtual void setLuatChoi(sbyte readByte) {
        if (gameControl.gameID == GameID.PHOM) {
            lb_luatchoi.text = luatchoi[readByte];
        } else {
            lb_luatchoi.text = "";
        }
    }

    public void removePlayer(String userinfo) {
        for (int i = 0; i < nUsers; i++) {
            if (players[i] != null) {
                if (players[i].getName().Equals(userinfo)) {
                    players[i].setExit();
                    return;
                }
            }
        }
    }

    public int getPlayer(String nick) {
        for (int i = 0; i < nUsers; i++) {
            if (players[i].isSit()) {
                if (players[i].getName().Equals(nick)) {
                    return i;
                }
            }
        }
        for (int i = 0; i < nUsers; i++) {
            if (!players[i].isSit()) {
                return i;
            }
        }
        return 0;
    }

    public virtual void setMaster(String nick) {
        masterID = nick;
        if (players != null) {
            for (int i = 0; i < nUsers; i++) {
                if (players[i] != null) {
                    if (players[i].getName().Equals(nick) && players[i].getName() != "") {
                        players[i].setMaster(true);
                        if (players[i].getName().Equals(
                                BaseInfo.gI().mainInfo.nick)) {
                            //groupKhoa.SetActive(true);
                            toggleLock.gameObject.SetActive(true);
                            toggleLock.value = true;
                            for (int j = 1; j < nUsers; j++) {
                                players[j].setInvite(true);
                            }
                        } else {
                            for (int j = 1; j < nUsers; j++) {
                                players[j].setInvite(false);
                            }
                        }
                        players[i].setReady(false);
                    } else {
                        players[i].setMaster(false);
                    }
                }
            }
        }
        setMasterSecond(nick);
    }

    public abstract void setMasterSecond(string master);

    protected int turntime = 30;
    protected String turnName = "";
    public long timeReceiveTurn;
    protected bool isReceiveInFoWinPlayer = false;
    public int preCard = -1, prevPlayer = -1;

    public virtual void startTableOk(int[] cardHand, Message msg, string[] nickPlay) {
        isReceiveInFoWinPlayer = false;
        preCard = -1;
        prevPlayer = -1;
        turnName = "";
        turntime = 30;
        timeReceiveTurn = 0;
        isStart = true;
        disableAllBtnTable();
        for (int i = 0; i < players.Length; i++) {

            players[i].resetData();
            if (players[i].isSit()) {
                players[i].setReady(false);
                players[i].resetData();
                players[i].setPlaying(false);
            }
        }
        for (int i = 0; i < nickPlay.Length; i++) {
            players[getPlayer(nickPlay[i])].setPlaying(true);
        }
        //if (progress != null) {
        //    progress.gameObject.SetActive(false);
        //}


        gameControl.sound.startchiabaiAudio();
    }

    public void actionNem(int id, string nem, string biNem) {
        ABSUser player1 = players[getPlayer(nem)];
        ABSUser player2 = players[getPlayer(biNem)];

        float distance = Vector2.Distance(player1.transform.position, player2.transform.position);
        float time = distance / 2;

        string actionsName = null;

        switch (id) {
            case 1:
                actionsName = "Bia";
                break;
            case 2:
                actionsName = "CaChua";
                break;
            case 3:
                actionsName = "DepTong";
                break;
            case 4:
                actionsName = "HoaHong";
                break;
            case 5:
                actionsName = "Bom";
                break;
            case 6:
                actionsName = "Trung";
                break;
            default:
                break;
        }
        GameObject obj = Instantiate(Resources.Load("" + actionsName, typeof(GameObject))) as GameObject;

        obj.transform.parent = player1.transform.parent;
        obj.transform.localPosition = player1.transform.localPosition;
        obj.transform.localScale = new Vector3(100, 100, 100);

        TweenPosition tw = TweenPosition.Begin(obj, time, player2.transform.localPosition);
        EventDelegate.Set(tw.onFinished, delegate { finish(obj); });
    }

    void finish(GameObject ob) {
        ob.GetComponent<Animator>().SetBool("isAction", true);
        Destroy(ob, 3f);
    }

    public void sendChatQuick() {
        SendData.onSendMsgChat(BaseInfo.gI().mainInfo.nick, "hahaha");
    }

    public virtual void disableAllBtnTable() {

    }

    public void setPlayerInfoView(PlayerInfo pl, int pos) {
        players[pos].CreateInfoPlayer(pl);
        if (getNumPlayer() == 1) {
            setMaster(players[pos].getName());
        }
    }
    private int getNumPlayer() {
        int num = 0;
        for (int i = 0; i < players.Length; i++) {
            if (!players[i].getName().Equals("")) {
                num++;
            }
        }
        return num;
    }
    public virtual void onStartForView(String[] playingName) {
        // TODO Auto-generated method stub
        turnName = "";
        turntime = 30;
        timeReceiveTurn = 0;
        isStart = true;
        for (int i = 0; i < players.Length; i++) {
            players[i].setVisibleRank(false);
        }
        for (int i = 0; i < players.Length; i++) {
            if (players[i].getName().Length > 0) {
                players[i].setReady(false);
                players[i].resetData();
                players[i].setPlaying(false);
            }
        }
        for (int i = 0; i < playingName.Length; i++) {
            players[getPlayer(playingName[i])].setPlaying(true);
        }
    }

    public override void onBack() {
        gameControl.panelYesNo.onShow("Bạn có muốn rời bàn không?", delegate {
            gameControl.panelWaiting.onShow();
            if (BaseInfo.gI().isView) {
                //SendData.onOutView ();
            } else {
                SendData.onOutTable();
            }
        });

    }

    protected void addCardHandOtherPlayer(int numCard, int pos) {
        players[pos].cardHand.setVisibleSobai(false);
        int[] card = new int[numCard];
        for (int i = 0; i < card.Length; i++) {
            card[i] = 52;
        }
        players[pos].cardHand.setArrCard(card, true, true, false);
        //players[pos].setSoBai(numCard);
    }

    public virtual void setTurn(String nick, Message message) {
        if (nick.Equals("")) {
            return;
        }
        for (int i = 0; i < nUsers; i++) {
            players[i].setTurn(false);
        }
        players[getPlayer(nick)].setReceiveTurnTime(timeReceiveTurn);
        if (getPlayer(nick) == 0 && !BaseInfo.gI().isView) {
            gameControl.sound.startTineCountAudio();
        }
        timeReceiveTurn = BaseInfo.gI().timerTurnTable;
        turnName = nick;

        if (nick.Equals(BaseInfo.gI().mainInfo.nick) && !BaseInfo.gI().isView) {
            gameControl.sound.pauseSound();
            gameControl.sound.startTineCountAudio();
            gameControl.sound.PlayVibrate();
        }
    }

    public void setTurn(String nick, int time) {
        if (nick.Equals("")) {
            return;
        }
        for (int i = 0; i < nUsers; i++) {
            players[i].setTurn(false);
        }
        players[getPlayer(nick)].setReceiveTurnTime(timeReceiveTurn);
        turnName = nick;
        timeReceiveTurn = BaseInfo.gI().timerTurnTable;

        if (nick.Equals(BaseInfo.gI().mainInfo.nick) && !BaseInfo.gI().isView) {
            gameControl.sound.pauseSound();
            gameControl.sound.startTineCountAudio();
            gameControl.sound.PlayVibrate();
        }
    }

    public int getTotalPlayer() {
        // tong so nguoi choi dang co tren ban
        int total = 0;
        for (int i = 0; i < nUsers; i++) {
            if (players[i].isSit()) {
                total++;
            }
        }
        return total;
    }

    public int getTotalPlayerReady() {
        // tong so nguoi choi dang co tren ban
        int total = 1;
        for (int i = 0; i < nUsers; i++) {
            if (players[i].isReady() && players[i].isSit()) {
                total++;
            }
        }
        return total;
    }

    public int getTotalPlayerPlaying() {
        // tong so nguoi choi dang co tren ban
        int total = 0;
        for (int i = 0; i < nUsers; i++) {
            if (players[i].isPlaying()) {
                total++;
            }
        }
        return total;
    }

    public virtual void onFireCard(string nick, string turnname, int[] card) {
        //BaseInfo.gI().startchiabaiAudio();
        //SoundManager.Get().startAudio(SoundManager.AUDIO_TYPE.CHIABAI);
        try {
            nickFire = nick;
            finishTurn = false;
            players[getPlayer(nick)].cardHand.onfireCard(card);
            players[getPlayer(nick)].setTurn(false);
            //if(getPlayer (nick) != 0) {

            int sobai = players[getPlayer(nick)].cardHand.getSoBai() - card.Length;
            if (sobai < 0) {
                sobai = 0;
            }
            players[getPlayer(nick)].cardHand.setSobai(sobai);
            //}
            setTurn(turnname, null);
        } catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    public virtual void onFireCardFail() {
        gameControl.toast.showToast("Không thể đánh!");
    }

    public virtual void onNickSkip(string nick, string turnName) {
        players[getPlayer(nick)].setAction(Res.AC_BOLUOT);
        players[getPlayer(nick)].setTurn(false);
        //SoundManager.Get().pauseAudio(SoundManager.AUDIO_TYPE.COUNT_DOWN);
    }

    public virtual void onNickSkip(string nick, Message msg) {

    }

    public virtual void onFinishTurn() {

    }

    public virtual void allCardFinish(string nick, int[] card) {
        card = RTL.sort(card);
        if (players[getPlayer(nick)].isPlaying()) {
            players[getPlayer(nick)].setCardHandInFinishGame(card);
        }
    }

    public virtual void onFinishGame(Message message) {
        try {
            isPlaying = false;
            prevPlayer = -1;
            preCard = -1;
            isStart = false;
            int total = message.reader().ReadByte();
            BaseInfo.gI().infoWin.Clear();
            for (int i = 0; i < nUsers; i++) {
                players[i].setMoneyChip(0);
            }
            for (int i = 0; i < total; i++) {
                String nick = message.reader().ReadUTF();
                int rank = message.reader().ReadByte();
                long money = message.reader().ReadLong();

                if (getPlayer(nick) == 0) {
                    BaseInfo.gI().infoWin.Add(new InfoWin(i + 1 + ". ", nick,
                            money, true));
                } else {
                    BaseInfo.gI().infoWin.Add(new InfoWin(i + 1 + ". ", nick,
                            money, false));
                }
                // gameControl.dialogKetQua.setInfo(BaseInfo.gI().infoWin);
                nickFire = "";
                for (int j = 0; j < nUsers; j++) {
                    if (players[j].isPlaying()
                        && players[j].getName().Equals(nick)) {
                        players[j].setRank(rank);
                        players[j].setReady(false);
                        break;
                    }
                }
            }
            disableAllBtnTable();
            onJoinTableSuccess(masterID);
            for (int j = 0; j < nUsers; j++) {
                if (players[j].isPlaying()) {
                    players[j].setPlaying(false);
                }
                players[j].setTurn(false);
                //}

                //if (!dangkyroiban && BaseInfo.gI().tudongsansang
                //        && (gameControl.currenStage == gameControl.tlmn || gameControl.currenStage == gameControl.phom || gameControl.currenStage == gameControl.xam)

                //        && !BaseInfo.gI().mainInfo.nick.Equals(masterID)) {
                //    //btn_sansang.setVisible(false);
                //    SendData.onReady(1);
            }
            ////SoundManager.Get().pauseAudio(SoundManager.AUDIO_TYPE.COUNT_DOWN);
        } catch (Exception ex) {
            Debug.LogException(ex);

        }

    }

    internal void onUpdateMoneyTbl(Message message) {
        try {
            int size = message.reader().ReadByte();
            String name = "";
            long money = 0;
            for (int i = 0; i < size; i++) {
                name = message.reader().ReadUTF();
                money = message.reader().ReadLong();
                long folowMoney = message.reader().ReadLong();
                bool isGetMoney = message.reader().ReadBoolean();
                int pos = getPlayer(name);

                players[pos].setMoney(folowMoney);
                // addTweenFlyMoney(players[pos]);
                if (pos == 0 && !BaseInfo.gI().isView) {
                    if (RoomControl.roomType == Res.ROOMFREE) {
                        BaseInfo.gI().mainInfo.moneyChip = money;
                    } else {
                        BaseInfo.gI().mainInfo.moneyXu = money;
                    }
                }
            }
        } catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    public virtual void InfoCardPlayerInTbl(Message message) {
        try {
            string turnName = message.reader().ReadUTF();
            int time = message.reader().ReadInt();
            sbyte numP = message.reader().ReadByte();
            InfoCardPlayerInTbl(message, turnName, time, numP);

        } catch (Exception e) {
            // TODO: handle exception
        }
    }

    public virtual void InfoCardPlayerInTbl(Message message, string turnName, int time, sbyte numP) {
        for (int i = 0; i < players.Length; i++) {
            players[i].setPlaying(false);
        }
    }

    public virtual void onInfome(Message message) {
        for (int i = 0; i < players.Length; i++) {
            //players[i].setPlaying(false);
        }
    }

    internal void onMsgChat(string nick, string msg) {
        players[getPlayer(nick)].setTextChat(msg);
    }

    public virtual void onGetCardNoc(String nick, int card) {

    }

    public virtual void onEatCardSuccess(String from, String to, int card) {

    }

    public virtual void onAttachCard(String from, String to, int[] arrayPhom,
            int[] cardsgui) {

    }

    public virtual void onDropPhomSuccess(String nick, int[] arrayPhom) {

    }

    public virtual void onBalanceCard(String from, String to, int card) {

    }

    protected bool CheckInArr(int value, int[] arr) {
        if (arr == null) {
            return false;
        }
        for (int i = 0; i < arr.Length; i++) {
            if (value == arr[i]) {
                return true;
            }
        }
        return false;
    }

    public virtual void onNickCuoc(long moneyInPot, long soTienTo, long moneyBoRa, string nick, Message message) {

    }

    public virtual void onHaveNickTheo(long money, string nick, Message message) {

    }

    public virtual void onInfo(Message message) {

    }

    public virtual void onAddCardTbl(Message message) {

    }


    public virtual void onTimeAuToStart(sbyte time) {
        //if (progress != null) {
        //    valueProgress = 1;
        //    velocityProgress = (float)1 / time;
        //    disableAllBtnTable();
        //    progress.value = 1;
        //    progress.gameObject.SetActive(true);
        //}
    }

    public static long GetCurrentMilli() {
        TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        long millis = (long)ts.TotalMilliseconds;
        return millis;
    }

    public void onMoCard(Card card, int id) {
        Vector3 lc = card.gameObject.transform.localScale;

        if (lc.x < ArrayCard.sizeCardSmall) {
            lc.x = lc.y;
        }
        card.setId(id);
        //SoundManager.Get().startAudio(SoundManager.AUDIO_TYPE.CHIABAI);

        card.gameObject.transform.localScale = new Vector3(0, 1, 0);
        TweenScale.Begin(card.gameObject, 0.6f, lc);
    }

    public virtual void onInfoWinPlayer(List<InfoWinTo> info) {

    }

    public virtual void onHoiBaoXam(sbyte time) {

    }

    public virtual void onNickBaoXam(String name) {

    }

    public void clickSetting() {
        GameControl.instance.sound.startClickButtonAudio();
        gameControl.panelSetting.onShow();
    }

    public void clickNapChuyenXu() {
        GameControl.instance.sound.startClickButtonAudio();
        gameControl.panelNapChuyenXu.onShow();
    }

    public void clickButtonHistoryChat() {
        GameControl.instance.sound.startClickButtonAudio();
        isOpenChat = !isOpenChat;
        if (isOpenChat) {
            TweenPosition.Begin(historychat.gameObject, 0.2f, new Vector3(0, 0, 0));
        } else {
            TweenPosition.Begin(historychat.gameObject, 0.2f, new Vector3(-304, 0, 0));
        }
    }

    public virtual void startFlip(sbyte p) {

    }

    public virtual void onCardFlip(sbyte p) {

    }

    protected void flyMoney() {
        for (int i = 0; i < nUsers; i++) {
            players[i].flyMoney();
        }
    }

    public virtual void onBeginRiseBacay(Message message) {
    }

    public virtual void onCuoc3Cay(Message message) {

    }

    public virtual void onSapBaChi(string namePlayer, long moneyEarn) {
        // TODO Auto-generated method stub

    }

    public virtual void onLung(string namePlayer, long moneyEarn) {
        // TODO Auto-generated method stub

    }

    public virtual void onRankMauBinh(byte chi, string namePlayer, byte typeCard,
            long moneyEarn, int[] cardChi) {
        // TODO Auto-generated method stub

    }

    public virtual void onRankMauBinh(Message message) {
        try {
            sbyte chi = message.reader().ReadByte();
            if (chi != 4 && chi != 5) {
                sbyte size = message.reader().ReadByte();
                for (int i = 0; i < size; i++) {
                    string namePlayer = message.reader().ReadUTF();
                    sbyte typeCard = message.reader().ReadByte();
                    long moneyEarn = message.reader().ReadLong();
                    sbyte size2 = message.reader().ReadByte();
                    int[] cardChi = new int[size2];
                    for (int k = 0; k < size2; k++) {
                        cardChi[k] = message.reader().ReadByte();
                    }
                    onRankMauBinh(chi, namePlayer, typeCard, moneyEarn, cardChi);
                }
            } else if (chi == 4) {
                sbyte size = message.reader().ReadByte();
                for (int i = 0; i < size; i++) {
                    string namePlayer = message.reader().ReadUTF();
                    long moneyEarn = message.reader().ReadLong();
                    onSapBaChi(namePlayer, moneyEarn);
                }
            } else if (chi == 5) {
                sbyte size = message.reader().ReadByte();
                for (int i = 0; i < size; i++) {
                    string namePlayer = message.reader().ReadUTF();
                    long moneyEarn = message.reader().ReadLong();
                    onLung(namePlayer, moneyEarn);
                }
            }

        } catch (Exception e) {
            // TODO: handle exception
        }
    }

    public virtual void onRankMauBinh(sbyte chi, string namePlayer, sbyte typeCard,
            long moneyEarn, int[] cardChi) {
        // TODO Auto-generated method stub

    }

    public virtual void onFinalMauBinh(string name) {
        // TODO Auto-generated method stub

    }

    public virtual void onWinMauBinh(string name, sbyte typeCard) {
        // TODO Auto-generated method stub

    }

    // bool isLock = false;

    public void clickButtonKhoa() {
        GameControl.instance.sound.startClickButtonAudio();
        if (toggleLock.value) {
            SendData.onSetTblPass("");
            GameControl.instance.toast.showToast("Đã MỞ KHÓA bàn!");
        } else {
            SendData.onSetTblPass("khoa");
            GameControl.instance.toast.showToast("Đã KHÓA bàn!");
        }
        //isLock = !isLock;
    }

    public void onCardXepMB(int[] cardss) {

    }

    public virtual void onPhomha(Message message) {
    }

    //Xoc dia
    public virtual void onBeGinXocDia(int time) {

    }

    public virtual void onBeginXocDiaTimeDatcuoc(int time) {

    }

    //Tra ve so quan do.
    public virtual void onXocDiaMobat(int numRed) {

    }

    public virtual void onXocdiaNhanCacMucCuoc(long muc1, long muc2, long muc3, long muc4) {

    }

    public virtual void onXocDiaHistory(List<int> aIDs) {

    }

    public virtual void onXocDiaDatcuoc(string nick, sbyte cua, long money, int typeCHIP) {

    }

    public virtual void onXocDiaDatGapdoi(string nick, sbyte socua, Message message) {

    }

    public virtual void onXocDiaDatlai(string nick, sbyte socua, Message message) {

    }

    public virtual void onXocDiaHuycuoc(string nick, long moneycua0, long moneycua1, long moneycua2,
        long moneycua3, long moneycua4, long moneycua5) {

    }

    public virtual void onXocDiaUpdateCua(Message message) {

    }

    public virtual void onXocDiaChucNangHuycua(Message message) {

    }

    public virtual void onXocDiaBeginTimerDungCuoc(Message message) {

    }
    //Xoc dia
}
