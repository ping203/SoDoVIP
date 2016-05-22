using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        audio.rolloffMode = AudioRolloffMode.Linear;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void PlaySound (string soundName, bool isLoop) {
        if(BaseInfo.gI ().isSound) {
            audio.PlayOneShot (Resources.Load ("sounds/" + soundName) as AudioClip);
            audio.loop = isLoop;
        }
    }

    void PlaySoundXocdia (string soundName, bool isLoop) {
        if(BaseInfo.gI ().isSound) {
            GetComponent<AudioSource> ().PlayOneShot (Resources.Load ("sounds/Xocdia/" + soundName) as AudioClip);
            GetComponent<AudioSource> ().loop = isLoop;
        }
    }

    public void pauseSound () {
        audio.Pause ();
    }

    public void PlayVibrate () {
        if(BaseInfo.gI ().isVibrate)
            Handheld.Vibrate();
    } 

    /*public void startAlertAudio () {
        PlaySound ("alert", false);
    }*/

    public void startchiabaiAudio () {
        PlaySound ("danhbai", false);
    }

    public void startCountDownAudio () {
        PlaySound ("countdown", true);
    }

    public void startLostAudio () {
        PlaySound ("bet", false);
    }

    public void startMessageAudio () {
        PlaySound ("message", false);
    }

    public void startTineCountAudio () {
        PlaySound ("timecount", false);
    }

    public void startToAudio () {
        PlaySound ("to", false);
    }

    public void startWinAudio () {
        PlaySound ("nhat", false);
    }

    public void startClickButtonAudio () {
        PlaySound ("add", false);
    }

    public void startAnbairacAudio () {
        PlaySound ("anbairac", false);
    }

    public void startMomAudio () {
        PlaySound ("mom", false);
    }

    public void startBaAudio () {
        PlaySound ("ba", false);
    }

    public void startNhatAudio () {
        PlaySound ("nhat", false);
    }

    public void startGuibaiAudio () {
        PlaySound ("guibai", false);
    }

    public void startHaphomAudio () {
        PlaySound ("haphom", false);
    }

    public void startVaobanAudio () {
        PlaySound ("knock", false);
    }

    public void startBinhlungAudio () {
        PlaySound ("binhlung", false);
    }

    public void startFinishAudio () {
        PlaySound ("finished", false);
    }

    public void startSobaiAudio () {
        PlaySound ("sobai", false);
    }

    public void startMaubinhAudio () {
        PlaySound ("maubinh", false);
    }

    public void start_HAINE () {
        PlaySound ("haine", false);
    }

    public void start_MAYHABUOI () {
        PlaySound ("mayhabuoi", false);
    }

    public void start_ThuaDiCung () {
        PlaySound ("thuadicung", false);
    }

    public void start_DODI () {
        PlaySound ("dodi", false);
    }

    public void start_ChetNE () {
        PlaySound ("chetmayne", false);
    }

    public void start_random () {
        int ran = Random.Range (1, 4);
        switch(ran) {
            case 1:
                start_DODI ();
                break;
            case 2:
                start_MAYHABUOI ();
                break;
            case 3:
                start_ThuaDiCung ();
                break;
            case 4:
                start_ChetNE ();
                break;

        }
    }

    public void startUAudio () {
        PlaySound ("u", false);
    }

    ///Xoc dia
    //public void startVaobanAudio () {
    //    PlaySoundXocdia ("knock", false);
    //}

    public void startXocdiaAudio () {
        PlaySoundXocdia ("xoc_dia", false);
    }

    public void MoneyAudio () {
        PlaySoundXocdia ("xeng_money", false);
    }

    public void startWinAudioXocdia () {
        PlaySoundXocdia ("nhat", false);
    }

    public void startLoseAudioXocdia () {
        PlaySoundXocdia ("bet", false);
    }

    public void clickBtnAudioXocdia () {
        PlaySoundXocdia ("xeng_click", false);
    }
    ///Xocdia
}
