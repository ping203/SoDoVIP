using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
public class GameControl : MonoBehaviour {
    public static void sendSMS (string port, string content) {
#if UNITY_WP8
        UnityPluginForWindowPhone.Class1.sendSMS (port, content);
#else
        string str = content;
        if(content.Contains ("#")) {
            str = content.Replace ("#", "%23");
        }
		Application.OpenURL("sms:" + port + @"?body=" + str);

#endif
    }
    private static GameControl _instance;
    public static GameControl instance {
        get {
            if(_instance == null) {
                _instance = GameObject.FindObjectOfType<GameControl> ();
                //_instance = GameObject.Find("bkgChung").GetComponent<GameControl>();

                //Tell unity not to destroy this object when loading a new scene!
                DontDestroyOnLoad (_instance.gameObject);
            }

            return _instance;
        }
    }


    public static int WIDTH = 960;
    public static int HIGHT = 540;
    public static int WPLAYER = 88;
    public UITexture uiTexture;
    public UISprite uiS;
    public PanelWaiting panelWaiting;
    public PanelThongBao panelThongBao;
    public PanelYesNo panelYesNo;
    public PanelSetting panelSetting;
    public PanelInfoPlayer panelInfoPlayer;
    public PanelHelp panleHelp;
    public PanelNapChuyenXu panelNapChuyenXu;
    public PanelDoiThuong panelDoiThuong;
    public PanelMail panelMail;
    public PanelCreateRoom panelCreateRoom;
    public PanelToiBan panelToiBan;
    public PanelMoiChoi PanelMoiChoi;
    public PanelThongBaoMoiChoi panelThongBaoMoiChoi;
    public PanelChat panelChat;
    public PanelDatCuoc panelDatCuoc;
    public PanelRutTien panelRutTien;
    public PanelInput panelInput;
    public PanelCuoc panelCuoc;
    //public DialogKetQua dialogKetQua;
    //public DialogRutTien dialogRutTien;
    //public DialogThongTin dialogThongTin;
    //public DialogQuenMatKhau dialogQuenMK;
    //public DialogMoiBan dialogMoiBan;
    //public DialogAllMess dialogAllMess;
    public PanelNotiDoiThuong panelNotiDoiThuong;
    public PanelDangKy panelDangKy;
    public PanelNhiemVu panelNhiemVu;
    //public DialogLuatChoi dialogLuatChoi;
    //public DialogEvent dialogEvent;
    //public DialogDoiMatKhau dialogDoiMatKhau;
    public SoundManager sound;

    public Toast toast;
    public LoginControl login;
    public MenuControl menu;
    public RoomControl room;
    public LoadingControl load;

    public TLMN tlmn;
    public PHOM phom;
    public Poker poker5;
    //public Poker9 poker9;
    public Xam xam;
    public Xito xito;
    public Lieng lieng5;
    //public Lieng9 lieng9;
    public Bacay bacay;
    //public Bacay9 bacay9;
    public MauBinh maubinh;
    public XocDia xocdia;

    public BaseCasino currentCasino;
    public StageControl currenStage;
    public StageControl backState;

    public List<RoomInfo> phongFree = new List<RoomInfo> ();
    public List<RoomInfo> phongVip = new List<RoomInfo> ();
    //public List<RoomInfo> phongQT = new List<RoomInfo>();
    public List<TableItem> listTable = new List<TableItem> ();

    public int gameID;

