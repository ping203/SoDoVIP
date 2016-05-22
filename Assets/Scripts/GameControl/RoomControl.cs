using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class RoomControl : StageControl {
    public UILabel lb_textnoti;
    public static int roomType; // 1:Thuong, 2:VIP
    //public UIToggle vip;
    //public UIToggle thuong;
    public List<TableBehavior> listTableBehavior = new List<TableBehavior> ();

    public UILabel lb_tengame;

    public UILabel displayName, lb_ID;
    public UILabel displayChip;
    //public UILabel displayXu;
    public UISprite spriteAvata;
    public UIToggle tg_anBanFull;

    public InstanListViewControler _instanListViewControler;
    WWW www;
    bool isSet = false;

    public bool anbanfull = true;
    // Use this for initialization
    void Start () {
        updateAvataName ();
        setGameName ();
       // setToggle ();
    }
    void OnEnable () {
        www = null;
        isSet = false;
        tg_anBanFull.value = anbanfull;
    }
    // Update is called once per frame
    void Update () {
        displayChip.text = BaseInfo.formatMoneyNormal (BaseInfo.gI ().mainInfo.moneyChip) + Res.MONEY_FREE;
        //displayXu.text = BaseInfo.formatMoneyNormal (BaseInfo.gI ().mainInfo.moneyXu) + Res.MONEY_VIP;
        if(gameObject.activeInHierarchy && Input.GetKeyDown (KeyCode.Escape)) {
            gameControl.disableAllDialog ();
            onBack ();
        }
        if(www != null) {
            if(www.isDone && !isSet) {
                spriteAvata.mainTexture = www.texture;
                isSet = true;
            }
        }
    }
    void deActive () {
        gameObject.SetActive (false);
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

    public override void onBack () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.setStage (gameControl.menu);
    }
    public void setGameName () {
        string name = "CHỌN GAME";
        switch(gameControl.gameID) {
            case GameID.TLMN:
                name = "TIẾN LÊN MN";
                break;
            case GameID.XAM:
                name = "SÂM";
                break;
            case GameID.LIENG:
                name = "LIÊNG";
                break;
            case GameID.BACAY:
                name = "BA CÂY";
                break;
            case GameID.PHOM:
                name = "PHỎM";
                break;
            case GameID.POKER:
                name = "POKER";
                break;
            case GameID.XITO:
                name = "XÌ TỐ";
                break;
            case GameID.MAUBINH:
                name = "MẬU BINH";
                break;
            default:
                break;
        }
        lb_tengame.text = name;
    }
    public void createScollPane (List<TableItem> listTable, int typeRoom) {
        gameControl.panelWaiting.onShow ();
        setGameName ();
        listTableBehavior.Clear ();
        try {
            if(this.gameObject.activeInHierarchy) {
                for(int i = 0; i < listTable.Count; i++) {
                    TableBehavior tableBehavior = new TableBehavior ();
                    tableBehavior.id = listTable[i].id;
                    tableBehavior.status = listTable[i].status;
                    tableBehavior.name = listTable[i].name;
                    tableBehavior.masid = listTable[i].masid;
                    tableBehavior.nUser = listTable[i].nUser;
                    tableBehavior.maxUser = listTable[i].maxUser;
                    tableBehavior.money = listTable[i].money;
                    tableBehavior.needMoney = listTable[i].needMoney;
                    tableBehavior.maxMoney = listTable[i].maxMoney;
                    tableBehavior.Lock = listTable[i].Lock;
                    tableBehavior.typeTable = listTable[i].typeTable;
                    tableBehavior.choinhanh = listTable[i].choinhanh;
                    listTableBehavior.Add (tableBehavior);

                    if(BaseInfo.gI ().isHideTabeFull) {
                        if(tableBehavior.nUser == tableBehavior.maxUser) {
                            continue;
                        }
                    }
                }
                _instanListViewControler.InitTableView (listTableBehavior, 0);
            }

            //setToggle ();
        } catch(Exception e) {
            Debug.LogException (e);
        }
        gameControl.panelWaiting.onHide ();
    }
    public void updateAvataName () {
        string dis = BaseInfo.gI ().mainInfo.displayname;
        if(dis.Length > 15) {
            dis = dis.Substring (0, 14) + "...";
        }
        displayName.text = dis;
        lb_ID.text = "ID: " + BaseInfo.gI ().mainInfo.userid;
        int idAvata = BaseInfo.gI ().mainInfo.idAvata;
        string link_ava = BaseInfo.gI ().mainInfo.link_Avatar;

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

    public void clickAvatar () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelInfoPlayer.infoMe ();
        gameControl.panelInfoPlayer.onShow ();
    }

    public void clickButtonLamMoi () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelWaiting.onShow ();
        SendData.onUpdateRoom ();
    }

    public void sortBanCuoc () {
        GameControl.instance.sound.startClickButtonAudio ();
        BaseInfo.gI ().sort_giam_dan_bancuoc = !BaseInfo.gI ().sort_giam_dan_bancuoc;
        //BaseInfo.gI().sort_giam_dan_bancuoc = true;
        BaseInfo.gI ().type_sort = 1;
        SendData.onUpdateRoom ();
    }

    public void sortMucCuoc () {
        GameControl.instance.sound.startClickButtonAudio ();
        BaseInfo.gI ().sort_giam_dan_muccuoc = !BaseInfo.gI ().sort_giam_dan_muccuoc;
        BaseInfo.gI ().type_sort = 2;
        SendData.onUpdateRoom ();
    }

    public void sortTrangThai () {
        GameControl.instance.sound.startClickButtonAudio ();
        BaseInfo.gI ().sort_giam_dan_nguoichoi = !BaseInfo.gI ().sort_giam_dan_nguoichoi;
        BaseInfo.gI ().type_sort = 3;
        SendData.onUpdateRoom ();
    }

    public void clickButtonChoiNgay () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelWaiting.onShow ();
        SendData.onAutoJoinTable ();
    }
    public void clickAnBanFull (bool isChecked) {
        gameControl.panelWaiting.onShow ();
      //  BaseInfo.gI ().isHideTabeFull = isChecked;
        anbanfull = isChecked;
        SendData.onUpdateRoom ();
    }
    //public void clickRoomVip () {
    //    GameControl.instance.sound.startClickButtonAudio ();
    //    if(vip.value) {
    //        BaseInfo.gI ().typetableLogin = Res.ROOMVIP;
    //        SendData.onJoinRoom (Res.ROOMVIP);
    //        gameControl.panelWaiting.onShow ();
    //    }
    //    //gameControl.menu.setToggle ();
    //}
    //public void clickRoopFree () {
    //    GameControl.instance.sound.startClickButtonAudio ();
    //    if(thuong.value) {
    //        BaseInfo.gI ().typetableLogin = Res.ROOMFREE;
    //        SendData.onJoinRoom (Res.ROOMFREE);
    //        gameControl.panelWaiting.onShow ();
    //    }
    // //   gameControl.menu.setToggle ();
    //}
    public void clickSetting () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelSetting.onShow ();
    }

    public void clickHelp () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panleHelp.onShow ();
    }

    public void clickCreateRoom () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelCreateRoom.onShow ();
    }

    public void clickToiBan () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelToiBan.onShow ();
    }

    public void clickNoti () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelNotiDoiThuong.onShow ();
    }

    public void clickPlayNow () {
        GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelWaiting.onShow ();
        SendData.onAutoJoinTable ();
    }
}
