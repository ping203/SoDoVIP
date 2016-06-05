using UnityEngine;
using System.Collections.Generic;

public class XocDia : BaseCasino {

    public TimerXocdia m_timerWaiting;
    public ThongbaoXocdia m_thongbaoXocdia;
    public ThongbaoXocdia m_thongbaoXocdia1;
    public DiaComponent m_diaComponent;
    public WinXocdia m_winXocdia;
    public List<UILabel> m_cacMucCuoc;

    //Cai xoc dia.
    public static Transform PlayerLamcai;
    public static bool SystemIsMaster;
    public UILabel m_btnDatGapDoiLabel;
    public UILabel m_btnDatLaiLabel;
    public UILabel m_btnHuyCuocLabel;
    public UILabel m_btnHuyChanLabel;
    public UILabel m_btnHuyLeLabel;
    public UILabel m_btnLamcaiLabel;

    //For create bai xoc dia.
    public Transform m_baixocdia;
    public List<Transform> m_pos;
    public GameObject xdRed;
    public GameObject xdWhite;

    public List<int> m_indexPos;
    public List<GameObject> m_baiXocdia;

    //For create bang lich su.
    public Transform m_bangLichsu;
    public GameObject m_lsDo;
    public GameObject m_lsTrang;
    public Transform m_posLichsu;
    public UILabel m_nChan;
    public UILabel m_nLe;

    private float m_deltaX = 21.0f;
    private float m_deltaY = -19.5f;
    private List<GameObject> m_lsCount;

    //Ten nguoi choi lam cai.
    //private string m_master;
    //So luong chip mau do.
    private int m_chipRed = -1;

    //Muc cuoc cua nguoi choi
    public List<GameObject> m_mucCuocAnim;
    //public List<GameObject> m_chips;

    private long m_mucCuoc;
    //private GameObject m_chipCuoc;

    //Cho phep dat cuoc cua.
    private bool m_isDatCuocCua = false;

    //Tong so tien cuoc cua cac cua.BaseInfo
    public UILabel[] m_totalMoneyCuaCuoc;
    public UILabel[] m_totalMeMoneyCuaCuoc;
    private long[] m_totalMoney;
    private long[] m_totalMeMoney;

    //Xu ly viec dat cuoc gap doi.
    private const int DOUBLE_BET_MAX = 4;
    private int m_double_bet_me = 0;

    //Button with color disable(0, 100, 100, 255).set color normal
    public UIButton m_btnDatGapDoi;
    public UIButton m_btnDatLai;
    public UIButton m_btnHuyCuoc;
    public UIButton m_btnLamCai;
    public UIButton m_btnHuyChan;
    public UIButton m_btnHuyLe;
    public UIButton m_btnHuyCai;
    private static Color COLOR_DISABLE_BTN = new Color(170.0f / 255.0f, 170.0f / 255.0f, 170.0f / 255.0f);
    private static Color COLOR_DISABLE_TXT_BTN = new Color(255.0f / 255.0f, 0.0f, 0.0f);

    //Kiem tra viec dat cuoc cua player0.
    private bool m_isBetMe = false;
    private bool m_isBetMeAgain = false;
    //Kiem tra xem van truoc co dat cuoc ko.
    private bool m_isBetPrevious = false;

    //Player dat cuoc cua le va chan.
    private List<ABSUser> m_playerCCC = new List<ABSUser>();
    private List<ABSUser> m_playerCCL = new List<ABSUser>();

    private bool m_me_master = false;

    private bool m_chipStopMove = false;

    void Awake() {
        ChipXocdia.onChipMove += OnChipMove;
        ChipXocdia.onChipMoveFinish += OnChipMoveFinish;
    }

    public void Start() {
        //--------------------
        //So tien cuoc thong ke
        if (m_totalMoney == null) {
            m_totalMoney = new long[6];
        } else {
            for (int i = 0; i < m_totalMoney.Length; i++) {
                m_totalMoney[i] = 0;
            }
        }

        if (m_totalMeMoney == null) {
            m_totalMeMoney = new long[6];
        } else {
            for (int i = 0; i < m_totalMeMoney.Length; i++) {
                m_totalMeMoney[i] = 0;
            }
        }

        for (int i = 0; i < m_totalMoneyCuaCuoc.Length; i++) {
            m_totalMoneyCuaCuoc[i].text = "0";
        }

        for (int i = 0; i < m_totalMeMoneyCuaCuoc.Length; i++) {
            m_totalMeMoneyCuaCuoc[i].text = "0";
        }
        //So tien cuoc thong ke.
        //--------------------
    }

    private void CreateIndexPos() {
        m_indexPos = new List<int>();
        for (int i = 0; i < 9; i++) {
            m_indexPos.Add(i);
        }
    }

    private void ClearIndexPos() {
        if (m_indexPos.Count > 0) {
            m_indexPos.Clear();
        }
    }

    private void ClearBaixocdia() {
        if (m_baiXocdia != null && m_baiXocdia.Count > 0) {
            for (int i = 0; i < m_baiXocdia.Count; i++) {
                Destroy(m_baiXocdia[i]);
            }

            m_baiXocdia.Clear();
        }
    }

