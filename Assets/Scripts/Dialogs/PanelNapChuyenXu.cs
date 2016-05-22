using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelNapChuyenXu : PanelGame {
    public PanelSMS9029 panelSMS;
    public UIToggle tgTheCao;
    public List<Item9029> list_viettel = new List<Item9029> ();
    public List<Item9029> list_vina = new List<Item9029> ();
    public List<Item9029> list_mobi = new List<Item9029> ();
    //public GameObject panelTheCao;
	public PanelTheCao panelTheCao;
    // Use this for initialization
    void Start () {
		//panelTheCao.onShow ();
    }


    public void clickTabDoiChip () {
        if(BaseInfo.gI ().mainInfo.moneyXu <= 0) {
            tgTheCao.value = true;
            GameControl.instance.panelThongBao.onShow ("Bạn không còn " + Res.MONEY_VIP + " để đổi!", delegate {
            });
        }
    }

    public void clickTabDoiXu () {
        if(BaseInfo.gI ().mainInfo.moneyChip <= 0) {
            tgTheCao.value = true;
            GameControl.instance.panelThongBao.onShow ("Bạn không còn " + Res.MONEY_FREE + " để đổi!", delegate {
            });
        }
    }

    public void clickTabChuyenXu () {
        if(BaseInfo.gI ().mainInfo.moneyXu <= 0) {
            tgTheCao.value = true;
            GameControl.instance.panelThongBao.onShow ("Bạn không còn " + Res.MONEY_VIP + " để chuyển!", delegate {
            });
        }
    }

    public void initPanelViettel () {
       // panelSMS.add9022 (list_viettel, 0);
    }
    public void initPanelVina () {
      //  panelSMS.add9022 (list_vina, 1);
    }
    public void initPanelMobi () {
      //  panelSMS.add9022 (list_mobi, 2);
    }
    /*
    public void clickSMS9029(){
        string net = UnityPluginForWindowPhone.Class1.getDeviceNetworkInformation ();
        switch(net) {
            //case "VIETTEL":
            //    BaseInfo.gI ().TELCO_CODE = 1;
            //    break;
            //case "VN VINAPHONE":
            //case "VINAPHONE":
            //case "VN VINAPHONE-VinaPhone":
            //case "VN VINAPHONE-VINAPHONE":
            //    BaseInfo.gI ().TELCO_CODE = 2;
            //    break;
            //case "MOBIFONE":
            //case "VN MOBIFONE":
            //case "VN MOBIFONE-MobiFone":
            //case "VN MOBIFONE-MOBIFONE":
            //    BaseInfo.gI ().TELCO_CODE = 3;
            //    break;
            //default:
            //    BaseInfo.gI ().TELCO_CODE = 0;
            //    break;

            case "VIETTEL":
                BaseInfo.gI ().TELCO_CODE = 1;
                break;
            case "VINAPHONE":
                BaseInfo.gI ().TELCO_CODE = 2;
                break;
            case "MOBIFONE":
                BaseInfo.gI ().TELCO_CODE = 3;
                break;
            default:
                BaseInfo.gI ().TELCO_CODE = 1;
                break;
        }
        Debug.Log (net + " ------=========== " + BaseInfo.gI ().TELCO_CODE);
        if(BaseInfo.gI ().TELCO_CODE != 0) {
            SendData.onSendSms9029 (BaseInfo.gI ().TELCO_CODE);
        }
    }*/
}
