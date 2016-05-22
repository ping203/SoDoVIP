using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MauBinhPlayer : ABSUser {
    // Use this for initialization
    void Start () {
        setDeleClick (0);
        setDeleClick (1);
        setDeleClick (2);

    }

    void setDeleClick (int index) {
        for(int i = 0; i < cardMauBinh[index].getSizeArrayCard (); i++) {
            MauBinhInput mbIP = new MauBinhInput ((MauBinh) casinoStage, this, cardHand, cardMauBinh,
                           cardMauBinh[index].getCardbyPos (i));
            cardMauBinh[index].getCardbyPos (i).setListenerClick (delegate {
                mbIP.click ();
            });
        }
    }

    public override void setExit () {
        base.setExit ();
        cardMauBinh[0].removeAllCard ();
        cardMauBinh[1].removeAllCard ();
        cardMauBinh[2].removeAllCard ();
        setLung (false);
    }
    public override void resetData () {
        base.resetData ();
        /*if (pos == 0)
        {
            Vector3 posi = this.transform.localPosition;
            posi.x = 0;
            this.transform.localPosition = posi;
        }*/
        //if (sp_thang != null)
        //{
        //    sp_thang.gameObject.SetActive(false);
        //}

        cardMauBinh[0].removeAllCard ();
        cardMauBinh[1].removeAllCard ();
        cardMauBinh[2].removeAllCard ();

        setLung (false);
    }

    public override void setRank (int rank) {
        //base.setRank (rank);

        int idTR = -1;
        switch(rank) {
            case 0:
                idTR = 3;
                if(pos == 0) {
                    GameControl.instance.sound.startLostAudio ();
                }
                cardHand.setAllMo (true);
                break;
            case 1:
                idTR = 1;
                sp_xoay.gameObject.SetActive (true);
                if(pos == 0) {
                    GameControl.instance.sound.startWinAudio ();
                }
                cardHand.setAllMo (false);
                break;
            case 2:
            case 3:
            case 4:
                if(pos == 0) {
                    GameControl.instance.sound.startLostAudio ();
                }
                cardHand.setAllMo (true);
                break;
            case 5:
                idTR = 0;
                sp_xoay.gameObject.SetActive (true);
                if(pos == 0) {
                    GameControl.instance.sound.startWinAudio ();
                }
                break;

            default:
                break;
        }

    }

    public override void setLoaiBai (int type) {

        base.setLoaiBai (type);
        if(type == -1) {
            //sp_thang.StopAllCoroutines();
            //sp_thang.gameObject.SetActive(true);

            //sp_thang.spriteName = Res.TypeCard_Name[type];
            //sp_thang.MakePixelPerfect();
            //sp_thang.gameObject.transform.localPosition = new Vector3(0, -50, 0);
            //sp_thang.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }

        if(type < 0 || type > 8) {
            return;
        }
        sp_typeCard.StopAllCoroutines ();
        sp_typeCard.gameObject.SetActive (true);

        sp_typeCard.spriteName = Res.TypeCard_Name[type];
        sp_typeCard.MakePixelPerfect ();
        sp_typeCard.gameObject.transform.localPosition = new Vector3 (0, -50, 0);
        sp_typeCard.gameObject.transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
        TweenPosition.Begin (sp_typeCard.gameObject, 0.5f, new Vector3 (0, -20, 0));
        StartCoroutine (setVisible (sp_typeCard.gameObject, 2.5f));
    }

    String[] animationMB = new String[] { "aniRongCuon", "aniSanhRong",
				"ani5Doi1Sam", "aniLucPheBon", "ani3CaiThung", "ani3CaiSanh" };

    public override void setThangTrang (int type) {
        if(type < 0 || type > 6) {
            return;
        }

        StartCoroutine (delayThangTrang(type));
    }

    IEnumerator delayThangTrang (int type) {
        yield return new WaitForSeconds (2f);
        GameControl.instance.sound.startMaubinhAudio ();
        sp_thang.spriteName = animationMB[type];
        sp_thang.MakePixelPerfect ();
        sp_thang.gameObject.SetActive (true);
        Invoke ("setVisibleThang", 5f);
    }

    void setVisibleThang () {
        sp_thang.gameObject.SetActive (false);
        sp_xoay.gameObject.SetActive (false);
    }
}

