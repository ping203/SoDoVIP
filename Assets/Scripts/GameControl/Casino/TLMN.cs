using UnityEngine;
using System.Collections;
using System;
public class TLMN : HasMasterCasino {
    public UIButton btn_danhbai;
    public UIButton btn_boluot;
    bool boluot;
    // Use this for initialization
    void Start () {
    }
    void Awake () {
        nUsers = 4;
        tableArrCard1.setAllMo (true);
    }
    // Update is called once per frame
    void Update () {

    }

    public override void resetData () {
        base.resetData ();

        tableArrCard1.removeAllCard ();
        tableArrCard2.removeAllCard ();

        tableArrCard = null;
        //players[0].gameObject.transform.localPosition = new Vector3(0, -115, 0);
        btn_danhbai.gameObject.SetActive (false);
        btn_boluot.gameObject.SetActive (false);
    }

    public override void startTableOk (int[] cardHand, Message msg, string[] nickPlay) {
        base.startTableOk (cardHand, msg, nickPlay);
        tableArrCard1.removeAllCard ();
        tableArrCard2.removeAllCard ();
        tableArrCard = null;
        //players[0].gameObject.transform.localPosition = new Vector3(xPlay, -115, 0);
        players[0].setCardHand (cardHand, true, false, false);
        for(int i = 1; i < players.Length; i++) {
            if(players[i].isPlaying () && !players[i].getName ().Equals ("")) {
                addCardHandOtherPlayer (13, i);
            }
        }
    }
    public override void onStartForView (string[] playingName) {
        base.onStartForView (playingName);
        if(players[0].getName ().Equals (BaseInfo.gI ().mainInfo.nick) && !BaseInfo.gI ().isView) {
            lb_Btn_sansang.text = "Xin chờ...";
            btn_sansang.gameObject.SetActive (true);
        }
        tableArrCard1.removeAllCard ();
        tableArrCard2.removeAllCard ();
        tableArrCard = null;
        for(int i = 0; i < players.Length; i++) {
            if(players[i].isPlaying () && !players[i].getName ().Equals ("")) {
                addCardHandOtherPlayer (13, i);
            }
        }
    }

    public override void setTurn (string nick, Message message) {
        base.setTurn (nick, message);
        try {
            if(nick.ToLower ().Equals (
                    BaseInfo.gI ().mainInfo.nick.ToLower ())) {
                if(tableArrCard != null) {
                    if(tableArrCard.Length > 0) {
                        btn_boluot.gameObject.SetActive (true);
                        /*Vector3 vt = btn_danhbai.transform.localPosition;
                        vt.x = btn_datcuoc.transform.localPosition.x;
                        btn_danhbai.transform.localPosition = vt;*/
                    } else {
                        btn_boluot.gameObject.SetActive (false);
                        /*Vector3 vt = btn_danhbai.transform.localPosition;
                        vt.x = btn_boluot.transform.localPosition.x;
                        btn_danhbai.transform.localPosition = vt;*/
                    }
                } else {
                    btn_boluot.gameObject.SetActive (false);
                    /*Vector3 vt = btn_danhbai.transform.localPosition;
                    vt.x = btn_boluot.transform.localPosition.x;
                    btn_danhbai.transform.localPosition = vt;*/
                }
                btn_danhbai.gameObject.SetActive (true);
            } else {
                btn_danhbai.gameObject.SetActive (false);
                btn_boluot.gameObject.SetActive (false);
            }
            players[getPlayer (nick)].cardHand.setAllMo (false);
            turnName = nick;
            if(turnName.Equals (nickFire)) {
                finishTurn = true;
                tableArrCard1.setArrCard (new int[] { }, false, false, false);
                tableArrCard2.setArrCard (new int[] { }, false, false, false);
                tableArrCard = null;
                /*Vector3 vt = btn_danhbai.transform.localPosition;
                vt.x = btn_boluot.transform.localPosition.x;
                btn_danhbai.transform.localPosition = vt;*/
                btn_boluot.gameObject.SetActive (false);
                for(int i = 0; i < players.Length; i++) {
                    players[i].cardHand.setAllMo (false);
                }
            }

        } catch(Exception e) {
            Debug.LogException (e);
        }
    }

    public void clickButtonDanh () {
        int[] card = players[0].cardHand.getArrCardChoose ();
        if(card == null) {
            gameControl.toast.showToast ("Chưa chọn bài!");
        } else {
            SendData.onFireCardTL (card);
            boluot = btn_boluot.gameObject.activeInHierarchy;
            btn_danhbai.gameObject.SetActive (false);
            btn_boluot.gameObject.SetActive (false);
        }
    }
    public void clickButtonBo () {
        SendData.onSendSkipTurn ();
        btn_boluot.gameObject.SetActive (false);
        boluot = btn_boluot.gameObject.activeInHierarchy;
        btn_danhbai.gameObject.SetActive (false);
    }
    public override void onFireCard (string nick, string turnname, int[] card) {
        base.onFireCard (nick, turnname, card);
        if(players[0].cardHand.getArrCardChoose () != null) {
            if(!TLMNChooseCard.compareCard (
                    players[0].cardHand.getArrCardChoose (), card)) {
                players[0].cardHand.reAddAllCard ();
            }
        }
        //gameControl.sound.startchiabaiAudio();

        // 3 con 2
        if(card.Length == 3) {
            if((laydu (card[0]) == 2) && (laydu (card[1]) == 2) && (laydu (card[2]) == 2)) {
                gameControl.sound.start_ThuaDiCung ();
                return;
            }
            // sam co
            if((laydu (card[0]) == laydu (card[1]) && laydu (card[1]) == laydu (card[2]))) {
                gameControl.sound.start_DODI ();
                return;
            }
        }

        // 2 con 2
        if(card.Length == 2) {
            if((laydu (card[0]) == 2) && (laydu (card[1]) == 2)) {
                gameControl.sound.start_MAYHABUOI ();
            }
        }
        // 1 con 2
        if(card.Length == 1) {
            if((laydu (card[0]) == 2)) {
                gameControl.sound.start_HAINE ();
            }
        }

        if(card.Length >= 2) {
            gameControl.sound.start_random ();
        }
    }

