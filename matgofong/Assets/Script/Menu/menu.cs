using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class menu : MonoBehaviour {
	public Button id1but;
	public Button id2but;
	public Button startbut;
	public Text id;
	UserInfo user;
	// Use this for initialization
	void Start () {
		

		//Screen.SetResolution(270, 450, false);
		Camera.main.aspect = 480f / 800f;
		
		user = UserInfo.getInstance();
		id1but.onClick.AddListener(() => { setUI("abc"); });
		id2but.onClick.AddListener(() => { setUI("abc2"); });
		startbut.onClick.AddListener(() => { gamestart(); });
	}
	
	// Update is called once per frame
	void Update () {
	}

	void setUI(string s) {
		id.text = "id : " + s;
		user.Id = s;
	}
	void gamestart() {
		Debug.Log(user.Id);
		Application.LoadLevel("game");
	}
}
