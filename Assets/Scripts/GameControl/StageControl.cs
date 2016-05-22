using UnityEngine;
using System.Collections;

public class StageControl : MonoBehaviour {
    public GameControl gameControl;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public virtual void Appear()
    {
        //gameObject.transform.localPosition = new Vector3(-960, 0, 0);
        ///TweenPosition.Begin(gameObject, 0.3f, new Vector3(0, 0, 0));
        //TweenAlpha.Begin(gameObject, 0f, 0);
        //TweenAlpha.Begin(gameObject, 0.3f, 1);
        gameObject.SetActive(true);
    }
    public void DisAppear() {
        gameObject.SetActive(false);
    }
   /* public void showDialogNapXu()
    {
        gameControl.panelNapChuyenXu.onShow();
    }
    public void showDialogSetting()
    {
        gameControl.panelSetting.onShow();
    }*/
    public virtual void onBack()
    {

    }
}
