using UnityEngine;
using System.Collections;

public class ItemAnimation : MonoBehaviour {
    private UIButton m_bt;

    private float ScaleMax = 1.2f;
    private float ScaleMin = 1.0f;

    Transform trans;
    // Use this for initialization
    void OnEnable()
    {
        m_bt = gameObject.GetComponent<UIButton>();
        trans = gameObject.transform;
        trans.localScale = new Vector3(ScaleMin, ScaleMin, 1.0f);

        UIEventTrigger evt = gameObject.AddComponent<UIEventTrigger>();
        EventDelegate.Add(evt.onClick, onClickBtn);
        EventDelegate.Add(evt.onRelease, OnReleaseBtn);
        EventDelegate.Add(evt.onPress, onPressBtn);
    }

    public void OnReleaseBtn()
    {
        if (gameObject.activeInHierarchy == true)
            StartCoroutine(ScaleTo());
    }

    public void onClickBtn()
    {
        if (gameObject.activeInHierarchy == true)
            StartCoroutine(ScaleTo());
    }

    private IEnumerator ScaleTo()
    {
        float delta = ScaleMax - ScaleMin;
        float rate = ScaleMin;
        float r = delta / 10.0f;
        for (int i = 0; i < 10; i++)
        {
            rate += r;
            trans.localScale = new Vector3(rate, rate, 1.0f);
            yield return new WaitForEndOfFrame();
        }

        rate = ScaleMax;
        for (int i = 0; i < 10; i++)
        {
            rate -= r;
            trans.localScale = new Vector3(rate, rate, 1.0f);
            yield return new WaitForEndOfFrame();
        }
    }

    public void onPressBtn()
    {
        if (m_bt != null)
        {
            m_bt.pressed = new Color(183.0f / 255.0f, 163.0f / 255.0f, 123.0f / 255.0f, 100.0f / 255.0f);
            m_bt.hover = new Color(183.0f / 255.0f, 163.0f / 255.0f, 123.0f / 255.0f, 100.0f / 255.0f);
        }
    }
}
