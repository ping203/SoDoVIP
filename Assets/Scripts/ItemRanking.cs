using UnityEngine;
using System.Collections;

public class ItemRanking : MonoBehaviour {

    public UISprite bg, avatar;
    public UILabel lb_Name;
    public UILabel lb_Bim;
    public UILabel lb_stt;

    public void SetData(int stt, int idAvatar, string name, long money) {
        bg.spriteName = "BXH";
        if (stt % 2 == 0) {
            bg.spriteName = "btn_table_tt";
        }
        if (lb_stt != null) {
            lb_stt.text = stt.ToString();
        }

        if (avatar != null) {
            avatar.spriteName = idAvatar.ToString();
        }

        if (lb_Name != null) {
            if (name.Length > 10) {
                name = name.Substring(0, 10) + "...";
            }

            lb_Name.text = name;
        }

        if (lb_Bim != null) {
            lb_Bim.text = BaseInfo.formatMoney(money);
        }

    }
}
