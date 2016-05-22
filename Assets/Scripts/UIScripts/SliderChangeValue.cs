using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SliderChangeValue : MonoBehaviour {

	Slider slider;
	public GameObject text;

	float rate;

	// Use this for initialization
	void Start () {
		slider = GetComponent<Slider> ();
		slider.onValueChanged.AddListener (changedValue);
		rate = 1000;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changedValue(float f){
		text.GetComponent<InputField> ().text = (int)(f * rate) + "";
	}
}
