using UnityEngine;
using System.Collections;

public class PanelInfoPlayer : PanelGame {
    public UILabel id;
    public UILabel name;
    public UILabel chip;
    public UISprite spriteAvata;
    public UIButton btnAvata;
    public PanelChangePassword panelChangePassword;
    public PanelChangeName panelChangeName;
    public PanelChangeAvata panelChangeAvata;
    public GameObject changePass, changeName;

    public GameObject itemTT, parentGrid;
    WWW www;
    bool isSet = false;
    void OnEnable () {
        www = null;
        isSet = false;
    }
    // Use this for initialization
    void Start () {
    }
    // Update is called once per frame
    void Update () {
        if(www != null) {
            if(www.isDone && !isSet) {
                spriteAvata.mainTexture = www.texture;
                isSet = true;
            }
        }
    }

    public void clickChangePass () {
        GameControl.instance.sound.startClickButtonAudio ();
        panelChangePassword.onShow ();
    }

    public void clickChangeName () {
        GameControl.instance.sound.startClickButtonAudio ();
        panelChangeName.onShow (BaseInfo.gI ().mainInfo.displayname);
    }

    public void clickChangeAvata () {
        GameControl.instance.sound.startClickButtonAudio ();
        panelChangeAvata.loadAva ();
        panelChangeAvata.onShow ();
    }

    public void infoMe () {
        string n = BaseInfo.gI ().mainInfo.displayname;
        long uid = BaseInfo.gI ().mainInfo.userid;
        long xuMe = BaseInfo.gI ().mainInfo.moneyXu;
        long chipMe = BaseInfo.gI ().mainInfo.moneyChip;
        string slt = BaseInfo.gI ().mainInfo.soLanThang;
        string slth = BaseInfo.gI ().mainInfo.soLanThua;
        int idAva = BaseInfo.gI ().mainInfo.idAvata;
        string link_ava = BaseInfo.gI ().mainInfo.link_Avatar;
        string email = BaseInfo.gI ().mainInfo.email;
        string phone = BaseInfo.gI ().mainInfo.phoneNumber;

        infoProfile (n, uid, xuMe, chipMe, slt, slth, link_ava, idAva, email, phone);
    }

    public void updateAvata () {
        int id = BaseInfo.gI ().mainInfo.idAvata;
        if(id != 0) {
            spriteAvata.spriteName = id + "";
        }
    }

    public void infoProfile (string nameinfo, long userid, long xuinfo, long chipinfo,
        string slthang, string slthua, string link_avata, int idAvata,
        string email, string phone) {
        bool isMe = false;
        if(nameinfo == BaseInfo.gI ().mainInfo.displayname) {
            isMe = true;
        }
        changePass.SetActive (isMe);
        changeName.SetActive (isMe);
        btnAvata.gameObject.SetActive(isMe);

        name.text = "Tên: " + nameinfo;
        id.text = "ID: " + userid;
        chip.text = Res.MONEY_FREE + ": " + BaseInfo.formatMoneyDetailDot (chipinfo);
        if(parentGrid.transform.childCount == 0) {
            string[] slt = slthang.Split (',');
            string[] slth = slthua.Split (',');
            for(int i = 0; i < slt.Length; i++) {
                GameObject obj = Instantiate (itemTT) as GameObject;
                parentGrid.GetComponent<UIGrid> ().AddChild (obj.transform);

                obj.transform.localScale = new Vector3 (1, 1, 1);
                obj.transform.localPosition = new Vector3 (0, 0, 0);

                obj.GetComponent<ItemThangThua> ().setText (slt, slth, i);
            }
        }
        if (link_avata == "") {
            if (idAvata != 0) {
                spriteAvata.GetComponent<UISprite>().enabled = true;
                spriteAvata.GetComponent<UITexture>().enabled = false;
                spriteAvata.spriteName = idAvata + "";
            } else {
                spriteAvata.spriteName = "0";
            }
        } else {
            www = new WWW(link_avata);
            if (www.error != null) {
                Debug.Log("Image WWW ERROR: " + www.error);
            } else {
                spriteAvata.GetComponent<UISprite>().enabled = false;
                spriteAvata.GetComponent<UITexture>().enabled = true;
                // spriteAvata.GetComponent<UITexture> ().mainTexture = www.texture;
            }
        }
    }
}
