using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class ArrayCard : MonoBehaviour {

    public GameObject prefabCard;
    public int type; // 0: center, 1: left, 2: right
    public int maxW = 100;
    private List<Card> arrCard;
    private List<int> arrIntCard;
    private float wCard;
    private float disCard;
    public bool isSmall = false;
    public bool inone;
    public const float sizeCardSmall = 2.5f / 3f;
    public bool isTouch = false;
    public int maxCard = 13;
    public GameControl gameControl;
    public Transform mainTransform;

    public UILabel lb_SoBai;
    private int soBai;
    public int beginDepth = 0;
    public void setVisibleSobai (bool isVisible) {
        if(lb_SoBai != null) {
            lb_SoBai.gameObject.SetActive (isVisible);
        }
    }
    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {

    }

    void Awake () {
        init (type, maxW, isSmall, maxCard, inone, isTouch, gameControl);
    }
    public void init (int type, int maxW, bool isSmall, int maxCard,
            bool inone, bool isTouch, GameControl gameControl) {
        this.type = type;
        this.maxW = maxW;
        //this.maxW = GameControl.WIDTH - GameControl.WPLAYER - 30;
        this.isSmall = isSmall;
        this.maxCard = maxCard;
        this.gameControl = gameControl;
        if(this.isSmall) {
            wCard = Card.W_CARD * sizeCardSmall;
        } else {
            wCard = Card.W_CARD;
        }
        this.inone = inone;
        arrCard = new List<Card> ();
        for(int i = 0; i < maxCard; i++) {
            GameObject obj = (GameObject) Instantiate (prefabCard);
            obj.transform.parent = this.gameObject.transform;
            obj.transform.localPosition = new Vector3 (0, 0, 0);
            obj.transform.localScale = new Vector3 (1, 1, 1);

            Card card = obj.GetComponent<Card> ();
            card.setId (53);
            card.setDepth (i + beginDepth);
            //card.gameObject.SetActive(false);
            if(this.isSmall) {
                card.gameObject.transform.localScale = new Vector3 (sizeCardSmall, sizeCardSmall, sizeCardSmall);
            }
            if(!isTouch) {
                card.setTouchable (false);
            }
            arrCard.Add (card);

        }
        for(int i = 0; i < maxCard; i++) {
            arrCard[i].setId (2);
        }
        arrIntCard = new List<int> ();
        isAllMo = false;
        reSet ();
    }
    public void setTypeCard (int type, int maxW, bool isSmall) {
        this.type = type;
        this.maxW = maxW;
        //this.maxW = GameControl.WIDTH - GameControl.WPLAYER - 12;
        this.isSmall = isSmall;
        if(this.isSmall) {
            wCard = Card.W_CARD * sizeCardSmall;
        } else {
            wCard = Card.W_CARD;
        }
    }
    public void addCard (int id) {
        arrIntCard.Add (id);
        reAddAllCard ();
    }

    public void addCard (int id, int[] cardMo) {
        arrIntCard.Add (id);
        reAddAllCard (cardMo);
    }

    public void setCardMo (int[] cards) {
        if(cards == null || cards.Length <= 0) {
            return;
        } else {
            setAllMo (false);
            for(int i = 0; i < cards.Length; i++) {
                if(getCardbyID (cards[i]) != null) {
                    getCardbyID (cards[i]).setMo (true);
                }
            }
        }
    }

    public void resetPostionCard (int newMaxW) {
        this.maxW = newMaxW;
        try {
            if(maxW >= (arrIntCard.Count * wCard)) {
                disCard = wCard;
            } else {
                disCard = maxW / arrIntCard.Count;
            }
            if(arrIntCard.Count % 2 == 0) {
                for(int i = 0; i < arrIntCard.Count; i++) {
                    arrCard[i].gameObject.transform.localPosition = new Vector3 (
                            -((int) arrIntCard.Count / 2 - 0.5f)
                                    * disCard + i * disCard, 0, 0);
                }
            } else {
                for(int i = 0; i < arrIntCard.Count; i++) {
                    arrCard[i].gameObject.transform.localPosition = new Vector3 (
                            -((int) arrIntCard.Count / 2) * disCard
                                    + i * disCard, 0, 0);
                }
            }
        } catch(Exception e) {
            // TODO: handle exception
        }
    }

    public void reAddAllCard () {
        try {
            for(int i = 0; i < maxCard; i++) {
                if(this.isSmall) {
                    arrCard[i].gameObject.transform.localScale = new Vector3 (sizeCardSmall, sizeCardSmall, sizeCardSmall);
                } else {
                    arrCard[i].gameObject.transform.localScale = new Vector3 (1, 1, 1);
                }
            }
            for(int i = 0; i < arrIntCard.Count; i++) {
                arrCard[i].setId (arrIntCard[i]);
                arrCard[i].gameObject.SetActive (true);
            }
            for(int i = arrIntCard.Count; i < maxCard; i++) {
                arrCard[i].gameObject.SetActive (false);
            }
            if(inone) {
                for(int i = 0; i < arrIntCard.Count; i++) {
                    arrCard[i].gameObject.transform.localPosition = new Vector3 (0, 0, 0);
                }
            } else {

                if(type == 0) {

                    if(maxW >= (arrIntCard.Count * wCard)) {
                        disCard = wCard;
                    } else {
                        disCard = maxW / arrIntCard.Count;
                    }
                    if(arrIntCard.Count % 2 == 0) {
                        for(int i = 0; i < arrIntCard.Count; i++) {
                            arrCard[i].gameObject.transform.localPosition = new Vector3 (
                                    -((int) arrIntCard.Count / 2 - 0.5f)
                                            * disCard + i * disCard, 0, 0);
                        }
                    } else {
                        for(int i = 0; i < arrIntCard.Count; i++) {
                            arrCard[i].gameObject.transform.localPosition = new Vector3 (
                                    -((int) arrIntCard.Count / 2) * disCard
                                            + i * disCard, 0, 0);
                        }
                    }

                } else if(type == 1) {
                    if(maxW >= (arrIntCard.Count * wCard)) {
                        disCard = wCard;
                    } else {
                        disCard = maxW / arrIntCard.Count;
                    }
                    for(int i = 0; i < arrIntCard.Count; i++) {
                        arrCard[i].gameObject.transform.localPosition = new Vector3 (i * disCard, 0, 0);
                    }
                } else if(type == 2) {
                    if(maxW >= (arrIntCard.Count * wCard)) {
                        disCard = wCard;
                    } else {
                        disCard = maxW / arrIntCard.Count;
                    }
                    for(int i = 0; i < arrIntCard.Count; i++) {
                        arrCard[i].gameObject.transform.localPosition = new Vector3 (
                                (-(getSize () - 1) * disCard + i * disCard), 0, 0);
                    }

                }
            }
        } catch(Exception e) {
            // TODO: handle exception
        }

    }

    public void reAddAllCard (int[] cardMo) {
        try {
            for(int i = 0; i < maxCard; i++) {
                if(this.isSmall) {
                    arrCard[i].gameObject.transform.localScale = new Vector3 (sizeCardSmall, sizeCardSmall, sizeCardSmall);
                } else {
                    arrCard[i].gameObject.transform.localScale = new Vector3 (1, 1, 1);
                }
            }

            for(int i = 0; i < arrIntCard.Count; i++) {
                arrCard[i].setId (arrIntCard[i]);
                arrCard[i].gameObject.SetActive (true);
                arrCard[i].setMo (false);
            }

            if(cardMo != null && cardMo.Length > 0) {
                for(int i = 0; i < arrIntCard.Count; i++) {
                    for(int j = 0; j < cardMo.Length; j++) {
                        if(arrCard[i].getId () == cardMo[j] && !arrCard[i].isMo()) {
                            arrCard[i].setMo (true);
                        }
                    }
                }
            }

            for(int i = arrIntCard.Count; i < maxCard; i++) {
                arrCard[i].gameObject.SetActive (false);
            }
            if(inone) {
                for(int i = 0; i < arrIntCard.Count; i++) {
                    arrCard[i].gameObject.transform.localPosition = new Vector3 (0, 0, 0);
                }
            } else {

                if(type == 0) {

                    if(maxW >= (arrIntCard.Count * wCard)) {
                        disCard = wCard;
                    } else {
                        disCard = maxW / arrIntCard.Count;
                    }
                    if(arrIntCard.Count % 2 == 0) {
                        for(int i = 0; i < arrIntCard.Count; i++) {
                            arrCard[i].gameObject.transform.localPosition = new Vector3 (
                                    -((int) arrIntCard.Count / 2 - 0.5f)
                                            * disCard + i * disCard, 0, 0);
                        }
                    } else {
                        for(int i = 0; i < arrIntCard.Count; i++) {
                            arrCard[i].gameObject.transform.localPosition = new Vector3 (
                                    -((int) arrIntCard.Count / 2) * disCard
                                            + i * disCard, 0, 0);
                        }
                    }

                } else if(type == 1) {
                    if(maxW >= (arrIntCard.Count * wCard)) {
                        disCard = wCard;
                    } else {
                        disCard = maxW / arrIntCard.Count;
                    }
                    for(int i = 0; i < arrIntCard.Count; i++) {
                        arrCard[i].gameObject.transform.localPosition = new Vector3 (i * disCard, 0, 0);
                    }
                } else if(type == 2) {
                    if(maxW >= (arrIntCard.Count * wCard)) {
                        disCard = wCard;
                    } else {
                        disCard = maxW / arrIntCard.Count;
                    }
                    for(int i = 0; i < arrIntCard.Count; i++) {
                        arrCard[i].gameObject.transform.localPosition = new Vector3 (
                                (-(getSize () - 1) * disCard + i * disCard), 0, 0);
                    }

                }
            }
        } catch(Exception e) {
            // TODO: handle exception
        }

    }

    public Card getCardbyID (int id) {
        try {
            for(int i = 0; i < arrIntCard.Count; i++) {
                if(arrCard[i].getId () == id) {
                    return arrCard[i];
                }
            }
        } catch(Exception e) {
            // TODO: handle exception
        }

        return null;
    }

    public Card getCardbyPos (int pos) {
        if(pos > arrCard.Count - 1) {
            pos = arrCard.Count - 1;
        }
        return arrCard[pos];
    }

    public void removeCardByID (int id) {
        for(int i = 0; i < arrIntCard.Count; i++) {
            if(arrIntCard[i] == id) {
                arrIntCard.RemoveAt (i);
                reAddAllCard ();
                return;
            }
        }
    }

    public void removeCardByPos (int pos) {
        if(pos < arrIntCard.Count) {
            arrIntCard.RemoveAt (pos);
            reAddAllCard ();
        }
    }

    public bool isAllMo;

    public void setAllMo (bool isMo) {
        isAllMo = isMo;
        for(int i = 0; i < arrCard.Count; i++) {
            arrCard[i].setMo (isMo);
        }
    }


    public void setCardMoByID (int id, bool isMo) {
        getCardbyID (id).setMo (isMo);
    }

    public void removeAllCard () {
        arrIntCard.Clear ();
        reAddAllCard ();
    }

    public int getSizeArrayCard () {
        return arrCard.Count;
    }

    public void reSet () {
        setAllMo (false);
        for(int i = 0; i < arrCard.Count; i++) {
            arrCard[i].setChoose (false);
        }
    }

    public int getSize () {
        return arrIntCard.Count;
    }

    public void setArrCard (int[] cards) {
        arrIntCard.Clear ();
        reAddAllCard ();
        for(int i = 0; i < cards.Length; i++) {
            addCard (cards[i]);
        }
    }

    public void setArrCard (int[] cards, int[] cardMo) {
        arrIntCard.Clear ();
        reAddAllCard (cardMo);
        for(int i = 0; i < cards.Length; i++) {
            addCard (cards[i], cardMo);
        }
    }

    public void setArrCard (int[] cards, bool isDearling,
            bool inone, bool isFlipCard) {
        if(cards == null) {
            setArrCard (new int[] { });
            return;
        }
        if(cards.Length == 0) {
            setArrCard (cards);
            return;
        }
        this.inone = inone;
        setArrCard (cards);
        for(int i = 0; i < getSize (); i++) {
            if(isDearling) {
                Card card = getCardbyPos (i);
                Vector3 oldPos = card.gameObject.transform.localPosition;
                card.gameObject.transform.parent = mainTransform;
                card.gameObject.transform.localPosition = new Vector3 (0, 0, 0);
                card.gameObject.transform.parent = this.transform;
                StartCoroutine (card.moveTo (oldPos, 0.25f, i * 0.15f, true));
                //gameControl.sound.startchiabaiAudio ();
            }
        }
        if(inone) {
            Invoke ("setSoBaiOnChia", 1f);
        }
    }

    public void setArrCard (int[] cards, int[] cardMo, bool isDearling,
        bool inone, bool isFlipCard) {
        if(cards == null) {
            setArrCard (new int[] { });
            return;
        }
        if(cards.Length == 0) {
            setArrCard (cards);
            return;
        }
        this.inone = inone;
        setArrCard (cards, cardMo);
        for(int i = 0; i < getSize (); i++) {
            if(isDearling) {
                Card card = getCardbyPos (i);
                Vector3 oldPos = card.gameObject.transform.localPosition;
                card.gameObject.transform.parent = mainTransform;
                card.gameObject.transform.localPosition = new Vector3 (0, 0, 0);
                card.gameObject.transform.parent = this.transform;
                StartCoroutine (card.moveTo (oldPos, 0.25f, i * 0.15f, true));
            }
        }
        if(inone) {
            Invoke ("setSoBaiOnChia", 1f);
        }
    }

    void setSoBaiOnChia () {
        setSobai (getSize ());
    }

    public int[] getArrCardChoose () { // có sắp xếp, có trả về null
        // return super.getArrCard();
        int[] arr = null;
        for(int i = 0; i < getSize (); i++) {
            if(arrCard[i].isChoose) {
                arr = RTL.insertArray (arr, arrCard[i].getId ());
            }
        }
        if(arr != null) {
            arr = RTL.sort (arr);
        }
        return arr;
    }
    public int[] getArrCardAll () { // có sắp xếp, không trả về null
        int[] arr = null;
        for(int i = 0; i < getSize (); i++) {
            arr = RTL.insertArray (arr, arrIntCard[i]);
        }
        if(arr != null) {
            arr = RTL.sort (arr);
        } else {

        }
        if(arr == null) {
            return new int[] { };
        }
        return arr;
    }
    public int[] getArrCardAll2 () { // có sắp xếp, không trả về null
        int[] arr = null;
        for(int i = 0; i < getSize (); i++) {
            arr = RTL.insertArray (arr, arrIntCard[i]);
        }
        if(arr == null) {
            return new int[] { };
        }
        return arr;
    }
    public int[] getArrayCardAllTrue () { // không sắp xếp
        int[] arr = null;
        for(int i = 0; i < arrCard.Count; i++) {
            arr = RTL.insertArray (arr, arrCard[i].getId ());
        }
        if(arr == null) {
            return new int[] { };
        }
        return arr;
    }
    public List<Card> getArrCardObj () {
        return this.arrCard;
    }
    public void onfireCard (int[] cards) {

        if(cards == null) {
            return;
        }

        if(gameControl.gameID == GameID.TLMN || gameControl.gameID == GameID.XAM) {
            gameControl.currentCasino.tableArrCard = cards;
        }
        if(gameControl.currentCasino.tableArrCard2
                            .getArrCardAll2 () != null) {
            gameControl.currentCasino.tableArrCard1.
                setArrCard (gameControl.currentCasino.tableArrCard2.getArrCardAll2 (), false, false, false);
            foreach(Card card in gameControl.currentCasino.tableArrCard1.getArrCardObj ()) {
                card.setDepth (0);
            }
        }
        gameControl.currentCasino.tableArrCard2.setArrCard (
                cards, false, false, false);
        foreach(Card card in gameControl.currentCasino.tableArrCard2.getArrCardObj ()) {
            card.setDepth (1);
        }
        gameControl.currentCasino.tableArrCard2.gameObject.SetActive (false);
        for(int i = 0; i < cards.Length; i++) {
            Card card0 = getCardbyID (cards[i]);
            if(card0 == null) {
                card0 = arrCard[0];
            }
            if(card0 == null) {
                return;
            }
            //Card card1 = gameControl.currentCasino.tableArrCard2.getCardbyID(cards[i]);
            //card1.transform.parent = this.transform;
            //Vector3 newPos = card1.transform.localPosition;
            //StartCoroutine(card0.moveTo(newPos, 0.4f, 0, true));
            //card1.transform.parent = gameControl.currentCasino.tableArrCard2.transform;

            card0.transform.parent = gameControl.currentCasino.tableArrCard2.transform;
            Vector3 oldPos = card0.transform.localPosition;
            Card card1 = gameControl.currentCasino.tableArrCard2.getCardbyID (cards[i]);
            Vector3 newPos = card1.transform.localPosition;
            card1.transform.localPosition = oldPos;
            StartCoroutine (card1.moveTo (newPos, 0.15f, 0f, true));
            card0.transform.parent = this.transform;
        }

        StartCoroutine (setActiveTableArrCard2 (cards));
    }
    IEnumerator setActiveTableArrCard2 (int[] cards) {
        yield return new WaitForSeconds (0f);
        for(int i = 0; i < cards.Length; i++) {
            this.removeCardByID (cards[i]);
        }
        yield return new WaitForSeconds (0f);
        gameControl.currentCasino.tableArrCard2.gameObject.SetActive (true);
    }

    public void setSobai (int soBai) {
        this.soBai = soBai;
        if(lb_SoBai == null) {
            return;
        }
        if(soBai <= 0) {
            lb_SoBai.gameObject.SetActive (false);
        } else {
            lb_SoBai.gameObject.SetActive (true);
        }
        lb_SoBai.text = soBai + "";
    }

    public int getSoBai () {
        return soBai;
    }

}
