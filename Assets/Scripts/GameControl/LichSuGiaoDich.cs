using UnityEngine;
using System.Collections;

public class LichSuGiaoDich : MonoBehaviour {
    public int stt;
    public string tenvatpham, trangthai, thoigiangiaodich;

    public UILabel[] lb;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void setInfo() {
        lb[0].text = stt + "";
        lb[1].text = tenvatpham;
        lb[2].text = trangthai;
        lb[3].text = thoigiangiaodich;
    }
}
