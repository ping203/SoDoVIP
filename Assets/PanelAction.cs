using UnityEngine;
using System.Collections;

public class PanelAction : PanelGame {

    public UIToggle toggleAction;
    public ABSUser player;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void onHidePanel () {
        base.onHide ();
        toggleAction.value = false;
        player.onHide ();
        if(player.buttonKick != null)
            player.buttonKick.gameObject.SetActive (false);
    }

    bool isAction = true;
    float timeAction = 0;

    // GameObject ob;
    public void sendActions (int id) {
        player.sendActions (id);
        if(player.buttonKick != null)
            player.buttonKick.gameObject.SetActive (false);
    }

    public void clickButtonActionBia () {
        sendActions (1);
    }

    public void clickButtonActionCaChua () {
        sendActions (2);
    }

    public void clickButtonActionDep () {
        sendActions (3);
    }

    public void clickButtonActionHoa () {
        sendActions (4);
    }

    public void clickButtonActionLuuDan () {
        sendActions (5);
    }

    public void clickButtonActionTrung () {
        sendActions (6);
    }
}
