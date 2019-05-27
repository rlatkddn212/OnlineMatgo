using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Balloon : MonoBehaviour {
	GameResourceManager gameResource;
	public static Balloon instance;

	GameObject board1;
	GameObject userChar;
	GameObject oppChar;
	GameObject charboard1;
	GameObject charboard2;
	GameObject point1;
	GameObject point2;
	GameObject selecting1;
	GameObject selecting2;
	GameObject speechbubble1;
	GameObject speechbubble2;
	GameObject title;
	GameObject ballooncoin;
	GameObject balloonboard2;
	GameObject Btn_x1;
	GameObject Btn_Yes1;
	GameObject Btn_No1;
	GameObject Btn_x2;
	GameObject Btn_Yes2;
	GameObject Btn_No2;
	GameObject btext1;
	GameObject btext2;
	GameObject gauge;
	GameObject voidGauge;
	GameObject okspeechballoon1;
	GameObject okspeechballoon2;
	GameObject ok1;
	GameObject ok2;
	GameObject no1;
	GameObject no2;

	GameNetwork gameNetwork;

	BALLOONSTATE balloonState;

	bool oppSelected;
	int dotlocation;

	float pointRate = 0.3f;
	private float nextPoint = 0.0f;
	private bool nextSceneFlag;


	enum BALLOONSTATE {
		INIT,
		SELECTING,
		SELECTED,
		ANIMATION,
		END
	}

	void Awake() {
		instance = this;
	}

	void Start() {
		gameResource = GameResourceManager.instance;
		gameNetwork = GameNetwork.instance;
		
		gameResource.setBalloonResource();
		balloonState = BALLOONSTATE.INIT;
		oppSelected = false;
		nextSceneFlag = true;
	}

	public static Balloon getInstance() {
		return instance;
	}

	public bool isEnd(){
		if (balloonState == BALLOONSTATE.END) {
			return true;
		}
		return false;
	}

	private void clickedYesBtn() {
		//불래요.
		
		//보드 판을 없앰
		nextSceneFlag = true;
		gameNetwork.balloonBtn("yes");
	}
	
	private void clickedXBtn() {
		//안불래염
		

		//보드 판을 없앰
		nextSceneFlag = true;

		gameNetwork.balloonBtn("no");
	}

	public void clickYes() {

		Debug.Log("balloon : ok");
		speechbubble1.transform.position = okspeechballoon1.transform.position;
		okspeechballoon1.transform.position = new Vector3(-1.7f, 1.9f, -1);
		selecting1.transform.position = ok1.transform.position;
		ok1.transform.position = new Vector3(-1.7f, 1.9f, -2);
	}
	public void ClickX() {
		Debug.Log("balloon : no");
		no1.transform.position = new Vector3(-1.7f, 1.9f, -2);
	}
	public void oppClickYes() {
		oppSelected = true;
		speechbubble2.transform.position = okspeechballoon2.transform.position;
		okspeechballoon2.transform.position = new Vector3(1.7f, 1.9f, -1);
		selecting2.transform.position = ok2.transform.position;
		ok2.transform.position = new Vector3(1.7f, 1.9f, -2);
		if (balloonState == BALLOONSTATE.SELECTED) {
			balloonState = BALLOONSTATE.ANIMATION;
			nextSceneFlag = true;
		}
	}

	public void oppClickX() {
		oppSelected = true;
		no2.transform.position = new Vector3(1.7f, 1.9f, -2);
		if(balloonState == BALLOONSTATE.SELECTED){
			balloonState = BALLOONSTATE.ANIMATION;
			nextSceneFlag = true;
		}
	}

	public void select(int muti, string dec) {
		if (dec == "yes") {
			clickYes();
		}
		else {
			ClickX();
		}
	}
	public void oppSelect(int muti , string dec) {
		if(dec == "yes"){
			oppClickYes();
		}
		else {
			oppClickX();
		}
	}

	public void enableTouch() {
		if (Input.GetMouseButtonDown(0)) {
			Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
			if (hit.collider != null) {
				if(hit.collider.tag == "balloonYesBtn"){
					Btn_Yes1.transform.position = Btn_Yes2.transform.position;
					Btn_Yes2.transform.position = new Vector3(0.7f, -2.7f, -2);
					balloonState = BALLOONSTATE.SELECTED;
					
					clickedYesBtn();
				}else if(hit.collider.tag == "balloonXBtn"){
					
					Btn_x1.transform.position = Btn_x2.transform.position;
					Btn_x2.transform.position = new Vector3(1.5f, -0.7f, -2);
					balloonState = BALLOONSTATE.SELECTED;
					
					clickedXBtn();
				}else if(hit.collider.tag == "balloonNoBtn"){
					Btn_No1.transform.position = Btn_No2.transform.position;
					Btn_No2.transform.position = new Vector3(-0.7f, -2.7f, -2);
					//swapGameObject(Btn_No1, Btn_No2);
					balloonState = BALLOONSTATE.SELECTED;

					clickedXBtn();
				}

				if (balloonState == BALLOONSTATE.SELECTED && oppSelected) {
					balloonState = BALLOONSTATE.ANIMATION;
				}
			}
		}
	}

	public void balloonAnimation() {
		//만약 선택중일 경우
		//float t = Time.deltaTime;
		if (balloonState == BALLOONSTATE.SELECTING || balloonState == BALLOONSTATE.SELECTED) {

			if (balloonState == BALLOONSTATE.SELECTING) {
				gauge.transform.localScale -= new Vector3(0.003f, 0, 0);
				if (gauge.transform.localScale.x < 0.01) {
					balloonState = BALLOONSTATE.SELECTED;
					clickedXBtn();
					if (balloonState == BALLOONSTATE.SELECTED && oppSelected) {
						balloonState = BALLOONSTATE.ANIMATION;
					}
				}
			}

			//point를 이동시킴
			if (Time.time > nextPoint) {
				nextPoint = Time.time + pointRate;
				if (dotlocation == 1) {
					if (balloonState == BALLOONSTATE.SELECTING) {
						point1.transform.position = new Vector3(-1.86f, 1.9f, -2);
					}
					if (!oppSelected) {
						point2.transform.position = new Vector3(1.54f, 1.9f, -2);
					}
					dotlocation = 2;
				}
				else if (dotlocation == 2) {
					if (balloonState == BALLOONSTATE.SELECTING) {
						point1.transform.position = new Vector3(-1.7f, 1.9f, -2);
					}
					if (!oppSelected) {
						point2.transform.position = new Vector3(1.7f, 1.9f, -2);
					}
					dotlocation = 3;
				}
				else {
					if (balloonState == BALLOONSTATE.SELECTING) {
						point1.transform.position = new Vector3(-1.54f, 1.9f, -2);
					}
					if (!oppSelected) {
						point2.transform.position = new Vector3(1.86f, 1.9f, -2);
					}
					dotlocation = 1;
				}
			}
		}
	}
	public void setBalloon() {

		if (nextSceneFlag) {
			if (balloonState == BALLOONSTATE.INIT) {
				nextSceneFlag = false;
				balloonState = BALLOONSTATE.SELECTING;

				Btn_x2 = gameResource.getBalloonResource("balloon/Btn_x2");
				Btn_Yes2 = gameResource.getBalloonResource("balloon/Btn_Yes2");
				Btn_No2 = gameResource.getBalloonResource("balloon/Btn_No2");

				okspeechballoon1 = gameResource.getBalloonResource("balloon/okspeechballoon1");
				okspeechballoon2 = gameResource.getBalloonResource("balloon/okspeechballoon2");

				ok1 = gameResource.getBalloonResource("balloon/ok1");
				ok2 = gameResource.getBalloonResource("balloon/ok2");
				no1 = gameResource.getBalloonResource("balloon/no1");
				no2 = gameResource.getBalloonResource("balloon/no2");

				// 리소스 이동시키기
				board1 = gameResource.getBalloonResource("balloon/balloonboard1");
				board1.transform.position = new Vector3(0.0f, -2.0f, -1.0f);
				//todo : +된 숫자를 유저케릭터 번호로 바꿔준다.
				userChar = gameResource.getBalloonResource("balloon/char" + 1);
				userChar.transform.position = new Vector3(-1.0f, 1.0f, -1.0f);
				oppChar = gameResource.getBalloonResource("balloon/oppchar" + 2);
				oppChar.transform.position = new Vector3(1.0f, 1.0f, -1.0f);

				charboard1 = gameResource.getBalloonResource("balloon/charboard1");
				charboard1.transform.position = new Vector3(-1.0f, 0.3f, -1);

				charboard2 = gameResource.getBalloonResource("balloon/charboard2");
				charboard2.transform.position = new Vector3(1.0f, 0.3f, -1);


				point1 = gameResource.getBalloonResource("balloon/point1");
				point1.transform.position = new Vector3(0.0f, -2.0f, -1);
				point2 = gameResource.getBalloonResource("balloon/point2");
				point2.transform.position = new Vector3(0.0f, -2.0f, -1);

				selecting1 = gameResource.getBalloonResource("balloon/selecting1");
				selecting1.transform.position = new Vector3(-1.7f, 1.9f, -2);
				selecting2 = gameResource.getBalloonResource("balloon/selecting2");
				selecting2.transform.position = new Vector3(1.7f, 1.9f, -2);

				speechbubble1 = gameResource.getBalloonResource("balloon/speechbubble1");
				speechbubble1.transform.position = new Vector3(-1.7f, 1.9f, -1);
				speechbubble2 = gameResource.getBalloonResource("balloon/speechbubble2");
				speechbubble2.transform.position = new Vector3(1.7f, 1.9f, -1);

				title = gameResource.getBalloonResource("balloon/title");
				title.transform.position = new Vector3(0.0f, 3.0f, -1);
				ballooncoin = gameResource.getBalloonResource("balloon/ballooncoin");
				ballooncoin.transform.position = new Vector3(0.8f, -1.5f, -2);
				balloonboard2 = gameResource.getBalloonResource("balloon/balloonboard2");
				balloonboard2.transform.position = new Vector3(-0.6f, -1.3f, -2);
				Btn_x1 = gameResource.getBalloonResource("balloon/Btn_x1");
				Btn_x1.transform.position = new Vector3(1.5f, -0.7f, -2);
				Btn_Yes1 = gameResource.getBalloonResource("balloon/Btn_Yes1");
				Btn_Yes1.transform.position = new Vector3(0.7f, -2.7f, -2);
				Btn_No1 = gameResource.getBalloonResource("balloon/Btn_No1");
				Btn_No1.transform.position = new Vector3(-0.7f, -2.7f, -2);

				btext1 = gameResource.getBalloonResource("balloon/btext1");
				btext1.transform.position = new Vector3(-0.4f, -1.8f, -2);

				btext2 = gameResource.getBalloonResource("balloon/btext2");
				btext2.transform.position = new Vector3(-0.6f, -1.3f, -3);

				gauge = gameResource.getBalloonResource("balloon/gauge");
				gauge.transform.position = new Vector3(-1.372f, -2.1f, -4);
				voidGauge = gameResource.getBalloonResource("balloon/voidgauge");
				voidGauge.transform.position = new Vector3(0.0f, -2.1f, -3);
			}

			if (balloonState == BALLOONSTATE.SELECTED) {

				nextSceneFlag = false;
				//point2.transform.position = new Vector3(5, 0, 0);
				//selecting1.transform.position = new Vector3(5, 0, 0);
				//selecting2.transform.position = new Vector3(5, 0, 0);
				//speechbubble1.transform.position = new Vector3(5, 0, 0);
				//speechbubble2.transform.position = new Vector3(5, 0, 0);
				//title.transform.position = new Vector3(5, 0, 0);
				Btn_No2.transform.position = new Vector3(5, 0, 0);
				Btn_x2.transform.position = new Vector3(5, 0, 0);
				Btn_Yes2.transform.position = new Vector3(5, 0, 0);
				board1.transform.position = new Vector3(5, 0, 0);
				point1.transform.position = new Vector3(5, 0, 0);
				ballooncoin.transform.position = new Vector3(5, 0, 0);
				balloonboard2.transform.position = new Vector3(5, 0, 0);
				Btn_x1.transform.position = new Vector3(5, 0, 0);
				Btn_Yes1.transform.position = new Vector3(5, 0, 0);
				Btn_No1.transform.position = new Vector3(5, 0, 0);
				btext1.transform.position = new Vector3(5, 0, 0);
				btext2.transform.position = new Vector3(5, 0, 0);
				gauge.transform.position = new Vector3(5, 0, 0);
				voidGauge.transform.position = new Vector3(5, 0, 0);
			}
			if (balloonState == BALLOONSTATE.ANIMATION) {


				nextSceneFlag = false;

				selecting1.transform.position = new Vector3(5, 0, 0);
				selecting2.transform.position = new Vector3(5, 0, 0);
				speechbubble1.transform.position = new Vector3(5, 0, 0);
				speechbubble2.transform.position = new Vector3(5, 0, 0);
				title.transform.position = new Vector3(5, 0, 0);
				Btn_No2.transform.position = new Vector3(5, 0, 0);
				Btn_x2.transform.position = new Vector3(5, 0, 0);
				Btn_Yes2.transform.position = new Vector3(5, 0, 0);
				board1.transform.position = new Vector3(5, 0, 0);
				point1.transform.position = new Vector3(5, 0, 0);
				point2.transform.position = new Vector3(5, 0, 0);
				ballooncoin.transform.position = new Vector3(5, 0, 0);
				balloonboard2.transform.position = new Vector3(5, 0, 0);
				Btn_x1.transform.position = new Vector3(5, 0, 0);
				Btn_Yes1.transform.position = new Vector3(5, 0, 0);
				Btn_No1.transform.position = new Vector3(5, 0, 0);
				btext1.transform.position = new Vector3(5, 0, 0);
				btext2.transform.position = new Vector3(5, 0, 0);
				gauge.transform.position = new Vector3(5, 0, 0);
				voidGauge.transform.position = new Vector3(5, 0, 0);

				StartCoroutine("setAni");
				// 미구현 애니메이션 보여주고
				
				nextSceneFlag = true;
			}
			if (balloonState == BALLOONSTATE.END) {
				nextSceneFlag = false;
				//모든 객체 초기화 위치로
			}
		}
		
	}
	IEnumerator setAni() {
		yield return new WaitForSeconds(0.2f);
		okspeechballoon1.transform.position = new Vector3(5, 0, 0);
		okspeechballoon2.transform.position = new Vector3(5, 0, 0);
		ok1.transform.position = new Vector3(5, 0, 0);
		ok2.transform.position = new Vector3(5, 0, 0);
		no1.transform.position = new Vector3(5, 0, 0);
		no2.transform.position = new Vector3(5, 0, 0);
		userChar.transform.position = new Vector3(5, 0, 0);
		oppChar.transform.position = new Vector3(5, 0, 0);

		charboard1.transform.position = new Vector3(5, 0, 0);
		charboard2.transform.position = new Vector3(5, 0, 0);
		yield return new WaitForSeconds(0.2f);
		balloonState = BALLOONSTATE.END;
	}
}
