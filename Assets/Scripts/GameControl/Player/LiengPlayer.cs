using UnityEngine;
using System.Collections;

public class LiengPlayer : ABSUser
{

    // Use this for initialization
    void Start()
    {
        //if (pos != 0)
        //{
        //    cardHand.getCardbyPos(0).transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 20));
        //    cardHand.getCardbyPos(1).transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //    cardHand.getCardbyPos(2).transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -20));

        //}
        for (int i = 0; i < cardHand.getSizeArrayCard(); i++)
        {
            LiengInput liengIP = new LiengInput(cardHand,
                           cardHand.getCardbyPos(i),this);

            cardHand.getCardbyPos(i).setListenerClick(delegate
            {
                liengIP.click();
            });
        }
    }

    // Update is called once per frame
    public override void setInfo(string diem) {
        if (diem.Length > 0) {
            lb_name_sansang.text = diem;
        }
    }
}
