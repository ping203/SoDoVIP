using UnityEngine;
using System.Collections;

public class ToggleScript : MonoBehaviour {
    private UIToggle m_bt;
    private UISprite m_sprite;

    public UISprite background;
    public UISprite checkmark;

    void Awake()
    {
        m_bt = gameObject.GetComponent<UIToggle>();
        background.gameObject.SetActive(false);
        checkmark.gameObject.SetActive(true);
        if (m_bt != null)
        {
            EventDelegate.Add(m_bt.onChange, OnValueChange);
        }
    }

    public void OnValueChange()
    {
        if(m_bt.value == true)
        {
            background.gameObject.SetActive(false);
            checkmark.gameObject.SetActive(true);
        }
        else
        {
            background.gameObject.SetActive(true);
            checkmark.gameObject.SetActive(false);
        }
    }

    private void onClickBtn()
    {

    }
}
