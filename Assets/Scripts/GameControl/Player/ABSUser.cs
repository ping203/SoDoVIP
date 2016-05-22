using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ABSUser : MonoBehaviour {
    public BaseCasino casinoStage;

    public GameObject master;
    //public GameObject ready;

    public ArrayCard cardHand;
    public ArrayCard cardHand3Cay;
    // phom
    public ArrayCard[] cardPhom;
    public int diem = 0;

    // mau binh
    public ArrayCard[] cardMauBinh;
    public Vector3[] posCardMB;
    public Card cardMauBinhTouch;

    public int[] card_win = new int[3];
    public int[] allCardPhom;

    public GameObject body;
    public Timer timer;
    public Chat chat;
    public UIButton buttonInvite;
    public UIToggle toggleAction;
    public UIPanel panelAction;
    public UIButton buttonKick;
    public UIButton buttonInfo;
    public UISprite avatar;
    public UILabel lb_name_sansang;
    public UILabel lb_money;

    public UISprite sp_sap3chi;
    public UISprite sp_thang;
    public UISprite sp_xoay;
    public UISprite sp_typeCard;
    public UISprite sp_action;
    public UISprite sp_xepXong;
    public UISprite sp_lung;
    public UISprite sp_baoSam;

    public string st_name;
    public string display_name;
    private long folowMoney;
    public int pos, serverPos;
    public int[][] cardhand_xepbai = null;
    public int id_xepbai = 0;
    public bool isUserXepbai = false;
    public sbyte gender;

    protected bool isMasters;
    private bool isReadys;
    private bool isSits;
    private bool isPlayings;
    public bool isVisibleXoays;
    protected bool isTurns;

    public GameObject lb_money_result2;
    //public UILabel lb_money_am;

    public Chip chip;
    private Vector3 posChip = new Vector3 ();

    // Use this for initialization
    public void Start () {
    }
    int isToggle = 0;
    public void toggleActiononChange () {
        for(int i = 0; i < casinoStage.players.Length; i++) {

            if(this != casinoStage.players[i]) {
                casinoStage.players[i].onHide ();
            }
        }
        isToggle++;

        if(isToggle == 2) {
            isToggle = 0;
            toggleAction.value = false;
        } else {
            toggleAction.value = true;
        }

        if(BaseInfo.gI ().isView) {
            panelAction.gameObject.SetActive (false);
            return;
        } else {
            if(casinoStage.players[0].st_name == st_name) {
                buttonInfo.gameObject.SetActive (false);
                clickButtonInfo ();
                if(buttonKick != null)
                    buttonKick.gameObject.SetActive (false);
                return;
            } else {
                if(buttonKick != null) {
                    if(!casinoStage.players[0].isMaster ()) {
                        buttonKick.gameObject.SetActive (false);
                    } else {
                        if(!buttonKick.gameObject.activeInHierarchy && isToggle == 1)
                            buttonKick.gameObject.SetActive (true);
                        else
                            buttonKick.gameObject.SetActive (false);
                    }
                }
            }
        }
    }

    void Awake () {
        //setExit();
        if(chip != null) {
            posChip = chip.transform.localPosition;
        }
    }
    //public 
    private float dura = 0;
    // Update is called once per frame
    public virtual void setInfo (string diem) {

    }
    bool isOne = true;
    void Update () {
        //Debug.Log(getName() + " isTurns + " + isTurns);
        if(isTurns && getName ().Length != 0) {
            timer.gameObject.SetActive (true);
            dura += Time.deltaTime;

            if(dura < BaseInfo.gI ().timerTurnTable) {
                float percent;
                if(casinoStage.timeReceiveTurn == 0) {
                    percent = 1;
                } else {
                    percent = dura * 100 / casinoStage.timeReceiveTurn;
                }
                float temp = 100f - percent;
                if(temp > 75) {
                    if(pos == 0 && casinoStage.isStart && isOne && !BaseInfo.gI ().isView) {
                        GameControl.instance.sound.startCountDownAudio ();
                        isOne = false;
                    }
                    timer.sprite.color = Color.green;

                } else if(temp > 50) {
                    if(pos == 0 && !BaseInfo.gI ().isView) {
                        GameControl.instance.sound.pauseSound ();
                    }
                    timer.sprite.color = Color.green;
                } else if(temp > 25) {
                    if(pos == 0 && !BaseInfo.gI ().isView) {
                        GameControl.instance.sound.pauseSound ();
                    }
                    timer.sprite.color = Color.yellow;
                } else {
                    if(pos == 0 && !BaseInfo.gI ().isView) {
                        GameControl.instance.sound.pauseSound ();
                    }
                    timer.sprite.color = Color.red;
                }

                timer.setPercentage (percent);
            }
            if(dura >= BaseInfo.gI ().timerTurnTable) {
                if(pos == 0 && !BaseInfo.gI ().isView) {
                    GameControl.instance.sound.pauseSound ();
                }
            }


        } else {
            dura = 0; // loop
            if(pos == 0 && !BaseInfo.gI ().isView) {
                GameControl.instance.sound.pauseSound ();
            }

            timer.gameObject.SetActive (false);
        }

        timeAction += Time.deltaTime;
        if(timeAction >= 10) {
            isAction = true;
        }
        updateAvata ();
    }
    public void onClick () {
        if(pos != 0 && BaseInfo.gI ().mainInfo.nick
                                .Equals (casinoStage.masterID)) {
            //            buttonDuoi.gameObject.SetActive (!buttonDuoi.gameObject.activeInHierarchy);
            //           buttonInfo.gameObject.SetActive (!buttonInfo.gameObject.activeInHierarchy);

        } else {
            if(pos == 0) {
                GameControl.instance.panelInfoPlayer.onShow ();
            } else {
                SendData.onViewInfoFriend (getName ());
                casinoStage.gameControl.panelWaiting.onShow ();
            }

        }

    }
    bool isAction = true;
    float timeAction = 0;

    // GameObject ob;
    public void sendActions (int id) {
        GameControl.instance.sound.startClickButtonAudio ();
        if(isAction) {
            isAction = false;
            if(timeAction > 10) {
                timeAction = 0;
            }
            SendData.onBuyItem (id, BaseInfo.gI ().mainInfo.nick, st_name);
        }
        onHide ();
    }

    public void onHide () {
        isToggle = 0;
        toggleAction.value = false;
        panelAction.gameObject.SetActive (false);
        buttonInfo.gameObject.SetActive (false);

        if(buttonKick != null) {
            buttonKick.gameObject.SetActive (false);
        }
    }

    public void setName (string name) {
        this.st_name = name;
    }
    public void setDisplayeName (string name) {
        this.display_name = name;
        string nameFN;
        if(name.Length > 10) {
            nameFN = name.Substring (0, 6) + "...";
        } else {
            nameFN = name;
        }

        lb_name_sansang.text = nameFN;
    }
    public string getName () {
        return st_name;
    }
    public void setMoney (long folowMoney) {
        long m = folowMoney - this.folowMoney;
        if(m > 0) {
            setTextMDSmall ("+" + BaseInfo.formatMoneyDetailDot (m) + "");
        } else if(m < 0) {
            setTextMDSmall ("-" + BaseInfo.formatMoneyDetailDot (-m) + "");
        }

        this.folowMoney = folowMoney;
        if(folowMoney.ToString ().Length < 7) {
            lb_money.text = ("" + BaseInfo.formatMoneyDetailDot (folowMoney));
        } else {
            lb_money.text = ("" + BaseInfo.formatMoneyNormal (folowMoney));
        }
        //lb_money.text = BaseInfo.formatMoneyDetailDot (folowMoney) + "";
    }

    public void setTextMDSmall (string text) {
        lb_money_result2.SetActive (true);
        lb_money_result2.GetComponent<UILabel> ().depth = 999;
        lb_money_result2.GetComponent<UILabel> ().text = text;
        lb_money_result2.transform.localPosition = new Vector3 (0, -10, 0);
        TweenPosition.Begin (lb_money_result2, 0.5f, new Vector3 (0, 20, 0));
        StartCoroutine (setVisible (lb_money_result2, 3f));
    }

    public void setMoneyMB (long money) {
        string m = "";
        if(money > 0)
            m = "+";
        else {
            money = Math.Abs (money);
            m = "-";
        }
        //string m2 = m;
        if(money.ToString ().Length < 7) {
            m += ("" + BaseInfo.formatMoneyDetailDot (money));
        } else {
            m += ("" + BaseInfo.formatMoneyNormal (money));
        }

        lb_money_result2.SetActive (true);
        lb_money_result2.GetComponent<UILabel> ().depth = 999;
        lb_money_result2.GetComponent<UILabel> ().text = m;
        lb_money_result2.transform.localPosition = new Vector3 (0, -10, 0);
        TweenPosition.Begin (lb_money_result2, 0.5f, new Vector3 (0, 20, 0));
        StartCoroutine (setVisible (lb_money_result2, 2f));
    }

    protected IEnumerator setVisible (GameObject obj, float dur) {
        yield return new WaitForSeconds (dur);
        obj.gameObject.SetActive (false);

    }
    public long getFgetFolowMoney () {
        return folowMoney;
    }
    public void setSit (bool isSit) {
        this.isSits = isSit;
        avatar.gameObject.SetActive (isSit);

    }
    public virtual void setExit () {
        //body.SetActive(false);
        setSit (false);
        setReady (false);
        setMaster (false);
        resetData ();
        setMoneyChip (0);
        lb_money.text = "";

        //lb_money_result.gameObject.SetActive(false);
        if(lb_money_result2 != null && lb_money_result2.activeSelf)
            lb_money_result2.SetActive (false);

        if(buttonKick != null)
            buttonKick.gameObject.SetActive (false);

        if(chat != null)
            chat.gameObject.SetActive (false);

        //lb_money_am.gameObject.SetActive(false);

        setInvite (true);
        setName ("");
        setDisplayeName ("");
    }

    public void CreateInfoPlayer (PlayerInfo pl) {
        body.SetActive (true);
        if(sp_sap3chi != null) {
            sp_sap3chi.gameObject.SetActive (false);
        }

        if(sp_action != null) {
            sp_action.gameObject.SetActive (false);
        }
        setMoneyChip (0);
        this.gender = pl.gender;
        //setGendel(gender);
        this.setName (pl.name);
        this.setDisplayeName (pl.displayname);
        this.folowMoney = pl.folowMoney;
        if(folowMoney.ToString ().Length < 7) {
            this.lb_money.text = ("" + BaseInfo.formatMoneyDetailDot (folowMoney));
        } else {
            this.lb_money.text = ("" + BaseInfo.formatMoneyNormal (folowMoney));
        }
        setMaster (pl.isMaster);
        if(!isMaster ()) {
            //setReady(pl.isReady);
            this.isReadys = pl.isReady;
        }
        setInvite (pl.isVisibleInvite);
        this.serverPos = pl.posServer;
        setSit (true);

        if(cardHand != null) {
            cardHand.removeAllCard ();
        }
        
        setSoBai (0);
        isSits = true;
        isPlayings = false;

        if(sp_xoay != null) {
            sp_xoay.gameObject.SetActive (false);
        }

        if(sp_thang != null) {
            sp_thang.gameObject.SetActive (false);
        }

        if(sp_baoSam != null) {
            sp_baoSam.gameObject.SetActive (false);
        }
        if(sp_xepXong != null) {
            sp_xepXong.gameObject.SetActive (false);
        }
        if(sp_lung != null) {
            sp_lung.gameObject.SetActive (false);
        }

        //lb_money_result.gameObject.SetActive(false);
        if(lb_money_result2 != null && lb_money_result2.activeSelf)
            lb_money_result2.SetActive (false);

        if(pl.idAvata < 0) {
            WWW www = new WWW (pl.link_avatar);
            if(www.error != null) {
                Debug.Log ("Image WWW ERROR: " + www.error);
            } else {
                while(!www.isDone) {
                }
                avatar.GetComponent<UISprite> ().enabled = false;
                avatar.GetComponent<UITexture> ().enabled = true;
                avatar.GetComponent<UITexture> ().mainTexture = www.texture;
            }
        } else {
            avatar.GetComponent<UISprite> ().enabled = true;
            avatar.GetComponent<UITexture> ().enabled = false;
            if(pl.idAvata >= 0) {
                avatar.spriteName = pl.idAvata + "";
            } else {
                avatar.spriteName = "Avata_nau";
            }
        }
    }
    public void updateAvata () {
        if(casinoStage.players[0].st_name == st_name && !BaseInfo.gI ().isView) {
            int id = BaseInfo.gI ().mainInfo.idAvata;
            //string link_ava = BaseInfo.gI ().mainInfo.link_Avatar;

            /* if(link_ava != "") {
                 WWW www = new WWW (link_ava);
                 if(www.error != null) {
                     Debug.Log ("Image WWW ERROR: " + www.error);
                 } else {
                     while(!www.isDone) {
                     }
                     avatar.GetComponent<UISprite> ().enabled = false;
                     avatar.GetComponent<UITexture> ().enabled = true;
                     avatar.GetComponent<UITexture> ().mainTexture = www.texture;
                 }
             } else {
                 avatar.GetComponent<UISprite> ().enabled = true;
                 avatar.GetComponent<UITexture> ().enabled = false;*/
            if(id >= 0) {
                avatar.spriteName = id + "";
            } else {
                avatar.spriteName = "Avata_nau";
            }
            //}
        }
    }

    private void setSoBai (int p) {
        //throw new NotImplementedException();
    }
    //string[] genders = new string[] { "women_head1", "men_head1" };
    //private void setGendel(sbyte gender) {
    //    this.gender = gender;
    //    avatar.spriteName = genders[gender];
    //    //throw new NotImplementedException();
    //}

    public bool isSit () {
        return isSits;
    }

    public void setInvite (bool p) {
        if(buttonInvite != null) {
            buttonInvite.gameObject.SetActive (p);
        }

        if(toggleAction != null) {
            toggleAction.gameObject.SetActive (!p);
            toggleAction.value = false;
        }
        //panelAction.gameObject.SetActive (p);
    }

    public bool isMaster () {
        return isMasters;
    }

    public virtual void setMaster (bool isMaster) {
        this.isMasters = isMaster;
        if(master != null)
            master.SetActive (isMaster);
    }


    public bool isReady () {
        return isReadys;
    }

    public void setReady (bool isReady) {
        this.isReadys = isReady;
        //ready.SetActive(isReady);
        if(isReady) {
            lb_name_sansang.text = Res.TXT_SANSANG;
        } else {
            setDisplayeName (display_name);
        }
    }

    public void setPlaying (bool isPlaying) {
        this.isPlayings = isPlaying;
    }

    public bool isPlaying () {
        return isPlayings;
    }

    public bool isTurn () {
        return isTurns;
    }

    public void setTurn (bool isTurn) {
        this.isTurns = isTurn;
    }

    public virtual void resetData () {
        // TODO Auto-generated method stub
        if(cardHand != null) {
            cardHand.reSet ();
            cardHand.setSobai (0);
            cardHand.setAllMo (false);
            cardHand.removeAllCard ();
        }

        if(sp_sap3chi != null) {
            sp_sap3chi.gameObject.SetActive (false);
        }

        if(sp_thang) {
            sp_thang.gameObject.SetActive (false);
        }

        if(sp_xoay) {
            sp_xoay.gameObject.SetActive (false);
        }

        //lb_name_sansang.text = display_name;
        setDisplayeName (display_name);
        setTurn (false);
        if(sp_typeCard != null) {
            sp_typeCard.gameObject.SetActive (false);
        }

        if(cardHand3Cay != null) {
            cardHand3Cay.setAllMo (false);
            cardHand3Cay.removeAllCard ();
        }

        for(int i = 0; i < cardMauBinh.Length; i++) {
            cardMauBinh[i].setAllMo (false);
            cardMauBinh[i].removeAllCard ();
        }

        if(sp_action != null) {
            sp_action.gameObject.SetActive (false);
        }

        if(sp_xepXong != null)
            sp_xepXong.gameObject.SetActive (false);
        if(sp_baoSam != null)
            sp_baoSam.gameObject.SetActive (false);
        if(sp_lung != null) {
            sp_lung.gameObject.SetActive (false);
        }
    }
    public void setVisibleRank (bool isVi) {
        sp_thang.gameObject.SetActive (isVi);
        sp_xoay.gameObject.SetActive (isVi);
        if(!isVi) {
            sp_thang.gameObject.SetActive (false);
            sp_xoay.gameObject.SetActive (false);
            if(sp_baoSam != null) {
                sp_baoSam.gameObject.SetActive (false);
            }
            if(sp_xepXong != null) {
                sp_xepXong.gameObject.SetActive (false);
            }
            if(sp_lung != null) {
                sp_lung.gameObject.SetActive (false);
            }
            cardHand.setAllMo (false);
        }
    }

    public virtual void setCardHand (int[] card, bool isDearling,
            bool inone, bool isFlipCard) {
        cardHand.setArrCard (card, isDearling, inone, isFlipCard);
    }

    public virtual void setCardHand (int[] card, int[] cardMo, bool isDearling,
        bool inone, bool isFlipCard) {
        cardHand.setArrCard (card, cardMo, isDearling, inone, isFlipCard);
    }

    public void setReceiveTurnTime (long timeReceiveTurn) {
        // TODO Auto-generated method stub
        isTurns = true;
        dura = 0;
        isOne = true;
    }

    public virtual void setCardHandInFinishGame (int[] cards) {
        cardHand.setSobai (0);
        cardHand.StopAllCoroutines ();
        cardHand.setArrCard (cards, false, false, false);
        Invoke ("resetData2", 10f);
    }
    void resetData2 () {
        if(!casinoStage.isStart) {
            resetData ();
        }
    }
    public string[] ani_thang = new string[] { "rank_u", "rank_thang",
				"rank_thangtrang", "rank_mom", "rank_cong", "rank_lung"};

    public virtual void setRank (int rank) {
        // 0 mom, 1 nhat, 2 nhi, 3 ba, 4 bet, 5 u
        int idTR = -1;
        switch(rank) {
            case 0:
                idTR = 3;
                if(pos == 0 && !BaseInfo.gI ().isView) {
                    GameControl.instance.sound.startMomAudio ();
                }
                cardHand.setAllMo (true);
                break;
            case 1:
                idTR = 1;
                sp_xoay.gameObject.SetActive (true);
                if(pos == 0 && !BaseInfo.gI ().isView) {
                    GameControl.instance.sound.startWinAudio ();
                }
                cardHand.setAllMo (false);
                break;
            case 2:
            case 3:
                if(pos == 0) {
                    GameControl.instance.sound.startBaAudio ();
                }
                break;
            case 4:
                if(pos == 0 && !BaseInfo.gI ().isView) {
                    GameControl.instance.sound.startLostAudio ();
                }
                cardHand.setAllMo (true);
                break;
            case 5:
                idTR = 0;
                sp_xoay.gameObject.SetActive (true);
                if(pos == 0 && !BaseInfo.gI ().isView) {
                    GameControl.instance.sound.startUAudio ();
                }
                break;

            default:
                break;
        }

        if(idTR >= 0) {
            sp_thang.spriteName = ani_thang[idTR];
            sp_thang.MakePixelPerfect ();
            sp_thang.gameObject.SetActive (true);
        }
        Invoke ("setVisibleThang", 5f);
    }

    void setVisibleThang () {
        sp_thang.gameObject.SetActive (false);
        sp_xoay.gameObject.SetActive (false);
        if(sp_baoSam != null) {
            sp_baoSam.gameObject.SetActive (false);
        }
        if(sp_xepXong != null) {
            sp_xepXong.gameObject.SetActive (false);
        }
        if(sp_lung != null) {
            sp_lung.gameObject.SetActive (false);
        }
    }
    String[] action = new String[] { "action_xembai", "action_boluot",
				"action_check", "action_theo", "action_call", "action_upbo",
				"action_fold", "action_to", "action_raise" };
    internal void setAction (int id) {
        if(id < 0 || id > 8) {
            return;
        }
        sp_action.spriteName = action[id];
        sp_action.MakePixelPerfect ();
        sp_action.gameObject.SetActive (true);
        sp_action.transform.localPosition = new Vector3 (0, -30, 0);
        sp_action.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
        TweenPosition.Begin (sp_action.gameObject, 0.5f, new Vector3 (0, 50, 0));
        Invoke ("setVisibleAction", 2f);
    }
    void setVisibleAction () {
        sp_action.gameObject.SetActive (false);
    }

    internal void setTextChat (string mess) {
        if(mess.Equals ("")) {
            return;
        }
        chat.setText (mess);
        bool isMe = false;
        if(pos == 0) {
            isMe = true;
        }

        //casinoStage.historychat.addChat(getName(), mess, isMe);
    }
    public virtual int[] getEatCard () {
        return null;
    }

    public virtual void addToCardFromCard (ArrayCard arrCFrom, ArrayCard arrCTo, int idCard, bool isTouch) {
        Card cardFrom = arrCFrom.getCardbyID (idCard);
        if(arrCFrom.getSize () == 0) {
            return;
        }
        if(cardFrom == null) {
            cardFrom = arrCFrom.getCardbyPos (0);
        }
        Vector3 beginPos = cardFrom.transform.localPosition;

        arrCFrom.removeCardByID (idCard);
        arrCTo.addCard (idCard);

        Card card = arrCTo.getCardbyID (idCard);
        if(card == null) {
            card = arrCTo.getCardbyPos (arrCTo.getSize () - 1);
        }

        Vector3 endPos = card.transform.localPosition;
        card.transform.parent = arrCFrom.transform;
        card.transform.localPosition = beginPos;

        card.transform.parent = arrCTo.transform;
        StartCoroutine (card.moveTo (endPos, 0.2f, 0f, false));

    }

    public virtual void addToCardHand (int card, bool p) {

    }

    public virtual void onEatCard (int card) {

    }

    public virtual void setCardPhom (int[] arrayPhom) {

    }

    public virtual int[] getAllCardPhom () {
        return null;
    }

    public virtual void addToCard (ArrayCard arrC, int idCard, bool isDearling, bool isTouch, bool isSort) {
        arrC.addCard (idCard);
        if(isSort) {
            int[] temp = RTL.sort (cardHand.getArrCardAll ());
            if(pos == 0) {
                cardHand.setArrCard (temp);
            } else {
            }
        }
        Card card = arrC.getCardbyID (idCard);
        if(card == null) {
            card = arrC.getCardbyPos (arrC.getSize () - 1);
        }
        if(isDearling) {
            Vector3 oldPos = card.gameObject.transform.localPosition;
            card.gameObject.transform.parent = arrC.mainTransform;
            card.gameObject.transform.localPosition = new Vector3 (0, 0, 0);
            card.gameObject.transform.parent = arrC.transform;
            StartCoroutine (card.moveTo (oldPos, 0.25f, 0, true));
        }
    }

    public long getMoneyChip () {
        if(chip == null) {
            return 0;
        }
        return chip.getMoneyChip ();
    }

    public void setSoChip (long money) {
        if(chip != null) {
            chip.setSoChip (money);
        }

    }

    public void setMoneyChip (long money) {
        if(chip != null) {
            chip.setMoneyChip (money);
        }

    }

    internal long getFolowMoney () {
        return folowMoney;
    }

    internal void flyMoney () {
        if(chip != null) {
            if(chip.gameObject.activeInHierarchy) {
                setSoChip (0);
                chip.StopAllCoroutines ();
                chip.gameObject.transform.parent = this.gameObject.transform.parent;
                chip.gameObject.transform.localPosition = new Vector3 (0, 0, 0);
                chip.gameObject.transform.parent = this.gameObject.transform;
                Vector3 posTo = chip.gameObject.transform.localPosition;
                chip.gameObject.transform.localPosition = posChip;
                TweenPosition tp = TweenPosition.Begin (chip.gameObject, 0.6f, posTo);

                tp.AddOnFinished (delegate {
                    chip.gameObject.transform.localPosition = posChip;
                    setMoneyChip (0);
                });
                //TweenPosition.Begin();
            }

        }
    }

    public virtual void setLoaiBai (int p) {
    }
    public void clickButtonInvite () {
        if(BaseInfo.gI ().isView) {
            //SendData.onJoinTable (BaseInfo.gI ().mainInfo.nick, BaseInfo.gI ().idTable, "", -1);
            //if(game: xi to, poker, lieng)
            //Fix
        } else {
            casinoStage.gameControl.panelWaiting.onShow ();
            SendData.onGetListInvite ();
        }
    }

    /*public virtual void setXepXong (int p) {
        //throw new NotImplementedException ();
        //Debug.Log ("setXepXong");
    }*/

    public void clickButtonInfo () {
        SendData.onViewInfoFriend (getName ());
        casinoStage.gameControl.panelWaiting.onShow ();
        panelAction.gameObject.SetActive (false);
        buttonInfo.gameObject.SetActive (false);
        if(buttonKick != null)
            buttonKick.gameObject.SetActive (false);
        toggleAction.value = false;
    }
    public void clickButtonDuoi () {
        SendData.onKick (getName ());
        panelAction.gameObject.SetActive (false);
        buttonInfo.gameObject.SetActive (false);
        buttonKick.gameObject.SetActive (false);
        toggleAction.value = false;
    }

    public void setLung (bool islung) {
        sp_lung.gameObject.SetActive (islung);
        TweenScale.Begin (sp_lung.gameObject, 0.5f, new Vector3 (1.2f, 1.2f, 1.2f));
        StartCoroutine (waitLung (sp_lung.gameObject));
    }

    IEnumerator waitLung (GameObject obj) {
        yield return new WaitForSeconds (2f);
        TweenScale.Begin (obj, 0.5f, new Vector3 (0, 0, 0));
        yield return new WaitForSeconds (0.5f);
        obj.SetActive (false);
    }

    public void baoSam () {
        sp_baoSam.gameObject.SetActive (true);
        sp_baoSam.transform.localPosition = cardHand.transform.localPosition;
    }

    public virtual void setThangTrang (int type) {

    }
}
