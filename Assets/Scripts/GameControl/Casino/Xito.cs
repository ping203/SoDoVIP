﻿using UnityEngine;
using System.Collections;
using System;
public class Xito : BaseToCasino {
    public TimerLieng timerWaiting;
    public UILabel txt_chonbai;
    public UISprite girl;
    int time_chonbai;
    long timeReceiveTurnChonBai;
    public bool isChooseCard;

    public GameObject panelHelp;

    // Use this for initialization
    void Start () {

    }

    void Awake () {
        nUsers = 5;
    }

    void Update () {
        //act();
        if(txt_chonbai.gameObject.activeInHierarchy) {
            long time = time_chonbai
                    - (GetCurrentMilli () - timeReceiveTurnChonBai)
                    / 1000;
            if(isChooseCard) {
                txt_chonbai.text = (Res.TXT_CHONBAI + time);
            } else {
                txt_chonbai.text = ("Chờ: " + time);
            }
            if((byte) time == 0) {
                txt_chonbai.gameObject.SetActive (false);
                isChooseCard = false;
            }
        }
    }

    public override void onTimeAuToStart (sbyte time) {
        if(timerWaiting != null) {
            timerWaiting.setActive (time);
        }
    }

    public override void startTableOk (int[] cardHand, Message msg, string[] nickPlay) {
        base.startTableOk (cardHand, msg, nickPlay);
        players[0].setCardHand (new int[] { cardHand[0], cardHand[1] }, true, false, false);
        if(players[0].isPlaying () && players[0].getName ().Equals (BaseInfo.gI ().mainInfo.nick)) {
            players[0].setLoaiBai (PokerCard
                    .getTypeOfCardsPoker (players[0].cardHand.getArrCardAll ()));
        }
        players[0].cardHand.getCardbyPos (0).setMo (true);
        players[0].cardHand.getCardbyPos (1).setMo (true);

        for(int i = 0; i < players.Length; i++) {
            if(players[i].isPlaying () && !players[i].getName ().Equals ("")) {
                players[i].setMoneyChip (BaseInfo.gI ().moneyTable);
                if(i != 0) {
                    players[i].setCardHand (new int[] { 52, 52 }, true, false, false);
                }
            }
        }
    }

    public override void onStartForView (string[] playingName) {
        base.onStartForView (playingName);
        for(int i = 0; i < players.Length; i++) {
            if(players[i].isPlaying () && !players[i].getName ().Equals ("")) {
                players[i].setMoneyChip (BaseInfo.gI ().moneyTable);
                players[i].setCardHand (new int[] { 52, 52 }, true, false, false);
            }
        }
    }

    public override void setTurn (string nick, Message message) {
        base.setTurn (nick, message);
        try {
            if(!isStart) {
                return;
            }
            long moneyCuoc = message.reader ().ReadLong ();
            txt_chonbai.gameObject.SetActive (false);
            if(nick.ToLower ().Equals (
                    BaseInfo.gI ().mainInfo.nick.ToLower ())) {
                if(players[0].getFolowMoney () == 0) {
                    SendData.onAccepFollow ();
                } else {
                    baseSetturn (moneyCuoc);
                }
            } else {
                if(players[0].isPlaying ()) {
                    showAllButton (true, true, true);
                } else {
                    showAllButton (true, false, false);
                }
                setMoneyCuoc (moneyCuoc);
            }
        } catch(Exception ex) {
            Debug.LogException (ex);

        }
    }

    public override void onGetCardNoc (string nick, int card) {
        base.onGetCardNoc (nick, card);
        flyMoney ();
        int idpl = getPlayer (nick);

        if(idpl != -1) {
            if(players[idpl].cardHand.getCardbyPos (0).getId () == 52
                && players[idpl].cardHand.getCardbyPos (1).getId () == 52) {
                players[idpl].setCardHand (new int[] { 52, card }, false, false, false);
            } else {
                if(players[idpl].getName ().Equals (BaseInfo.gI ().mainInfo.nick)) {
                    players[idpl].addToCardHand (card, true);
                } else {
                    players[idpl].addToCardHand (card, false);
                }
            }
        }

        if(idpl == 0) {
            if(players[0].isPlaying () && players[0].getName ().Equals (BaseInfo.gI ().mainInfo.nick)) {
                players[0].setLoaiBai (PokerCard
                        .getTypeOfCardsPoker (players[0].cardHand
                                .getArrCardAll ()));
            }
        }
    }

