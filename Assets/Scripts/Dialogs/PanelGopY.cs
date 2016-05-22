using UnityEngine;
using System.Collections;

public class PanelGopY : PanelGame {
    public UIInput inputText;
    public GameControl gameControl;

    string df = "Nhập nội dung gửi đến quản trị.";

    public void onSendToAdmin() {
        GameControl.instance.sound.startClickButtonAudio();
        if (inputText.value != "" && !inputText.value.Equals(df)) {
           // SendData.onGopY(inputText.value);
            gameControl.panelThongBao.onShow("Cảm ơn bạn đã gửi góp ý tới admin!", delegate {
                onHide();
            });
        } else {
            gameControl.panelThongBao.onShow("Bạn phải nhập nội dung!", delegate {});
        }
    }

    public override void onHide() {
        base.onHide();
        inputText.value = df;
    }

    public void onchangeText() {
        //inputText.value.Length
    }
}
