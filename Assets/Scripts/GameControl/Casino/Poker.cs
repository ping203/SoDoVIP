using UnityEngine;
using System.Collections;
using System;
public class Poker : BaseToCasino {
    public TimerLieng timerWaiting;
    public UILabel txt_chonbai;
    public UISprite girl;

    public GameObject panelHelp;
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame

    void Awake () {
        nUsers = 5;

    }

    public override void onTimeAuToStart (sbyte time) {
        if(timerWaiting != null) {
            timerWaiting.setActive (time);
        }
    }

    public override void startTableOk (int[] cardHand, Message msg, string[] nickPlay) {
        base.startTableOk (cardHand, msg, nickPlay);
        for(int i = 0; i < cardTable.getSize (); i++) {
            cardTable.getCardbyPos (i).setChoose (false);
        }
        resetAllCardPlayer ();
        cardTable.setAllMo (false);
        cardTable.removeAllCard ();
        for(int i = 1; i < players.Length; i++) {
            if(players[i].isPlaying ()) {
                players[i].setCardHand (new int[] { 52, 52 }, true, false, false);
            }
        }
        players[0].setCardHand (cardHand, true, false, true);
    }

    private void resetAllCardPlayer () {
        for(int i = 0; i < nUsers; i++) {
            players[i].cardHand.setTypeCard (0, 250, true);
           // players[i].cardHand.getCardbyPos (0).transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, 20));
            //players[i].cardHand.getCardbyPos (1).transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, -10));
            players[i].cardHand.transform.localPosition = new Vector3 (0, 0, 0);
        }
    }