    public override void setMaster(string nick) {
        base.setMaster(nick);
        if (nick != "") {
            SystemIsMaster = false;
            PlayerLamcai = players[getPlayer(nick)].gameObject.transform;

            //Player-self lam cai.
            if (BaseInfo.gI().mainInfo.nick.Equals(nick)) {
                m_me_master = true;
                m_btnLamcaiLabel.text = "Hủy cái";
                SetVisibleButtonTable(false, false, false, true, true, true);
            } else {
                m_me_master = false;
                m_btnLamcaiLabel.text = "Làm cái";
                SetVisibleButtonTable(true, true, true, false, false, false);
            }

        } else {
            m_me_master = false;
            SystemIsMaster = true;
            PlayerLamcai = null;

            m_btnLamcaiLabel.text = "Làm cái";
            SetVisibleButtonTable(true, true, true, true, false, false);
            //SetEnableButtonTable (false, false, false, true);
        }

    }

    public override void onJoinTableSuccess(string master) {
        //Play sound.
        gameControl.sound.startVaobanAudio();
        //Play sound.

        for (int i = 0; i < nUsers; i++) {
            if (!players[i].isSit()) {
                players[i].setInvite(true);
            } else {
                players[i].setInvite(false);
            }
        }
        masterID = "";
        toggleLock.gameObject.SetActive(false);

        //Remove animation.
        m_winXocdia.RemoveWinXocdia();
        for (int i = 0; i < players.Length; i++) {
            players[i].GetComponent<XocdiaPlayer>().SetPlayerLose();
            players[i].GetComponent<XocdiaPlayer>().RemoveAllChip();
        }

        if (m_diaComponent != null) {
            m_diaComponent.SetAnimationXocdiaIdle();
        }

        //Chua dat cuoc
        m_isBetMe = false;
        m_isBetMeAgain = false;
        m_me_master = false;

        //Set lam cai.
        m_btnLamcaiLabel.text = "Làm cái";
        if (master.Equals("")) {
            //He thong lam cai.
            SystemIsMaster = true;
            SetVisibleButtonTable(true, true, true, true, false, false);
            //SetEnableButtonTable (false, false, false, true);
        } else {
            SetVisibleButtonTable(true, true, true, false, false, false);

            for (int i = 0; i < players.Length; i++) {
                if (players[i].getName().Equals(master)) {
                    players[i].setMaster(true);
                    SystemIsMaster = false;
                    PlayerLamcai = players[i].gameObject.transform;
                } else {
                    players[i].setMaster(false);
                }
            }
        }

        //Khong cho dat cuoc
        m_isDatCuocCua = false;

        //Set default loai chip.
        //Set chip cuoc
        // m_chipCuoc =  m_chips[0];

        //Set default animation muc cuoc.
        for (int i = 0; i < m_mucCuocAnim.Count; i++) {
            if (i == 0)
                m_mucCuocAnim[i].SetActive(true);
            else {
                m_mucCuocAnim[i].SetActive(false);
            }
        }
    }

    public override void onBack() {
        base.onBack();

        //Reset all data.
        PlayerLamcai = null;
        SystemIsMaster = true;
        ClearIndexPos();
        ClearBaixocdia();

        //Reset all time.
        m_timerWaiting.ResetAllTimer();
        m_winXocdia.RemoveWinXocdia();
        for (int i = 0; i < players.Length; i++) {
            players[i].GetComponent<XocdiaPlayer>().SetPlayerLose();
            players[i].GetComponent<XocdiaPlayer>().RemoveAllChip();
        }
        if (m_thongbaoXocdia != null) {
            m_thongbaoXocdia.SetAnimationThongbao_Idle();
        }
        if (m_thongbaoXocdia1 != null) {
            m_thongbaoXocdia1.EndFadeIn();
        }
        if (m_diaComponent != null) {
            m_diaComponent.SetAnimationXocdiaIdle();
        }

        //--------------------
        //So tien cuoc thong ke
        if (m_totalMoney == null) {
            m_totalMoney = new long[6];
        } else {
            for (int i = 0; i < m_totalMoney.Length; i++) {
                m_totalMoney[i] = 0;
            }
        }

        if (m_totalMeMoney == null) {
            m_totalMeMoney = new long[6];
        } else {
            for (int i = 0; i < m_totalMeMoney.Length; i++) {
                m_totalMeMoney[i] = 0;
            }
        }

        for (int i = 0; i < m_totalMoneyCuaCuoc.Length; i++) {
            m_totalMoneyCuaCuoc[i].text = "0";
        }

        for (int i = 0; i < m_totalMeMoneyCuaCuoc.Length; i++) {
            m_totalMeMoneyCuaCuoc[i].text = "0";
        }
        //So tien cuoc thong ke.
        //--------------------

        m_isBetMe = false;
        m_isBetMeAgain = false;
    }

    public override void setMasterSecond(string master) {
        for (int i = 0; i < nUsers; i++) {
            if (!players[i].isSit()) {
                players[i].setInvite(true);
            } else {
                players[i].setInvite(false);
            }
        }
        masterID = "";
        toggleLock.gameObject.SetActive(false);
    }