    public override void InfoCardPlayerInTbl (Message message, string turnName, int time, sbyte numP) {
        base.InfoCardPlayerInTbl (message, turnName, time, numP);
        try {
            for(int i = 0; i < numP; i++) {
                String name = message.reader ().ReadUTF ();
                sbyte isSkip = message.reader ().ReadByte (); // = 0 Skip.
                long chips = message.reader ().ReadLong ();
                players[getPlayer (name)].setMoneyChip (chips);
                players[getPlayer (name)].setPlaying (true);
                int ids = getPlayer (name);
                int sizeRub = message.reader ().ReadInt ();
                players[ids].cardHand.removeAllCard ();
                players[ids].cardHand.addCard (52);
                players[ids].cardHand.addCard (52);
                if(sizeRub > 0) {
                    players[ids].setCardHand (new int[] { 52 }, false, false, false);
                    for(int j = 0; j < sizeRub; j++) {
                        int card = message.reader ().ReadInt ();
                        if(card != -1) {
                            players[ids].cardHand.addCard (card);
                        }
                    }
                }
                if(isSkip == 0) {
                    players[ids].cardHand.setAllMo (true);
                }
            }

            sbyte len1 = message.reader ().ReadByte ();
            for(int i = 0; i < len1; i++) {
                long money = message.reader ().ReadLong ();
                sbyte len2 = message.reader ().ReadByte ();
                for(int j = 0; j < len2; j++) {
                    string name = message.reader ().ReadUTF ();
                    moneyInPot[i].addChip2 (money / len2, name, false);
                }
                moneyInPot[i].setmMoneyInPotNonModifier (money);
            }

            setTurn (turnName, time);
        } catch(Exception e) {
            // TODO: handle exception
            Debug.LogException (e);
        }
    }

    public override void onInfome (Message message) {
        base.onInfome (message);
        try {
            isStart = true;
            players[0].setPlaying (true);
            int sizeCardHand = message.reader ().ReadByte ();
            players[0].cardHand.removeAllCard ();
            for(int j = 0; j < sizeCardHand; j++) {
                int card = message.reader ().ReadByte ();
                if(card != -1) {
                    players[0].cardHand.addCard (card);
                }
            }
            bool isSkip = message.reader ().ReadBoolean ();
            if(isSkip) {
                players[0].cardHand.setAllMo (true);
            }
            String turnvName = message.reader ().ReadUTF ();
            int turnvTime = message.reader ().ReadInt ();
            long money = message.reader ().ReadLong ();
            long moneyC = message.reader ().ReadLong ();
            long mIP = message.reader ().ReadLong ();
            players[0].setMoneyChip (moneyC);
            setTurn (turnvName, turnvTime);

            if(turnvName.ToLower ().Equals (
                    BaseInfo.gI ().mainInfo.nick.ToLower ())) {
                baseSetturn (money);
            } else {
                if(players[0].isPlaying ()) {
                    showAllButton (true, true, true);
                } else {
                    showAllButton (true, false, false);
                }
                setMoneyCuoc (money);
            }
        } catch(Exception e) {
        }
    }

    protected override void infoWinPlayer (InfoWinTo infoWin) {
        base.infoWinPlayer (infoWin);
        int poss = getPlayer (infoWin.name);
        if(infoWin.typeCard >= 0 && infoWin.typeCard <= 8) {
            players[poss].sp_typeCard.spriteName = Res.TypeCard_Name[infoWin.typeCard];
            players[poss].sp_typeCard.MakePixelPerfect ();
            players[poss].sp_typeCard.transform.localPosition = new Vector3 (0, -20, 0);

            if(players[poss].isSit ()) {
                players[poss].sp_typeCard.gameObject.SetActive (true);
            }
        }
    }

    public override void startFlip (sbyte p) {
        isChooseCard = true;
        txt_chonbai.gameObject.SetActive (true);
        btn_ruttien.gameObject.SetActive (false);
        time_chonbai = p;
        this.timeReceiveTurnChonBai = GetCurrentMilli ();
    }

    public override void onCardFlip (sbyte readByte) {
        if(readByte == 0) {
            int temp0 = players[0].cardHand.getCardbyPos (0).getId ();
            int temp1 = players[0].cardHand.getCardbyPos (1).getId ();
            players[0].cardHand.setArrCard (new int[] { temp1, temp0 }, false,
                    false, false);
        }
        players[0].cardHand.getCardbyPos (0).setMo (true);
        players[0].cardHand.getCardbyPos (1).setMo (false);
    }

    public void showHelp () {
        panelHelp.SetActive (true);
    }
}
