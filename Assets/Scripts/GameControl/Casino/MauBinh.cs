using UnityEngine;
using System.Collections;
using System;
public class MauBinh : BaseCasino {
    public GameObject panelHelp;
    public UILabel lb_time_mb;
    public UISprite sp_chi1, sp_chi2, sp_chi3;
    public UISprite sp_lung;
    public UISprite sp_chi;
    public UIButton btn_xong, btn_xeplai;
    public GameObject go_chi;

    public TimerLieng timerWaiting;

    public bool isTouchCard = true;

    int turntimeMB;
    long timeReceiveTurnMB;

    public override void onTimeAuToStart (sbyte time) {
        if(timerWaiting != null) {
            timerWaiting.setActive (time);
        }
    }

    public override void onJoinTableSuccess (string master) {
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

    public void demoMB () {
        int[] card41 = { 1, 1, 1 };
        int[] card42 = { 11, 11, 11, 11, 11 };

        for(int i = 0; i < 4; i++) {
            players[i].cardMauBinh[0].setArrCard (card41);
            players[i].cardMauBinh[1].setArrCard (card42);
            players[i].cardMauBinh[2].setArrCard (card42);
        }
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

    public void clickButtonXepXong () {

        isTouchCard = false;
        //reset toa do
        players[0].cardMauBinh[0].resetPostionCard (210);
        players[0].cardMauBinh[1].resetPostionCard (350);
        players[0].cardMauBinh[2].resetPostionCard (350);

        players[0].cardMauBinh[2].transform.localPosition = new Vector3 (390, 10, 0);
        players[0].cardMauBinh[1].transform.localPosition = new Vector3 (390, 85, 0);
        players[0].cardMauBinh[0].transform.localPosition = new Vector3 (390, 160, 0);
        // setPositionCardHand (players[0].cardMauBinh[0].gameObject, false, 60);
        // setPositionCardHand (players[0].cardMauBinh[1].gameObject, false, 30);

        guiFinalMauBinh ();
        btn_xong.gameObject.SetActive (false);
        btn_xeplai.gameObject.SetActive (true);
        go_chi.SetActive (false);
    }

    public void clickButtonXepLai () {
        isTouchCard = true;
        btn_xong.gameObject.SetActive (true);
        btn_xeplai.gameObject.SetActive (false);
        players[0].sp_xepXong.gameObject.SetActive (false);
        reSetTypeCard0 ();

        go_chi.SetActive (true);

        players[0].cardMauBinh[0].resetPostionCard (210);
        players[0].cardMauBinh[1].resetPostionCard (350);
        players[0].cardMauBinh[2].resetPostionCard (350);
        setPositionCardHand (players[0].cardMauBinh[0].gameObject, false, 60);
        setPositionCardHand (players[0].cardMauBinh[1].gameObject, false, 30);
    }

    bool isSendCard;
    private void guiFinalMauBinh () {
        if(!players[0].cardMauBinh[0].getCardbyPos (2).gameObject.activeInHierarchy
                || players[0].cardMauBinh[0].getCardbyPos (2).getId () > 51) {
            return;
        }
        int[] cardFinal = new int[13];
        int[] chi1 = new int[5];
        int[] chi2 = new int[5];
        int[] chi3 = new int[3];

        chi1 = PokerCard.sortValue (players[0].cardMauBinh[2]
                .getArrayCardAllTrue ());
        chi2 = PokerCard.sortValue (players[0].cardMauBinh[1]
                .getArrayCardAllTrue ());
        chi3 = PokerCard.sortValue (players[0].cardMauBinh[0]
                .getArrayCardAllTrue ());

        for(int i = 0; i < chi1.Length; i++) {
            cardFinal[i] = chi1[i];
        }

        for(int i = 5; i < chi2.Length + 5; i++) {
            cardFinal[i] = chi2[i - 5];
        }

        for(int i = 10; i < chi3.Length + 10; i++) {
            cardFinal[i] = chi3[i - 10];
        }

        isSendCard = true;
        SendData.onFinalMauBinh (cardFinal);
        btn_xong.gameObject.SetActive (false);
        btn_xeplai.gameObject.SetActive (true);
        setTypeCard (0, 0, true, true);
        setTypeCard (0, 1, true, true);
        setTypeCard (0, 2, true, true);

        players[0].cardMauBinh[0].resetPostionCard (120);
        players[0].cardMauBinh[1].resetPostionCard (200);
        players[0].cardMauBinh[2].resetPostionCard (200);

        setPositionCardHand (players[0].cardMauBinh[0].gameObject, true, 60);
        setPositionCardHand (players[0].cardMauBinh[1].gameObject, true, 30);
    }

    public override void resetData () {
        base.resetData ();
        resetMB ();
    }

    public override void startTableOk (int[] cardHand, Message msg, string[] nickPlay) {
        base.startTableOk (cardHand, msg, nickPlay);
        isSendCard = false;
        btn_xeplai.gameObject.SetActive (false);
        timerWaiting.gameObject.SetActive (false);
        resetMB ();
        try {
            turntimeMB = msg.reader ().ReadByte ();
        } catch(Exception e) {
            // TODO: handle exception
        }
        turntime = 0;
        timeReceiveTurnMB = GetCurrentMilli ();

        lb_time_mb.text = ("" + turntimeMB);
        lb_time_mb.gameObject.SetActive (false);
        if(players.Length > 1)
            lb_time_mb.gameObject.SetActive (true);

        int[] card = new int[13];
        for(int i = 0; i < card.Length; i++) {
            card[i] = 52;
        }

        for(int i = 1; i < players.Length; i++) {
            if(players[i].isPlaying () && !players[i].getName ().Equals ("")) {
                StartCoroutine (setCardHand (card, i, true));
            }
        }

        btn_xong.gameObject.SetActive (false);
        StartCoroutine (setCardHand (cardHand, 0, true));
        StartCoroutine (delayXepXong ());
    }

    IEnumerator delayXepXong () {
        yield return new WaitForSeconds (1.6f);
        btn_xong.gameObject.SetActive (true);
    }

    public void setPositionCardHand (GameObject arr, bool isDown, float height) {
        Vector3 vt = arr.transform.localPosition;
        if(isDown)
            vt.y -= height;
        else
            vt.y += height;

        TweenPosition.Begin (arr, 0.2f, vt);
    }

    IEnumerator setCardHand (int[] cardHand, int pos, bool isDearling) {
        players[pos].cardMauBinh[0].setArrCard (new int[] { });
        players[pos].cardMauBinh[1].setArrCard (new int[] { });
        players[pos].cardMauBinh[2].setArrCard (new int[] { });

        int[] cardChi1 = new int[] { cardHand[0], cardHand[1], cardHand[2], cardHand[3], cardHand[4] };
        int[] cardChi2  = new int[] { cardHand[5], cardHand[6], cardHand[7], cardHand[8], cardHand[9] };
        int[] cardChi3  = new int[] { cardHand[10], cardHand[11], cardHand[12] };

        bool isFlip;

        if(isDearling) {
            isFlip = true;
        } else {
            isFlip = false;
        }
        float timeDelay = 0.5f;

        if(pos == 0 && players[pos].isPlaying ()) {
            players[pos].cardMauBinh[2].setArrCard (cardChi1,
                    isDearling, false, isFlip);
            setLoaiBai (1, PokerCard.getTypeOfCardsPoker (cardChi1));
        } else {
            players[pos].cardMauBinh[2].setArrCard (cardChi1,
                    isDearling, false, false);
        }
        yield return new WaitForSeconds (timeDelay);

        if(pos == 0 && players[pos].isPlaying ()) {
            players[pos].cardMauBinh[1].setArrCard (cardChi2,
                    isDearling, false, isFlip);
            setLoaiBai (2, PokerCard.getTypeOfCardsPoker (cardChi2));
        } else {
            players[pos].cardMauBinh[1].setArrCard (cardChi2,
                    isDearling, false, false);
        }

        yield return new WaitForSeconds (timeDelay);
        if(pos == 0 && players[pos].isPlaying ()) {
            players[pos].cardMauBinh[0].setArrCard (cardChi3,
                    isDearling, false, isFlip);
            setLoaiBai (3, PokerCard.getTypeOfCardsPoker (cardChi3));
            checkLung (cardChi1, cardChi2, cardChi3);
        } else {
            players[pos].cardMauBinh[0].setArrCard (cardChi3,
                    isDearling, false, false);
        }

        if(!isDearling) {
            timeDelay = 0;
        }

        /* if(players[0].getName ().Equals (BaseInfo.gI ().mainInfo.nick)) {
              if(infome) {
                  btn_xong.setVisible (true);
              } else
                  btn_xong.setVisible (isDearling);
          } else {
              btn_xong.setVisible (false);
          }*
         }*/
    }

    public override void onStartForView (string[] playingName) {
        base.onStartForView (playingName);
        resetMB ();

        turntime = 0;
        turntimeMB = 32;
        timeReceiveTurnMB = GetCurrentMilli ();
        btn_xeplai.gameObject.SetActive (false);
        lb_time_mb.text = ("" + turntimeMB);
        lb_time_mb.gameObject.SetActive (true);

        int[] card = new int[13];
        for(int i = 0; i < card.Length; i++) {
            card[i] = 52;
        }
        for(int i = 0; i < players.Length; i++) {
            if(players[i].isPlaying () && !players[i].st_name.Equals ("")) {
                StartCoroutine (setCardHand (card, i, true));
            }
        }
        go_chi.SetActive (false);
    }

    public void setLoaiBai (int chi, int typeCard) {
        if(chi == 1) {
            sp_chi1.spriteName = Res.TypeCard_Name[typeCard];
        } else if(chi == 2) {
            sp_chi2.spriteName = Res.TypeCard_Name[typeCard];
        } else if(chi == 3) {
            sp_chi3.spriteName = Res.TypeCard_Name[typeCard];
        }
        if(players[0].getName ().Equals (BaseInfo.gI ().mainInfo.nick))
            go_chi.gameObject.SetActive (true);
    }

    public void checkLung (int[] cardChi1, int[] cardChi2, int[] cardChi3) {
        if(PokerCard.isBiggerArray (cardChi2, cardChi1)
               || PokerCard.isBiggerArray (cardChi3, cardChi2)
               || PokerCard.isBiggerArray (cardChi3, cardChi1)) {
            setLung (true);
        } else {
            setLung (false);
        }
    }

    public void setLung (bool isLung) {
        sp_lung.gameObject.SetActive (isLung);
    }

    public override void disableAllBtnTable () {
        base.disableAllBtnTable ();
        btn_xong.gameObject.SetActive (false);
        btn_xeplai.gameObject.SetActive (false);
    }

    public override void onFinishGame (Message message) {
        base.onFinishGame (message);

        gameControl.sound.startFinishAudio ();

        btn_xeplai.gameObject.SetActive (false);
        btn_xong.gameObject.SetActive (false);
        lb_time_mb.gameObject.SetActive (false);
        sp_chi.gameObject.SetActive (false);
        sp_lung.gameObject.SetActive (false);
        for(int j = 0; j < players.Length; j++) {
            //players[j].setXepXong (0);
            players[j].sp_xepXong.gameObject.SetActive (false);
            players[j].sp_action.gameObject.SetActive (false);
            reSetTypeCard (j);
            setTypeCard (j, 0, true, true);
            setTypeCard (j, 1, true, true);
            setTypeCard (j, 2, true, true);
        }

        for(int i = 0; i < players[0].cardMauBinh[0].getSize (); i++) {
            players[0].cardMauBinh[0].getCardbyPos (i).transform.localScale = new Vector3 (1, 1, 1);
        }
        for(int i = 0; i < players[0].cardMauBinh[1].getSize (); i++) {
            players[0].cardMauBinh[1].getCardbyPos (i).transform.localScale = new Vector3 (1, 1, 1);
        }
        for(int i = 0; i < players[0].cardMauBinh[2].getSize (); i++) {
            players[0].cardMauBinh[2].getCardbyPos (i).transform.localScale = new Vector3 (1, 1, 1);
        }

        setTypeCard (0, 0, true, true);
        setTypeCard (0, 1, true, true);
        setTypeCard (0, 2, true, true);

        players[0].cardMauBinh[0].setAllMo (false);
        players[0].cardMauBinh[1].setAllMo (false);
        players[0].cardMauBinh[2].setAllMo (false);
        go_chi.gameObject.SetActive (false);
    }

    public override void setMaster (string nick) {
        //base.setMaster (nick);
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

    bool isFinishMauBinh;

    public override void onSapBaChi (string namePlayer, long moneyEarn) {
        base.onSapBaChi (namePlayer, moneyEarn);
        if(moneyEarn > 0) {
            players[getPlayer (namePlayer)].setTextMDSmall (
                    "+" + BaseInfo.formatMoneyDetailDot (moneyEarn) + "");
        } else if(moneyEarn < 0) {
            players[getPlayer (namePlayer)].setTextMDSmall (
                    "-" + BaseInfo.formatMoneyDetailDot (-moneyEarn) + "");
            players[getPlayer (namePlayer)].sp_sap3chi.gameObject.SetActive (true);
        }
    }
    public override void onLung (string namePlayer, long moneyEarn) {
        base.onLung (namePlayer, moneyEarn);

        if(namePlayer.Equals (BaseInfo.gI ().mainInfo.nick)) {
            gameControl.sound.startBinhlungAudio ();
        }

        players[getPlayer (namePlayer)].setLoaiBai (-1);
        players[getPlayer (namePlayer)].setMoneyMB (moneyEarn);

        players[getPlayer (namePlayer)].setLung (true);
    }

    public override void onRankMauBinh (sbyte chi, string namePlayer, sbyte typeCard,
        long moneyEarn, int[] cardChi) {
        base.onRankMauBinh (chi, namePlayer, typeCard, moneyEarn, cardChi);

        btn_xong.gameObject.SetActive (false);
        btn_xeplai.gameObject.SetActive (false);
        lb_time_mb.gameObject.SetActive (false);
        for(int i = 0; i < players.Length; i++) {
            players[i].sp_xepXong.gameObject.SetActive (false);
        }
        go_chi.SetActive (false);

        if(!BaseInfo.gI ().isView && players[0].isPlaying ()) {
            sp_chi.gameObject.SetActive (true);
            sp_chi.spriteName = "chi" + (chi);
            sp_chi.transform.localPosition = new Vector3 (-300, -175, 0);
            TweenPosition.Begin (sp_chi.gameObject, 2.0f, new Vector3 (-300f, -75f, 0f));
        }

        int iCard = 0;
        if(chi == 1) {
            iCard = 2;
        } else if(chi == 2) {
            iCard = 1;
        } else if(chi == 3) {
            iCard = 0;
        }

        int index = getPlayer (namePlayer);

        if(index != 0)
            players[getPlayer (namePlayer)].cardMauBinh[iCard]
                    .setArrCard (cardChi);
        else if(!players[0].getName ().Equals (BaseInfo.gI ().mainInfo.nick)) {
            players[getPlayer (namePlayer)].cardMauBinh[iCard]
                    .setArrCard (cardChi);
        }

        switch(index) {
            case 0: {
                    //if(!BaseInfo.gI ().isView && players[0].isPlaying ()) {//neu dang choi

                    gameControl.sound.startSobaiAudio ();

                    players[0].sp_typeCard.gameObject.SetActive (true);
                    players[0].sp_typeCard.transform.localScale = new Vector3 (0, 0, 0);
                    TweenScale.Begin (players[0].sp_typeCard.gameObject, 0.2f, new Vector3 (1.4f, 1.4f, 1.4f));
                    StartCoroutine (setInvisible (players[0].sp_typeCard.gameObject));
                    Vector3 vt = players[0].sp_typeCard.transform.localPosition;
                    switch(chi) {
                        case 1: {

                                for(int i = 0; i < players[0].cardMauBinh[0].getSize (); i++) {
                                    players[0].cardMauBinh[0].getCardbyPos (i).cardMo.gameObject.SetActive (true);
                                    players[0].cardMauBinh[0].getCardbyPos (i).transform.localScale = new Vector3 (1, 1, 1);
                                }
                                for(int i = 0; i < players[0].cardMauBinh[1].getSize (); i++) {
                                    players[0].cardMauBinh[1].getCardbyPos (i).cardMo.gameObject.SetActive (true);
                                    players[0].cardMauBinh[1].getCardbyPos (i).transform.localScale = new Vector3 (1, 1, 1);
                                }
                                for(int i = 0; i < players[0].cardMauBinh[2].getSize (); i++) {
                                    players[0].cardMauBinh[2].getCardbyPos (i).cardMo.gameObject.SetActive (false);
                                    players[0].cardMauBinh[2].getCardbyPos (i).transform.localScale = new Vector3 (1.2f, 1.2f, 1.2f);
                                }

                                players[0].sp_typeCard.spriteName = Res.TypeCard_Name[PokerCard.getTypeOfCardsPoker (players[0].cardMauBinh[2].getArrayCardAllTrue ())];
                                vt.y = players[0].cardMauBinh[2].transform.localPosition.y;
                                break;
                            }
                        case 2: {
                                for(int i = 0; i < players[0].cardMauBinh[0].getSize (); i++) {
                                    players[0].cardMauBinh[0].getCardbyPos (i).cardMo.gameObject.SetActive (true);
                                    players[0].cardMauBinh[0].getCardbyPos (i).transform.localScale = new Vector3 (1, 1, 1);
                                }
                                for(int i = 0; i < players[0].cardMauBinh[1].getSize (); i++) {
                                    players[0].cardMauBinh[1].getCardbyPos (i).cardMo.gameObject.SetActive (false);
                                    players[0].cardMauBinh[1].getCardbyPos (i).transform.localScale = new Vector3 (1.2f, 1.2f, 1.2f);
                                }
                                for(int i = 0; i < players[0].cardMauBinh[2].getSize (); i++) {
                                    players[0].cardMauBinh[2].getCardbyPos (i).cardMo.gameObject.SetActive (true);
                                    players[0].cardMauBinh[2].getCardbyPos (i).transform.localScale = new Vector3 (1, 1, 1);
                                }
                                players[0].sp_typeCard.spriteName = Res.TypeCard_Name[PokerCard.getTypeOfCardsPoker (players[0].cardMauBinh[1].getArrayCardAllTrue ())];
                                vt.y = players[0].cardMauBinh[1].transform.localPosition.y;
                                break;
                            }
                        case 3: {
                                for(int i = 0; i < players[0].cardMauBinh[0].getSize (); i++) {
                                    players[0].cardMauBinh[0].getCardbyPos (i).cardMo.gameObject.SetActive (false);
                                    players[0].cardMauBinh[0].getCardbyPos (i).transform.localScale = new Vector3 (1.2f, 1.2f, 1.2f);
                                }
                                for(int i = 0; i < players[0].cardMauBinh[1].getSize (); i++) {
                                    players[0].cardMauBinh[1].getCardbyPos (i).cardMo.gameObject.SetActive (true);
                                    players[0].cardMauBinh[1].getCardbyPos (i).transform.localScale = new Vector3 (1, 1, 1);
                                }
                                for(int i = 0; i < players[0].cardMauBinh[2].getSize (); i++) {
                                    players[0].cardMauBinh[2].getCardbyPos (i).cardMo.gameObject.SetActive (true);
                                    players[0].cardMauBinh[2].getCardbyPos (i).transform.localScale = new Vector3 (1, 1, 1);
                                }

                                players[0].sp_typeCard.spriteName = Res.TypeCard_Name[PokerCard.getTypeOfCardsPoker (players[0].cardMauBinh[0].getArrayCardAllTrue ())];
                                vt.y = players[0].cardMauBinh[0].transform.localPosition.y;
                                break;
                            }
                    }
                    players[0].sp_typeCard.transform.localPosition = vt;
                    players[0].setMoneyMB (moneyEarn);
                    /*} else { //Neu la xem
                        for(int i = 0; i < players[0].cardMauBinh[0].getSize (); i++) {
                            players[0].cardMauBinh[0].getCardbyPos (i).gameObject.SetActive (false);
                        }
                        for(int i = 0; i < players[0].cardMauBinh[1].getSize (); i++) {
                            players[0].cardMauBinh[1].getCardbyPos (i).gameObject.SetActive (false);
                        }
                        for(int i = 0; i < players[0].cardMauBinh[2].getSize (); i++) {
                            players[0].cardMauBinh[2].getCardbyPos (i).gameObject.SetActive (false);
                        }
                    }*/
                    break;
                }
            default: {
                    players[index].sp_typeCard.gameObject.SetActive (true);
                    players[index].sp_typeCard.transform.localScale = new Vector3 (0, 0, 0);
                    TweenScale.Begin (players[index].sp_typeCard.gameObject, 0.2f, new Vector3 (1.4f, 1.4f, 1.4f));
                    // StopAllCoroutines ();
                    StartCoroutine (setInvisible (players[index].sp_typeCard.gameObject));
                    switch(chi) {
                        case 1: {
                                for(int i = 0; i < players[index].cardMauBinh[2].getSize (); i++) {
                                    players[index].cardMauBinh[2].getCardbyPos (i).cardMo.gameObject.SetActive (false);
                                    players[index].cardMauBinh[2].getCardbyPos (i).setId (cardChi[i]);
                                }

                                players[index].sp_typeCard.spriteName = Res.TypeCard_Name[PokerCard.getTypeOfCardsPoker (players[index].cardMauBinh[2].getArrayCardAllTrue ())];
                                players[index].sp_typeCard.transform.localPosition = players[index].cardMauBinh[2].transform.localPosition;
                                break;
                            }
                        case 2: {
                                for(int i = 0; i < players[index].cardMauBinh[1].getSize (); i++) {
                                    players[index].cardMauBinh[1].getCardbyPos (i).cardMo.gameObject.SetActive (false);
                                    players[index].cardMauBinh[1].getCardbyPos (i).setId (cardChi[i]);
                                }
                                players[index].sp_typeCard.spriteName = Res.TypeCard_Name[PokerCard.getTypeOfCardsPoker (players[index].cardMauBinh[1].getArrayCardAllTrue ())];

                                players[index].sp_typeCard.transform.localPosition = players[index].cardMauBinh[1].transform.localPosition;
                                break;
                            }
                        case 3: {
                                for(int i = 0; i < players[index].cardMauBinh[0].getSize (); i++) {
                                    players[index].cardMauBinh[0].getCardbyPos (i).cardMo.gameObject.SetActive (false);
                                    players[index].cardMauBinh[0].getCardbyPos (i).setId (cardChi[i]);
                                }
                                players[index].sp_typeCard.spriteName = Res.TypeCard_Name[PokerCard.getTypeOfCardsPoker (players[index].cardMauBinh[0].getArrayCardAllTrue ())];
                                players[index].sp_typeCard.transform.localPosition = players[index].cardMauBinh[0].transform.localPosition;
                                break;
                            }
                    }
                    players[index].setMoneyMB (moneyEarn);
                    break;
                }
        }
    }

    public void setScale (GameObject obj) {
        obj.transform.localScale = new Vector3 (1, 1, 1);
        TweenScale.Begin (obj, 0.2f, new Vector3 (1.4f, 1.4f, 1.4f));
    }

    IEnumerator setInvisible (GameObject obj) {
        yield return new WaitForSeconds (2.0f);
        TweenScale.Begin (obj, 0.2f, new Vector3 (0, 0, 0));
        yield return new WaitForSeconds (0.3f);
        obj.SetActive (false);
    }

    public override void onFinalMauBinh (string name) {
        base.onFinalMauBinh (name);
        //players[getPlayer (name)].setXepXong (2);

        int index = getPlayer (name);
        if(players[index].isPlaying ()) {
            players[index].sp_xepXong.gameObject.SetActive (true);
        }
    }

    public override void allCardFinish (string nick, int[] card) {
        //base.allCardFinish (nick, card);
        int index = getPlayer (nick);
        if(index != 0) {
            if(card.Length > 0) {
                StartCoroutine (setCardHand (card, index, false));
            }
        }
        if(index == 0 && !BaseInfo.gI ().mainInfo.nick.Equals (nick)) {
            StartCoroutine (setCardHand (card, index, false));
        }
        go_chi.SetActive (false);
    }
    void setTypeCard (int pos, int i, bool isSmall, bool isSetPos) {

    }

    void reSetTypeCard0 () {

    }

    void reSetTypeCard (int pos) {


    }

    public override void onWinMauBinh (string name, sbyte typeCard) {
        // TODO Auto-generated method stub
        base.onWinMauBinh (name, typeCard);
        disableAllBtnTable ();
        btn_xong.StopAllCoroutines ();
        players[getPlayer (name)].setThangTrang (typeCard);

    }

    private void resetMB () {
        for(int i = 0; i < players.Length; i++) {
            players[i].resetData ();
        }
        isTouchCard = true;
        go_chi.SetActive (false);
        sp_lung.gameObject.SetActive (false);

        players[0].cardMauBinh[2].transform.localPosition = new Vector3 (390, 10, 0);
        players[0].cardMauBinh[1].transform.localPosition = new Vector3 (390, 85, 0);
        players[0].cardMauBinh[0].transform.localPosition = new Vector3 (390, 160, 0);

        players[0].cardMauBinh[0].resetPostionCard (210);
        players[0].cardMauBinh[1].resetPostionCard (350);
        players[0].cardMauBinh[2].resetPostionCard (350);
    }
    public override void InfoCardPlayerInTbl (Message message, string turnName, int time, sbyte numP) {
        base.InfoCardPlayerInTbl (message, turnName, time, numP);
        int[] card = new int[13];
        for(int i = 0; i < card.Length; i++) {
            card[i] = 52;
        }
        try {
            for(int i = 0; i < numP; i++) {
                string name = message.reader ().ReadUTF ();
                bool isDangXep = message.reader ().ReadBoolean ();

                players[getPlayer (name)].setPlaying (true);
                if(getPlayer (name) == 2) {
                }
                StartCoroutine (setCardHand (card, getPlayer (name), false));

            }
            if(time > 0) {
                turntimeMB = time;
                turntime = 0;
                timeReceiveTurnMB = GetCurrentMilli ();
                lb_time_mb.text = ("" + turntimeMB);
                lb_time_mb.gameObject.SetActive (true);
            }
        } catch(Exception e) {
            // TODO: handle exception
        }

    }
    public override void onInfome (Message message) {
        base.onInfome (message);
        try {
            isStart = true;
            players[0].setPlaying (true);
            //players[0].gameObject.transform.localPosition = new Vector3(xPlay, -115, 0);

            bool isDangXep = message.reader ().ReadBoolean ();
            sbyte len = message.reader ().ReadByte ();
            sbyte[] c = new sbyte[len];
            for(int i = 0; i < len; i++) {
                c[i] = message.reader ().ReadByte ();
            }

            int[] cardHand;
            cardHand = new int[c.Length];
            for(int i = 0; i < c.Length; i++) {
                cardHand[i] = c[i];
            }
            for(int i = 0; i < players[0].cardMauBinh[0].getSize (); i++) {
                players[0].cardMauBinh[0].getCardbyPos (i).transform.localScale = new Vector3 (1, 1, 1);
            }
            for(int i = 0; i < players[0].cardMauBinh[1].getSize (); i++) {
                players[0].cardMauBinh[1].getCardbyPos (i).transform.localScale = new Vector3 (1, 1, 1);
            }
            for(int i = 0; i < players[0].cardMauBinh[2].getSize (); i++) {
                players[0].cardMauBinh[2].getCardbyPos (i).transform.localScale = new Vector3 (1, 1, 1);
            }
            reSetTypeCard0 ();


            if(isDangXep) {
                StartCoroutine (setCardHand (cardHand, 0, false));
                btn_xong.gameObject.SetActive (true);
                btn_xeplai.gameObject.SetActive (false);
            } else {
                setTypeCard (0, 0, true, true);
                setTypeCard (0, 1, true, true);
                setTypeCard (0, 2, true, true);

                StartCoroutine (setCardHand (cardHand, 0, false));
                btn_xong.gameObject.SetActive (false);
                btn_xeplai.gameObject.SetActive (true);
            }

        } catch(Exception e) {
            // TODO: handle exception
        }
    }
    void Update () {
        //base.act();
        if(lb_time_mb.gameObject.activeInHierarchy) {
            long time = turntimeMB
                    - (GetCurrentMilli () - timeReceiveTurnMB) / 1000;
            if(time < 10) {
                //SoundManager.Get().startAudio(SoundManager.AUDIO_TYPE.COUNT_DOWN);

            }
            if((int) time == 2 && !isSendCard) {
                isSendCard = true;
                guiFinalMauBinh ();
            }
            lb_time_mb.text = ("" + time + "");
            if(time <= 0) {
                lb_time_mb.gameObject.SetActive (false);
                //SoundManager.Get().pauseAudio(SoundManager.AUDIO_TYPE.COUNT_DOWN);
            }
        }
    }

    public void showHelp () {
        panelHelp.SetActive (true);
    }
}