    public override void onTimeAuToStart(sbyte time) {

        SetEnableButtonTable(false, false, false, true, false, false);

        //--------------------
        //So tien cuoc thong ke
        if (m_totalMoney == null) {
            m_totalMoney = new long[6];
        } else {
            for (int i = 0; i < m_totalMoney.Length; i++) {
                m_totalMoney[i] = 0;
            }
        }

        if (m_totalMeMoney == null) {
            m_totalMeMoney = new long[6];
        } else {
            for (int i = 0; i < m_totalMeMoney.Length; i++) {
                m_totalMeMoney[i] = 0;
            }
        }

        for (int i = 0; i < m_totalMoneyCuaCuoc.Length; i++) {
            m_totalMoneyCuaCuoc[i].text = "0";
        }

        for (int i = 0; i < m_totalMeMoneyCuaCuoc.Length; i++) {
            m_totalMeMoneyCuaCuoc[i].text = "0";
        }

        if (m_diaComponent != null) {
            m_diaComponent.SetAnimationXocdiaIdle();
        }

        //Chua dat cuoc.
        m_isBetMe = false;
        m_isBetMeAgain = false;

        //Khong cho dat cuoc
        m_isDatCuocCua = false;

        //Clear bai xoc dia cu.
        ClearBaixocdia();

        //Set timer.
        if (m_thongbaoXocdia != null) {
            m_thongbaoXocdia.SetLbThongbao("Chờ bắt đầu ván mới");
            m_thongbaoXocdia.SetAnimationThongbao_Xuong();
        }

        if (m_diaComponent != null) {
            m_diaComponent.SetAnimationUpbat();
        }

        if (m_timerWaiting != null) {
            m_timerWaiting.setTimeAutoStart(time);
        }
        //Set timer.
    }

    public void OnChipMove(string playerName) {
        if (BaseInfo.gI().mainInfo.nick.Equals(playerName))
            m_chipStopMove = false;
    }

    public void OnChipMoveFinish(string playerName) {
        if (BaseInfo.gI().mainInfo.nick.Equals(playerName))
            m_chipStopMove = true;
    }

    public override void onBeGinXocDia(int time) {
        //Play sound.
        gameControl.sound.startXocdiaAudio();
        //Play sound.

        if (m_diaComponent != null) {
            m_diaComponent.SetAnimationXocdiaIdle();
        }
        SetEnableButtonTable(false, false, false, false, false, false);

        if (m_timerWaiting != null) {
            m_timerWaiting.setTimeAutoStart(0);
            m_timerWaiting.hideTimeWaiting();
            m_thongbaoXocdia.SetAnimationThongbao_Xuong();
            m_thongbaoXocdia.SetLbThongbao("Nhà cái bắt đầu xóc");

            m_timerWaiting.setTimeBeginXocdia(time);
        }
        if (m_diaComponent != null) {
            m_diaComponent.SetAnimationXocdia();
        }
    }

    public override void onBeginXocDiaTimeDatcuoc(int time) {
        if (m_diaComponent != null) {
            m_diaComponent.SetAnimationXocdiaIdle();
        }

        if (m_me_master == true) {
            SetEnableButtonTable(false, false, false, false, false, false);
        } else {
            SetEnableButtonTable(true, true, true, false, false, false);
        }

        //Cho phep dat cuoc
        m_isDatCuocCua = true;

        if (time > 0) {
            if (m_timerWaiting != null) {
                m_timerWaiting.setTimeBeginXocdia(0);
                m_timerWaiting.hideTimeWaiting();
                m_timerWaiting.setTimeBeginDatcuoc(time);
            }
            if (m_thongbaoXocdia != null) {
                m_thongbaoXocdia.SetLbThongbao("Đặt cược");
                m_thongbaoXocdia.SetAnimationThongbao_Xuong();
            }
        }
    }

    public override void onXocDiaMobat(int numRed) {

        SetEnableButtonTable(false, false, false, false, false, false);

        //Khong cho dat cuoc
        m_isDatCuocCua = false;

        //Tao list index position
        CreateIndexPos();

        if (m_timerWaiting != null) {
            m_timerWaiting.setTimeBeginDatcuoc(0);
            m_timerWaiting.setTimeBeginDungcuoc(0);
            m_timerWaiting.hideTimeWaiting();
        }

        if (m_thongbaoXocdia != null) {
            m_thongbaoXocdia.SetLbThongbao("Mở bát");
            m_thongbaoXocdia.SetAnimationThongbao_Xuong();
        }

        if (m_diaComponent != null) {
            m_diaComponent.SetAnimationMobat();
        }

        m_baiXocdia = new List<GameObject>();
        int numWhite = 4 - numRed;
        for (int i = 0; i < numRed; i++) {
            //Random position
            int rd = Random.Range(0, m_indexPos.Count);
            int index = m_indexPos[rd];

            GameObject go = Instantiate(xdRed) as GameObject;
            go.transform.SetParent(m_baixocdia, true);
            go.transform.position = m_pos[index].position;
            go.transform.localScale = Vector3.one;

            m_indexPos.Remove(index);
            m_baiXocdia.Add(go);
        }

        for (int i = 0; i < numWhite; i++) {
            //Random position
            int rd = Random.Range(0, m_indexPos.Count);
            int index = m_indexPos[rd];

            GameObject go = Instantiate(xdWhite) as GameObject;
            go.transform.SetParent(m_baixocdia, true);
            go.transform.position = m_pos[index].position;
            go.transform.localScale = Vector3.one;

            m_indexPos.Remove(index);
            m_baiXocdia.Add(go);
        }

        m_chipRed = numRed;

        //Clear index
        ClearIndexPos();

        //Reset data.
        m_double_bet_me = 0;
    }

    public override void onXocdiaNhanCacMucCuoc(long muc1, long muc2, long muc3, long muc4) {
        if (m_cacMucCuoc == null)
            return;

        m_cacMucCuoc[0].text = formatMoney(muc1);
        m_cacMucCuoc[1].text = formatMoney(muc2);
        m_cacMucCuoc[2].text = formatMoney(muc3);
        m_cacMucCuoc[3].text = formatMoney(muc4);

        //Set default muc cuoc.
        m_mucCuoc = muc1;
        //Set default animation muc cuoc.
        for (int i = 0; i < m_mucCuocAnim.Count; i++) {
            if (i == 0)
                m_mucCuocAnim[i].SetActive(true);
            else {
                m_mucCuocAnim[i].SetActive(false);
            }
        }
    }

