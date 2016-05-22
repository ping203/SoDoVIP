using UnityEngine;
using System.Collections;

public class ThongbaoXocdia : MonoBehaviour {
    public UILabel m_lbThongbao;
    public Animator m_animator;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetLbThongbao (string aThongbao) {
        if(this.m_lbThongbao != null) {
            this.m_lbThongbao.text = aThongbao;
        }
    }

    public void SetAnimationThongbao_Idle () {
        if(this.m_animator != null) {
            this.m_animator.SetBool ("thongbaolen", false);
            this.m_animator.SetBool ("thongbaoxuong", false);
        }
    }

    public void SetAnimationThongbao_Len () {
        if(this.m_animator != null) {
            this.m_animator.SetBool ("thongbaolen", true);
            this.m_animator.SetBool ("thongbaoxuong", false);
        }
    }

    public void SetAnimationThongbao_Xuong () {
        if(this.m_animator != null) {
            this.m_animator.SetBool ("thongbaolen", false);
            this.m_animator.SetBool ("thongbaoxuong", true);
        }
    }

    public void ShowThongbao1 () {
        gameObject.GetComponent<UISprite> ().alpha = 1.0f;
        gameObject.SetActive (true);

        StartCoroutine (HideThongbaoTimer (2.0f));
    }

    public void HideThongbao1 (float timeWait) {
        StartCoroutine (HideThongbaoTimer(timeWait));
    }

    IEnumerator HideThongbaoTimer (float timeWait) {
        yield return new WaitForEndOfFrame ();
        yield return new  WaitForSeconds(timeWait);
        if(this.m_animator != null) {
            this.m_animator.SetBool ("fadeIn", true);
        }
    }

    public void EndFadeIn () {
        gameObject.SetActive (false);
        if(this.m_animator != null) {
            this.m_animator.SetBool ("fadeIn", false);
        }
    }
}