    private int laydu (int a) {
        return Card.aNumber[a] % 13 + 1;
    }

    public override void onFireCardFail () {
        base.onFireCardFail ();
        btn_boluot.gameObject.SetActive (boluot);
        btn_danhbai.gameObject.SetActive (true);
    }
    public override void onNickSkip (string nick, string turnname) {
        base.onNickSkip (nick, turnname);
        players[getPlayer (nick)].cardHand.setAllMo (true);
        setTurn (turnname, null);
    }
    public override void onFinishTurn () {
        base.onFinishTurn ();
        finishTurn = true;
        tableArrCard1.setArrCard (new int[] { }, false, false, false);
        tableArrCard2.setArrCard (new int[] { }, false, false, false);
        tableArrCard = null;
        btn_boluot.gameObject.SetActive (false);
        for(int i = 0; i < players.Length; i++) {
            players[i].cardHand.setAllMo (false);
        }
    }
    public override void InfoCardPlayerInTbl (Message message, string turnName, int time, sbyte numP) {
        base.InfoCardPlayerInTbl (message, turnName, time, numP);
        try {
            string[] playingName = new string[numP];
            for(int i = 0; i < numP; i++) {
                playingName[i] = message.reader ().ReadUTF ();
                players[getPlayer (playingName[i])].setPlaying (true);
                sbyte numCard = message.reader ().ReadByte ();
                int[] temp;
                temp = new int[numCard];
                for(int j = 0; j < temp.Length; j++) {
                    temp[j] = 52;
                }
                players[getPlayer (playingName[i])].cardHand.setArrCard (temp,
                        false, true, false);
                players[getPlayer (playingName[i])].cardHand.setVisibleSobai (true);

                players[getPlayer (playingName[i])].cardHand.setSobai (numCard);
            }
            setTurn (turnName, time);
        } catch(Exception e) {
            Debug.LogException (e);
        }
    }
    public override void onInfome (Message message) {
        base.onInfome (message);
        try {
            isStart = true;
            players[0].setPlaying (true);
            //players[0].gameObject.transform.localPosition = new Vector3(xPlay, -115, 0);
            disableAllBtnTable ();
            int sizeCardHand = message.reader ().ReadByte ();
            int[] cardHand = new int[sizeCardHand];
            for(int i = 0; i < sizeCardHand; i++) {
                cardHand[i] = message.reader ().ReadByte ();
            }
            players[0].setCardHand (cardHand, false, false, false);
            int sizeCardFire = message.reader ().ReadByte ();
            if(sizeCardFire > 0) {
                int[] cardFire = new int[sizeCardFire];
                for(int i = 0; i < sizeCardFire; i++) {
                    cardFire[i] = message.reader ().ReadByte ();
                }
                tableArrCard = cardFire;
                tableArrCard2.setArrCard (cardFire);
            }
            String turnName = message.reader ().ReadUTF ();
            int turnTime = message.reader ().ReadInt ();
            //this.nickFire = message.reader().ReadUTF();
            setTurn (turnName, turnTime);
            if(turnName.ToLower ().Equals (
                    BaseInfo.gI ().mainInfo.nick.ToLower ())) {
                if(tableArrCard != null) {
                    if(tableArrCard.Length > 0) {
                        /*Vector3 vt = btn_danhbai.transform.localPosition;
                        vt.x = btn_batdau.transform.localPosition.x;
                        btn_danhbai.transform.localPosition = vt;*/
                        btn_boluot.gameObject.SetActive (true);
                    } else {
                        /*Vector3 vt = btn_danhbai.transform.localPosition;
                        vt.x = btn_boluot.transform.localPosition.x;
                        btn_danhbai.transform.localPosition = vt;*/
                        btn_boluot.gameObject.SetActive (false);
                    }
                } else {
                    /*Vector3 vt = btn_danhbai.transform.localPosition;
                    vt.x = btn_boluot.transform.localPosition.x;
                    btn_danhbai.transform.localPosition = vt;*/
                    btn_boluot.gameObject.SetActive (false);
                }
                btn_danhbai.gameObject.SetActive (true);
            } else {
                btn_danhbai.gameObject.SetActive (false);
                btn_boluot.gameObject.SetActive (false);
            }
        } catch(Exception e) {
            Debug.LogException (e);
        }
    }
    public override void onFinishGame (Message message) {
        base.onFinishGame (message);
        btn_danhbai.gameObject.SetActive (false);
        btn_boluot.gameObject.SetActive (false);
        //tableArrCard1.removeAllCard();
        //tableArrCard2.removeAllCard();
        if(BaseInfo.gI ().isView) {
            btn_sansang.gameObject.SetActive (false);
        }
    }

    public override void disableAllBtnTable () {
        base.disableAllBtnTable ();
    }
}