    public override void onXocDiaHistory(List<int> aIDs) {
        if (m_lsCount == null) {
            m_lsCount = new List<GameObject>();
        } else {
            for (int i = 0; i < m_lsCount.Count; i++) {
                Destroy(m_lsCount[i]);
            }
            m_lsCount.Clear();
        }

        if (aIDs == null) {
            return;
        }

        int len = aIDs.Count;
        int iCount = 0;
        int jCount = 0;
        int nChan = 0;
        int nLe = 0;
        for (int i = 0; i < len; i++) {
            GameObject go;
            if (aIDs[i] == 0) {
                nChan++;
                go = Instantiate(m_lsTrang) as GameObject;
            } else {
                nLe++;
                go = Instantiate(m_lsDo) as GameObject;
            }

            go.transform.SetParent(m_bangLichsu, true);
            go.transform.localScale = Vector3.one;

            go.transform.localPosition = new Vector3(m_posLichsu.localPosition.x + iCount * m_deltaX,
                 m_posLichsu.localPosition.y + jCount * m_deltaY,
                 m_posLichsu.localPosition.z);

            iCount++;
            if ((iCount % 8) == 0) {
                {
                    iCount = 0;
                    jCount++;
                }
            }

            //Save total go lich su.
            //We will remove go lich su after function is called again.
            m_lsCount.Add(go);
        }

        if (m_nChan != null) {
            m_nChan.text = nChan.ToString();
        }

        if (m_nLe != null) {
            m_nLe.text = nLe.ToString();
        }
    }

    public void OnClickChangeMucCuoc(UILabel lbMucCuoc, UILabel lbId) {
        //Play sound.
        gameControl.sound.clickBtnAudioXocdia();
        //Play sound.

        string temp = lbMucCuoc.text;
        long muccuoc = 0;
        if (temp.Contains("K")) {
            string sCuoc = temp.Substring(0, temp.Length - 1);
            if (sCuoc.Contains(",")) {
                string[] ttt = System.Text.RegularExpressions.Regex.Split(sCuoc, ",");
                if (ttt.Length == 2) {
                    int m1 = System.Convert.ToInt32(ttt[0]);
                    int m2 = System.Convert.ToInt32(ttt[1]);

                    muccuoc = m1 * 1000 + m2 * 100;
                } else if (ttt.Length == 3) {

                }
            } else {
                muccuoc = System.Convert.ToInt32(sCuoc);
                muccuoc = muccuoc * 1000;
            }

        } else if (temp.Contains("M")) {
            string sCuoc = temp.Substring(0, temp.Length - 1);
            if (sCuoc.Contains(",")) {
                string[] ttt = System.Text.RegularExpressions.Regex.Split(sCuoc, ",");
                if (ttt.Length == 2) {
                    int m1 = System.Convert.ToInt32(ttt[0]);
                    int m2 = System.Convert.ToInt32(ttt[1]);

                    muccuoc = m1 * 1000000 + m2 * 100000;
                } else if (ttt.Length == 3) {

                }

            } else {
                muccuoc = System.Convert.ToInt32(sCuoc);
                muccuoc = muccuoc * 1000000;
            }
        } else {
            muccuoc = System.Convert.ToInt32(temp);
        }

        m_mucCuoc = muccuoc;

        //Check button
        switch (lbId.text) {
            case "1":
                //Set chip cuoc
                // m_chipCuoc =  m_chips[0];

                //Set animation
                for (int i = 0; i < m_mucCuocAnim.Count; i++) {
                    if (i == 0)
                        m_mucCuocAnim[i].SetActive(true);
                    else {
                        m_mucCuocAnim[i].SetActive(false);
                    }
                }
                break;
            case "2":
                //Set chip cuoc
                // m_chipCuoc =  m_chips[1];

                //Set animation
                for (int i = 0; i < m_mucCuocAnim.Count; i++) {
                    if (i == 1)
                        m_mucCuocAnim[i].SetActive(true);
                    else {
                        m_mucCuocAnim[i].SetActive(false);
                    }
                }
                break;
            case "3":
                //Set chip cuoc
                // m_chipCuoc =  m_chips[2];

                //Set animation
                for (int i = 0; i < m_mucCuocAnim.Count; i++) {
                    if (i == 2)
                        m_mucCuocAnim[i].SetActive(true);
                    else {
                        m_mucCuocAnim[i].SetActive(false);
                    }
                }
                break;
            case "4":
                //Set chip cuoc
                // m_chipCuoc =  m_chips[3];

                //Set animation
                for (int i = 0; i < m_mucCuocAnim.Count; i++) {
                    if (i == 3)
                        m_mucCuocAnim[i].SetActive(true);
                    else {
                        m_mucCuocAnim[i].SetActive(false);
                    }
                }
                break;
        }
    }

    public void OnClickDatCuocCua(UILabel lbId) {
        XocdiaPlayer xocdiaPlayer = players[0].GetComponent<XocdiaPlayer>();
        xocdiaPlayer.DatCuoc(lbId.text, m_mucCuoc, m_isDatCuocCua);
    }

