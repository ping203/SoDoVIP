using UnityEngine;
using System.Collections;

public class PanelChangePassword : PanelGame {
    // Use this for initialization
    void Start () {

    }

    public void ok (string oldPass, string newPass1, string newPass2) {
        GameControl.instance.sound.startClickButtonAudio ();
        if(oldPass == "" || newPass1 == "" || newPass2 == "") {
            GameControl.instance.panelThongBao.onShow ("Bạn hãy nhập đủ thông tin.", delegate { });
            return;
        }

        if(oldPass != BaseInfo.gI ().pass) {
            GameControl.instance.panelThongBao.onShow ("Mật khẩu cũ không đúng.", delegate { });
            return;
        }

        if(newPass1 != newPass2) {
            GameControl.instance.panelThongBao.onShow ("Mật khẩu không giống nhau.", delegate { });

            return;
        }

        /*if (newPass1.Length <= 4) {
            thongbao.onShow("");
        }*/
  string sms = BaseInfo.gI().SMS_CHANGE_PASS_SYNTAX + " " + BaseInfo.gI().mainInfo.userid
        + " " + oldPass+ " " + newPass1;

#if UNITY_EDITOR
                GameControl.instance.panelThongBao.onShow ("Soạn tin theo cú pháp " + BaseInfo.gI ().SMS_CHANGE_PASS_SYNTAX + " gửi đến " + BaseInfo.gI ().SMS_CHANGE_PASS_NUMBER, delegate { });
#else
             GameControl.instance.panelYesNo.onShow("Chương trình sẽ gửi tin nhắn để đổi mật khẩu (phí 1000đ), bạn có đồng ý không?",
                                          delegate { GameControl.sendSMS(BaseInfo.gI ().SMS_CHANGE_PASS_NUMBER, sms); });
#endif


                onHide ();
    }
}
