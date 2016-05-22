using UnityEngine;
using System.Collections;

public class Chip : MonoBehaviour
{
    private long soChip;
    public UISprite sp_chip;
    public UILabel lb_sochip;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setSoChip(long soChip)
    {
        this.soChip = soChip;
    }

    public void setMoneyChip(long money)
    {
       
        soChip = money;

        string name;
        if (money > BaseInfo.gI().moneyTable * 20)
        {
            name = "chip_05";
        }
        else if (money > BaseInfo.gI().moneyTable * 10)
        {
            name = "chip_04";
        }
        else if (money > BaseInfo.gI().moneyTable * 5)
        {
            name = "chip_03";
        }
        else if (money > BaseInfo.gI().moneyTable * 1)
        {
            name = "chip_02";
        }
        else
        {
            name = "chip_01";
        }
        sp_chip.spriteName = name;
        if (money == 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
            lb_sochip.text = "" + BaseInfo.formatMoneyDetailDot(money);
        }

    }

    public long getMoneyChip()
    {
        return soChip;
    }
}