    public override void setTurn (string nick, Message message) {
        base.setTurn (nick, message);
        try {
            if(!isStart) {
                return;
            }
            long moneyCuoc = message.reader ().ReadLong ();
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

    public override void InfoCardPlayerInTbl (Message message, string turnName, int time, sbyte numP) {
        base.InfoCardPlayerInTbl (message, turnName, time, numP);
        try {
            resetAllCardPlayer ();
            String[] playingName = new String[numP];
            for(int i = 0; i < numP; i++) {
                playingName[i] = message.reader ().ReadUTF ();
                sbyte isSkip = message.reader ().ReadByte (); // = 0 Skip.
                long chips = message.reader ().ReadLong ();
                players[getPlayer (playingName[i])].setMoneyChip (chips);

                players[getPlayer (playingName[i])].setPlaying (true);
                players[getPlayer (playingName[i])].cardHand.setArrCard (
                        new int[] { 52, 52 }, false, false, false);
                if(isSkip == 0) {
                    players[getPlayer (playingName[i])].cardHand.setAllMo (true);
                }
            }

            int size = message.reader ().ReadInt ();
            int[] card = new int[size];
            for(int i = 0; i < size; i++) {
                card[i] = message.reader ().ReadByte ();
            }
            cardTable.setArrCard (card, false, false, false);
            sbyte len1 = message.reader ().ReadByte ();
            for(int i = 0; i < len1; i++) {
                long money = message.reader ().ReadLong ();
                sbyte len2 = message.reader ().ReadByte ();
                for(int j = 0; j < len2; j++) {
                    String name = message.reader ().ReadUTF ();
                    moneyInPot[i].addChip2 (money / len2, name, false);
                }
                moneyInPot[i].setmMoneyInPotNonModifier (money);
            }
            setTurn (turnName, time);
        } catch(Exception e) {
            // TODO: handle exception
        }
    }
    public override void onStartForView (string[] playingName) {
        base.onStartForView (playingName);
        for(int i = 0; i < cardTable.getSize (); i++) {
            cardTable.getCardbyPos (i).setChoose (false);
        }
        resetAllCardPlayer ();
        cardTable.setAllMo (false);
        cardTable.removeAllCard ();
        for(int i = 0; i < players.Length; i++) {
            if(players[i].isPlaying ()) {
                players[i]
                        .setCardHand (new int[] { 52, 52 }, true, false, false);
            }
        }
    }
    public override void onInfo (Message msg) {
        base.onInfo (msg);
        try {
            String nicksb = msg.reader ().ReadUTF ();
            String nickbb = msg.reader ().ReadUTF ();
            long moneyInPot = msg.reader ().ReadLong ();
            // byte type = msg.reader().readByte();
            if (players[0].isPlaying() && players[0].getName().Equals(BaseInfo.gI().mainInfo.nick)) {
                players[0].setLoaiBai (PokerCard.getTypeOfCardsPoker (PokerCard
                        .add2ArrayCard (players[0].cardHand.getArrCardAll (),
                                cardTable.getArrCardAll ())));
            }
            players[getPlayer (nickbb)].setMoneyChip (moneyInPot * 2 / 3);
            players[getPlayer (nicksb)].setMoneyChip (moneyInPot / 3);
        } catch(Exception ex) {
            Debug.LogException (ex);
        }
    }
    public override void onAddCardTbl (Message message) {
        base.onAddCardTbl (message);
        try {
            // byte type = message.reader().readByte();
            int size = message.reader ().ReadInt ();
            int[] card = new int[size];
            for(int i = 0; i < size; i++) {
                card[i] = message.reader ().ReadByte ();
            }
            if(size >= 3) {
                flyMoney ();
            }
            //SoundManager.Get().startAudio(SoundManager.AUDIO_TYPE.CHIABAI);
            gameControl.sound.startchiabaiAudio ();
            if(size == 3) {
                cardTable.setArrCard (card, false, false, false);
                for(int i = 0; i < cardTable.getSize (); i++) {
                    onMoCard (cardTable.getCardbyPos (i),
                            cardTable.getCardbyPos (i).getId ());
                }
            } else {
                for(int i = cardTable.getSize (); i < size; i++) {
                    cardTable.addCard (card[i]);
                    onMoCard (cardTable.getCardbyPos (i),
                            cardTable.getCardbyPos (i).getId ());
                }

            }

            if (players[0].isPlaying() && players[0].getName().Equals(BaseInfo.gI().mainInfo.nick)) {
                players[0].setLoaiBai (PokerCard.getTypeOfCardsPoker (PokerCard
                        .add2ArrayCard (players[0].cardHand.getArrCardAll (),
                                cardTable.getArrCardAll ())));
            }

        } catch(Exception ex) {
            Debug.LogException (ex);
        }
    }
    public override void onInfome (Message message) {
        base.onInfome (message);
        resetAllCardPlayer ();
        isStart = true;
        players[0].setPlaying (true);
        int sizeCardHand = message.reader ().ReadByte ();
        int[] cardHand = new int[sizeCardHand];
        for(int j = 0; j < sizeCardHand; j++) {
            cardHand[j] = message.reader ().ReadByte ();
        }
        players[0].cardHand.setArrCard (cardHand);

        int size = message.reader ().ReadByte ();
        int[] card = new int[size];
        for(int i = 0; i < size; i++) {
            card[i] = message.reader ().ReadByte ();
        }
        cardTable.setArrCard (card, false, false, false);

        bool upBai = message.reader ().ReadBoolean ();
        if(upBai) {
            players[0].cardHand.setAllMo (true);
        }
        string turnvName = message.reader ().ReadUTF ();
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
    }

    protected override void infoWinPlayer (InfoWinTo infoWin) {
        base.infoWinPlayer (infoWin);
        int poss = getPlayer (infoWin.name);
        players[poss].cardHand.setAllMo (true);

        int k = infoWin.arrCard.Length - 5;
        if(k <= 0) {
            return;
        }
        if(infoWin.typeCard >= 0 && infoWin.typeCard <= 8) {

            players[poss].sp_typeCard.spriteName = Res.TypeCard_Name[infoWin.typeCard];
            players[poss].sp_typeCard.MakePixelPerfect ();
            players[poss].sp_typeCard.transform.localPosition = new Vector3 (0, -20, 0);

            if(players[poss].isSit ()) {
                players[poss].sp_typeCard.gameObject.SetActive (true);
            }

        }
        cardTable.setAllMo (true);
        cardTable.reAddAllCard ();
        for(int j = k; j < infoWin.arrCard.Length; j++) {

            if(CheckInArr (infoWin.arrCard[j], cardTable.getArrCardAll ())) {
                cardTable.getCardbyID (infoWin.arrCard[j]).setMo (false);
                cardTable.getCardbyID (infoWin.arrCard[j]).setChoose (true);
            }
        }

        for(int i = k; i < infoWin.arrCard.Length; i++) {

            if(CheckInArr (infoWin.arrCard[i],
                    players[poss].cardHand.getArrCardAll ())) {
                players[poss].cardHand.getCardbyID (infoWin.arrCard[i]).setMo (
                        false);
                players[poss].cardHand.getCardbyID (infoWin.arrCard[i])
                        .setChoose (true);
            }

        }
    }
    public override void resetData () {
        base.resetData ();

        for(int i = 0; i < cardTable.getSize (); i++) {
            cardTable.getCardbyPos (i).setChoose (false);
        }
        cardTable.setAllMo (false);
        cardTable.removeAllCard ();
        resetAllCardPlayer ();
    }
    
    public void showHelp () {
        panelHelp.SetActive (true);
    }
}
