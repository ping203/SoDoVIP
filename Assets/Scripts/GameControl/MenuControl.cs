using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuControl : StageControl {
    public UILabel lb_textnoti;
    public UILabel lb_name, lb_ID;
    public UILabel lb_chip;
    //public UILabel lb_xu;
    //public UILabel lb_numMess;
    /*public GameObject buttonDoiThuong;
    public UIButton avata;
    public UISprite[] stars;*/

    public UISprite spriteAvata;

   public GameObject btn_doithuong;
    //public UIToggle vip;
    //public UIToggle thuong;
    //   public GameObject info;
    // Use this for initialization
    WWW www;
    bool isSet = false;
    void Start () {
        updateAvataName ();
    }
    void OnEnable () {
        www = null;
        isSet = false;
        updateAvataName ();
        if (BaseInfo.gI().isDoiThuong == 0) {
            btn_doithuong.SetActive(false);
        } else {
            btn_doithuong.SetActive(true);
        }
        Debug.Log ("1111111111 Vao day roi!" + BaseInfo.gI ().isDoiThuong);

        //gameControl.panelNotiDoiThuong.onShow ();
        //StartCoroutine (showPopup ());
        // }
        // Debug.Log ("Vao day roi!" + BaseInfo.gI ().isLogin);
    }

    public void showP () {
        StartCoroutine (showPopup ());
    }

    IEnumerator showPopup () {
        yield return new WaitForSeconds (0.5f);
        //if(BaseInfo.gI ().isLogin) {
        //    BaseInfo.gI ().isLogin = false;
            gameControl.panelNotiDoiThuong.onShow ();
       // }
    }

    // Update is called once per frame
    void Update () {
        if(gameObject.activeInHierarchy && Input.GetKeyDown (KeyCode.Escape)) {
            gameControl.disableAllDialog ();
            onBack ();
        }

        lb_chip.text = (BaseInfo.formatMoneyNormal (BaseInfo.gI ().mainInfo.moneyChip) + Res.MONEY_FREE);
        //lb_xu.text = (BaseInfo.formatMoneyNormal (BaseInfo.gI ().mainInfo.moneyXu) + Res.MONEY_VIP);

        if(www != null) {
            if(www.isDone && !isSet) {
                spriteAvata.mainTexture = www.texture;
                isSet = true;
            }
        }
    }

    //public void setToggle () {
    //    if(BaseInfo.gI ().typetableLogin == Res.ROOMFREE) {
    //        vip.value = false;
    //        thuong.value = true;
    //    } else {
    //        vip.value = true;
    //        thuong.value = false;
    //    }
    //}

    void deActive () {
        gameObject.SetActive (false);
    }

    public void updateAvataName () {
        int idAvata = BaseInfo.gI ().mainInfo.idAvata;
        string link_ava = BaseInfo.gI ().mainInfo.link_Avatar;
        //int num_star = BaseInfo.gI ().mainInfo.level_vip;

        //for(int i = 0; i < 5; i++) {
        //    stars[i].spriteName = "Sao_toi_to";
        //    if(i < num_star) {
        //        stars[i].spriteName = "Sao_sang_to";
        //    }
        //}
        string dis = BaseInfo.gI ().mainInfo.displayname;
        if(dis.Length > 15) {
            dis = dis.Substring (0, 14) + "...";
        }
        lb_name.text = dis;
        lb_ID.text = "ID: " + BaseInfo.gI ().mainInfo.userid;
        if(link_ava == "") {
            if(idAvata != 0) {
                spriteAvata.GetComponent<UISprite> ().enabled = true;
                spriteAvata.GetComponent<UITexture> ().enabled = false;
                spriteAvata.spriteName = idAvata + "";
            } else {
                spriteAvata.spriteName = "0";
            }
        } else {
            www = new WWW (link_ava);
            if(www.error != null) {
                Debug.Log ("Image WWW ERROR: " + www.error);
            } else {
                spriteAvata.GetComponent<UISprite> ().enabled = false;
                spriteAvata.GetComponent<UITexture> ().enabled = true;
                // spriteAvata.GetComponent<UITexture> ().mainTexture = www.texture;
            }
        }
    }

    public override void onBack () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.setStage (gameControl.login);
    }

    public void onClickGame (GameObject obj) {
        switch(obj.name) {
            case "tlmn":
                gameControl.gameID = GameID.TLMN;
                break;
            case "phom":
                gameControl.gameID = GameID.PHOM;
                break;
            case "xito":
                gameControl.gameID = GameID.XITO;
                break;
            case "poker":
                gameControl.gameID = GameID.POKER;
                break;
            case "bacay":
                gameControl.gameID = GameID.BACAY;
                break;
            case "lieng":
                gameControl.gameID = GameID.LIENG;
                break;
            case "maubinh":
                gameControl.gameID = GameID.MAUBINH;
                break;
            case "xam":
                gameControl.gameID = GameID.XAM;
                break;
            case "xocdia":
                gameControl.gameID = GameID.XOCDIA;
                break;
        }

        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelWaiting.onShow ();
        gameControl.room.setGameName ();
        gameControl.room.updateAvataName ();
        SendData.onSendGameID ((sbyte) gameControl.gameID);
    }
    public void clickSetting () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelSetting.onShow ();
    }

    public void clickHelp () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panleHelp.onShow ();
    }

    public void clickNapXu () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelNapChuyenXu.onShow ();

    }

    public void clickDoiThuong () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelWaiting.onShow ();
        SendData.onGetInfoGift ();
        //}
    }

    public void clickAvatar () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelInfoPlayer.infoMe ();
        gameControl.panelInfoPlayer.onShow ();
    }

    public void clickHomThu () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelMail.onShow ();
    }

    public void clickDienDan () {
        //GameControl.instance.sound.startClickButtonAudio ();
        //gameControl.dialogNotification.onShow("Bạn có muốn chuyển đến diễn đàn?", delegate {
        //    Application.OpenURL(Res.linkForum);
        //});

    }

    public void clickLuatChoi () {
        //GameControl.instance.sound.startClickButtonAudio ();
        //gameControl.dialogLuatChoi.onShow();
    }

    public override void Appear () {
        base.Appear ();
        /*if(BaseInfo.gI ().isPurchase) {
            buttonDoiThuong.SetActive (false);
        } else {
            buttonDoiThuong.SetActive (true);
        }*/
    }

    public void clickNoti () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelNotiDoiThuong.onShow ();
    }

    //public void onClickPhongFree () {
    //    BaseInfo.gI ().typetableLogin = Res.ROOMFREE;
    //    //gameControl.room.setToggle ();
    //}

    //public void onClickPhongVip () {
    //    BaseInfo.gI ().typetableLogin = Res.ROOMVIP;
    //    //gameControl.room.setToggle ();
    //}
}