    public override void onXocDiaDatcuoc(string nick, sbyte cua, long money, int typeCHIP) {
        //Play sound.
        gameControl.sound.MoneyAudio();
        //Play sound.

        m_isBetMe = true;

        XocdiaPlayer xocdiaPlayer = null;

        for (int i = 0; i < players.Length; i++) {
            if (players[i].getName().Equals(nick)) {
                if (xocdiaPlayer == null) {
                    xocdiaPlayer = players[i].GetComponent<XocdiaPlayer>();
                }
                xocdiaPlayer.ActionChipDatcuoc(cua, typeCHIP);
            }
        }

        switch (cua) {
            case 0:
                m_totalMoney[0] += money;
                m_totalMoneyCuaCuoc[0].text = BaseInfo.formatMoney(m_totalMoney[0]);
                if (BaseInfo.gI().mainInfo.nick.Equals(nick)) {
                    m_totalMeMoney[0] += money;
                    m_totalMeMoneyCuaCuoc[0].text = BaseInfo.formatMoney(m_totalMeMoney[0]);
                }

                //Cache player bet the cua chan
                m_playerCCC.Add(players[getPlayer(nick)]);
                break;
            case 1:
                m_totalMoney[1] += money;
                m_totalMoneyCuaCuoc[1].text = BaseInfo.formatMoney(m_totalMoney[1]);
                if (BaseInfo.gI().mainInfo.nick.Equals(nick)) {
                    m_totalMeMoney[1] += money;
                    m_totalMeMoneyCuaCuoc[1].text = BaseInfo.formatMoney(m_totalMeMoney[1]);
                }

                //Cache player bet the cua le
                m_playerCCL.Add(players[getPlayer(nick)]);
                break;
            case 2:
                m_totalMoney[2] += money;
                m_totalMoneyCuaCuoc[2].text = BaseInfo.formatMoney(m_totalMoney[2]);
                if (BaseInfo.gI().mainInfo.nick.Equals(nick)) {
                    m_totalMeMoney[2] += money;
                    m_totalMeMoneyCuaCuoc[2].text = BaseInfo.formatMoney(m_totalMeMoney[2]);
                }
                break;
            case 3:
                m_totalMoney[3] += money;
                m_totalMoneyCuaCuoc[3].text = BaseInfo.formatMoney(m_totalMoney[3]);
                if (BaseInfo.gI().mainInfo.nick.Equals(nick)) {
                    m_totalMeMoney[3] += money;
                    m_totalMeMoneyCuaCuoc[3].text = BaseInfo.formatMoney(m_totalMeMoney[3]);
                }
                break;
            case 4:
                m_totalMoney[4] += money;
                m_totalMoneyCuaCuoc[4].text = BaseInfo.formatMoney(m_totalMoney[4]);
                if (BaseInfo.gI().mainInfo.nick.Equals(nick)) {
                    m_totalMeMoney[4] += money;
                    m_totalMeMoneyCuaCuoc[4].text = BaseInfo.formatMoney(m_totalMeMoney[4]);
                }
                break;
            case 5:
                m_totalMoney[5] += money;
                m_totalMoneyCuaCuoc[5].text = BaseInfo.formatMoney(m_totalMoney[5]);
                if (BaseInfo.gI().mainInfo.nick.Equals(nick)) {
                    m_totalMeMoney[5] += money;
                    m_totalMeMoneyCuaCuoc[5].text = BaseInfo.formatMoney(m_totalMeMoney[5]);
                }
                break;
            default:
                break;
        }
    }

    public void OnClickBtnDatGapDoi() {
        //Play sound.
        gameControl.sound.clickBtnAudioXocdia();
        //Play sound.

        if (m_me_master == true) {
            return;
        }

        if (m_double_bet_me < DOUBLE_BET_MAX && m_isBetMe == true) {
            SendData.onSendDatGapDoi();
        }
    }

    public void OnClickDatLai() {
        //Play sound.
        gameControl.sound.clickBtnAudioXocdia();
        //Play sound.

        if (m_isBetPrevious == true && m_isBetMeAgain == false) {
            SendData.onSendDatLai();
        }

    }

    public void OnClickHuyCuoc() {
        if (m_chipStopMove == false) {
            return;
        }

        //Play sound.
        gameControl.sound.clickBtnAudioXocdia();
        //Play sound.

        if (m_isBetMe == true) {
            SendData.onSendHuyCuoc();
        }
    }

    public void OnClickLamCai() {
        //Play sound.
        gameControl.sound.clickBtnAudioXocdia();
        //Play sound.
        //if( m_me_master == false) {
        SendData.onSendLamCai();
        //}
    }

    public void OnClickHuyChan() {
        //Play sound.
        gameControl.sound.clickBtnAudioXocdia();
        //Play sound.

        SendData.onSendChucNangCai((byte)2);
        if (m_me_master)
            SetEnableButtonTable(false, false, false, false, false, false);
    }

    public void OnClickHuyLe() {
        //Play sound.
        gameControl.sound.clickBtnAudioXocdia();
        //Play sound.

        SendData.onSendChucNangCai((byte)1);
        if (m_me_master)
            SetEnableButtonTable(false, false, false, false, false, false);
    }

