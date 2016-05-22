using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Card : MonoBehaviour {
    public delegate void CallBack ();
    public CallBack onClickOK;

    public UI2DSprite ui2dSprite;
    public UISprite cardMo;
    public GameObject clickObj;
    public GameObject child;
    public UISprite sp_click;
    private bool isCardMo;
    private int id;
    float deltaY = 0;

    public bool isChoose;

    public static int[] pNumber = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
                            13,14,15, 16, 17, 18, 19, 20, 21, 22, 23, 24,25,
                            26,27, 28, 29, 30, 31,32, 33, 34, 35, 36,37, 38,
                            39,40, 41, 42, 43, 44, 45, 46, 47, 48,49, 50, 51, 
                            52 };
    public static int[] aNumber = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 0, 1, 15, 16,
			17, 18, 19, 20, 21, 22, 23, 24, 25, 13, 14, 28, 29, 30, 31, 32, 33,
			34, 35, 36, 37, 38, 26, 27, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50,
			51, 39, 40, 52 };
    public static int[] cardPaint = aNumber;

    public static void setCardType (int type) {
        if(type == 0) {// phom
            cardPaint = pNumber;
        } else {
            cardPaint = aNumber;
        }
    }

    // Use this for initialization
    void Awake () {
        SetupSpriteFrames ();
        setId (5);
    }
    void Start () {


    }

    // Update is called once per frame

    void Update () {
        if(isChoose) {
            if(deltaY < 20) {
                deltaY = deltaY + 100 * Time.deltaTime;
            } else {
                deltaY = 20;
            }
        } else {
            if(deltaY > 0) {
                deltaY = deltaY - 100 * Time.deltaTime;
            } else {
                deltaY = 0;
            }
        }
        this.child.transform.localPosition = new Vector3 (0, deltaY, 0);

    }
    public int getId () {
        return id;
    }

    public void setId (int id) {
        if(id > 52) {
            id = 52;
        }
        if(id < 0) {
            id = 52;
        }
        this.id = id;
        isChoose = false;
        ui2dSprite.sprite2D = sprites[cardPaint[id]];
    }

    public void setSprite (int i) {
        ui2dSprite.sprite2D = sprites[i];
    }

    public void setMo (bool isCardMo) {
        this.isCardMo = isCardMo;
        cardMo.gameObject.SetActive (isCardMo);
    }

    public bool isMo () {
        return isCardMo;
    }

    public const float W_CARD = 56f;
    public const float H_CARD = 75f;

    private Sprite[] sprites;

    private void SetupSpriteFrames () {
        sprites = Resources.LoadAll<Sprite> ("Cards/cardAll");
    }
    public void OnClickCard () {
        // setChoose(!this.isChoose);
        if(onClickOK != null) {
            onClickOK.Invoke ();
        }

    }
    public void setListenerClick (CallBack click) {
        this.onClickOK = click;
    }
    public IEnumerator moveFromTo (Vector3 from, Vector3 to, float dur, float wait, bool isDeal) {
        yield return new WaitForSeconds (wait);
        this.gameObject.transform.localPosition = from;
        TweenPosition.Begin (this.gameObject, dur, to);
    }

    public IEnumerator moveTo (Vector3 to, float dur, float wait, bool isDeal) {
        int ids = id;
        if(isDeal) {
            this.gameObject.SetActive (false);
            setId (52);
        }

        yield return new WaitForSeconds (wait);
        if(isDeal) {
            this.gameObject.SetActive (true);
            setId (ids);
        }

        TweenPosition.Begin (this.gameObject, dur, to);
        GameControl.instance.sound.startchiabaiAudio ();
    }
    public void setChoose (bool isChoose) {

        this.isChoose = isChoose;

        //if (isChoose)
        //{
        //    TweenPosition.Begin(child, 0.2f, new Vector3(0, 30, 0));
        //}
        //else
        //{
        //    TweenPosition.Begin(child, 0.2f, new Vector3(0, 0, 0));
        //}
    }
    public void setTouchable (bool isTouchable) {
        clickObj.collider2D.enabled = isTouchable;
    }
    public void setDepth (int index) {
        ui2dSprite.depth = 11 + index * 3;
        cardMo.depth = 12 + index * 3;
        sp_click.depth = 13 + index * 3;

        //Vector3 vt3 = clickObj.GetComponent<BoxCollider>().size;
        //vt3.z = ui2dSprite.depth;
        //clickObj.GetComponent<BoxCollider>().size = vt3;
    }

    //public float getDepth() {
    //    return clickObj.GetComponent<BoxCollider>().size.z;
    //}
}
