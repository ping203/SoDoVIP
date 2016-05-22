using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Toast : MonoBehaviour
{
    public UILabel label;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void showToast(string mess)
    {
        this.gameObject.SetActive(true);
        label.text = mess;
        StartCoroutine(showT());
       
    }

    IEnumerator showT()
    {
        TweenAlpha.Begin(gameObject, 0f, 0);
        TweenAlpha.Begin(gameObject, 0.5f, 1);
        yield return new WaitForSeconds(3f);
        TweenAlpha.Begin(gameObject, 0.5f, 0);
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }
}