    public override void onXocDiaDatGapdoi(string nick, sbyte socua, Message message) {
        //Play sound.
        gameControl.sound.MoneyAudio();
        //Play sound.

        if (BaseInfo.gI().mainInfo.nick.Equals(nick)) {
            m_double_bet_me++;
        }

        if (m_double_bet_me >= DOUBLE_BET_MAX) {
            //Disable btnDatX2
            m_btnDatGapDoi.enabled = false;
            m_btnDatGapDoi.defaultColor = new Color(0, 100.0f / 255.0f, 100.0f / 255.0f);
        } else {
            //Enable btnDatx2
            m_btnDatGapDoi.enabled = true;
            m_btnDatGapDoi.defaultColor = new Color(1.0f, 1.0f, 1.0f);
        }

        XocdiaPlayer xocdiaPlayer = null;
        for (int i = 0; i < players.Length; i++) {
            if (players[i].getName().Equals(nick)) {
                xocdiaPlayer = players[i].gameObject.GetComponent<XocdiaPlayer>();
            }
        }

        for (int i = 0; i < socua; i++) {
            sbyte cua = message.reader().ReadByte();
            xocdiaPlayer.ActionChipDatGapDoi(cua);
            //xocdiaPlayer.CalculateMoneyDoubleBet (cua);
        }
    }

    public override void onXocDiaDatlai(string nick, sbyte socua, Message message) {
        //Play sound.
        gameControl.sound.MoneyAudio();
        //Play sound.

        m_isBetMe = true;
        m_isBetMeAgain = true;

        for (int i = 0; i < socua; i++) {
            sbyte cua = message.reader().ReadByte();
            sbyte a = message.reader().ReadByte();
            if (a == 1) {
                sbyte soloaichip = message.reader().ReadByte();
                for (int j = 0; j < soloaichip; j++) {
                    sbyte loaichip = message.reader().ReadByte();
                    int sochip = message.reader().ReadInt();
                    for (int k = 0; k < sochip; k++) {
                        players[getPlayer(nick)].GetComponent<XocdiaPlayer>().ActionChipDatcuoc(cua, loaichip);
                    }
                }
            }
        }
        //end
    }

    public override void onXocDiaHuycuoc(string nick, long moneycua0, long moneycua1, long moneycua2,
    long moneycua3, long moneycua4, long moneycua5) {
        m_isBetMe = false;
        m_isBetMeAgain = false;
        players[getPlayer(nick)].GetComponent<XocdiaPlayer>().ActionTraTienCuoc(moneycua0, moneycua1, moneycua2, moneycua3,
            moneycua4, moneycua5);
    }

    public override void onXocDiaUpdateCua(Message message) {
        try {
            string nick = message.reader().ReadUTF();
            for (int i = 0; i < 6; i++) {
                m_totalMoney[i] = message.reader().ReadLong();
                m_totalMoneyCuaCuoc[i].text = BaseInfo.formatMoney(m_totalMoney[i]);
                if (BaseInfo.gI().mainInfo.nick.Equals(nick)) {
                    m_totalMeMoney[i] = message.reader().ReadLong();
                    m_totalMeMoneyCuaCuoc[i].text = BaseInfo.formatMoney(m_totalMeMoney[i]);
                }
            }
        } catch (System.IO.IOException e) {
            Debug.LogException(e);
        }
    }

    public void SetVisibleButtonTable(bool btn1, bool btn2, bool btn3, bool btn4, bool btn5, bool btn6) {
        if (btn1 == true) {
            m_btnDatGapDoi.gameObject.SetActive(true);
        } else {
            m_btnDatGapDoi.gameObject.SetActive(false);
        }

        if (btn2 == true) {
            m_btnDatLai.gameObject.SetActive(true);
        } else {
            m_btnDatLai.gameObject.SetActive(false);
        }

        if (btn3 == true) {
            m_btnHuyCuoc.gameObject.SetActive(true);
        } else {
            m_btnHuyCuoc.gameObject.SetActive(false);
        }

        if (btn4 == true) {
            m_btnLamCai.gameObject.SetActive(true);
        } else {
            m_btnLamCai.gameObject.SetActive(false);
        }

        if (btn5 == true) {
            m_btnHuyChan.gameObject.SetActive(true);
        } else {
            m_btnHuyChan.gameObject.SetActive(false);
        }

        if (btn6 == true) {
            m_btnHuyLe.gameObject.SetActive(true);
        } else {
            m_btnHuyLe.gameObject.SetActive(false);
        }
    }

