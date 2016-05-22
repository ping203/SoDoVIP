using UnityEngine;
using System.Collections;

public class PanelDoiXuChip : PanelGame {
	public UIInput ip_doi;
	public UISlider slider;
	public UILabel lbNhan, tyle_xu_chip, tyle_chip_xu;

    int chip = 0, xu = 0;

	// Use this for initialization
	void Start () {
		if(tyle_xu_chip!=null)
		tyle_xu_chip.text = "(Tỷ lệ 1 " + Res.MONEY_VIP_UPPERCASE +" = " + BaseInfo.gI().tyle_xu_sang_chip + " " + Res.MONEY_FREE_UPPERCASE +")";
		if(tyle_chip_xu!=null)
            tyle_chip_xu.text = "(Tỷ lệ " + BaseInfo.gI ().tyle_chip_sang_xu + " " + Res.MONEY_FREE_UPPERCASE + " = 1 " + Res.MONEY_VIP_UPPERCASE + ")";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	//panel doi chip
	public void onChangeValueSliderXuToChip(){
		xu = (int)(BaseInfo.gI ().mainInfo.moneyXu * slider.value);
        ip_doi.value = xu + "";

        long c = xu * BaseInfo.gI ().tyle_xu_sang_chip;
        lbNhan.text = Res.MONEY_FREE_UPPERCASE + " = " + BaseInfo.formatMoneyDetailDot (c);
	}

	public void onChangeValueInputDoiChip(){
		if (!ip_doi.value.Trim ().Equals ("")) {
            int xuu = int.Parse (ip_doi.value);
            if(xuu > BaseInfo.gI().mainInfo.moneyXu){
                GameControl.instance.panelThongBao.onShow ("Số xu không đủ!", delegate{
                    ip_doi.isSelected = true;
                });
                ip_doi.isSelected = false;
                ip_doi.value = "0";
                slider.value = 0;
                lbNhan.text = Res.MONEY_FREE_UPPERCASE + " = 0";
                return;
            }

            //slider.value = (float) xuu / BaseInfo.gI ().mainInfo.moneyXu;
            long c = xuu * BaseInfo.gI ().tyle_xu_sang_chip;
            lbNhan.text = Res.MONEY_FREE_UPPERCASE + " = " + BaseInfo.formatMoneyDetailDot (c);

		} else {
            lbNhan.text = Res.MONEY_FREE_UPPERCASE + " = 0";
		}
	}

	//panel doi xu
	public void onChangeValueSliderChipToXu(){
        chip = (int) (BaseInfo.gI ().mainInfo.moneyChip * slider.value);
        ip_doi.value = chip + "";

        int x = (int) (chip / BaseInfo.gI ().tyle_chip_sang_xu);
        lbNhan.text = Res.MONEY_VIP_UPPERCASE + " = " + BaseInfo.formatMoneyDetailDot (x);
		//onChangeValueInputDoiXu ();
	}
	
	public void onChangeValueInputDoiXu(){
		if (!ip_doi.value.Trim ().Equals ("")) {
            int chipp = int.Parse (ip_doi.value);
            if(chipp > BaseInfo.gI ().mainInfo.moneyChip) {
                GameControl.instance.panelThongBao.onShow ("Số Chip không đủ!", delegate {
                    ip_doi.isSelected = true;
                });
                ip_doi.isSelected = false;
                ip_doi.value = "0";
                slider.value = 0;
                lbNhan.text = Res.MONEY_VIP_UPPERCASE + " = 0";
                return;
            }
            //slider.value = (float) chipp / BaseInfo.gI ().mainInfo.moneyChip;
            int x = (int)(chipp / BaseInfo.gI ().tyle_chip_sang_xu);
            lbNhan.text = Res.MONEY_VIP_UPPERCASE + " = " + BaseInfo.formatMoneyDetailDot (x);
		} else {
            lbNhan.text = Res.MONEY_VIP_UPPERCASE + " = 0";
		}
	}

    public void xuToChip () {
		if (ip_doi.value.Trim ().Equals ("")) {
            GameControl.instance.panelThongBao.onShow ("Bạn hãy nhập đủ thông tin.", delegate { });
			return;
		}

		if (!BaseInfo.gI ().checkNumber (ip_doi.value.Trim ())) {
            GameControl.instance.panelThongBao.onShow ("Nhập sai!", delegate { });
			return;
		}

		if (long.Parse(ip_doi.value.Trim ()) > BaseInfo
		    .gI().mainInfo.moneyXu) {
                GameControl.instance.panelThongBao.onShow ("Số xu chuyển phải <= số " + Res.MONEY_VIP +" hiện tại!", delegate { });
			return;
		}

		SendData.onXuToChip (long.Parse (ip_doi.value.Trim ()));
        TuChoi ();
	}

    public void chipToXu () {
		if (ip_doi.value.Trim ().Equals ("")) {
            GameControl.instance.panelThongBao.onShow ("Bạn hãy nhập đủ thông tin.", delegate { });
			return;
		}
		
		if (!BaseInfo.gI ().checkNumber (ip_doi.value.Trim ())) {
            GameControl.instance.panelThongBao.onShow ("Nhập sai!", delegate { });
			return;
		}
		
		if (long.Parse(ip_doi.value.Trim ()) > BaseInfo
		    .gI().mainInfo.moneyChip) {
                GameControl.instance.panelThongBao.onShow ("Số chip chuyển phải <= số " + Res.MONEY_FREE +" hiện tại!", delegate { });
			return;
		}
		
		SendData.onChipToXu (long.Parse (ip_doi.value.Trim ()));
        TuChoi ();
	}

    public void TuChoi () {
        GameControl.instance.sound.startClickButtonAudio ();
		ip_doi.value = "";
		slider.value = 0;
		lbNhan.text = "";
	}
}
