using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Result : MonoBehaviour {
	GameResourceManager gameResource;
	enum STATE {
		RECV,
		POPUP,
		END
	};

	public static Result instance;
	public bool start;
	STATE resultState;
	GameNetwork gameNetwork;

	public Text winScore;
	public Text loseScore;


	void Awake() {
		instance = this;
	}

	void Start() {
		resultState = STATE.RECV;
		start = false;
		gameResource = GameResourceManager.instance;
		gameNetwork = GameNetwork.instance;
		gameResource.setResultResource();
	}

	public static Result getInstance() {
		return instance;
	}

	void clearPopup() {
		GameObject obj = gameResource.getResultResource("result/win");
		obj.transform.position = new Vector3(5f, 0f, -10f);

		GameObject obj2 = gameResource.getResultResource("result/lose");
		obj2.transform.position = new Vector3(5f, 0f, -10f);
	}

	void Update() {
		if(resultState == STATE.POPUP){
			//버튼 입력을 받음

			if (Input.GetMouseButtonDown(0)) {

				Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
				if (hit.collider != null) {
					if (hit.collider.tag == "YesBtn") {
						gameNetwork.reGameMessage();
						clearPopup();
					}
					else if (hit.collider.tag == "NoBtn") {
						gameNetwork.outMessage();
						clearPopup();
					}
				}
			}
		}
	}

	public void getResult() {
		resultState = STATE.RECV;
		gameNetwork.getResult();
	}

	//추가 사항
	public void setResult(string winner , int score) {
		resultState = STATE.POPUP;

		if (winner == UserInfo.getInstance().Id) {
			//승리 팝업
			//winPopUp
			GameObject obj = gameResource.getResultResource("result/win");
			obj.transform.position = new Vector3(0f, 0f, -10f);
			winScore.text = score.ToString();
		}
		else {
			//패배 팝업
			GameObject obj = gameResource.getResultResource("result/lose");

			obj.transform.position = new Vector3(0f, 0f, -10f);
			loseScore.text = score.ToString();
		}
	}
	public bool isEnd() {
		if(resultState == STATE.END){
			resultState = STATE.RECV;
			return true;
		}else{
			return false;
		}
	}
}