    public void SetEnableButtonTable(bool btn1, bool btn2, bool btn3, bool btn4, bool btn5, bool btn6) {
        if (btn1 == true) {
            m_btnDatGapDoi.enabled = true;
            m_btnDatGapDoi.defaultColor = Color.white;
            m_btnDatGapDoiLabel.color = Color.white;
        } else {
            m_btnDatGapDoi.enabled = false;
            m_btnDatGapDoi.defaultColor = COLOR_DISABLE_BTN;
            m_btnDatGapDoiLabel.color = COLOR_DISABLE_TXT_BTN;
        }

        if (btn2 == true) {
            m_btnDatLai.enabled = true;
            m_btnDatLai.defaultColor = Color.white;
            m_btnDatLaiLabel.color = Color.white;
        } else {
            m_btnDatLai.enabled = false;
            m_btnDatLai.defaultColor = COLOR_DISABLE_BTN;
            m_btnDatLaiLabel.color = COLOR_DISABLE_TXT_BTN;
        }

        if (btn3 == true) {
            m_btnHuyCuoc.enabled = true;
            m_btnHuyCuoc.defaultColor = Color.white;
            m_btnHuyCuocLabel.color = Color.white;
        } else {
            m_btnHuyCuoc.enabled = false;
            m_btnHuyCuoc.defaultColor = COLOR_DISABLE_BTN;
            m_btnHuyCuocLabel.color = COLOR_DISABLE_TXT_BTN;
        }

        if (btn4 == true) {
            m_btnLamCai.enabled = true;
            m_btnLamCai.defaultColor = Color.white;
            m_btnLamcaiLabel.color = Color.white;
        } else {
            m_btnLamCai.enabled = false;
            m_btnLamCai.defaultColor = COLOR_DISABLE_BTN;
            m_btnLamcaiLabel.color = COLOR_DISABLE_TXT_BTN;
        }

        if (btn5 == true) {
            m_btnHuyChan.enabled = true;
            m_btnHuyChan.defaultColor = Color.white;
            m_btnHuyChanLabel.color = Color.white;
        } else {
            m_btnHuyChan.enabled = false;
            m_btnHuyChan.defaultColor = COLOR_DISABLE_BTN;
            m_btnHuyChanLabel.color = COLOR_DISABLE_TXT_BTN;
        }

        if (btn6 == true) {
            m_btnHuyLe.enabled = true;
            m_btnHuyLe.defaultColor = Color.white;
            m_btnHuyLeLabel.color = Color.white;
        } else {
            m_btnHuyLe.enabled = false;
            m_btnHuyLe.defaultColor = COLOR_DISABLE_BTN;
            m_btnHuyLeLabel.color = COLOR_DISABLE_TXT_BTN;
        }
    }

    public override void onInfome(Message message) {
        ClearIndexPos();
        ClearBaixocdia();

        //Reset all time.
        m_timerWaiting.ResetAllTimer();
        m_winXocdia.RemoveWinXocdia();
        for (int i = 0; i < players.Length; i++) {
            players[i].GetComponent<XocdiaPlayer>().SetPlayerLose();
            players[i].GetComponent<XocdiaPlayer>().RemoveAllChip();
        }
        if (m_thongbaoXocdia != null) {
            m_thongbaoXocdia.SetAnimationThongbao_Idle();
        }
        if (m_thongbaoXocdia1 != null) {
            m_thongbaoXocdia1.EndFadeIn();
        }
        if (m_diaComponent != null) {
            m_diaComponent.SetAnimationXocdiaIdle();
        }

        //--------------------
        //So tien cuoc thong ke
        if (m_totalMoney == null) {
            m_totalMoney = new long[6];
        } else {
            for (int i = 0; i < m_totalMoney.Length; i++) {
                m_totalMoney[i] = 0;
            }
        }

        if (m_totalMeMoney == null) {
            m_totalMeMoney = new long[6];
        } else {
            for (int i = 0; i < m_totalMeMoney.Length; i++) {
                m_totalMeMoney[i] = 0;
            }
        }

        for (int i = 0; i < m_totalMoneyCuaCuoc.Length; i++) {
            m_totalMoneyCuaCuoc[i].text = "0";
        }

        for (int i = 0; i < m_totalMeMoneyCuaCuoc.Length; i++) {
            m_totalMeMoneyCuaCuoc[i].text = "0";
        }
        //So tien cuoc thong ke.
        //--------------------

        // m_isBetMe = false;
        // m_isBetMeAgain = false;

        try {
            sbyte status = message.reader().ReadByte();
            int time = message.reader().ReadInt();

            switch (status) {
                case 1:
                    //Ko cho dat cuoc
                    m_isDatCuocCua = false;
                    SetEnableButtonTable(false, false, false, false, false, false);

                    if (m_thongbaoXocdia != null) {
                        m_thongbaoXocdia.SetAnimationThongbao_Xuong();
                        m_thongbaoXocdia.SetLbThongbao("Nhà cái bắt đầu xóc");
                    }
                    if (m_timerWaiting != null) {
                        m_timerWaiting.setTimeAutoStart(0);
                        m_timerWaiting.hideTimeWaiting();
                        m_timerWaiting.setTimeBeginXocdia(time);
                    }
                    if (m_diaComponent != null) {
                        m_diaComponent.SetAnimationXocdia();
                    }
                    break;
                case 2:
                    //Cho phep dat cuoc
                    m_isDatCuocCua = true;
                    SetEnableButtonTable(true, true, true, false, true, true);

                    if (m_thongbaoXocdia != null) {
                        m_thongbaoXocdia.SetLbThongbao("Đặt cược");
                        m_thongbaoXocdia.SetAnimationThongbao_Xuong();
                    }
                    if (m_timerWaiting != null) {
                        m_timerWaiting.setTimeBeginXocdia(0);
                        m_timerWaiting.hideTimeWaiting();
                        m_timerWaiting.setTimeBeginDatcuoc(time);
                    }
                    break;
                case 3:
                    //Ko cho dat cuoc
                    m_isDatCuocCua = false;
                    SetEnableButtonTable(false, false, false, false, false, false);

                    if (m_thongbaoXocdia != null) {
                        m_thongbaoXocdia.SetLbThongbao("Nhà cái ngừng nhận cược");
                        m_thongbaoXocdia.SetAnimationThongbao_Xuong();
                    }
                    if (m_timerWaiting != null) {
                        m_timerWaiting.setTimeBeginDatcuoc(0);
                        m_timerWaiting.hideTimeWaiting();
                        m_timerWaiting.setTimeBeginDungcuoc(time);
                    }
                    break;
            }
        } catch (System.IO.IOException e) {
            Debug.LogException(e);
        }
    }

