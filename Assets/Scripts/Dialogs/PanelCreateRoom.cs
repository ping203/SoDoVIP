using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PanelCreateRoom : PanelGame {
	public UISlider sliderMoney;
    public UIInput inputMoney;
    public UIInput inputMaxPlayer;

	float rateVIP, rateFREE;

	// Use this for initialization
	void Start () {
		EventDelegate.Set (sliderMoney.onChange, onChangeMoney);
	}

	public void onChangeMoney(){
		bool isOk = false;
		float vl = sliderMoney.value;
		if (RoomControl.roomType == 1) {
			for (int j = 0; j < BaseInfo.gI().listBetMoneysFREE.Count; j++) {
				BetMoney b = BaseInfo.gI ().listBetMoneysFREE [j];
				if (BaseInfo.gI ().mainInfo.moneyXu < b.maxMoney) {
					rateFREE = (float)1 / b.listBet.Count;
					for (int i = 0; i < b.listBet.Count; i++) {
						if (vl <= i * rateFREE) {
							inputMoney.value = b.listBet [i] + "";
							isOk = true;
							break;
						}
					}
				}
				if(isOk) break;
			}
		} else {
			for (int j = 0; j < BaseInfo.gI().listBetMoneysVIP.Count; j++) {
				BetMoney b = BaseInfo.gI ().listBetMoneysVIP [j];
				if (BaseInfo.gI ().mainInfo.moneyXu < b.maxMoney) {
					rateVIP = (float)1 / b.listBet.Count;
					for (int i = 0; i < b.listBet.Count; i++) {
						if (vl <= i * rateVIP) {
							inputMoney.value = b.listBet [i] + "";
							isOk = true;
							break;
						}
					}
				}
				if(isOk) break;
			}
		}
	}

    public void createTableGame () {
        GameControl.instance.sound.startClickButtonAudio ();
        string strMoney = inputMoney.value;
        string strMaxPlayer = inputMaxPlayer.value;
		int gameid = GameControl.instance.gameID;
		int roomid = 0;
		if (strMoney == "" || strMaxPlayer == "") {
            GameControl.instance.panelThongBao.onShow ("Bạn chưa điền đủ thông tin!", delegate { });
			return;
		}
		if (!BaseInfo.gI ().checkNumber (strMoney) || !BaseInfo.gI ().checkNumber (strMaxPlayer)) {
            GameControl.instance.panelThongBao.onShow ("Nhập sai!", delegate { });
			return;
		}

		long money = long.Parse(strMoney);
		int maxplayer = int.Parse (strMaxPlayer);

		bool check = false;
		string info = "";
		switch (GameControl.instance.gameID) {
		case GameID.TLMN:
        case GameID.PHOM:
        case GameID.XAM:
		case GameID.MAUBINH:{
			if(maxplayer > 4 || maxplayer < 2){
				check = false;
				info = "Số người phải lớn hơn 2 và nhỏ hơn 4";
			}else{
				check = true;
			}
			break;
		}
		case GameID.POKER:
		case GameID.XITO:
		case GameID.LIENG:
		case GameID.BACAY:{
			if (maxplayer > 5 || maxplayer < 2) {
				check = false;
				info = "Số người phải lớn hơn 2 và nhỏ hơn 5";
			} else {
				check = true;
			}
			break;
		}
            case GameID.XOCDIA:
                if(maxplayer != 9) {
                    check = false;
                    info = "Số người phải bằng 9.";
                } else {
                    check = true;
                }
                break;
		}
		if (check) {
			if (RoomControl.roomType == 1) {//free
				if (10 * money > BaseInfo.gI ().mainInfo.moneyChip) {
                    GameControl.instance.panelThongBao.onShow ("Không đủ tiền để tạo bàn!", delegate { });
				} else {
					SendData.onCreateTable (gameid, 1, money, maxplayer, 0, "");
				}
			} else {
				if (10 * money > BaseInfo.gI ().mainInfo.moneyXu) {
                    GameControl.instance.panelThongBao.onShow ("Không đủ tiền để tạo bàn!", delegate { });
				} else {
					SendData.onCreateTable (gameid, 2, money, maxplayer, 0, "");
				}
			}
		} else {
            GameControl.instance.panelThongBao.onShow (info, delegate { });
		}
	}

	public void onShow(){
		sliderMoney.value = 0;
		onChangeMoney();
		base.onShow ();
	}
}