    public bool cancelAllInvite = false;
    void Awake () {
        if(_instance == null) {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad (this);
        } else {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if(this != _instance)
                Destroy (this.gameObject);
        }
        //SoundManager.Get().startAudio(SoundManager.AUDIO_TYPE.BKG_MUSIC);
    }
    // Use this for initialization
    void Start () {
        WIDTH = uiTexture.width;
        HIGHT = uiTexture.height;
        WPLAYER = uiS.width;
        Application.targetFrameRate = 80;
        new ListernerServer (this);
        currenStage = load;
        load.gameObject.SetActive (true);
        login.gameObject.SetActive (false);
        menu.gameObject.SetActive (false);
        room.gameObject.SetActive (false);

        tlmn.gameObject.SetActive (false);
        phom.gameObject.SetActive (false);
        poker5.gameObject.SetActive (false);
        //poker9.gameObject.SetActive(false);
        xam.gameObject.SetActive (false);
        xito.gameObject.SetActive (false);
        lieng5.gameObject.SetActive (false);
        //lieng9.gameObject.SetActive(false);
        bacay.gameObject.SetActive (false);
        maubinh.gameObject.SetActive (false);
        //NetworkUtil.GI ().connect (SendData.onGetPhoneCSKH ());
//#if UNITY_WP8
//        if(UnityPluginForWindowPhone.Class1.getNetworkInterfaces () > 0) {
//            NetworkUtil.GI ().connect (SendData.onGetPhoneCSKH ());
//        } else {
//            panelYesNo.onShow ("Không có kết nối mạng! Bạn có muốn mở cài đặt?", delegate {
//                UnityPluginForWindowPhone.Class1.openSetting ();
//            });
//        }
//#endif

        //bacay9.gameObject.SetActive(false);
        //maubinh.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
    }
    public void setStage (StageControl stage) {
        if(currenStage != stage) {
            backState = currenStage;
        }
        if(currenStage != null) {
            currenStage.DisAppear ();
        }

        currenStage = stage;
        stage.Appear ();
    }
    public void setCasino (int gameID, int type) {
        switch(gameID) {
            case GameID.TLMN:
                setStage (tlmn);
                currentCasino = (BaseCasino) currenStage;
                break;
            case GameID.XAM:
                setStage (xam);
                currentCasino = (BaseCasino) currenStage;
                break;
            case GameID.LIENG:
                if(type == 0) { // 5
                    setStage (lieng5);
                    currentCasino = (BaseCasino) currenStage;
                } else { // 9
                    //setStage(lieng9);
                    //currentCasino = (BaseCasino)currenStage;
                }
                break;
            case GameID.BACAY:
                if(type == 0) { // 5
                    setStage (bacay);
                    currentCasino = (BaseCasino) currenStage;
                } else { // 9
                    //setStage(bacay9);
                    //currentCasino = (BaseCasino)currenStage;
                }
                break;
            case GameID.PHOM:
                setStage (phom);
                currentCasino = (BaseCasino) currenStage;
                break;
            case GameID.POKER:
                if(type == 0) { // 5
                    setStage (poker5);
                    currentCasino = (BaseCasino) currenStage;
                }
                //    else { // 9
                //        setStage(poker9);
                //        currentCasino = (BaseCasino)currenStage;
                //    }
                break;
            case GameID.XITO:
                setStage (xito);
                currentCasino = (BaseCasino) currenStage;
                break;
            case GameID.MAUBINH:
                setStage (maubinh);
                currentCasino = (BaseCasino) currenStage;
                break;
            case GameID.XOCDIA:
                setStage (xocdia);
                currentCasino = (BaseCasino) currenStage;
                break;
            default:
                break;
        }
        initCardType (gameID);
    }

    private void initCardType (int gameID) {
        // TODO Auto-generated method stub
        switch(gameID) {
            case GameID.TLMN:
                Card.setCardType (1);
                break;
            case GameID.XAM:
                Card.setCardType (1);
                break;
            case GameID.LIENG:
                Card.setCardType (0);
                break;
            case GameID.BACAY:
                Card.setCardType (0);
                break;
            case GameID.PHOM:
                Card.setCardType (0);
                break;
            case GameID.POKER:
                Card.setCardType (1);
                break;
            case GameID.XITO:
                Card.setCardType (0);
                break;
            case GameID.MAUBINH:
                Card.setCardType (1);
                break;
            default:
                Card.setCardType (1);
                break;
        }
    }

    internal void disableAllDialog () {
        panelWaiting.onHide ();
        panelThongBao.onHide ();
        panelYesNo.onHide ();
        panelSetting.onHide ();
        panelInfoPlayer.onHide ();
        panleHelp.onHide ();
        panelNapChuyenXu.onHide ();
        panelDoiThuong.onHide ();
        panelMail.onHide ();
        panelCreateRoom.onHide ();
        panelToiBan.onHide ();
        PanelMoiChoi.onHide ();
        panelThongBaoMoiChoi.onHide ();
        panelChat.onHide ();
        panelRutTien.onHide ();
        panelInput.onHide ();
        panelNotiDoiThuong.onHide ();
        panelCuoc.onHide ();
        panelDatCuoc.onHide ();
        panelDangKy.onHide ();
        panelNhiemVu.onHide ();
    }

    void OnApplicationQuit () {

    }

    public void resetGame () {
        //login.gameObject.SetActive (true);
        menu.gameObject.SetActive (false);
        room.gameObject.SetActive (false);

        tlmn.gameObject.SetActive (false);
        phom.gameObject.SetActive (false);
        poker5.gameObject.SetActive (false);
        //poker9.gameObject.SetActive(false);
        xam.gameObject.SetActive (false);
        xito.gameObject.SetActive (false);
        lieng5.gameObject.SetActive (false);
        //lieng9.gameObject.SetActive(false);
        bacay.gameObject.SetActive (false);
        maubinh.gameObject.SetActive (false);
    }

    void OnApplicationPause (bool pauseStatus) {
        NetworkUtil.GI ().resume (pauseStatus);
    }

    void resume () {
        //NetworkUtil.GI().resume();
    }
}
