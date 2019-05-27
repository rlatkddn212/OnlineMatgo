using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameResourceManager : MonoBehaviour {
	public static GameResourceManager instance;
	SortedDictionary<string, GameObject> balloonResourcePool;
	SortedDictionary<string, GameObject> gameResourcePool;
	SortedDictionary<string, GameObject> resultResourcePool;
	//풍선껌 리스트
	string[] balloonList = { "balloon/ok2", "balloon/ok1", "balloon/char1", "balloon/char2", "balloon/char3", "balloon/char4",
                            "balloon/char5","balloon/char6","balloon/char7","balloon/char8","balloon/char9","balloon/char10",
							"balloon/oppchar1", "balloon/oppchar2", "balloon/oppchar3", "balloon/oppchar4",
                            "balloon/oppchar5","balloon/oppchar6","balloon/oppchar7","balloon/oppchar8","balloon/oppchar9","balloon/oppchar10",
                            "balloon/charboard1","balloon/charboard2","balloon/no1","balloon/no2","balloon/point1","balloon/point2",
                            "balloon/selecting1","balloon/selecting2","balloon/speechbubble1","balloon/speechbubble2","balloon/title",
                            "balloon/ballooncoin","balloon/x2","balloon/x3","balloon/x5","balloon/x7","balloon/x10",
                            "balloon/balloonboard2","balloon/okspeechballoon1","balloon/okspeechballoon2","balloon/balloonboard1",
                            "balloon/Btn_x1","balloon/Btn_x2","balloon/gauge","balloon/voidgauge",
                            "balloon/btext1","balloon/btext2","balloon/Btn_Yes1","balloon/Btn_Yes2","balloon/Btn_No1","balloon/Btn_No2",};

	public static int CARDLENGTH = 51;
	string[] paeFlag = { 
						   "card/Btn_패버튼002","card/패표시001","card/패표시002","card/패표시003","card/패표시004","card/패표시005",
						   "card/패표시006","card/패표시007","card/패표시008","card/패표시010","card/패표시011",
						   "card/패표시012","card/패표시013","card/패표시014"
					   };
	string[] resultList = {"result/win", "result/lose" };


	void Awake() {
		instance = this;
		balloonResourcePool = new SortedDictionary<string, GameObject>();
		gameResourcePool = new SortedDictionary<string, GameObject>();
		resultResourcePool = new SortedDictionary<string, GameObject>();
	}


	public GameObject getGameResource(string s) {
		GameObject ret = null;
		if (gameResourcePool.TryGetValue(s, out ret)) {
			//ret에 객체가 입력됨
		}
		//객체가 존재하지 않는 경우 불러옴
		else {
			Debug.Log(s);
			ret = (GameObject)Instantiate(Resources.Load(s, typeof(GameObject)));
			gameResourcePool.Add(s, ret);
		}
		return ret;
	}

	public GameObject getResultResource(string s) {
		GameObject ret = null;
		if (resultResourcePool.TryGetValue(s, out ret)) {
			//ret에 객체가 입력됨
		}
		//객체가 존재하지 않는 경우 불러옴
		else {
			Debug.Log(s);
			ret = (GameObject)Instantiate(Resources.Load(s, typeof(GameObject)));
			resultResourcePool.Add(s, ret);
		}
		return ret;
	}

	public GameObject getBalloonResource(string s) {
		GameObject ret = null;
		if (balloonResourcePool.TryGetValue(s, out ret)) {
			//ret에 객체가 입력됨
		}
		//객체가 존재하지 않는 경우 불러옴
		else {
			Debug.Log(s);
			ret = (GameObject)Instantiate(Resources.Load(s, typeof(GameObject)));
			balloonResourcePool.Add(s, ret);
		}
		return ret;
	}
	
	public void setResultResource() {
		for (int i = 0; i < resultList.Length; i++) {
			Debug.Log("로드된 리소스 : " + resultList[i]);
			resultResourcePool.Add(resultList[i], (GameObject)Instantiate(Resources.Load(resultList[i], typeof(GameObject))));
		}
	}

	public void setBalloonResource() {
		for (int i = 0; i < balloonList.Length; i++) {
			Debug.Log("로드된 리소스 : " + balloonList[i]);
			balloonResourcePool.Add(balloonList[i], (GameObject)Instantiate(Resources.Load(balloonList[i], typeof(GameObject))));
		}
	}

	public void initGameResource() {
		foreach (KeyValuePair<string, GameObject> val in gameResourcePool) {
			GameObject obj = val.Value;
			obj.transform.position = new Vector3(5.0f, 0.0f, 0.0f);
		}
	}

	public void setGameResource(){
		//팝업에 나타날 패
		for (int i = 1; i <= 48; i++) {

			if (i < 10) {
				GameObject card = (GameObject)Instantiate(Resources.Load("popup/pae000" + i, typeof(GameObject)));
				Card cardClass = card.GetComponent<Card>();
				cardClass.setCardNum(i);
				gameResourcePool.Add("popupcard" + i, card);
			}
			else {
				GameObject card = (GameObject)Instantiate(Resources.Load("popup/pae00" + i, typeof(GameObject)));
				Card cardClass = card.GetComponent<Card>();
				cardClass.setCardNum(i);
				gameResourcePool.Add("popupcard" + i, card);
			}
		}
		GameObject goStopPopUp = (GameObject)Instantiate(Resources.Load("popup/고스톱팝업", typeof(GameObject)));
		gameResourcePool.Add("gostoppopup", goStopPopUp);
		GameObject selectCardPopUp = (GameObject)Instantiate(Resources.Load("popup/패선택팝업", typeof(GameObject)));
		gameResourcePool.Add("selectcardpopup", selectCardPopUp);
		GameObject tenEnd = (GameObject)Instantiate(Resources.Load("popup/국열끗팝업", typeof(GameObject)));
		gameResourcePool.Add("tenendpopup", tenEnd);

		//카드 리스트를 불러옴
		gameResourcePool.Add("card/paeset", (GameObject)Instantiate(Resources.Load("card/paeset", typeof(GameObject))));
		for (int i = 0; i <= CARDLENGTH; i++) {
			
			if(i < 10){
				if(i == 0){
					for (int j = 0; j <= 28; j++) {
						GameObject overturnedCard = (GameObject)Instantiate(Resources.Load("card/pae0000", typeof(GameObject)));
						Card cardClass = overturnedCard.GetComponent<Card>();
						cardClass.setCardNum(j);
						gameResourcePool.Add("card/pae0000" + j, overturnedCard);
					}

				}else{
					GameObject card = (GameObject)Instantiate(Resources.Load("card/pae000" + i, typeof(GameObject)));
					Card cardClass = card.GetComponent<Card>();
					cardClass.setCardNum(i);
					gameResourcePool.Add("card/pae000" + i, card);
				}
			}
			else {
				GameObject card = (GameObject)Instantiate(Resources.Load("card/pae00" + i, typeof(GameObject)));
				Card cardClass = card.GetComponent<Card>();
				cardClass.setCardNum(i);
				gameResourcePool.Add("card/pae00" + i, card);
			}

			Debug.Log("로드된 리소스 : " + "card/pae00" + i);
		}

		//
		for (int i = 0; i < 8; i++) {
			gameResourcePool.Add(paeFlag[0] + i, (GameObject)Instantiate(Resources.Load(paeFlag[0], typeof(GameObject))));
		}

		for (int i = 1; i < paeFlag.Length; i++) {
			gameResourcePool.Add(paeFlag[i], (GameObject)Instantiate(Resources.Load(paeFlag[i], typeof(GameObject))));
		}
	}

	public void init() {
		balloonResourcePool.Clear();
	}
}
