using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ListernerServer : IChatListener {
    GameControl gameControl;

    public void initConnect() {
        NetworkUtil.GI().registerHandler(ProcessHandler.getInstance());
        ProcessHandler.setListenner(this);
        PHandler.setListenner(this);
        TLMNHandler.setListenner(this);
        XiToHandler.setListenner(this);
    }

    public ListernerServer(GameControl gameControl) {
        // TODO Auto-generated constructor stub
        this.gameControl = gameControl;
        initConnect();
    }

    public void onDisConnect() {
        gameControl.panelWaiting.onHide();
        gameControl.panelThongBao.onShowDCN("Mất kết nối!", delegate {
            gameControl.disableAllDialog();
            //SoundManager.Get().pauseAudio(SoundManager.AUDIO_TYPE.COUNT_DOWN);
            gameControl.setStage(gameControl.login);
            gameControl.resetGame();
            NetworkUtil.GI().close();
        });
    }

    //public void onConnectionFail()
    //{

    //}

    public void onLoginSuccess(Message msg) {
        gameControl.room.anbanfull = true;
        BaseInfo.gI().typetableLogin = Res.ROOMFREE;
        BaseInfo.gI().isNhanLoiMoiChoi = true;
        BaseInfo.gI().tudongsansang = true;
        BaseInfo.gI().sort_giam_dan_muccuoc = false;
        BaseInfo.gI().type_sort = 2;
        BaseInfo.gI().sort_giam_dan_bancuoc = false;
        try {
            string link = msg.reader().ReadUTF();
            BaseInfo.gI().SMS_CHANGE_PASS_SYNTAX = msg.reader().ReadUTF();
            BaseInfo.gI().SMS_CHANGE_PASS_NUMBER = msg.reader().ReadUTF();
            BaseInfo.gI().isDoiThuong = msg.reader().ReadByte();
        } catch (Exception ex) {
            Debug.LogException(ex);
        }
        SendData.onSendSmsSyntax();
        //if(BaseInfo.gI ().TELCO_CODE != 0) {
        //    SendData.onSendSms9029 (BaseInfo.gI ().TELCO_CODE);
        //}
        gameControl.panelWaiting.onHide();
        //  BaseInfo.gI ().isLogin = true;
        gameControl.setStage(gameControl.menu);
        gameControl.menu.showP();

        //  Debug.Log ("Da vao day!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! " + BaseInfo.gI ().isLogin);
    }

    public void onLoginFail(int id, string info) {
        /*gameControl.panelWaiting.onHide();
        gameControl.panelThongBao.onShow(info);*/
        string info2 = "";
        switch (id) {
            case 0:
                gameControl.panelYesNo.onShow("Tài khoản hoặc mật khẩu không đúng! Bạn có muốn Lấy lại mật khẩu không?", delegate {
                    //gameControl.panelThongBao.onShow ("");
                    gameControl.panelInput.onShow("Lấy lại mật khẩu", "", "Tên đăng nhập:", delegate {
                        string nick = gameControl.panelInput.ip_enter.value;
                        bool kt = false;

                        if (nick.Equals("")) {
                            info2 = "Nhập vào tên đăng nhập.";
                            kt = true;
                        }
                        if (kt) {
                            gameControl.panelThongBao.onShow(info2, delegate { });
                            return;
                        }
                        SendData.onGetPass(nick);
                        gameControl.panelInput.onHide();
                    });
                });
                break;
            case 2:
                gameControl.panelInput.onShow("Nhập số điện thoại", info, "SĐT:", delegate {
                    string phoneNumber = gameControl.panelInput.ip_enter.value;

                    bool kt = false;

                    if (phoneNumber.Equals("")) {
                        info2 = "Nhập vào số điện thoại.";
                        kt = true;
                    } else if (gameControl.panelInput.checkSDT(phoneNumber) == -1) {
                        info2 = "Sai định dạng số điện thoại!";
                        kt = true;
                    } else if (gameControl.panelInput.checkSDT(phoneNumber) == -3) {
                        info2 = "Số điện thoại phải nhiều hơn 9 và ít hơn 12 ký tự.";
                        kt = true;
                    }
                    if (kt) {
                        gameControl.panelThongBao.onShow(info2, delegate { });
                        return;
                    }

                    string imei = SystemInfo.deviceUniqueIdentifier;

                    gameControl.login.login(4, BaseInfo.gI().username, BaseInfo.gI().pass, imei, "", 1, BaseInfo.gI().username, "", phoneNumber);
                    gameControl.panelInput.onHide();
                });
                break;
            default:
                break;
        }
        gameControl.panelWaiting.onHide();
    }

    public void onListRoom(Message message) {
        //return;
        try {
            int len = message.reader().ReadByte();
            gameControl.phongFree.Clear();
            gameControl.phongVip.Clear();
            //gameControl.phongQT.Clear();
            for (int i = 0; i < len; i++) {
                RoomInfo r = new RoomInfo();
                r.setName(message.reader().ReadUTF());
                r.setId(message.reader().ReadByte());
                r.setMoney(message.reader().ReadLong());
                r.setNeedMoney(message.reader().ReadLong());
                r.setnUser(message.reader().ReadShort());
                r.setLevel(message.reader().ReadByte());
            }

            //int idRoom = Res.ROOMFREE;
            BaseInfo.gI().typetableLogin = message.reader().ReadInt();
            // Debug.Log("ttttttttttttt " + BaseInfo.gI().typetableLogin);
            //message.reader ().ReadInt ();
            SendData.onJoinRoom(BaseInfo.gI().mainInfo.nick, Res.ROOMFREE);

        } catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    public void onListTable(int totalTB, Message message) {
        //return;
        gameControl.listTable.Clear();
        List<TableItem> temp = new List<TableItem>();
        for (int i = 0; i < totalTB; i++) {
            try {
                TableItem ctb = new TableItem();
                ctb.id = (int)(message.reader().ReadShort());
                ctb.status = (message.reader().ReadByte());
                ctb.nUser = (message.reader().ReadByte());
                sbyte ilock = message.reader().ReadByte();
                ctb.Lock = ilock;
                ctb.money = message.reader().ReadLong();
                ctb.needMoney = message.reader().ReadLong();
                ctb.maxMoney = message.reader().ReadLong();
                ctb.maxUser = (message.reader().ReadByte());
                ctb.typeTable = message.reader().ReadInt();
                ctb.choinhanh = message.reader().ReadInt();
                if (gameControl.room.anbanfull && ctb.nUser == ctb.maxUser) {
                    continue;
                }
                gameControl.listTable.Add(ctb);

            } catch (IOException ex) {
                Debug.LogException(ex);
            }
        }

        try {
            int totalTBC = message.reader().ReadUnsignedShort();
            for (int i = 0; i < totalTB; i++) {
                TableItem ctb = new TableItem();
                ctb.id = (int)(message.reader().ReadShort());
                ctb.status = (message.reader().ReadByte());
                ctb.nUser = (message.reader().ReadByte());
                sbyte ilock = message.reader().ReadByte();
                ctb.Lock = ilock;
                ctb.money = message.reader().ReadLong();
                ctb.needMoney = message.reader().ReadLong();
                ctb.maxMoney = message.reader().ReadLong();
                ctb.maxUser = (message.reader().ReadByte());
                ctb.typeTable = message.reader().ReadInt();
                ctb.choinhanh = message.reader().ReadInt();
                if (gameControl.room.anbanfull && ctb.nUser == ctb.maxUser) {
                    continue;
                }
                temp.Add(ctb);

            }
        } catch (IOException ex) {
            Debug.LogException(ex);
        }

        BaseInfo.gI().sort_giam_dan_muccuoc = false;
        gameControl.room.sortMucCuoc();

        int dem5 = 0, dem9 = 0;
        int MAX = 2;
        //if(gameControl.gameID == GameID.TLMN || gameControl.gameID == GameID.PHOM
        //    || gameControl.gameID == GameID.XITO
        //    || gameControl.gameID == GameID.MAUBINH) {
        //    MAX = 1;
        //}
        long muccuoc;
        muccuoc = gameControl.listTable[0].money;
        for (int i = 0; i < gameControl.listTable.Count; i++) {
            try {
                if (gameControl.listTable[i].nUser != 0) {
                    temp.Add(gameControl.listTable[i]);
                    if (gameControl.listTable[i].money != muccuoc) {
                        dem5 = 0;
                        dem9 = 0;
                    }
                    muccuoc = gameControl.listTable[i].money;
                    continue;
                } else {
                    if (gameControl.listTable[i].money == muccuoc
                        && (gameControl.listTable[i].maxUser < 9)) {
                        dem5++;
                        if (dem5 <= MAX) {
                            temp.Add(gameControl.listTable[i]);
                            muccuoc = gameControl.listTable[i].money;
                            continue;
                        }

                    } else {
                    }
                    if (gameControl.listTable[i].money == muccuoc
                        && (gameControl.listTable[i].maxUser == 9)) {
                        dem9++;
                        if (dem9 <= MAX) {
                            temp.Add(gameControl.listTable[i]);
                            muccuoc = gameControl.listTable[i].money;
                            continue;
                        }
                    } else {
                    }
                }
                if (gameControl.listTable[i].money != muccuoc) {
                    dem5 = 0;
                    dem9 = 0;
                    if ((gameControl.listTable[i].maxUser < 9)) {
                        dem5++;
                        if (dem5 <= MAX) {
                            temp.Add(gameControl.listTable[i]);
                            muccuoc = gameControl.listTable[i].money;
                            continue;
                        }

                    } else {
                    }
                    if ((gameControl.listTable[i].maxUser == 9)) {
                        dem9++;
                        if (dem9 <= MAX) {
                            temp.Add(gameControl.listTable[i]);
                            muccuoc = gameControl.listTable[i].money;
                            continue;
                        }
                    } else {
                    }

                }
                muccuoc = gameControl.listTable[i].money;
            } catch (Exception e) {
                // TODO: handle exception��
                Debug.LogException(e);
                continue;
            }

        }
        //if(RoomControl.roomType == 1) {
        //    BaseInfo.gI ().moneyName = " " + Res.MONEY_FREE;
        //    BaseInfo.gI ().typetableLogin = 1;
        //} else {
        //    BaseInfo.gI ().moneyName = " " + Res.MONEY_VIP;
        //    BaseInfo.gI ().typetableLogin = 0;
        //}
        if (temp.Count > 0) {
            if (temp[0].typeTable == Res.ROOMFREE) {
                BaseInfo.gI().typetableLogin = Res.ROOMFREE;
            } else {
                BaseInfo.gI().typetableLogin = Res.ROOMVIP;
            }
        }
        this.gameControl.listTable.Clear();
        this.gameControl.listTable.AddRange(temp);
        gameControl.panelWaiting.onHide();
        gameControl.setStage(gameControl.room);
        gameControl.room.createScollPane(gameControl.listTable, RoomControl.roomType);

    }

    public void onJoinGame(Message message) {
        try {
            onGameID(message);
            onListRoom(message);
        } catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    public void onUserJoinTable(int tbid, string nick, string displayname,
        string link_avatar, int idAvata, sbyte pos, long money,
        long folowmoney) {
        PlayerInfo pl = new PlayerInfo();
        pl.name = nick;
        pl.displayname = displayname;
        pl.link_avatar = link_avatar;
        pl.idAvata = idAvata;
        pl.money = money;
        pl.pos = pl.posServer = pos;
        pl.folowMoney = folowmoney;
        //gameControl.currentCasino.setPlayerInfo(pl,
        //       gameControl.currentCasino.players[0].serverPos);

        if (!BaseInfo.gI().isView) {
            gameControl.currentCasino.setPlayerInfo(pl,
        gameControl.currentCasino.players[0].serverPos);
        } else {
            gameControl.currentCasino.setPlayerInfoView(pl,
       pl.posServer);
        }
        gameControl.sound.startVaobanAudio();
        gameControl.sound.PlayVibrate();
    }
    public void onExitView(Message message) {
        //gameControl.currentCasino.onExitView(message);
        gameControl.currentCasino.resetData();
        gameControl.panelWaiting.onHide();
        gameControl.setStage(gameControl.room);
        BaseInfo.gI().isView = false;
    }
    public void onUserExitTable(int idTb, string master, string nick) {
        if (nick.Equals(BaseInfo.gI().mainInfo.nick)) {
            try {
                gameControl.disableAllDialog();
                gameControl.currentCasino.resetData();
                gameControl.currentCasino.DisAppear();
                gameControl.setStage(gameControl.backState);
                BaseInfo.gI().isView = false;
            } catch (Exception e) {
                Debug.LogException(e);
            }
        } else {
            try {
                gameControl.currentCasino.removePlayer(nick);
                gameControl.currentCasino.masterID = master;
                gameControl.currentCasino.setMaster(master);
            } catch (Exception e) {
                // TODO: handle exception
                Debug.LogException(e);
            }
        }
    }

    public void onJoinTablePlaySuccess(Message message) {
        if (BaseInfo.gI().numberPlayer == 9) {
            gameControl.setCasino(gameControl.gameID, 1);
        } else {
            gameControl.setCasino(gameControl.gameID, 0);
        }
        gameControl.currentCasino.onJoinTableSuccess(message);
    }

    public void onJoinRoomFail(string info) {
        gameControl.panelWaiting.onHide();
        gameControl.panelThongBao.onShow(info, delegate { });
    }

    public void onJoinTableSuccess(Message message) {
        if (BaseInfo.gI().numberPlayer == 9) {
            gameControl.setCasino(gameControl.gameID, 1);
        } else {
            gameControl.setCasino(gameControl.gameID, 0);
        }
        gameControl.currentCasino.onJoinTableSuccess(message);
    }

    public void onJoinTablePlayFail(String info) {
        gameControl.panelWaiting.onHide();
        gameControl.panelYesNo.onShow(info + ". Bạn có muốn nạp thêm tiền không ?", delegate {
            //SendData.onJoinTableForView(BaseInfo.gI().idTable, "");
            gameControl.panelNapChuyenXu.onShow();
        });
    }

    public void onJoinTableFail(string info) {
        gameControl.panelWaiting.onHide();
        gameControl.panelYesNo.onShow(info + ". Bạn có muốn nạp thêm tiền không ?", delegate {
            gameControl.panelNapChuyenXu.onShow();
        });
    }

    public void onProfile(Message msg) {
        try {
            BaseInfo.gI().mainInfo.nick = msg.reader().ReadUTF();
            BaseInfo.gI().mainInfo.userid = msg.reader().ReadLong();
            BaseInfo.gI().mainInfo.moneyXu = msg.reader().ReadLong();
            BaseInfo.gI().mainInfo.moneyChip = msg.reader().ReadLong();
            msg.reader().ReadUTF();

            Debug.Log("xuuuuuuuuuuuuuuuu " + BaseInfo.gI().mainInfo.moneyXu);
            Debug.Log("chipppppppppppppp " + BaseInfo.gI().mainInfo.moneyChip);
            BaseInfo.gI().mainInfo.displayname = msg.reader().ReadUTF();
            BaseInfo.gI().mainInfo.link_Avatar = msg.reader().ReadUTF();
            BaseInfo.gI().mainInfo.idAvata = msg.reader().ReadInt();
            BaseInfo.gI().mainInfo.exp = msg.reader().ReadLong();
            BaseInfo.gI().mainInfo.score_vip = msg.reader().ReadLong();
            BaseInfo.gI().mainInfo.total_money_charging = msg.reader().ReadLong();
            BaseInfo.gI().mainInfo.total_time_play = msg.reader().ReadLong();

            BaseInfo.gI().mainInfo.soLanThang = msg.reader().ReadUTF();
            BaseInfo.gI().mainInfo.soLanThua = msg.reader().ReadUTF();
            BaseInfo.gI().mainInfo.soTienMax = msg.reader().ReadLong();
            BaseInfo.gI().mainInfo.soChipMax = msg.reader().ReadLong();
            BaseInfo.gI().mainInfo.soGDThanhCong = msg.reader().ReadInt();
            string email_sdt = msg.reader().ReadUTF();

            BaseInfo.gI().mainInfo.gender = msg.reader().ReadByte();

            string[] s = Regex.Split(email_sdt, "\\*");
            BaseInfo.gI().mainInfo.email = s[0];
            if (s.Length > 1)
                BaseInfo.gI().mainInfo.phoneNumber = s[1];
            else {
                BaseInfo.gI().mainInfo.phoneNumber = "";
            }

            if (BaseInfo.gI().mainInfo.exp < 10) {
                BaseInfo.gI().mainInfo.level_vip = 0;
            } else if (BaseInfo.gI().mainInfo.exp >= 30 && BaseInfo.gI().mainInfo.exp < 100) {
                BaseInfo.gI().mainInfo.level_vip = 1;
            } else if (BaseInfo.gI().mainInfo.exp >= 100 && BaseInfo.gI().mainInfo.exp < 300) {
                BaseInfo.gI().mainInfo.level_vip = 2;
            } else if (BaseInfo.gI().mainInfo.exp >= 300 && BaseInfo.gI().mainInfo.exp < 10000) {
                BaseInfo.gI().mainInfo.level_vip = 3;
            } else if (BaseInfo.gI().mainInfo.exp >= 10000 && BaseInfo.gI().mainInfo.exp < 100000) {
                BaseInfo.gI().mainInfo.level_vip = 4;
            } else if (BaseInfo.gI().mainInfo.exp > 100000) {
                BaseInfo.gI().mainInfo.level_vip = 5;
            }

            //switch(UnityPluginForWindowPhone.Class1.getDeviceNetworkInformation ()) {
            //    case "VIETTEL":
            //        BaseInfo.gI ().TELCO_CODE = 1;
            //        break;
            //    case "VINAPHONE":
            //        BaseInfo.gI ().TELCO_CODE = 2;
            //        break;
            //    case "MOBIFONE":
            //        BaseInfo.gI ().TELCO_CODE = 3;
            //        break;
            //    default:
            //        BaseInfo.gI ().TELCO_CODE = 1;
            //        break;
            //}

            gameControl.menu.updateAvataName();
            gameControl.room.updateAvataName();
            BaseInfo.gI().isNhanLoiMoiChoi = true;
        } catch (Exception e) {
            // TODO Auto-generated catch block
            Debug.LogException(e);
        }
    }

    public void onStartFail(string info) {
        gameControl.panelWaiting.onHide();
        gameControl.panelThongBao.onShow(info, delegate { });
    }

    public void onStartSuccess(Message message) {
        try {
            int[] cardHand = new int[1];
            int size = message.reader().ReadInt();
            sbyte[] c = new sbyte[size];
            for (int i = 0; i < size; i++) {
                c[i] = message.reader().ReadByte();
            }
            cardHand = new int[c.Length];
            for (int i = 0; i < c.Length; i++) {
                cardHand[i] = c[i];
            }

            int size1 = message.reader().ReadByte();
            string[] playingName = new string[size1];
            for (int i = 0; i < size1; i++) {
                playingName[i] = message.reader().ReadUTF();
            }

            // dialog_waiting.dismiss();
            gameControl.currentCasino.startTableOk(cardHand, message,
                    playingName);
            gameControl.currentCasino.isStart = true;
            gameControl.currentCasino.isPlaying = true;

        } catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    public void onStartForView(Message message) {
        try {
            int size = message.reader().ReadByte();
            string[] playingName = new string[size];
            for (int i = 0; i < size; i++) {
                playingName[i] = message.reader().ReadUTF();
            }
            gameControl.currentCasino.onStartForView(playingName);
        } catch (Exception e) {
        }
    }

    public void onSetTurn(Message message) {
        try {
            string nick = message.reader().ReadUTF();
            gameControl.currentCasino.setTurn(nick, message);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void onFireCard(string nick, string turnname, int[] card) {
        gameControl.currentCasino.onFireCard(nick, turnname, card);
    }

    public void onFireCardFail() {
        gameControl.currentCasino.onFireCardFail();
    }

    public void onGetCardNocSuccess(string nick, int card) {
        gameControl.currentCasino.onGetCardNoc(nick, card);
    }

    public void onEatCardSuccess(string from, string to, int card) {
        gameControl.currentCasino.onEatCardSuccess(from, to, card);
    }

    public void onBalanceCard(string from, string to, int card) {
        gameControl.currentCasino.onBalanceCard(from, to, card);
    }

    public void onReady(Message message) {
        try {
            int tbid = message.reader().ReadShort();
            int totalReady = message.reader().ReadByte();
            for (int i = 0; i < totalReady; i++) {
                String nick = message.reader().ReadUTF();
                bool ready = message.reader().ReadBoolean();
                int pl = gameControl.currentCasino.getPlayer(nick);
                if (pl != -1) {
                    if (ready) {
                        gameControl.currentCasino.players[pl].resetData();
                    }
                    gameControl.currentCasino.players[pl]
                            .setReady(ready);
                }
                if (nick.Equals(BaseInfo.gI().mainInfo.nick)) {
                    if (gameControl.currenStage is HasMasterCasino) {
                        if (!ready) {
                            ((HasMasterCasino)gameControl.currenStage).lb_Btn_sansang
                                    .text = (Res.TXT_SANSANG);
                        } else {
                            ((HasMasterCasino)gameControl.currenStage).btn_sansang.gameObject.SetActive(false);
                        }
                    }

                }
            }
        } catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    public void onAttachCard(string from, string to, int[] cards, int[] cardsgui) {
        gameControl.currentCasino.onAttachCard(from, to, cards, cardsgui);
    }

    public void onAllCardPlayerFinish(Message message) {
        try {
            string nick = message.reader().ReadUTF();
            int size = message.reader().ReadInt();
            sbyte[] c = new sbyte[size];
            for (int i = 0; i < size; i++) {
                c[i] = message.reader().ReadByte();
            }
            int[] card = new int[c.Length];
            for (int i = 0; i < c.Length; i++) {
                card[i] = c[i];
            }
            gameControl.currentCasino.allCardFinish(nick, card);
        } catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    public void onFinishGame(Message message) {
        gameControl.currentCasino.onFinishGame(message);
    }

    public void onDropPhomSuccess(string nick, int[] arrayPhom) {
        gameControl.currentCasino.onDropPhomSuccess(nick, arrayPhom);
    }

    public void onInvite(string nickInvite, string displayName, sbyte gameid, short roomid, short tblid, long money, long minMoney, long maxMoney) {
        string gameName = "";
        switch (gameid) {
            case GameID.PHOM:
                gameName = "Game Phỏm";
                break;
            case GameID.TLMN:
                gameName = "Game Tiến Lên Miền Nam";
                break;
            case GameID.XAM:
                gameName = "Game Sâm";
                break;
            case GameID.XITO:
                gameName = "Game Xito";
                break;
            case GameID.MAUBINH:
                gameName = "Game Mậu Binh";
                break;
            case GameID.BACAY:
                gameName = "Game Ba Cây";
                break;
            case GameID.LIENG:
                gameName = "Game Liêng";
                break;
            case 7:
                return;
            // break;
            case GameID.POKER:
                gameName = "Game Pocker";
                break;
            case GameID.XOCDIA:
                gameName = "Game Xóc đĩa";
                break;
        }
        if (BaseInfo.gI().isNhanLoiMoiChoi) {// && !gameControl.dialogNapXu.isShow && !gameControl.dialogDoiThuong.isShow
            string m = "";
            if (roomid == 1) {
                m = Res.MONEY_FREE;
            } else {
                m = Res.MONEY_VIP;
            }
            gameControl.panelThongBaoMoiChoi.onShow(displayName.Equals("") ? nickInvite
                                          : displayName
                + " mời bạn đánh " + gameName + ", mức cược là  " + money + m
                + ", bạn có đồng ý không?", delegate {
                    BaseInfo.gI().moneyNeedTable = minMoney;
                    if (maxMoney == -1) {
                        SendData.onAcceptInviteFriend(gameid, roomid, tblid, -1);
                        SendData.onJoinTablePlay(tblid, "", -1);
                        gameControl.panelWaiting.onShow();
                    } else {
                        int temp;
                        if (roomid == 2) {
                            temp = 2;
                        } else {
                            temp = 1;
                        }
                        gameControl.panelRutTien.show(
                                     (int)(minMoney * 2.5f), maxMoney, 1, tblid,
                                     roomid, gameid, temp);
                    }
                    gameControl.panelWaiting.onHide();
                });
        }
    }

    public void onRegSuccess() {
        try {
            gameControl.panelWaiting.onHide();
            gameControl.panelDangKy.onHide();
            gameControl.panelThongBao
                    .onShow("Đăng ký thành công", delegate { });
            gameControl.login.input_username.value = gameControl.panelDangKy.ip_name.value;
            gameControl.login.input_passsword.value = gameControl.panelDangKy.ip_pass.value;
        } catch (Exception ex) {
            Debug.LogException(ex);

        }
    }

    public void onRegFail(string info) {
        gameControl.panelWaiting.onHide();
        gameControl.panelThongBao.onShow(info, delegate { });
    }

    public void onKickOK() {
        gameControl.panelThongBao.onShow("Bạn bị đuổi khỏi bàn chơi!", delegate { });
    }

    public void onSysKick() {
        gameControl.panelYesNo.onShow("Tài khoản của bạn không đủ tiền để chơi tiếp./n Bạn có muốn nạp thêm tiền?", delegate {
            gameControl.panelNapChuyenXu.onShow();
        });
    }

    public void onChatMessage(string nick, string msg, bool outs) {
        gameControl.currentCasino.onMsgChat(nick, msg);
    }

    //public void onUpdateVersion(string version, string link, string decription)
    //{
    //    NetworkUtil.GI().close();
    //    gameControl.setStage(gameControl.login);
    //    gameControl.dialogNotification
    //                .onShow("Bạn phải cập nhập lên phiên bản " + version
    //                    + "để tiếp tục chơi game?", delegate { Application.OpenURL(link); });

    //}

    public void onUpdateProfile(int code, string info) {
        gameControl.panelThongBao.onShow("Cập nhật thông tin cá nhân thành công!", delegate {
        });
    }

    public void onUnReadMessage(Message messge) {
        try {
            int nMail = messge.reader().ReadInt();
            BaseInfo.gI().soTinNhan = nMail;
        } catch (Exception e) {
            // TODO: handle exception
        }
    }

    public void onGameID(Message message) {
        try {
            gameControl.gameID = message.reader().ReadByte();
            if (gameControl.gameID == -99) {
                gameControl.gameID = message.reader().ReadByte();
                int currentRoom = message.reader().ReadByte();
                //if (currentRoom == Res.ROOMTAPSU) {
                //    // setRoomFocus(1);
                //    BaseInfo.gI().idRoom = 1;
                //    BaseInfo.gI().nameTale = "Bình dân";
                //}
                //else if (currentRoom == Res.ROOMDAIGIA) {
                //    // setRoomFocus(2);
                //    BaseInfo.gI().idRoom = 2;
                //    BaseInfo.gI().nameTale = "Đại gia";
                //}
                //else if (currentRoom == Res.ROOMVIP) {
                //    // setRoomFocus(4);
                //    BaseInfo.gI().idRoom = 4;
                //    BaseInfo.gI().nameTale = "VIP";
                //}
                String nameRoom = message.reader().ReadUTF();
            }
        } catch (Exception ex) {
            Debug.LogException(ex);
        }
        switch (gameControl.gameID) {
            case GameID.PHOM:
                Card.setCardType(0);
                BaseInfo.gI().nameTale = "Phỏm";
                ProcessHandler.setSecondHandler(PHandler.getInstance());
                break;
            case GameID.TLMN:
                Card.setCardType(1);
                BaseInfo.gI().nameTale = "Tiến Lên Miền Nam";
                ProcessHandler.setSecondHandler(TLMNHandler.getInstance());
                break;
            case GameID.XAM:
                Card.setCardType(1);
                BaseInfo.gI().nameTale = "Sâm";
                ProcessHandler.setSecondHandler(TLMNHandler.getInstance());
                break;
            case GameID.BACAY:
                Card.setCardType(0);
                BaseInfo.gI().nameTale = "Ba Cây";
                ProcessHandler.setSecondHandler(TLMNHandler.getInstance());
                break;
            case GameID.XITO:
                Card.setCardType(0);
                BaseInfo.gI().nameTale = "Xì Tố";
                ProcessHandler.setSecondHandler(XiToHandler.getInstance());
                break;
            case GameID.LIENG:
                Card.setCardType(0);
                BaseInfo.gI().nameTale = "Liêng";
                ProcessHandler.setSecondHandler(XiToHandler.getInstance());
                break;
            case GameID.POKER:
                Card.setCardType(1);
                BaseInfo.gI().nameTale = "Poker";
                ProcessHandler.setSecondHandler(XiToHandler.getInstance());
                break;
            case GameID.MAUBINH:
                Card.setCardType(1);
                BaseInfo.gI().nameTale = "Mậu Binh";
                break;
            case GameID.XOCDIA:
                // Card.setCardType(1);
                BaseInfo.gI().nameTale = "Xóc Đĩa";
                break;
            default:
                break;
        }

    }

    public void onMessageServer(string info) {
        gameControl.panelWaiting.onHide();
        gameControl.panelThongBao.onShow(info, delegate { });
    }

    //public void onMsgChat(string from, string msg)
    //{

    //}

    public void onSetMoneyTable(long money) {

    }

    public void onDetailUser(Message message) {
        try {
            string nick = message.reader().ReadUTF();
            long userID = message.reader().ReadLong();
            long money = message.reader().ReadLong();
            long chip = message.reader().ReadLong();
            string name = message.reader().ReadUTF();
            string displayname = message.reader().ReadUTF();
            long exp = message.reader().ReadLong();
            long score_vip = message.reader().ReadLong();
            long total_money_charging = message.reader().ReadLong();
            long total_time_play = message.reader().ReadLong();
            // m.idAvatar = message.reader().readShort();

            string soLanThang = message.reader().ReadUTF();
            string soLanThua = message.reader().ReadUTF();
            long soTienMax = message.reader().ReadLong();
            long soChipMax = message.reader().ReadLong();
            int soGDThanhCong = message.reader().ReadInt();
            string LanDangNhapCuoi = message.reader().ReadUTF();
            string link_Avatar = message.reader().ReadUTF();
            int idAvata = message.reader().ReadInt();

            gameControl.panelInfoPlayer.infoProfile(displayname, userID, money, chip, soLanThang, soLanThua, link_Avatar, idAvata, "", "");

            gameControl.panelWaiting.onHide();
            gameControl.panelInfoPlayer.onShow();
        } catch (Exception e) {
            // TODO: handle exceptione.
            Debug.LogException(e);
        }
    }

    public void onInfoSMS(Message message) {
        try {
            sbyte len = message.reader().ReadByte();
            BaseInfo.gI().isCharging = len;
            Debug.Log("ccccccccccccccc " + len);
            for (int i = 0; i < 2; i++) {
                string name = message.reader().ReadUTF();
                string syntax = message.reader().ReadUTF();
                short port = message.reader().ReadShort();

                int type = (port % 1000) / 100;
                if (type == 6) {
                    BaseInfo.gI().syntax10 = syntax;
                    BaseInfo.gI().port10 = port + "";
                } else if (type == 7) {
                    BaseInfo.gI().syntax15 = syntax;
                    BaseInfo.gI().port15 = port + "";
                } else {
                    if (i == 0) {
                        BaseInfo.gI().syntax10 = syntax;
                        BaseInfo.gI().port10 = port + "";
                    } else {
                        BaseInfo.gI().syntax15 = syntax;
                        BaseInfo.gI().port15 = port + "";
                    }
                }
            }

        } catch (Exception e) {
            // TODO: handle exception
            Debug.LogException(e);
        }
    }

    public void onSetNewMaster(string nick) {
        gameControl.currentCasino.setMaster(nick);
    }

    public void onNickCuoc(long moneyInPot, long soTienTo, long moneyBoRa, string nick, Message message) {
        gameControl.currentCasino.onNickCuoc(moneyInPot, soTienTo,
                  moneyBoRa, nick, message);
    }

    public void onNickTheo(long money, string nick, Message message) {
        gameControl.currentCasino.onHaveNickTheo(money, nick, message);
    }

    public void onNickSkip(string nick, string turnName) {
        gameControl.currentCasino.onNickSkip(nick, turnName);
    }

    public void onNickSkip(string nick, Message msg) {
        gameControl.currentCasino.onNickSkip(nick, msg);
    }

    public void onUpdateMoneyMessage(string readstring, int type, long readInt) {
        // Debug.Log ("== " + readstring + " Type: " + type + " Money: " + readInt);
        if (readstring.Equals(BaseInfo.gI().mainInfo.nick)) {
            if (type == 0) {
                BaseInfo.gI().mainInfo.moneyXu = readInt;
            } else {
                BaseInfo.gI().mainInfo.moneyChip = readInt;
            }
            if (gameControl.currentCasino != null) {
                gameControl.currentCasino.players[0].setMoney(readInt);
            }

        }
    }

    //public void onUpdateVersion(Message message)
    //{
    //    try
    //    {
    //        NetworkUtil.GI().close();
    //        gameControl.setStage(gameControl.login);

    //        sbyte updateType = message.reader().ReadByte();
    //        string linkUpdate = message.reader().ReadUTF();
    //        string mess = message.reader().ReadUTF();
    //        gameControl.dialogWaiting.onHide();
    //        gameControl.dialogNotification.onShow(mess, delegate { Application.OpenURL(linkUpdate); });
    //    }
    //    catch (Exception e)
    //    {
    //        // TODO: handle exception
    //    }
    //}





    public void onInfoPockerTbale(Message message) {
        gameControl.currentCasino.onInfo(message);
    }

    public void onAddCardTbl(Message message) {
        gameControl.currentCasino.onAddCardTbl(message);
    }

    public void onChangeRuleTbl(sbyte readByte) {
        gameControl.currentCasino.setLuatChoi(readByte);
    }

    public void onUpdateMoneyTbl(Message message) {
        gameControl.currentCasino.onUpdateMoneyTbl(message);
    }

    public void onUpdateRoom(int readShort, Message message) {
        gameControl.listTable.Clear();
        List<TableItem> temp = new List<TableItem>();
        for (int i = 0; i < readShort; i++) {
            try {
                TableItem ctb = new TableItem();
                ctb.id = (message.reader().ReadShort());
                ctb.status = (message.reader().ReadByte());
                ctb.nUser = (message.reader().ReadByte());
                sbyte ilock = message.reader().ReadByte();
                ctb.Lock = ilock;
                ctb.money = message.reader().ReadLong();
                ctb.needMoney = message.reader().ReadLong();
                ctb.maxMoney = message.reader().ReadLong();
                ctb.maxUser = (message.reader().ReadByte());
                ctb.typeTable = message.reader().ReadInt();
                ctb.choinhanh = message.reader().ReadInt();
                if (gameControl.room.anbanfull && ctb.nUser == ctb.maxUser) {
                    continue;
                }
                gameControl.listTable.Add(ctb);

            } catch (IOException ex) {
                Debug.LogException(ex);
            }
        }

        try {
            int totalTBC = message.reader().ReadUnsignedShort();
            for (int i = 0; i < totalTBC; i++) {
                TableItem ctb = new TableItem();
                ctb.id = (int)(message.reader().ReadShort());
                ctb.status = (message.reader().ReadByte());
                ctb.nUser = (message.reader().ReadByte());
                sbyte ilock = message.reader().ReadByte();
                ctb.Lock = ilock;
                ctb.money = message.reader().ReadLong();
                ctb.needMoney = message.reader().ReadLong();
                ctb.maxMoney = message.reader().ReadLong();
                ctb.maxUser = (message.reader().ReadByte());
                ctb.typeTable = message.reader().ReadInt();
                ctb.choinhanh = message.reader().ReadInt();
                if (gameControl.room.anbanfull && ctb.nUser == ctb.maxUser) {
                    continue;
                }
                temp.Add(ctb);

            }
        } catch (IOException ex) {
            Debug.LogException(ex);
        }
        int dem5 = 0, dem9 = 0;
        int MAX = 2;
        //if(gameControl.gameID == GameID.TLMN || gameControl.gameID == GameID.PHOM
        //    || gameControl.gameID == GameID.XITO
        //    || gameControl.gameID == GameID.MAUBINH) {
        //    MAX = 1;
        //}
        long muccuoc;
        muccuoc = gameControl.listTable[0].money;
        for (int i = 0; i < gameControl.listTable.Count; i++) {
            try {
                if (gameControl.listTable[i].nUser != 0) {
                    temp.Add(gameControl.listTable[i]);
                    if (gameControl.listTable[i].money != muccuoc) {
                        dem5 = 0;
                        dem9 = 0;
                    }
                    muccuoc = gameControl.listTable[i].money;
                    continue;
                } else {
                    if (gameControl.listTable[i].money == muccuoc
                        && (gameControl.listTable[i].maxUser < 9)) {
                        dem5++;
                        if (dem5 <= MAX) {
                            temp.Add(gameControl.listTable[i]);
                            muccuoc = gameControl.listTable[i].money;
                            continue;
                        }

                    } else {
                    }
                    if (gameControl.listTable[i].money == muccuoc
                        && (gameControl.listTable[i].maxUser == 9)) {
                        dem9++;
                        if (dem9 <= MAX) {
                            temp.Add(gameControl.listTable[i]);
                            muccuoc = gameControl.listTable[i].money;
                            continue;
                        }
                    } else {
                    }
                }
                if (gameControl.listTable[i].money != muccuoc) {
                    dem5 = 0;
                    dem9 = 0;
                    if ((gameControl.listTable[i].maxUser < 9)) {
                        dem5++;
                        if (dem5 <= MAX) {
                            temp.Add(gameControl.listTable[i]);
                            muccuoc = gameControl.listTable[i].money;
                            continue;
                        }

                    } else {
                    }
                    if ((gameControl.listTable[i].maxUser == 9)) {
                        dem9++;
                        if (dem9 <= MAX) {
                            temp.Add(gameControl.listTable[i]);
                            muccuoc = gameControl.listTable[i].money;
                            continue;
                        }
                    } else {
                    }

                }
                muccuoc = gameControl.listTable[i].money;
                if (RoomControl.roomType == 1) {
                    BaseInfo.gI().moneyName = " " + Res.MONEY_FREE;
                } else {
                    BaseInfo.gI().moneyName = " " + Res.MONEY_VIP;
                }
            } catch (Exception e) {
                // TODO: handle exception��
                continue;
            }

        }
        this.gameControl.listTable.Clear();
        this.gameControl.listTable.AddRange(temp);

        switch (BaseInfo.gI().type_sort) {
            case 1:// ban cuonc
                if (BaseInfo.gI().sort_giam_dan_bancuoc) {
                    gameControl.listTable.Sort(delegate (TableItem tb1, TableItem tb2) {
                        return tb1.id.CompareTo(tb2.id);
                    });
                } else {
                    gameControl.listTable.Sort(delegate (TableItem tb1, TableItem tb2) {
                        return tb2.id.CompareTo(tb1.id);
                    });
                }
                break;
            case 2:// muc cuoc
                if (BaseInfo.gI().sort_giam_dan_muccuoc) {
                    //Collections.sort (this.listTable, sort_giam_dan_muccuoc);
                    gameControl.listTable.Sort(delegate (TableItem tb1, TableItem tb2) {
                        return tb1.money.CompareTo(tb2.money);
                    });
                } else {
                    //Collections.sort (this.listTable, sort_tang_dan_muccuoc);
                    gameControl.listTable.Sort(delegate (TableItem tb1, TableItem tb2) {
                        return tb2.money.CompareTo(tb1.money);
                    });
                }
                break;
            case 3:// nguoi choi
                if (BaseInfo.gI().sort_giam_dan_nguoichoi) {
                    //Collections.sort (this.listTable, sort_giam_dan_songuoi);
                    gameControl.listTable.Sort(delegate (TableItem tb1, TableItem tb2) {
                        return tb1.nUser.CompareTo(tb2.nUser);
                    });
                } else {
                    //Collections.sort (this.listTable, sort_tang_dan_songuoi);
                    gameControl.listTable.Sort(delegate (TableItem tb1, TableItem tb2) {
                        return tb2.nUser.CompareTo(tb1.nUser);
                    });
                }
                break;
            default:
                break;
        }

        gameControl.panelWaiting.onHide();

        //gameControl.setStage(gameControl.room);
        gameControl.room.createScollPane(gameControl.listTable, RoomControl.roomType);
    }

    //public void onGetPhoneCSKH (Message message) {

    //}

    public void onGetAlertLink(Message message) {
        try {
            sbyte type = message.reader().ReadByte();
            if (type == 0) {
                //int nMail = message.reader().ReadInt();

                //AlertMess al = new AlertMess();
                //al.link = "";
                //al.mess = " Bạn có" + nMail + " tin nhắn";
                //BaseInfo.gI().msgAlert.Add(al);
                //BaseInfo.gI().soTinNhan = nMail;

            } else if (type == 1) {
                string title = message.reader().ReadUTF();
                string link = message.reader().ReadUTF();
                AlertMess al = new AlertMess();
                al.mess = title;
                al.link = link;
                BaseInfo.gI().msgAlert.Add(al);
                if (BaseInfo.gI().msgAlert[0].mess.Contains("\n"))
                    BaseInfo.gI().msgAlert[0].mess = BaseInfo.gI().msgAlert[0].mess.Replace("\n", " ");
                //TODO: bo
                //gameControl.menu.lb_textnoti.text = BaseInfo.gI ().msgAlert[0].mess;
                //gameControl.main.lb_textnoti.text = BaseInfo.gI ().msgAlert[0].mess;
            }
        } catch (IOException e) {
            // TODO Auto-generated catch block
            Debug.LogException(e);
        }
    }

    //public void onGetBoxGift(Message message)
    //{

    //}

    public void infoWinPlayer(Message message) {
        try {
            int len = message.reader().ReadByte();
            List<InfoWinTo> info = new List<InfoWinTo>();
            for (int i = 0; i < len; i++) {
                InfoWinTo inf = new InfoWinTo();
                inf.name = message.reader().ReadUTF();
                inf.rank = i + 1;
                inf.money = message.reader().ReadLong();
                inf.type = message.reader().ReadByte();
                inf.typeCard = message.reader().ReadByte();
                sbyte len2 = message.reader().ReadByte();
                inf.arrCard = new sbyte[len2];
                for (int j = 0; j < len2; j++) {
                    inf.arrCard[j] = message.reader().ReadByte();
                }
                if (inf.money > 0) {
                    info.Add(inf);
                    int l = info.Count - 1;
                    if (info[l].arrCard.Length > 0) {
                        if (l > 0) {
                            int k = inf.arrCard.Length - 5;
                            bool isSame = true;
                            for (int j = k; j < inf.arrCard.Length; j++) {
                                if (info[l].arrCard[j] % 13 != info
                                        [l - 1].arrCard[j] % 13) {
                                    isSame = false;
                                    break;
                                }
                            }
                            if (isSame) {
                                info[l].rank = info[l - 1].rank;
                            }
                        }
                    }
                }

            }

            gameControl.currentCasino.onInfoWinPlayer(info);
        } catch (IOException e) {
            // TODO Auto-generated catch block
            Debug.LogException(e);
        }
    }

    public void InfoCardPlayerInTbl(Message message) {
        gameControl.currentCasino.InfoCardPlayerInTbl(message);
    }

    public void onChangeBetMoney(Message message) {
        try {
            long betMoney = message.reader().ReadLong();
            String info = message.reader().ReadUTF();
            BaseInfo.gI().betMoney = betMoney;
            //if (betMoney > 0)
            //    currentScreen.setTableName((BaseInfo.gI().typeTable == Res.TABLE_CHIP ? "Phòng FREE" : "Phòng VIP")
            //            + " -Bàn: " + BaseInfo.gI().idTable + " - Mức cược "
            //            + (BaseInfo.formatMoney(BaseInfo.gI().betMoney))
            //            + (BaseInfo.gI().typeTable == Res.TABLE_KIM_CUONG ? " XU" : " CHIP"));
            //dialog_thongbao2.onShow(currentScreen.stageDialog, info, new ChildScrListener() {

            //    @Override
            //    public void onChildScrDismiss() {

            //    }
            //});
            BaseInfo.gI().moneyTable = betMoney;

            if (betMoney > 0) {
                gameControl.currentCasino.changBetMoney(betMoney, info);
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }
    public void onGetMoney() {
        if (((BaseToCasino)gameControl.currentCasino) != null && ((BaseToCasino)gameControl.currentCasino).btn_ruttien != null) {
            ((BaseToCasino)gameControl.currentCasino).btn_ruttien
                    .gameObject.SetActive(true);
        }
        long moneyPlayer = 0;
        if (RoomControl.roomType == 1) {
            moneyPlayer = BaseInfo.gI().mainInfo.moneyChip;
        } else {
            moneyPlayer = BaseInfo.gI().mainInfo.moneyXu;
        }
        if (BaseInfo.gI().tuDongRutTien) {
            // SendData.onSendGetMoney(-1);
            if (moneyPlayer < BaseInfo.gI().soTienRut) {
                SendData.onSendGetMoney(moneyPlayer);
            } else {
                SendData.onSendGetMoney(BaseInfo.gI().soTienRut);
            }
            if (((BaseToCasino)gameControl.currentCasino).btn_ruttien != null) {
                ((BaseToCasino)gameControl.currentCasino).btn_ruttien
                        .gameObject.SetActive(false);
            }
        } else {
            if (moneyPlayer < BaseInfo.gI().moneyNeedTable) {
                gameControl.panelYesNo.onShow(
                        "Không đủ tiền để rút, bạn có muốn nạp thêm?",
                    delegate {
                        // show dialog nap tien
                    });
            } else {
                gameControl.panelYesNo.onShow(
                        "Không đủ tiền, bạn có muốn lấy thêm "
                                + " để tiếp tục chơi?", delegate {
                                    gameControl.panelRutTien.show(
                                                        BaseInfo.gI().currentMinMoney,
                                                        BaseInfo.gI().currentMaxMoney, 2, 0, 0,
                                                        0, RoomControl.roomType);
                                });
            }

        }

    }

    public void onTimeAuToStart(Message message) {
        try {
            gameControl.currentCasino.onTimeAuToStart(message.reader()
                     .ReadByte());
        } catch (Exception e) {
            // TODO: handle exception
        }
    }

    public void startFlip(Message message) {
        try {
            gameControl.currentCasino.startFlip(message.reader()
                    .ReadByte());
        } catch (Exception e) {
            // TODO: handle exception
        }
    }

    public void onCardFlip(Message message) {
        try {
            gameControl.currentCasino.onCardFlip(message.reader()
                    .ReadByte());
        } catch (Exception e) {
            // TODO Auto-generated catch block

        }
    }

    //public void onUpdateVersionNew(string link)
    //{
    //    gameControl.dialogWaiting.onHide();
    //    gameControl.dialogNotification.onShow("Cập nhập phiên bản?", delegate { Application.OpenURL(link); });
    //}

    //public void onIntroduceFriend(string mess, string link)
    //{
    //    gameControl.dialogWaiting.onHide();
    //    gameControl.dialogNotification.onShow("Nhập số điện thoại, bạn có đồng ý không?", delegate { GameControl.sendSMS(link, mess); });
    //}

    public void onRankMauBinh(Message message) {
        gameControl.currentCasino.onRankMauBinh(message);
    }

    public void onFinalMauBinh(string name) {
        gameControl.currentCasino.onFinalMauBinh(name);
    }

    public void onWinMauBinh(string name, sbyte typeCard) {
        gameControl.currentCasino.onWinMauBinh(name, typeCard);
    }

    public void onInfoMe(Message message) {
        gameControl.currentCasino.onInfome(message);
    }

    public void onBeginRiseBacay(Message message) {
        gameControl.currentCasino.onBeginRiseBacay(message);
    }

    public void onFlip3Cay(Message message) {

    }

    public void onCuoc3Cay(Message message) {
        gameControl.currentCasino.onCuoc3Cay(message);
    }

    public void onBaoXam(Message message) {
        try {
            sbyte type = message.reader().ReadByte();
            if (type == 0) {
                sbyte time = message.reader().ReadByte();
                gameControl.currentCasino.onHoiBaoXam(time);
            } else if (type == 1) {
                String name = message.reader().ReadUTF();
                gameControl.currentCasino.onNickBaoXam(name);
            }
        } catch (Exception e) {
            // TODO: handle exception
        }
    }

    public void onFinishTurn() {
        gameControl.currentCasino.onFinishTurn();
    }

    //public void onNewEvent(Message message)
    //{
    //    try {
    //        sbyte size = message.reader().ReadByte();
    //        BaseInfo.gI().listEvent.Clear();
    //        for (int i = 0; i < size; i++) {
    //            VuaBaiEvent events = new VuaBaiEvent();
    //            events.title = message.reader().ReadUTF();
    //            events.content = message.reader().ReadUTF();

    //            events.link = message.reader().ReadUTF();
    //            events.dateOpen = message.reader().ReadUTF();
    //            events.dateEnd = message.reader().ReadUTF();
    //            BaseInfo.gI().listEvent.Add(events);
    //        }
    //        if (BaseInfo.gI().listEvent.Count > 0) {
    //            gameControl.dialogEvent.onShow();
    //        }

    //    } catch (Exception e) {
    //        // TODO: handle exception
    //    }
    //}

    //public void onInfoSMSAppStore(Message message)
    //{
    //    try
    //    {
    //        BaseInfo.gI().isPurchase = true;
    //        if (BaseInfo.gI().isPurchase)
    //        {
    //            gameControl.menu.buttonDoiThuong.SetActive(false);
    //        }
    //        else
    //        {
    //            gameControl.menu.buttonDoiThuong.SetActive(true);
    //        }
    //        //sbyte size = message.reader().ReadByte();
    //        //if (size == 0)
    //        //{
    //        //    BaseInfo.gI().isPurchase = false;
    //        //} else{
    //        //    BaseInfo.gI().isPurchase = true;
    //        //}
    //        //for (int i = 0; i < size; i++ )
    //        //{
    //        //    string key = message.reader().ReadUTF();
    //        //    string value = message.reader().ReadUTF();
    //        //}
    //    } catch (Exception ex){

    //    }
    //}
    public void onBuyItem(Message message) {
        try {
            sbyte type = message.reader().ReadByte();
            if (type == 0) {
                int id = message.reader().ReadInt();
                string nick_nem = message.reader().ReadUTF();
                string nick_bi_nem = message.reader().ReadUTF();
                gameControl.currentCasino.actionNem(id, nick_nem, nick_bi_nem);
            }

        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void InfoGift(Message message) {
        try {
            if (gameControl.panelDoiThuong.tblContaintGiftTheCao.transform.childCount == 0
                || gameControl.panelDoiThuong.tblContaintGiftVatPham.transform.childCount == 0) {
                int size = message.reader().ReadInt();
                for (int i = 0; i < size; i++) {
                    int id = message.reader().ReadInt();
                    int type = message.reader().ReadInt();
                    // type 1: the cao
                    // type 2: vat pham
                    string name = message.reader().ReadUTF();
                    long cost = message.reader().ReadLong();
                    string telco = message.reader().ReadUTF();
                    long price = message.reader().ReadLong();
                    long balance = message.reader().ReadLong();
                    string des = message.reader().ReadUTF();
                    string links = message.reader().ReadUTF();

                    gameControl.panelDoiThuong.addGiftInfo(id, type, name, price, links);
                }
            }
            //        BaseInfo.gI().soDu = message.reader().ReadInt();
            gameControl.panelDoiThuong.onShow();
            gameControl.panelWaiting.onHide();
        } catch (Exception e) {
            // TODO: handle exception
            Debug.LogException(e);
        }
    }

    public void onListInvite(Message msg) {
        gameControl.panelWaiting.onHide();
        try {
            short total = msg.reader().ReadShort();
            if (total <= 0) {
                gameControl.panelThongBao.onShow("Tất cả người chơi đều đang bận!", delegate { });
            } else {
                gameControl.PanelMoiChoi.ClearParent();
                for (int i = 0; i < total; i++) {
                    string name = msg.reader().ReadUTF();
                    string displayname = msg.reader().ReadUTF();
                    long money = msg.reader().ReadLong();
                    gameControl.PanelMoiChoi.addIcon(name, displayname, money);
                }
                gameControl.PanelMoiChoi.onShow();
            }
        } catch (Exception e) {
            // TODO: handle exception
            Debug.LogException(e);
        }

    }


    public void onJoinView(Message message) {
        if (BaseInfo.gI().numberPlayer == 9) {
            gameControl.setCasino(gameControl.gameID, 1);
        } else {
            gameControl.setCasino(gameControl.gameID, 0);
        }
        gameControl.currentCasino.onJoinView(message);
    }

    public void onUpdataAvata(Message message) {
        try {
            sbyte type = message.reader().ReadByte();
            string info = message.reader().ReadUTF();

            gameControl.panelInfoPlayer.infoMe();
            gameControl.menu.updateAvataName();
            gameControl.room.updateAvataName();
            //gameControl.main.updateAvataName ();
            //gameControl.panelDangKy.avata.spriteName = BaseInfo.gI().mainInfo.idAvata + "";
            gameControl.panelThongBao.onShow(info, delegate { });

        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void onChangeName(Message message) {
        try {
            sbyte type = message.reader().ReadByte();
            string info = message.reader().ReadUTF();
            if (type == 1) {
                string name = message.reader().ReadUTF();

                BaseInfo.gI().mainInfo.displayname = name;
                gameControl.panelInfoPlayer.infoMe();
                gameControl.menu.updateAvataName();
                gameControl.room.updateAvataName();
                //gameControl.main.updateAvataName ();
            }
            gameControl.panelThongBao.onShow(info, delegate { });
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void onPopupNotify(Message message) {
        try {
            if (gameControl.panelNotiDoiThuong.tgParent.transform.childCount == 0) {
                int size = message.reader().ReadInt();
                for (int i = 0; i < size; i++) {
                    int id = message.reader().ReadInt();
                    int type = message.reader().ReadInt();
                    string title = message.reader().ReadUTF();
                    string content = message.reader().ReadUTF();
                    //Debug.Log("ID: " + id + " Type: " + type + " Title: " + title + " Content: " + content);

                    gameControl.panelNotiDoiThuong.setActiveTab(title, content);
                }
            }
            //gameControl.panelNotiDoiThuong.onShow();
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void onCreateTable(Message message) {
        try {
            sbyte type = message.reader().ReadByte();
            if (type == 1) {
                gameControl.panelThongBao.onShow("Tạo bàn thành công", delegate { });
                gameControl.panelCreateRoom.onHide();
            } else {
                gameControl.panelThongBao.onShow("Tạo bàn thất bại", delegate { });
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void onListBetMoney(Message message) {
        try {
            BaseInfo.gI().listBetMoneysVIP.Clear();
            BaseInfo.gI().listBetMoneysFREE.Clear();

            int size = message.reader().ReadInt();
            for (int i = 0; i < size; i++) {
                BetMoney betM = new BetMoney();
                betM.typeMoney = message.reader().ReadInt();
                betM.maxMoney = message.reader().ReadLong();
                string listBet = message.reader().ReadUTF();
                if (listBet.Length > 0) {
                    betM.setListBet(listBet);
                    if (betM.typeMoney == 0) {
                        BaseInfo.gI().listBetMoneysVIP.Add(betM);
                    } else if (betM.typeMoney == 1) {
                        BaseInfo.gI().listBetMoneysFREE.Add(betM);
                    }
                }
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void onRateScratchCard(Message message) {
        if (BaseInfo.gI().list_tygia != null)
            BaseInfo.gI().list_tygia.Clear();
        try {
            int size = message.reader().ReadInt();
            for (int i = 0; i < size; i++) {
                int menhgia = message.reader().ReadInt();
                int xu = message.reader().ReadInt();
                TyGia tygia = new TyGia(menhgia, xu);
                BaseInfo.gI().list_tygia.Add(tygia);
            }
            BaseInfo.gI().sms10 = message.reader().ReadInt();
            BaseInfo.gI().sms15 = message.reader().ReadInt();
            BaseInfo.gI().tyle_xu_sang_chip = message.reader().ReadInt();
            BaseInfo.gI().tyle_chip_sang_xu = message.reader().ReadInt();

        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void onXuToNick(Message message) {
        try {
            sbyte type = message.reader().ReadByte();
            if (type == 1) {
                long xu = message.reader().ReadLong();
                BaseInfo.gI().mainInfo.moneyXu += xu;
            }
            string info = message.reader().ReadUTF();
            gameControl.panelThongBao.onShow(info, delegate { });
        } catch (Exception e) {
            Debug.LogException(e);
            gameControl.panelThongBao.onShow("Vui lòng nhập đúng UserID cần chuyển đến, xem UserID trong phần thông tin cá nhân!", delegate { });
        }
    }

    public void onXuToChip(Message message) {
        try {
            sbyte type = message.reader().ReadByte();
            if (type == 1) {
                long xu = message.reader().ReadLong();
                long chip = message.reader().ReadLong();
                BaseInfo.gI().mainInfo.moneyXu -= xu;
                BaseInfo.gI().mainInfo.moneyChip += chip;
            }
            string info = message.reader().ReadUTF();
            gameControl.panelThongBao.onShow(info, delegate { });
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void onChipToXu(Message message) {
        try {
            sbyte type = message.reader().ReadByte();
            if (type == 1) {
                long xu = message.reader().ReadLong();
                long chip = message.reader().ReadLong();
                BaseInfo.gI().mainInfo.moneyXu += xu;
                BaseInfo.gI().mainInfo.moneyChip -= chip;
            }
            string info = message.reader().ReadUTF();
            gameControl.panelThongBao.onShow(info, delegate { });
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void onInboxMessage(Message message) {
        try {
            sbyte total = message.reader().ReadByte();
            for (int i = 0; i < total; i++) {
                int id = message.reader().ReadInt();
                string guitu = message.reader().ReadUTF();
                string guiLuc = message.reader().ReadUTF();
                string noiDung = message.reader().ReadUTF();
                sbyte isread = message.reader().ReadByte();
                gameControl.panelMail.addIconTinNhan(id, guitu, guiLuc, noiDung, isread);
            }
            gameControl.panelWaiting.onHide();
            //gameControl.panelMail.onShow();

        } catch (Exception e) {
            // TODO: handle exception
            Debug.LogException(e);
        }
    }

    public void onDelMessage(Message message) {

    }


    public void onListEvent(Message message) {
        try {
            gameControl.panelMail.clearSuKien();
            int total = message.reader().ReadInt();

            string strEvent = "";
            for (int i = 0; i < total; i++) {
                int id = message.reader().ReadInt();
                string title = message.reader().ReadUTF();
                string content = message.reader().ReadUTF();
                if (content.Length > 0) {
                    gameControl.panelMail.addIconSuKien(id, title, content);
                }

                if (strEvent.Length > 0) {
                    strEvent = strEvent + ";                                                " + title;
                } else {
                    strEvent = title;
                }
            }

            //TODO: fixed notification.
            gameControl.menu.lb_textnoti.text = strEvent;
            gameControl.room.lb_textnoti.text = strEvent;
            gameControl.panelWaiting.onHide();

        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void onReadMessage(Message message) {
        if (BaseInfo.gI().soTinNhan > 0)
            BaseInfo.gI().soTinNhan--;
    }

    public void onGetPass(Message message) {
        try {
            string sms = message.reader().ReadUTF();
            string ds = message.reader().ReadUTF();
#if UNITY_EDITOR
            gameControl.panelThongBao.onShow("Soạn tin theo cú pháp " + sms + " gửi đến " + ds, delegate { });
#else
            gameControl.panelYesNo.onShow("Chương trình sẽ gửi tin nhắn để đổi mật khẩu (phí 1000đ), bạn có đồng ý không?",
                                          delegate { GameControl.sendSMS(ds, sms); });
#endif
            gameControl.panelWaiting.onHide();

        } catch (Exception ex) {
            Debug.LogException(ex);
        } finally {
            SendData.getPass = false;
        }
    }

    public void onPhoneCSKH(Message message) {
        try {
            BaseInfo.gI().cskh = message.reader().ReadUTF();
            PlayerPrefs.SetString("CSKH", BaseInfo.gI().cskh);
            PlayerPrefs.Save();
            gameControl.login.setCSKH();
            BaseInfo.gI().linkdownload = message.reader().ReadUTF();
            BaseInfo.gI().current_version = message.reader().ReadInt();
        } catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    public void onReceiveFreeMoney(Message message) {
        //System.out.println("COng tien");
        try {
            sbyte x = message.reader().ReadByte();
            if (x == 1) {
                long tiencong = message.reader().ReadLong();
                BaseInfo.gI().mainInfo.moneyChip += tiencong;
                gameControl.panelThongBao.onShow("Bạn đã được cộng " + tiencong + " tiền free", delegate { });
            } else {
                gameControl.panelThongBao.onShow("Bạn không đủ điều kiện nhận tiền free", delegate { });
            }
        } catch (IOException e) {
            //e.printStackTrace();
            Debug.LogException(e);
        }
    }

    public void onLoginfirst(Message message) {
        sbyte type = message.reader().ReadByte();
        if (type == 0) {
            // tao nv
            //gameControl.panelDangKy.avata.spriteName = BaseInfo.gI ().mainInfo.idAvata + "";
            //gameControl.panelDangKy.tg_sex.value = false;
            //if(BaseInfo.gI ().mainInfo.gender == 1)
            //    gameControl.panelDangKy.tg_sex.value = true;
            //gameControl.panelDangKy.onShow ();
            gameControl.panelInput.onShow("THÔNG BÁO", "Nhập số điện thoại để hoàn tất việc đăng ký.\nLưu ý : Nhập chính xác sđt để dùng khi đổi mật khẩu.", "SĐT: ", delegate {

            });
        } else if (type == -1) {
            // khong thanh cong
            gameControl.panelThongBao.onShow("Không thành công", delegate { });
        } else if (type == 1) {
            // thanh cong
            if (gameControl.panelDangKy.isShow) {
                gameControl.panelDangKy.onHide();
            }
            BaseInfo.gI().mainInfo.idAvata = message.reader().ReadInt();
            BaseInfo.gI().mainInfo.displayname = message.reader().ReadUTF();
            BaseInfo.gI().mainInfo.gender = message.reader().ReadByte();
            BaseInfo.gI().mainInfo.phoneNumber = message.reader().ReadUTF();

            gameControl.menu.updateAvataName();
            gameControl.room.updateAvataName();
        }
    }

    public void onInfoNhiemvu(Message message) {
        try {
            gameControl.panelNhiemVu.clearParent();
            sbyte size = message.reader().ReadByte();
            for (int i = 0; i < size; i++) {
                int id = message.reader().ReadByte();
                string des = message.reader().ReadUTF();
                int xuBonus = message.reader().ReadInt();
                string giftcode = message.reader().ReadUTF();
                bool check = message.reader().ReadBoolean();

                gameControl.panelNhiemVu.addItem(id, des, xuBonus, giftcode, check);
            }

        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void onUpdateNhiemvu(Message message) {
        try {
            sbyte id = message.reader().ReadByte();
            string tt = message.reader().ReadUTF();
            bool check = message.reader().ReadBoolean();
            gameControl.panelNhiemVu.updateItem(id, tt, check);
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void onTop(Message message) {
        try {
            sbyte gameid = message.reader().ReadByte();
            //gameControl.main.clearGrid (gameid);
            int size = message.reader().ReadByte();
            for (int i = 0; i < size; i++) {
                string displayname = message.reader().ReadUTF();
                string linkAvatar = message.reader().ReadUTF();
                int idAvata = message.reader().ReadInt();
                int win = message.reader().ReadInt();
                //gameControl.main.addItemXepHang (gameid, displayname, linkAvatar, idAvata, win, i);
            }
            //gameControl.main.setActiveXH (gameid);
            // gameControl.main.isEnableButton (true);

        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    public void onSMS9029(Message message) {
        try {
            sbyte size = message.reader().ReadByte();
            if (size > 0) {
                BaseInfo.gI().is9029 = true;
            } else {
                BaseInfo.gI().is9029 = false;
            }

            sbyte telco = message.reader().ReadByte();
            sbyte size2 = message.reader().ReadByte();
            switch (telco) {
                case 1:// viettel
                    if (gameControl.panelNapChuyenXu.list_viettel.Count <= 0) {
                        for (int j = 0; j < size2; j++) {
                            Item9029 sms = new Item9029();
                            sms.name = message.reader().ReadUTF();
                            sms.sys = message.reader().ReadUTF();
                            sms.port = message.reader().ReadShort();
                            sms.money = message.reader().ReadLong();
                            gameControl.panelNapChuyenXu.list_viettel.Add(sms);
                        }
                        gameControl.panelNapChuyenXu.initPanelViettel();
                    }
                    break;
                case 2:// vina
                    if (gameControl.panelNapChuyenXu.list_vina.Count <= 0) {
                        for (int j = 0; j < size2; j++) {
                            Item9029 sms = new Item9029();
                            sms.name = message.reader().ReadUTF();
                            sms.sys = message.reader().ReadUTF();
                            sms.port = message.reader().ReadShort();
                            sms.money = message.reader().ReadLong();
                            gameControl.panelNapChuyenXu.list_vina.Add(sms);
                        }
                        gameControl.panelNapChuyenXu.initPanelVina();
                    }
                    break;
                case 3:// mobi
                    if (gameControl.panelNapChuyenXu.list_mobi.Count <= 0) {
                        for (int j = 0; j < size2; j++) {
                            Item9029 sms = new Item9029();
                            sms.name = message.reader().ReadUTF();
                            sms.sys = message.reader().ReadUTF();
                            sms.port = message.reader().ReadShort();
                            sms.money = message.reader().ReadLong();
                            gameControl.panelNapChuyenXu.list_mobi.Add(sms);
                        }
                        gameControl.panelNapChuyenXu.initPanelMobi();
                    }
                    break;
            }
        } catch (IOException e) {
            Debug.LogException(e);
        }
    }

    public void onCardXepMB(Message message) {
        try {
            int size = message.reader().ReadInt();
            byte[] cards = new byte[size];
            int[] cardss = new int[size];
            message.reader().Read(cards, 0, size);
            for (int i = 0; i < size; i++) {
                cardss[i] = cards[i];
            }
            //mainGame.mainScreen.curentCasino.onCardXepMB (cardss);

        } catch (IOException e) {
            Debug.LogException(e);
        }
    }

    public void onPhomha(Message message) {
        //mainGame.mainScreen.curentCasino.onPhomha (message);
        gameControl.currentCasino.onPhomha(message);
    }

    //Xoc dia
    public void onBeGinXocDia(Message message) {
        try {
            int time = message.reader().ReadByte();
            gameControl.currentCasino.onBeGinXocDia(time);
        } catch (IOException e) {
            Debug.LogException(e);
        }
    }

    public void onBeginXocDiaTimeDatcuoc(Message message) {
        try {
            int time = message.reader().ReadByte();
            gameControl.currentCasino.onBeginXocDiaTimeDatcuoc(time);
        } catch (IOException e) {
            Debug.LogException(e);
        }
    }

    public void onXocDiaMobat(Message message) {
        try {
            //Lay so luong quan do tu server gui ve.
            int numRed = message.reader().ReadByte();
            gameControl.currentCasino.onXocDiaMobat(numRed);
        } catch (IOException e) {
            Debug.LogException(e);
        }
    }

    public void onXocDiaDatcuoc(Message message) {
        try {
            string nick = message.reader().ReadUTF();
            sbyte cua = message.reader().ReadByte();
            long money = message.reader().ReadLong();
            sbyte typeCHIP = message.reader().ReadByte();
            gameControl.currentCasino.onXocDiaDatcuoc(nick, cua, money, typeCHIP);
        } catch (IOException e) {
            Debug.LogException(e);
        }
    }

    public void onXocDiaCacMucCuoc(Message message) {
        try {
            long muc1 = message.reader().ReadLong();
            long muc2 = message.reader().ReadLong();
            long muc3 = message.reader().ReadLong();
            long muc4 = message.reader().ReadLong();
            gameControl.currentCasino.onXocdiaNhanCacMucCuoc(muc1, muc2, muc3, muc4);
        } catch (IOException e) {
            Debug.LogException(e);
        }
    }

    public void onXocDiaDatlai(Message message) {
        try {
            string nick = message.reader().ReadUTF();
            sbyte socua = message.reader().ReadByte();
            gameControl.currentCasino.onXocDiaDatlai(nick, socua, message);
        } catch (IOException e) {
            Debug.LogException(e);
        }
    }

    public void onXocDiaDatGapdoi(Message message) {
        try {
            string nick = message.reader().ReadUTF();
            sbyte socua = message.reader().ReadByte();
            gameControl.currentCasino.onXocDiaDatGapdoi(nick, socua, message);
        } catch (IOException e) {
            Debug.LogException(e);
        }
    }

    public void onXocDiaHuycuoc(Message message) {
        try {
            string nick = message.reader().ReadUTF();
            long moneycua0 = message.reader().ReadLong();
            long moneycua1 = message.reader().ReadLong();
            long moneycua2 = message.reader().ReadLong();
            long moneycua3 = message.reader().ReadLong();
            long moneycua4 = message.reader().ReadLong();
            long moneycua5 = message.reader().ReadLong();
            gameControl.currentCasino.onXocDiaHuycuoc(nick, moneycua0, moneycua1, moneycua2, moneycua3, moneycua4, moneycua5);
        } catch (IOException e) {
            Debug.LogException(e);
        }
    }

    public void onXocDiaUpdateCua(Message message) {
        gameControl.currentCasino.onXocDiaUpdateCua(message);
    }

    public void onXocDiaHistory(Message message) {
        try {
            //Danh sach chua cac id history cua van choi xoc dia.
            //id = 1 : quan do
            //id = 0 : quan trang
            List<int> id = new List<int>();

            string a = message.reader().ReadUTF();
            string[] chuoi = Regex.Split(a, ",");
            if (chuoi.Length == 0) {

            } else {
                for (int i = 0; i < chuoi.Length; i++) {
                    if (chuoi[i] != "") {
                        int idQuan = System.Convert.ToInt32(chuoi[i]);
                        id.Add(idQuan);
                    }
                }

                gameControl.currentCasino.onXocDiaHistory(id);
            }

        } catch (IOException e) {
            Debug.LogException(e);
        }
    }

    public void onXocDiaChucNangHuycua(Message message) {
        gameControl.currentCasino.onXocDiaChucNangHuycua(message);
    }

    public void onXocDiaBeginTimerDungCuoc(Message message) {
        gameControl.currentCasino.onXocDiaBeginTimerDungCuoc(message);
    }
    //Xoc dia

    public void onListProduct(Message message) {
        //MainGame.listProductIDs.clear ();
        try {
            int size = message.reader().ReadInt();
            for (int i = 0; i < size; i++) {
                //ProductID productID = new ProductID ();
                string productid = message.reader().ReadUTF();
                int price = message.reader().ReadInt();
                int xu = message.reader().ReadInt();
                //MainGame.listProductIDs.add (productID);
            }
            //mainGame.ui.createBilling ();
            //mainGame.mainScreen.dialogNapTien.createIAP (mainGame,
            //     MainGame.listProductIDs);
        } catch (IOException e) {
            Debug.LogException(e);
        }
    }

    public void onListItem(Message message) {

        try {
            int size = message.reader().ReadInt();
            for (int i = 0; i < size; i++) {
                // ItemInfo item = new ItemInfo ();
                int id = message.reader().ReadInt();
                string name = message.reader().ReadUTF();
                int price = message.reader().ReadInt();
            }

            // int size2 = message.reader().readInt();
            // mainGame.listItems.clear();
            // for (int i = 0; i < size2; i++) {
            // ItemInfo item = new ItemInfo();
            // item.id = message.reader().readInt();
            // item.name = message.reader().readUTF();
            // item.price = message.reader().readInt();
            // item.type_money = message.reader().readInt();
            // mainGame.listItems.add(item);
            // }
        } catch (IOException e) {
            Debug.LogException(e);
        }
    }
    public void onLichSuGiaoDich(Message message) {
        try {
            List<LichSuGiaoDich> list = new List<LichSuGiaoDich>();
            int size = message.reader().ReadShort();
            for (int i = 0; i < size; i++) {
                LichSuGiaoDich lichSuGiaoDich = new LichSuGiaoDich();
                lichSuGiaoDich.stt = (i + 1);
                lichSuGiaoDich.tenvatpham = message.reader().ReadUTF();
                sbyte tt = message.reader().ReadByte();
                lichSuGiaoDich.thoigiangiaodich = message.reader().ReadUTF();
                switch (tt) {
                    case 1:
                        lichSuGiaoDich.trangthai = "Chờ duyệt";
                        break;
                    case 3:
                        lichSuGiaoDich.trangthai = "Đang xử lý";
                        break;
                    case 4:
                        lichSuGiaoDich.trangthai = "Thành công";
                        break;
                    case 5:
                        lichSuGiaoDich.trangthai = "Đã trả thưởng";
                        break;
                    case 99:
                        lichSuGiaoDich.trangthai = "Đã hủy";
                        break;
                    default:
                        break;
                }
                list.Add(lichSuGiaoDich);
            }
            gameControl.panelDoiThuong.createLSGD(list);
        } catch (IOException e) {
            Debug.LogException(e);
        }
    }
}