    public override void onFinishGame(Message message) {
        //Set win
        if (m_winXocdia != null) {
            m_winXocdia.SetWinXocdia(m_chipRed, players);
        }

        //Clear player data
        if (m_playerCCC != null) {
            m_playerCCC.Clear();
        }

        if (m_playerCCL != null) {
            m_playerCCL.Clear();
        }

        //Kiem tra xem van truoc co dat cuoc ko?
        if (m_isBetMe == true) {
            m_isBetPrevious = true;
        } else {
            m_isBetPrevious = false;
        }

        try {
            int cua1 = message.reader().ReadByte();
            int cua2 = message.reader().ReadByte();
            //set_anim_cuato (cua1);
            //set_anim_cuanho (cua2);

            int size = message.reader().ReadByte();
            for (int i = 0; i < size; i++) {
                string name = message.reader().ReadUTF();
                long moneyEarn = message.reader().ReadLong();
                if (moneyEarn > 0) {
                    players[getPlayer(name)].GetComponent<XocdiaPlayer>().SetPlayerWin();
                    //Play sound.
                    gameControl.sound.startWinAudioXocdia();
                    //Play sound.
                } else {
                    if (moneyEarn < 0) {
                        //Play sound.
                        gameControl.sound.startLoseAudioXocdia();
                        //Play sound.
                    }

                    players[getPlayer(name)].GetComponent<XocdiaPlayer>().SetPlayerLose();
                }
            }
        } catch (System.IO.IOException e) {
            Debug.LogException(e);
        }
    }

    public override void onXocDiaChucNangHuycua(Message message) {
        try {
            sbyte type = message.reader().ReadByte();
            switch (type) {
                case 1://Huy cua le.
                    if (m_thongbaoXocdia1 != null) {
                        m_thongbaoXocdia1.SetLbThongbao("Nhà cái hủy của lẻ");
                        m_thongbaoXocdia1.ShowThongbao1();
                    }

                    //Action tra tien player da cuoc
                    if (m_playerCCL != null && m_playerCCL.Count > 0) {
                        for (int i = 0; i < m_playerCCL.Count; i++) {
                            m_playerCCL[i].GetComponent<XocdiaPlayer>().ActionTraTienCuoc(-1, 1, -1, -1, -1, -1);
                        }
                    }

                    //Reset tien.
                    m_totalMoney[1] = 0;
                    m_totalMoneyCuaCuoc[1].text = BaseInfo.formatMoney(m_totalMoney[1]);
                    m_totalMeMoney[1] = 0;
                    m_totalMeMoneyCuaCuoc[1].text = BaseInfo.formatMoney(m_totalMeMoney[1]);

                    break;
                case 2://Huy cua chan.
                    if (m_thongbaoXocdia1 != null) {
                        m_thongbaoXocdia1.SetLbThongbao("Nhà cái hủy của chẵn");
                        m_thongbaoXocdia1.ShowThongbao1();
                    }

                    //Action tra tien player da cuoc
                    if (m_playerCCC != null && m_playerCCC.Count > 0) {
                        for (int i = 0; i < m_playerCCC.Count; i++) {
                            m_playerCCC[i].GetComponent<XocdiaPlayer>().ActionTraTienCuoc(1, -1, -1, -1, -1, -1);
                        }
                    }

                    //Reset tien.
                    m_totalMoney[0] = 0;
                    m_totalMoneyCuaCuoc[0].text = BaseInfo.formatMoney(m_totalMoney[0]);
                    m_totalMeMoney[0] = 0;
                    m_totalMeMoneyCuaCuoc[0].text = BaseInfo.formatMoney(m_totalMeMoney[0]);

                    break;
            }
        } catch (System.IO.IOException e) {
            Debug.LogException(e);
        }
    }

    public override void onXocDiaBeginTimerDungCuoc(Message message) {
        SetEnableButtonTable(false, false, false, false, true, true);
        try {
            int time = message.reader().ReadByte();
            if (time > 1) {
                if (m_thongbaoXocdia != null) {
                    m_thongbaoXocdia.SetLbThongbao("Nhà cái ngừng nhận cược");
                    m_thongbaoXocdia.SetAnimationThongbao_Xuong();
                }
                if (m_timerWaiting != null) {
                    m_timerWaiting.setTimeBeginDatcuoc(0);
                    m_timerWaiting.hideTimeWaiting();
                    m_timerWaiting.setTimeBeginDungcuoc(time);
                }
            }
        } catch (System.IO.IOException e) {
            Debug.LogException(e);
        }
    }

    public static string formatMoney(long money) {
        string strMoney = "";

        try {
            if (money < 0) {
                money = 0;
            }
            // strMoney.delete(0, strMoney.length());
            long strm = (long)(money / 1000000);
            long strk = 0;
            long strh = 0;
            if (strm > 0) {
                strk = (long)((money % 1000000) / 1000);
                long sss = (long)strk / 100;
                if (strk > 100) {
                    strMoney = strm + "," + sss + "M";
                } else if (strMoney.Length > 0) {
                    strMoney = strm + "," + "0" + strk + "M";
                }

            } else {
                strk = (long)(money / 1000);
                if (strk > 0) {
                    strh = (money % 1000 / 100);
                    if (strh > 0) {
                        strMoney = strk + "," + strh + "K";
                    } else if (strMoney.Length >= 0) {
                        strMoney = strk + "K";
                    }

                } else if (strMoney.Length >= 0) {
                    strMoney = money + "";
                }
            }
        } catch (System.Exception e) {
            Debug.LogException(e);

        }
        return strMoney.ToString();
    }
}