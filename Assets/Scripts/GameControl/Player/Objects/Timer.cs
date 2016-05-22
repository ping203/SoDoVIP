using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
    public UISprite sprite;
    // Use this for initialization
    void Start() {
        setPercentage(100);
    }

    // Update is called once per frame
    //void Update() {

    //}
    public void setPercentage(float percent) {
        percent = 1 - percent / 100;
        sprite.fillAmount = percent;
    }
}
