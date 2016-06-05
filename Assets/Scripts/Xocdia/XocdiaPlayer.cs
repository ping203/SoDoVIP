using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class XocdiaPlayer : ABSUser {
    public GameObject m_playerWin;
    public ThongbaoXocdia m_thongbaoXocdia;
    public List<GameObject> m_chips;
    public GameObject m_chipXocdiaParent;
    public Transform m_batTransfrom;

    //Dat cuoc.
    public List<GameObject> m_chipSpawn0 = new List<GameObject>();
    public List<GameObject> m_chipSpawn1 = new List<GameObject>();
    public List<GameObject> m_chipSpawn2 = new List<GameObject>();
    public List<GameObject> m_chipSpawn3 = new List<GameObject>();
    public List<GameObject> m_chipSpawn4 = new List<GameObject>();
    public List<GameObject> m_chipSpawn5 = new List<GameObject>();
    private Vector3 m_originPos;
    private Vector3 m_targetPos;

    float offsetLarge_X = 10;
    float offsetLarge_Y = 40;
    float offset = 20;
    //Vi tri chip nam tren cua lon

    public Transform[] cuadatcuoc;
    //private Vector2 m_cuaChanPos;
    //private Vector2 m_cuaLePos;
    ////Vi tri chip nam tren cua nho.
    //private Vector2 m_cuaNhoPos_1;//4 do
    //private Vector2 m_cuaNhoPos_2;//4 trang
    //private Vector2 m_cuaNhoPos_3;//1 trang, 3 do.
    //private Vector2 m_cuaNhoPos_4;//1 do, 3 trang.

    private Vector2 m_cuaChanPos = new Vector2(-120.0f, -50.0f);
    private Vector2 m_cuaLePos = new Vector2(120.0f, -50.0f);
    //Vi tri chip nam tren cua nho.
    private Vector2 m_cuaNhoPos_1 = new Vector2(-210.0f, -80.0f);//4 do
    private Vector2 m_cuaNhoPos_2 = new Vector2(-210.0f, 10.0f);//4 trang
    private Vector2 m_cuaNhoPos_3 = new Vector2(210.0f, -80.0f);//1 trang, 3 do.
    private Vector2 m_cuaNhoPos_4 = new Vector2(210.0f, 10.0f);//1 do, 3 trang.

    //Cache chip dat cuoc.
    private List<GameObject> m_chipDatCuoc0 = new List<GameObject>();
    private List<GameObject> m_chipDatCuoc1 = new List<GameObject>();
    private List<GameObject> m_chipDatCuoc2 = new List<GameObject>();
    private List<GameObject> m_chipDatCuoc3 = new List<GameObject>();
    private List<GameObject> m_chipDatCuoc4 = new List<GameObject>();
    private List<GameObject> m_chipDatCuoc5 = new List<GameObject>();
    //Chi tao ra toi da 20 chip object gap doi.
    private const int MAX_CHIP_DOUBLE = 15;

    public void Start() {
        base.Start();
        //m_cuaChanPos = cuadatcuoc[0].position;
        //m_cuaLePos = cuadatcuoc[1].position;
        //m_cuaNhoPos_1 = cuadatcuoc[2].position;
        //m_cuaNhoPos_2 = cuadatcuoc[3].position;
        //m_cuaNhoPos_3 = cuadatcuoc[4].position;
        //m_cuaNhoPos_4 = cuadatcuoc[5].position;
    }

    public void SetPlayerWin() {
        if (m_playerWin != null) {
            m_playerWin.SetActive(true);
            StartCoroutine(hideWin());
        }
    }

    IEnumerator hideWin() {
        yield return new WaitForSeconds(2);
        SetPlayerLose();
    }

    public void SetPlayerLose() {
        if (m_playerWin != null) {
            m_playerWin.SetActive(false);
        }
    }

    public void DatCuoc(string aCuaCuoc, long aMucCuoc, bool isDatCuocCua) {
        switch (aCuaCuoc) {
            case "cuachan":
                if (isDatCuocCua) {
                    SendData.onSendXocDiaDatCuoc((byte)0, aMucCuoc);
                } else {
                    if (m_thongbaoXocdia != null) {
                        m_thongbaoXocdia.SetLbThongbao("Chưa đến thời gian đặt cược !");
                        m_thongbaoXocdia.ShowThongbao1();
                    }
                }

                break;
            case "cuale":
                if (isDatCuocCua) {
                    SendData.onSendXocDiaDatCuoc((byte)1, aMucCuoc);
                } else {
                    if (m_thongbaoXocdia != null) {
                        m_thongbaoXocdia.SetLbThongbao("Chưa đến thời gian đặt cược !");
                        m_thongbaoXocdia.ShowThongbao1();
                    }
                }

                break;
            case "cuanho1":
                if (isDatCuocCua) {
                    //4 do
                    SendData.onSendXocDiaDatCuoc((byte)2, aMucCuoc);
                } else {
                    if (m_thongbaoXocdia != null) {
                        m_thongbaoXocdia.SetLbThongbao("Chưa đến thời gian đặt cược !");
                        m_thongbaoXocdia.ShowThongbao1();
                    }
                }

                break;
            case "cuanho2":
                if (isDatCuocCua) {
                    //4 trang
                    SendData.onSendXocDiaDatCuoc((byte)3, aMucCuoc);
                } else {
                    if (m_thongbaoXocdia != null) {
                        m_thongbaoXocdia.SetLbThongbao("Chưa đến thời gian đặt cược !");
                        m_thongbaoXocdia.ShowThongbao1();
                    }
                }

                break;
            case "cuanho3":
                if (isDatCuocCua) {
                    //1 trang, 3 do.
                    SendData.onSendXocDiaDatCuoc((byte)4, aMucCuoc);
                } else {
                    if (m_thongbaoXocdia != null) {
                        m_thongbaoXocdia.SetLbThongbao("Chưa đến thời gian đặt cược !");
                        m_thongbaoXocdia.ShowThongbao1();
                    }
                }

                break;
            case "cuanho4":
                if (isDatCuocCua) {
                    //1 do, 3 trang.
                    SendData.onSendXocDiaDatCuoc((byte)5, aMucCuoc);
                } else {
                    if (m_thongbaoXocdia != null) {
                        m_thongbaoXocdia.SetLbThongbao("Chưa đến thời gian đặt cược !");
                        m_thongbaoXocdia.ShowThongbao1();
                    }
                }

                break;
        }
    }

    public void ActionChipDatGapDoi(sbyte cua) {
        int size = 0;
        float xPos;
        float yPos;
        float zPos = 0.0f;
        GameObject go;
        float speed = 0.0f;
        m_originPos = gameObject.transform.localPosition;

        switch (cua) {
            case 0://cua chan
                size = m_chipDatCuoc0.Count;
                if (size > MAX_CHIP_DOUBLE) {
                    size = MAX_CHIP_DOUBLE;
                }
                for (int i = 0; i < size; i++) {
                    GameObject chipPrefab = m_chipDatCuoc0[i];

                    xPos = Random.Range(m_cuaChanPos.x - offsetLarge_X, m_cuaChanPos.x + offsetLarge_X);
                    yPos = Random.Range(m_cuaChanPos.y - offsetLarge_Y, m_cuaChanPos.y + offsetLarge_Y);
                    m_targetPos = new Vector3(xPos, yPos, zPos);

                    //go = Instantiate (chipPrefab) as GameObject;
                    go = ObjectPool.current.GetObject(chipPrefab);
                    go.transform.SetParent(m_chipXocdiaParent.transform, true);
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = m_originPos;
                    go.SetActive(true);

                    speed = 600.0f;
                    go.GetComponent<ChipXocdia>().MoveChip(true, m_targetPos, speed, getName());

                    m_chipSpawn0.Add(go);
                    m_chipDatCuoc0.Add(chipPrefab);
                }
                break;
            case 1://cua le
                size = m_chipDatCuoc1.Count;
                if (size > MAX_CHIP_DOUBLE) {
                    size = MAX_CHIP_DOUBLE;
                }
                for (int i = 0; i < size; i++) {
                    GameObject chipPrefab = m_chipDatCuoc1[i];

                    xPos = Random.Range(m_cuaLePos.x - offsetLarge_X, m_cuaLePos.x + offsetLarge_X);
                    yPos = Random.Range(m_cuaLePos.y - offsetLarge_Y, m_cuaLePos.y + offsetLarge_Y);
                    m_targetPos = new Vector3(xPos, yPos, zPos);

                    //go = Instantiate (chipPrefab) as GameObject;
                    go = ObjectPool.current.GetObject(chipPrefab);
                    go.transform.SetParent(m_chipXocdiaParent.transform, true);
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = m_originPos;
                    go.SetActive(true);

                    speed = 600.0f;
                    go.GetComponent<ChipXocdia>().MoveChip(true, m_targetPos, speed, getName());

                    m_chipSpawn1.Add(go);
                    m_chipDatCuoc1.Add(chipPrefab);
                }
                break;
            case 2://cua nho 1, 4 do.
                size = m_chipDatCuoc2.Count;
                if (size > MAX_CHIP_DOUBLE) {
                    size = MAX_CHIP_DOUBLE;
                }
                for (int i = 0; i < size; i++) {
                    GameObject chipPrefab = m_chipDatCuoc2[i];

                    xPos = Random.Range(m_cuaNhoPos_1.x - offset, m_cuaNhoPos_1.x + offset);
                    yPos = Random.Range(m_cuaNhoPos_1.y - offset, m_cuaNhoPos_1.y + offset);
                    m_targetPos = new Vector3(xPos, yPos, zPos);

                    //go = Instantiate (chipPrefab) as GameObject;
                    go = ObjectPool.current.GetObject(chipPrefab);
                    //go.SetActive (true);
                    go.transform.SetParent(m_chipXocdiaParent.transform, true);
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = m_originPos;
                    go.SetActive(true);

                    speed = 600.0f;
                    go.GetComponent<ChipXocdia>().MoveChip(true, m_targetPos, speed, getName());

                    m_chipSpawn2.Add(go);
                    m_chipDatCuoc2.Add(chipPrefab);
                }
                break;
            case 3://cua nho 2, 4 trang.
                size = m_chipDatCuoc3.Count;
                if (size > MAX_CHIP_DOUBLE) {
                    size = MAX_CHIP_DOUBLE;
                }
                for (int i = 0; i < size; i++) {
                    GameObject chipPrefab = m_chipDatCuoc3[i];

                    xPos = Random.Range(m_cuaNhoPos_2.x - offset, m_cuaNhoPos_2.x + offset);
                    yPos = Random.Range(m_cuaNhoPos_2.y - offset, m_cuaNhoPos_2.y + offset);
                    m_targetPos = new Vector3(xPos, yPos, zPos);

                    //go = Instantiate (chipPrefab) as GameObject;
                    go = ObjectPool.current.GetObject(chipPrefab);
                    //go.SetActive (true);
                    go.transform.SetParent(m_chipXocdiaParent.transform, true);
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = m_originPos;
                    go.SetActive(true);

                    speed = 600.0f;
                    go.GetComponent<ChipXocdia>().MoveChip(true, m_targetPos, speed, getName());

                    m_chipSpawn3.Add(go);
                    m_chipDatCuoc3.Add(chipPrefab);
                }
                break;
            case 4://cua nho 3, 1 trang + 3 do.
                size = m_chipDatCuoc4.Count;
                if (size > MAX_CHIP_DOUBLE) {
                    size = MAX_CHIP_DOUBLE;
                }
                for (int i = 0; i < size; i++) {
                    GameObject chipPrefab = m_chipDatCuoc4[i];

                    xPos = Random.Range(m_cuaNhoPos_3.x - offset, m_cuaNhoPos_3.x + offset);
                    yPos = Random.Range(m_cuaNhoPos_3.y - offset, m_cuaNhoPos_3.y + offset);
                    m_targetPos = new Vector3(xPos, yPos, zPos);

                    //go = Instantiate (chipPrefab) as GameObject;
                    go = ObjectPool.current.GetObject(chipPrefab);
                    //go.SetActive (true);
                    go.transform.SetParent(m_chipXocdiaParent.transform, true);
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = m_originPos;
                    go.SetActive(true);

                    speed = 600.0f;
                    go.GetComponent<ChipXocdia>().MoveChip(true, m_targetPos, speed, getName());

                    m_chipSpawn4.Add(go);
                    m_chipDatCuoc4.Add(chipPrefab);
                }
                break;
            case 5://cua nho 4, 1 do + 3 trang.
                size = m_chipDatCuoc5.Count;
                if (size > MAX_CHIP_DOUBLE) {
                    size = MAX_CHIP_DOUBLE;
                }
                for (int i = 0; i < size; i++) {
                    GameObject chipPrefab = m_chipDatCuoc5[i];

                    xPos = Random.Range(m_cuaNhoPos_4.x, m_cuaNhoPos_4.y);
                    yPos = Random.Range(m_cuaNhoPos_4.x, m_cuaNhoPos_4.y);
                    m_targetPos = new Vector3(xPos, yPos, zPos);

                    //go = Instantiate (chipPrefab) as GameObject;
                    go = ObjectPool.current.GetObject(chipPrefab);
                    //go.SetActive (true);
                    go.transform.SetParent(m_chipXocdiaParent.transform, true);
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = m_originPos;
                    go.SetActive(true);

                    speed = 600.0f;
                    go.GetComponent<ChipXocdia>().MoveChip(true, m_targetPos, speed, getName());

                    m_chipSpawn5.Add(go);
                    m_chipDatCuoc5.Add(chipPrefab);
                }
                break;
        }
    }

    public void ActionChipDatcuoc(sbyte cua, int typeCHIP) {
        float xPos;
        float yPos;
        float zPos = 0.0f;
        GameObject go;
        float speed = 0.0f;
        GameObject aChipCuocPrefab = null;
        m_originPos = gameObject.transform.localPosition;

        switch (typeCHIP) {
            case 0:
                aChipCuocPrefab = m_chips[0];
                break;
            case 1:
                aChipCuocPrefab = m_chips[1];
                break;
            case 2:
                aChipCuocPrefab = m_chips[2];
                break;
            case 3:
                aChipCuocPrefab = m_chips[3];
                break;
        }

        switch (cua) {
            case 0://cua chan
                xPos = Random.Range(m_cuaChanPos.x - offsetLarge_X, m_cuaChanPos.x + offsetLarge_X);
                yPos = Random.Range(m_cuaChanPos.y - offsetLarge_Y, m_cuaChanPos.y + offsetLarge_Y);
                m_targetPos = new Vector3(xPos, yPos, zPos);

                //go = Instantiate (aChipCuocPrefab) as GameObject;
                go = ObjectPool.current.GetObject(aChipCuocPrefab);
                //go.SetActive (true);
                go.transform.SetParent(m_chipXocdiaParent.transform, true);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = m_originPos;
                go.SetActive(true);

                speed = 600.0f;
                go.GetComponent<ChipXocdia>().MoveChip(true, m_targetPos, speed, getName());

                m_chipSpawn0.Add(go);
                m_chipDatCuoc0.Add(aChipCuocPrefab);
                break;
            case 1://cua le
                xPos = Random.Range(m_cuaLePos.x - offsetLarge_X, m_cuaLePos.x + offsetLarge_X);
                yPos = Random.Range(m_cuaLePos.y - offsetLarge_Y, m_cuaLePos.y + offsetLarge_Y);
                m_targetPos = new Vector3(xPos, yPos, zPos);

                //go = Instantiate (aChipCuocPrefab) as GameObject;
                go = ObjectPool.current.GetObject(aChipCuocPrefab);
                //go.SetActive (true);
                go.transform.SetParent(m_chipXocdiaParent.transform, true);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = m_originPos;
                go.SetActive(true);

                speed = 600.0f;
                go.GetComponent<ChipXocdia>().MoveChip(true, m_targetPos, speed, getName());

                m_chipSpawn1.Add(go);
                m_chipDatCuoc1.Add(aChipCuocPrefab);
                break;
            case 2://cua nho 1, 4 do.
                xPos = Random.Range(m_cuaNhoPos_1.x - offset, m_cuaNhoPos_1.x + offset);
                yPos = Random.Range(m_cuaNhoPos_1.y - offset, m_cuaNhoPos_1.y + offset);
                m_targetPos = new Vector3(xPos, yPos, zPos);

                //go = Instantiate (aChipCuocPrefab) as GameObject;
                go = ObjectPool.current.GetObject(aChipCuocPrefab);
                //go.SetActive (true);
                go.transform.SetParent(m_chipXocdiaParent.transform, true);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = m_originPos;
                go.SetActive(true);

                speed = 600.0f;
                go.GetComponent<ChipXocdia>().MoveChip(true, m_targetPos, speed, getName());

                m_chipSpawn2.Add(go);
                m_chipDatCuoc2.Add(aChipCuocPrefab);
                break;
            case 3://cua nho 2, 4 trang.
                xPos = Random.Range(m_cuaNhoPos_2.x - offset, m_cuaNhoPos_2.x + offset);
                yPos = Random.Range(m_cuaNhoPos_2.y - offset, m_cuaNhoPos_2.y + offset);
                m_targetPos = new Vector3(xPos, yPos, zPos);

                //go = Instantiate (aChipCuocPrefab) as GameObject;
                go = ObjectPool.current.GetObject(aChipCuocPrefab);
                //go.SetActive (true);
                go.transform.SetParent(m_chipXocdiaParent.transform, true);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = m_originPos;
                go.SetActive(true);

                speed = 600.0f;
                go.GetComponent<ChipXocdia>().MoveChip(true, m_targetPos, speed, getName());

                m_chipSpawn3.Add(go);
                m_chipDatCuoc3.Add(aChipCuocPrefab);
                break;
            case 4://cua nho 3, 1 trang + 3 do.
                xPos = Random.Range(m_cuaNhoPos_3.x - offset, m_cuaNhoPos_3.x + offset);
                yPos = Random.Range(m_cuaNhoPos_3.y - offset, m_cuaNhoPos_3.y + offset);
                m_targetPos = new Vector3(xPos, yPos, zPos);

                //go = Instantiate (aChipCuocPrefab) as GameObject;
                go = ObjectPool.current.GetObject(aChipCuocPrefab);
                //go.SetActive (true);
                go.transform.SetParent(m_chipXocdiaParent.transform, true);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = m_originPos;
                go.SetActive(true);

                speed = 600.0f;
                go.GetComponent<ChipXocdia>().MoveChip(true, m_targetPos, speed, getName());

                m_chipSpawn4.Add(go);
                m_chipDatCuoc4.Add(aChipCuocPrefab);
                break;
            case 5://cua nho 4, 1 do + 3 trang.
                xPos = Random.Range(m_cuaNhoPos_4.x - offset, m_cuaNhoPos_4.x + offset);
                yPos = Random.Range(m_cuaNhoPos_4.y - offset, m_cuaNhoPos_4.y + offset);
                m_targetPos = new Vector3(xPos, yPos, zPos);

                //go = Instantiate (aChipCuocPrefab) as GameObject;
                go = ObjectPool.current.GetObject(aChipCuocPrefab);
                //go.SetActive (true);
                go.transform.SetParent(m_chipXocdiaParent.transform, true);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = m_originPos;
                go.SetActive(true);

                speed = 600.0f;
                go.GetComponent<ChipXocdia>().MoveChip(true, m_targetPos, speed, getName());

                m_chipSpawn5.Add(go);
                m_chipDatCuoc5.Add(aChipCuocPrefab);
                break;
        }
    }

    public void ActionChipWin(bool cuachan, bool cuale, bool cuanho1, bool cuanho2, bool cuanho3, bool cuanho4) {

        //Xac dinh vi tri cua nha cai.
        Vector2 tempX0 = Vector2.zero;
        Vector2 tempY0 = Vector2.zero;
        if (XocDia.SystemIsMaster == true) {
            tempX0 = new Vector2(m_batTransfrom.localPosition.x - 20,
                 m_batTransfrom.localPosition.x + 20);
            tempY0 = new Vector2(m_batTransfrom.localPosition.y - 20,
                 m_batTransfrom.localPosition.y + 20);
        } else {
            if (XocDia.PlayerLamcai != null) {
                tempX0 = new Vector2(XocDia.PlayerLamcai.localPosition.x - 15,
                    XocDia.PlayerLamcai.localPosition.x + 15);
                tempY0 = new Vector2(XocDia.PlayerLamcai.localPosition.y - 20,
                    XocDia.PlayerLamcai.localPosition.y + 20);
            }
        }

        int size = 0;
        float xPos;
        float yPos;
        float zPos = 0.0f;
        GameObject go;
        float speed = 0.0f;
        m_originPos = gameObject.transform.localPosition;

        if (cuachan) {
            size = m_chipSpawn0.Count;
            if (size > MAX_CHIP_DOUBLE) {
                size = MAX_CHIP_DOUBLE;
            }

            for (int i = 0; i < size; i++) {
                //Spawn chip tu nha cai.
                GameObject chipPrefab = m_chipSpawn0[i];
                xPos = Random.Range(tempX0.x, tempX0.y);
                yPos = Random.Range(tempY0.x, tempY0.y);
                m_originPos = new Vector3(xPos, yPos, zPos);
                //go = Instantiate (chipPrefab) as GameObject;
                go = ObjectPool.current.GetObject(chipPrefab);
                //go.SetActive (true);
                go.transform.SetParent(m_chipXocdiaParent.transform, true);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = m_originPos;
                go.SetActive(true);

                //Move chip tu nha cai toi cua cuoc.
                xPos = Random.Range(m_cuaChanPos.x - offsetLarge_X, m_cuaChanPos.x + offsetLarge_X);
                yPos = Random.Range(m_cuaChanPos.y - offsetLarge_Y, m_cuaChanPos.y + offsetLarge_Y);
                m_targetPos = new Vector3(xPos, yPos, zPos);

                speed = 300.0f;
                if (i == size - 1) {
                    go.GetComponent<ChipXocdia>().SequenceMove(true, m_targetPos, speed, getName(),
                        onSequenceMoveComplete, 0);
                } else {
                    go.GetComponent<ChipXocdia>().SequenceMove(true, m_targetPos, speed, getName());
                }

                m_chipSpawn0.Add(go);
            }
        }

        if (cuale) {
            size = m_chipSpawn1.Count;
            if (size > MAX_CHIP_DOUBLE) {
                size = MAX_CHIP_DOUBLE;
            }

            for (int i = 0; i < size; i++) {
                //Spawn chip tu nha cai.
                GameObject chipPrefab = m_chipSpawn1[i];
                xPos = Random.Range(tempX0.x, tempX0.y);
                yPos = Random.Range(tempY0.x, tempY0.y);

                m_originPos = new Vector3(xPos, yPos, zPos);
                //go = Instantiate (chipPrefab) as GameObject;
                go = ObjectPool.current.GetObject(chipPrefab);
                //go.SetActive (true);
                go.transform.SetParent(m_chipXocdiaParent.transform, true);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = m_originPos;
                go.SetActive(true);

                //Move chip tu nha cai toi cua cuoc.
                xPos = Random.Range(m_cuaLePos.x - offsetLarge_X, m_cuaLePos.x + offsetLarge_X);
                yPos = Random.Range(m_cuaLePos.y - offsetLarge_Y, m_cuaLePos.y + offsetLarge_Y);
                m_targetPos = new Vector3(xPos, yPos, zPos);

                speed = 300.0f;
                if (i == size - 1) {
                    go.GetComponent<ChipXocdia>().SequenceMove(true, m_targetPos, speed, getName(),
                        onSequenceMoveComplete, 1);
                } else {
                    go.GetComponent<ChipXocdia>().SequenceMove(true, m_targetPos, speed, getName());
                }

                m_chipSpawn1.Add(go);
            }

        }

        if (cuanho1) {
            size = m_chipSpawn2.Count;
            if (size > MAX_CHIP_DOUBLE) {
                size = MAX_CHIP_DOUBLE;
            }

            for (int i = 0; i < size; i++) {
                //Spawn chip tu nha cai.
                GameObject chipPrefab = m_chipSpawn2[i];
                xPos = Random.Range(tempX0.x, tempX0.y);
                yPos = Random.Range(tempY0.x, tempY0.y);
                m_originPos = new Vector3(xPos, yPos, zPos);
                //go = Instantiate (chipPrefab) as GameObject;
                go = ObjectPool.current.GetObject(chipPrefab);
                //go.SetActive (true);
                go.transform.SetParent(m_chipXocdiaParent.transform, true);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = m_originPos;
                go.SetActive(true);

                //Move chip tu nha cai toi cua cuoc.				
                xPos = Random.Range(m_cuaNhoPos_1.x - offset, m_cuaNhoPos_1.x + offset);
                yPos = Random.Range(m_cuaNhoPos_1.y - offset, m_cuaNhoPos_1.y + offset);
                m_targetPos = new Vector3(xPos, yPos, zPos);

                speed = 300.0f;
                if (i == size - 1) {
                    go.GetComponent<ChipXocdia>().SequenceMove(true, m_targetPos, speed, getName(),
                        onSequenceMoveComplete, 2);
                } else {
                    go.GetComponent<ChipXocdia>().SequenceMove(true, m_targetPos, speed, getName());
                }

                m_chipSpawn2.Add(go);
            }

        }

        if (cuanho2) {
            size = m_chipSpawn3.Count;
            if (size > MAX_CHIP_DOUBLE) {
                size = MAX_CHIP_DOUBLE;
            }

            for (int i = 0; i < size; i++) {
                //Spawn chip tu nha cai.
                GameObject chipPrefab = m_chipSpawn3[i];
                xPos = Random.Range(tempX0.x, tempX0.y);
                yPos = Random.Range(tempY0.x, tempY0.y);
                m_originPos = new Vector3(xPos, yPos, zPos);
                //go = Instantiate (chipPrefab) as GameObject;
                go = ObjectPool.current.GetObject(chipPrefab);
                //go.SetActive (true);
                go.transform.SetParent(m_chipXocdiaParent.transform, true);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = m_originPos;
                go.SetActive(true);

                //Move chip tu nha cai toi cua cuoc.
                xPos = Random.Range(m_cuaNhoPos_2.x - offset, m_cuaNhoPos_2.x + offset);
                yPos = Random.Range(m_cuaNhoPos_2.y - offset, m_cuaNhoPos_2.y + offset);
                m_targetPos = new Vector3(xPos, yPos, zPos);

                speed = 300.0f;
                if (i == size - 1) {
                    go.GetComponent<ChipXocdia>().SequenceMove(true, m_targetPos, speed, getName(),
                        onSequenceMoveComplete, 3);
                } else {
                    go.GetComponent<ChipXocdia>().SequenceMove(true, m_targetPos, speed, getName());
                }

                m_chipSpawn3.Add(go);
            }

        }

        if (cuanho3) {
            size = m_chipSpawn4.Count;
            if (size > MAX_CHIP_DOUBLE) {
                size = MAX_CHIP_DOUBLE;
            }

            for (int i = 0; i < size; i++) {
                //Spawn chip tu nha cai.
                GameObject chipPrefab = m_chipSpawn4[i];
                xPos = Random.Range(tempX0.x, tempX0.y);
                yPos = Random.Range(tempY0.x, tempY0.y);
                m_originPos = new Vector3(xPos, yPos, zPos);
                //go = Instantiate (chipPrefab) as GameObject;
                go = ObjectPool.current.GetObject(chipPrefab);
                //go.SetActive (true);
                go.transform.SetParent(m_chipXocdiaParent.transform, true);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = m_originPos;
                go.SetActive(true);

                //Move chip tu nha cai toi cua cuoc.
                xPos = Random.Range(m_cuaNhoPos_3.x - offset, m_cuaNhoPos_3.x + offset);
                yPos = Random.Range(m_cuaNhoPos_3.y - offset, m_cuaNhoPos_3.y + offset);
                m_targetPos = new Vector3(xPos, yPos, zPos);

                speed = 300.0f;
                if (i == size - 1) {
                    go.GetComponent<ChipXocdia>().SequenceMove(true, m_targetPos, speed, getName(),
                        onSequenceMoveComplete, 4);
                } else {
                    go.GetComponent<ChipXocdia>().SequenceMove(true, m_targetPos, speed, getName());
                }

                m_chipSpawn4.Add(go);
            }

        }

        if (cuanho4) {
            size = m_chipSpawn5.Count;
            if (size > MAX_CHIP_DOUBLE) {
                size = MAX_CHIP_DOUBLE;
            }

            for (int i = 0; i < size; i++) {
                //Spawn chip tu nha cai.
                GameObject chipPrefab = m_chipSpawn5[i];
                xPos = Random.Range(tempX0.x, tempX0.y);
                yPos = Random.Range(tempY0.x, tempY0.y);
                m_originPos = new Vector3(xPos, yPos, zPos);
                //go = Instantiate (chipPrefab) as GameObject;
                go = ObjectPool.current.GetObject(chipPrefab);
                //go.SetActive (true);
                go.transform.SetParent(m_chipXocdiaParent.transform, true);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = m_originPos;
                go.SetActive(true);

                //Move chip tu nha cai toi cua cuoc.
                xPos = Random.Range(m_cuaNhoPos_4.x - offset, m_cuaNhoPos_4.x + offset);
                yPos = Random.Range(m_cuaNhoPos_4.y - offset, m_cuaNhoPos_4.y + offset);
                m_targetPos = new Vector3(xPos, yPos, zPos);

                speed = 300.0f;
                if (i == size - 1) {
                    go.GetComponent<ChipXocdia>().SequenceMove(true, m_targetPos, speed, getName(),
                        onSequenceMoveComplete, 5);
                } else {
                    go.GetComponent<ChipXocdia>().SequenceMove(true, m_targetPos, speed, getName());
                }

                m_chipSpawn5.Add(go);
            }

        }
    }

    private void onSequenceMoveComplete(int cua) {
        //Xac dinh vi tri chip tai playerwin.
        Vector2 tempX = new Vector2(gameObject.transform.localPosition.x - 15.0f,
            gameObject.transform.localPosition.x + 15.0f);
        Vector2 tempY = new Vector2(gameObject.transform.localPosition.y - 20.0f,
            gameObject.transform.localPosition.y + 20);
        Vector3 targetPos2 = gameObject.transform.localPosition;

        switch (cua) {
            case 0:

                for (int i = 0; i < m_chipSpawn0.Count; i++) {
                    //Random position for chip move to player win
                    float posX = Random.Range(tempX.x, tempX.y);
                    float posY = Random.Range(tempY.x, tempY.y);
                    targetPos2 = new Vector3(posX, posY, 0.0f);

                    if (m_chipSpawn0[i] != null)
                        m_chipSpawn0[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos2, 300.0f, getName(),
                           onActionComplete, 0, i);
                }
                break;
            case 1:

                for (int i = 0; i < m_chipSpawn1.Count; i++) {
                    //Random position for chip move to player win
                    float posX = Random.Range(tempX.x, tempX.y);
                    float posY = Random.Range(tempY.x, tempY.y);
                    targetPos2 = new Vector3(posX, posY, 0.0f);

                    if (m_chipSpawn1[i] != null)
                        m_chipSpawn1[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos2, 300.0f, getName(),
                           onActionComplete, 1, i);
                }
                break;
            case 2:
                for (int i = 0; i < m_chipSpawn2.Count; i++) {
                    //Random position for chip move to player win
                    float posX = Random.Range(tempX.x, tempX.y);
                    float posY = Random.Range(tempY.x, tempY.y);
                    targetPos2 = new Vector3(posX, posY, 0.0f);

                    if (m_chipSpawn2[i] != null)
                        m_chipSpawn2[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos2, 300.0f, getName(),
                           onActionComplete, 2, i);
                }

                break;
            case 3:
                for (int i = 0; i < m_chipSpawn3.Count; i++) {
                    //Random position for chip move to player win
                    float posX = Random.Range(tempX.x, tempX.y);
                    float posY = Random.Range(tempY.x, tempY.y);
                    targetPos2 = new Vector3(posX, posY, 0.0f);

                    if (m_chipSpawn3[i] != null)
                        m_chipSpawn3[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos2, 300.0f, getName(),
                           onActionComplete, 3, i);
                }

                break;
            case 4:
                for (int i = 0; i < m_chipSpawn4.Count; i++) {
                    //Random position for chip move to player win
                    float posX = Random.Range(tempX.x, tempX.y);
                    float posY = Random.Range(tempY.x, tempY.y);
                    targetPos2 = new Vector3(posX, posY, 0.0f);

                    if (m_chipSpawn4[i] != null)
                        m_chipSpawn4[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos2, 300.0f, getName(),
                           onActionComplete, 4, i);
                }

                break;
            case 5:
                for (int i = 0; i < m_chipSpawn5.Count; i++) {
                    //Random position for chip move to player win
                    float posX = Random.Range(tempX.x, tempX.y);
                    float posY = Random.Range(tempY.x, tempY.y);
                    targetPos2 = new Vector3(posX, posY, 0.0f);

                    if (m_chipSpawn5[i] != null)
                        m_chipSpawn5[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos2, 300.0f, getName(),
                           onActionComplete, 5, i);
                }

                break;
        }
    }

    private void onActionComplete(int idSender, int index, GameObject self) {
        ObjectPool.current.PoolObject(self);
        switch (idSender) {
            case 0:
                //Destroy (self);
                //ObjectPool.current.PoolObject (self);
                if (index == m_chipSpawn0.Count - 1) {
                    m_chipSpawn0.Clear();
                    m_chipDatCuoc0.Clear();
                }
                break;
            case 1:
                //Destroy (self);
                if (index == m_chipSpawn1.Count - 1) {
                    m_chipSpawn1.Clear();
                    m_chipDatCuoc1.Clear();
                }
                break;
            case 2:
                //Destroy (self);
                if (index == m_chipSpawn2.Count - 1) {
                    m_chipSpawn2.Clear();
                    m_chipDatCuoc2.Clear();
                }
                break;
            case 3:
                //Destroy (self);
                if (index == m_chipSpawn3.Count - 1) {
                    m_chipSpawn3.Clear();
                    m_chipDatCuoc3.Clear();
                }
                break;
            case 4:
                //Destroy (self);
                if (index == m_chipSpawn4.Count - 1) {
                    m_chipSpawn4.Clear();
                    m_chipDatCuoc4.Clear();
                }
                break;
            case 5:
                //Destroy (self);
                if (index == m_chipSpawn5.Count - 1) {
                    m_chipSpawn5.Clear();
                    m_chipDatCuoc5.Clear();
                }
                break;
        }
    }

    public void ActionChipLose(bool cuachan, bool cuale, bool cuanho1, bool cuanho2, bool cuanho3, bool cuanho4) {
        Vector2 tempX = Vector2.zero;
        Vector2 tempY = Vector2.zero;

        if (XocDia.SystemIsMaster == true) {
            tempX = new Vector2(m_batTransfrom.localPosition.x - 20,
                 m_batTransfrom.localPosition.x + 20);
            tempY = new Vector2(m_batTransfrom.localPosition.y - 20,
                 m_batTransfrom.localPosition.y + 20);
        } else {
            if (XocDia.PlayerLamcai != null) {
                tempX = new Vector2(XocDia.PlayerLamcai.localPosition.x - 15,
                    XocDia.PlayerLamcai.localPosition.x + 15);
                tempY = new Vector2(XocDia.PlayerLamcai.localPosition.y - 20,
                    XocDia.PlayerLamcai.localPosition.y + 20);
            }
        }


        Vector3 targetPos2 = m_batTransfrom.localPosition;

        if (cuachan) {
            for (int i = 0; i < m_chipSpawn0.Count; i++) {
                //Random position for chip movement.
                float posX = Random.Range(tempX.x, tempX.y);
                float posY = Random.Range(tempY.x, tempY.y);
                targetPos2 = new Vector3(posX, posY, 0.0f);

                if (m_chipDatCuoc0[i] != null)
                    m_chipSpawn0[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos2,
                       300.0f, getName(), onActionComplete, 0, i);
            }
        }

        if (cuale) {
            for (int i = 0; i < m_chipSpawn1.Count; i++) {
                //Random position for chip movement.
                float posX = Random.Range(tempX.x, tempX.y);
                float posY = Random.Range(tempY.x, tempY.y);
                targetPos2 = new Vector3(posX, posY, 0.0f);

                if (m_chipDatCuoc1[i] != null)
                    m_chipSpawn1[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos2,
                       300.0f, getName(), onActionComplete, 1, i);
            }
        }

        if (cuanho1) {
            for (int i = 0; i < m_chipSpawn2.Count; i++) {
                //Random position for chip movement.
                float posX = Random.Range(tempX.x, tempX.y);
                float posY = Random.Range(tempY.x, tempY.y);
                targetPos2 = new Vector3(posX, posY, 0.0f);

                if (m_chipDatCuoc2[i] != null)
                    m_chipSpawn2[i].GetComponent<ChipXocdia>().MoveChip(true,
                       targetPos2, 300.0f, getName(), onActionComplete, 2, i);
            }
        }

        if (cuanho2) {
            for (int i = 0; i < m_chipSpawn3.Count; i++) {
                //Random position for chip movement.
                float posX = Random.Range(tempX.x, tempX.y);
                float posY = Random.Range(tempY.x, tempY.y);
                targetPos2 = new Vector3(posX, posY, 0.0f);

                if (m_chipDatCuoc3[i] != null)
                    m_chipSpawn3[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos2,
                       300.0f, getName(), onActionComplete, 3, i);
            }
        }

        if (cuanho3) {
            for (int i = 0; i < m_chipSpawn4.Count; i++) {
                //Random position for chip movement.
                float posX = Random.Range(tempX.x, tempX.y);
                float posY = Random.Range(tempY.x, tempY.y);
                targetPos2 = new Vector3(posX, posY, 0.0f);

                if (m_chipDatCuoc4[i] != null)
                    m_chipSpawn4[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos2,
                       300.0f, getName(), onActionComplete, 4, i);
            }
        }

        if (cuanho4) {
            for (int i = 0; i < m_chipSpawn5.Count; i++) {
                //Random position for chip movement.
                float posX = Random.Range(tempX.x, tempX.y);
                float posY = Random.Range(tempY.x, tempY.y);
                targetPos2 = new Vector3(posX, posY, 0.0f);

                if (m_chipDatCuoc5[i] != null)
                    m_chipSpawn5[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos2,
                       500.0f, getName(), onActionComplete, 5, i);
            }
        }

    }

    public void ActionTraTienCuoc(long cua0, long cua1, long cua2, long cua3, long cua4, long cua5) {
        //Xac dinh vi tri chip tai cua cuoc.
        Vector2 tempX = new Vector2(gameObject.transform.localPosition.x - 15.0f,
            gameObject.transform.localPosition.x + 15.0f);
        Vector2 tempY = new Vector2(gameObject.transform.localPosition.y - 20.0f,
            gameObject.transform.localPosition.y + 20);
        Vector3 targetPos = gameObject.transform.localPosition;

        if (cua0 > 0) {
            for (int i = 0; i < m_chipSpawn0.Count; i++) {
                float posX = Random.Range(tempX.x, tempX.y);
                float posY = Random.Range(tempY.x, tempY.y);
                targetPos = new Vector3(posX, posY, 0.0f);

                if (m_chipSpawn0[i] != null)
                    m_chipSpawn0[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos, 300.0f, getName(),
                        onActionComplete, 0, i);
            }
        }

        if (cua1 > 0) {
            for (int i = 0; i < m_chipSpawn1.Count; i++) {
                float posX = Random.Range(tempX.x, tempX.y);
                float posY = Random.Range(tempY.x, tempY.y);
                targetPos = new Vector3(posX, posY, 0.0f);

                if (m_chipSpawn1[i] != null)
                    m_chipSpawn1[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos, 300.0f, getName(),
                        onActionComplete, 1, i);
            }
        }

        if (cua2 > 0) {
            for (int i = 0; i < m_chipSpawn2.Count; i++) {
                float posX = Random.Range(tempX.x, tempX.y);
                float posY = Random.Range(tempY.x, tempY.y);
                targetPos = new Vector3(posX, posY, 0.0f);

                if (m_chipSpawn2[i] != null)
                    m_chipSpawn2[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos, 300.0f, getName(),
                        onActionComplete, 2, i);
            }
        }

        if (cua3 > 0) {
            for (int i = 0; i < m_chipSpawn3.Count; i++) {
                float posX = Random.Range(tempX.x, tempX.y);
                float posY = Random.Range(tempY.x, tempY.y);
                targetPos = new Vector3(posX, posY, 0.0f);

                if (m_chipSpawn3[i] != null)
                    m_chipSpawn3[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos, 300.0f, getName(),
                        onActionComplete, 3, i);
            }
        }

        if (cua4 > 0) {
            for (int i = 0; i < m_chipSpawn4.Count; i++) {
                float posX = Random.Range(tempX.x, tempX.y);
                float posY = Random.Range(tempY.x, tempY.y);
                targetPos = new Vector3(posX, posY, 0.0f);

                if (m_chipSpawn4[i] != null)
                    m_chipSpawn4[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos, 300.0f, getName(),
                        onActionComplete, 4, i);
            }
        }

        if (cua5 > 0) {
            for (int i = 0; i < m_chipSpawn5.Count; i++) {
                float posX = Random.Range(tempX.x, tempX.y);
                float posY = Random.Range(tempY.x, tempY.y);
                targetPos = new Vector3(posX, posY, 0.0f);

                if (m_chipSpawn5[i] != null)
                    m_chipSpawn5[i].GetComponent<ChipXocdia>().MoveChip(true, targetPos, 300.0f, getName(),
                        onActionComplete, 5, i);
            }
        }
    }

    public void RemoveAllChip() {
        if (m_chipSpawn0 != null) {
            for (int i = 0; i < m_chipSpawn0.Count; i++) {
                if (m_chipSpawn0[i] != null) {
                    //Destroy ( m_chipSpawn0[i]);
                    ObjectPool.current.PoolObject(m_chipSpawn0[i]);
                }
            }
            m_chipSpawn0.Clear();
            m_chipDatCuoc0.Clear();
        }

        if (m_chipSpawn1 != null) {
            for (int i = 0; i < m_chipSpawn1.Count; i++) {
                if (m_chipSpawn1[i] != null) {
                    //Destroy ( m_chipSpawn1[i]);
                    ObjectPool.current.PoolObject(m_chipSpawn1[i]);
                }
            }
            m_chipSpawn1.Clear();
            m_chipDatCuoc1.Clear();
        }

        if (m_chipSpawn2 != null) {
            for (int i = 0; i < m_chipSpawn2.Count; i++) {
                if (m_chipSpawn2[i] != null) {
                    //Destroy ( m_chipSpawn2[i]);
                    ObjectPool.current.PoolObject(m_chipSpawn2[i]);
                }
            }
            m_chipSpawn2.Clear();
            m_chipDatCuoc2.Clear();
        }

        if (m_chipSpawn3 != null) {
            for (int i = 0; i < m_chipSpawn3.Count; i++) {
                if (m_chipSpawn3[i] != null) {
                    //Destroy ( m_chipSpawn3[i]);
                    ObjectPool.current.PoolObject(m_chipSpawn3[i]);
                }
            }
            m_chipSpawn3.Clear();
            m_chipDatCuoc3.Clear();
        }

        if (m_chipSpawn4 != null) {
            for (int i = 0; i < m_chipSpawn4.Count; i++) {
                if (m_chipSpawn4[i] != null) {
                    //Destroy ( m_chipSpawn4[i]);
                    ObjectPool.current.PoolObject(m_chipSpawn4[i]);
                }
            }
            m_chipSpawn4.Clear();
            m_chipDatCuoc4.Clear();
        }

        if (m_chipSpawn5 != null) {
            for (int i = 0; i < m_chipSpawn5.Count; i++) {
                if (m_chipSpawn5[i] != null) {
                    //Destroy ( m_chipSpawn5[i]);
                    ObjectPool.current.PoolObject(m_chipSpawn5[i]);
                }
            }
            m_chipSpawn5.Clear();
            m_chipDatCuoc5.Clear();
        }
    }
}
