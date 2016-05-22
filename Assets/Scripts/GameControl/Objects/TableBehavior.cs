using UnityEngine;
using System.Collections;

public class TableBehavior : MonoBehaviour {
    public UILabel lb_ban;
    public UILabel lb_muccuoc;
    public UISlider slide_tinhtrang;
    public UILabel numPlayer;
    public UISprite spriteLock;

    //control
    public UISprite backgroundSprite;
    public UIPanel panel;
    public InstanListViewControler listPopulator;
    public int itemNumber;
    public int itemDataIndex;
    private bool isVisible = true;

    public int id;
    public int status;
    public string name = "";
    public string masid = "";
    public int nUser;
    public int maxUser;
    public long money;
    public long needMoney;
    public long maxMoney;
    public int Lock = 0;
    public int typeTable;
    public int choinhanh = 0;
    void Start () {
        transform.localScale = new Vector3 (1, 1, 1);
    }

    // Update is called once per frame
    void Update () {
        if(Mathf.Abs (listPopulator.draggablePanel.currentMomentum.y) > 0) {
            CheckVisibilty ();
        }
    }
    public void setInFo (int staste) {
        if(staste % 2 == 0) {
            backgroundSprite.spriteName = "row1";
        } else {
            backgroundSprite.spriteName = "row3";
        }

        lb_ban.text = "Bàn " + id;
        lb_muccuoc.text = "$"
                        + BaseInfo.formatMoney (money) + " ("
                        + BaseInfo.formatMoney (needMoney) + ")";
        if(typeTable == Res.ROOMFREE) {
            lb_muccuoc.text = BaseInfo.formatMoneyDetailDot (money) + " " + Res.MONEY_FREE;
        } else {
            lb_muccuoc.text = BaseInfo.formatMoneyDetailDot (money) + " " + Res.MONEY_VIP;
        }
        float tt = (float) nUser / maxUser;
        slide_tinhtrang.value = tt;

        spriteLock.gameObject.SetActive (false);
        if(Lock == 1) {
            spriteLock.gameObject.SetActive (true);
        }
        numPlayer.text = nUser + "/" + maxUser;
    }
    public void clickTable () {
        GameControl.instance.sound.startClickButtonAudio ();
        long moneyTemp = 0;
        string money = "";
        if(BaseInfo.gI ().typetableLogin == Res.ROOMFREE) {
            moneyTemp = BaseInfo.gI ().mainInfo.moneyChip;
            money = Res.MONEY_FREE;
        } else {
            moneyTemp = BaseInfo.gI ().mainInfo.moneyXu;
            money = Res.MONEY_VIP;
        }
        if(BaseInfo.gI ().checkHettien ()) {
            GameControl.instance.panelYesNo.onShow ("Bạn không còn tiền. Bạn có muốn nạp tiền không?", delegate {
                GameControl.instance.panelNapChuyenXu.onShow ();
            });
        } else {
            if(moneyTemp < BaseInfo.gI ().needMoney) {
                GameControl.instance.panelYesNo.onShow ("Bạn không còn tiền. Bạn có muốn nạp tiền không?", delegate {
                    GameControl.instance.panelNapChuyenXu.onShow ();
                });
            } else {
                BaseInfo.gI ().numberPlayer = BaseInfo.gI ().maxUser = maxUser;
                if(GameControl.instance.gameID == GameID.POKER
                        || GameControl.instance.gameID == GameID.XITO
                        || GameControl.instance.gameID == GameID.LIENG) {
                    BaseInfo.gI ().moneyNeedTable = needMoney;
                    GameControl.instance.panelRutTien.show ((long) (needMoney * 2.5f), maxMoney, 0, id, 0, 0, BaseInfo.gI ().typetableLogin);
                } else {
                    GameControl.instance.panelWaiting.onShow ();
                    SendData.onJoinTablePlay (id, "", -1);
                }

            }
        }
    }


    public bool verifyVisibility () {
        return (panel.IsVisible (backgroundSprite));
    }

    void CheckVisibilty () {
        bool currentVisibilty = panel.IsVisible (backgroundSprite);
        if(currentVisibilty != isVisible) {
            isVisible = currentVisibilty;

            if(!isVisible) {
                StartCoroutine (listPopulator.ItemIsInvisible (itemNumber));
            }
        }
    }
}
