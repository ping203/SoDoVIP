using UnityEngine;
using System.Collections;

public class DiaComponent : MonoBehaviour {
    public Animator m_animator;

    private bool m_mobat = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetAnimationXocdia () {
        if(this.m_animator != null) {
            this.m_animator.SetBool ("isXocdia", true);
            this.m_animator.SetBool ("mobat", false);
            this.m_animator.SetBool ("upbat", false);
        }
    }

    public void SetAnimationXocdiaIdle () {
        if(this.m_animator != null) {
            this.m_animator.SetBool ("isXocdia", false);
            this.m_animator.SetBool ("mobat", false);
            this.m_animator.SetBool ("upbat", false);
        }
    }

    public void SetAnimationMobat () {
        this.m_mobat = true;

        if(this.m_animator != null) {
            this.m_animator.SetBool ("mobat", true);
            this.m_animator.SetBool ("upbat", false);
            this.m_animator.SetBool ("isXocdia", false);
        }
    }

    public void SetAnimationUpbat () {
        if(this.m_mobat == false) {
            return;
        }

        if(this.m_animator != null) {
            this.m_animator.SetBool ("upbat", true);
            this.m_animator.SetBool ("isXocdia", false);
            this.m_animator.SetBool ("mobat", false);
            this.m_mobat = false;
        }
    }
}
