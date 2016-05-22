using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IconSmile : MonoBehaviour {
	public GameObject iconPosition;
	public GameObject par;
	public GameObject panel;
	Button but;

	// Use this for initialization
	void Start () {
		but = GetComponent<Button> ();
		//but.onClick = createIcon ();
		but.onClick.AddListener (createIcon);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void createIcon(){
		panel.SetActive (false);
	//	GameObject obj = (GameObject)Instantiate (Resources.Load ("Prefabs" + _level + "/" + i.name, typeof(GameObject)), spaws [indexPos].position, Quaternion.identity);
		GameObject obj = (GameObject) Instantiate (Resources.Load("iconSmile/" + transform.gameObject.name, typeof(GameObject)), iconPosition.transform.position, Quaternion.identity);
		obj.transform.parent = par.transform;

		Destroy (obj, 2.0f);
		//iconPosition.GetComponent<Image>().sprite = Resources.Load("iconSmile/a1", typeof(Sprite)) as Sprite;
	}



	/*void onClick(){
		createIcon ();
	}*/
}
