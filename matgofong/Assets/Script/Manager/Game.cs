using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	/// <summary>
	/// state
	/// </summary>

	enum GAMESTATE {
		INIT,//초기화
		WAITING,//잠시 대기
		SELECTTURN,//선선택
		MYTURN,//나의턴
		SELECTEDCARD,//카드 선택됨
		CALCSCORE,//점수계산
		SELECTPOPUP,//2개중1개선택
		NEXTSELECTPOPUP,
		TENENDPOPUP,//10끗
		GOSTOPPOPUP,//고스톱
		OPPTURN,//상대방턴
		OPPSELECTINGCARD,//카드 선택중
		END,
	}

	enum AUTOSTATE {
		TRUE,
		FALSE
	}

	/// <summary>
	/// 상수
	/// </summary>
	private readonly float[] oppHandCardsPointX = new float[] { -2.25f, -2.10f, -1.95f, -1.80f, -1.65f, -2.25f, -2.10f, -1.95f, -1.80f, -1.65f };
	private readonly float[] oppHandCardsPointY = new float[] { 2.9f, 2.9f, 2.9f, 2.9f, 2.9f, 2.7f, 2.7f, 2.7f, 2.7f, 2.7f };

	private readonly float[] boardCardPointX = new float[] { -0.6f, -1f, -0.3f, 0.4f, -1.3f, 0.1f, -1.7f, -1f, -0.3f, -1.7f, -2f, 0.8f, 0.4f };
	private readonly float[] boardCardPointY = new float[] { 1f, 2f, 2f, 2f, 1f, 1f, 0f, 0f, 0f, 2f, 1f, 1f, 0f };

	private readonly float[] boardCardStackX = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f };
	private readonly float[] boardCardStackY = new float[] { -0.1f, -0.2f, -0.3f, -0.4f, -0.5f, -0.6f };
	private readonly float[] boardCardStackZ = new float[] { -1.0f, -2.0f, -3.0f, -4.0f, -5.0f, -6.0f };

	private readonly float[] MyHandCardsPointX = new float[] { -1.9f, -1.0f, -0.1f, 0.9f, 1.8f, -1.8f, -0.9f, -0.0f, 1.0f, 1.9f };
	private readonly float[] MyHandCardsPointY = new float[] { -2.45f, -2.45f, -2.45f, -2.45f, -2.45f, -3.3f, -3.3f, -3.3f, -3.3f, -3.3f };
	private readonly float[] MyHandCardsPointZ = new float[] { -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f };


	private readonly float[] oppGwangX = new float[] { -2.1f, -2.0f, -1.9f, -1.8f, -1.7f };
	private readonly float[] oppGwangY = new float[] { 3.4f, 3.4f, 3.4f, 3.4f, 3.4f };
	private readonly float[] oppGwangZ = new float[] { -1.0f, -1.1f, -1.2f, -1.3f, -1.4f };

	private readonly float[] oppDanX = new float[] { -1.2f, -1.1f, -1.0f, -0.9f, -0.8f, -0.7f, -0.6f, -0.5f, -0.4f, -0.3f };
	private readonly float[] oppDanY = new float[] { 2.9f, 2.9f, 2.9f, 2.9f, 2.9f, 2.9f, 2.9f, 2.9f, 2.9f, 2.9f };
	private readonly float[] oppDanZ = new float[] { -1.0f, -1.1f, -1.2f, -1.3f, -1.4f, -1.5f, -1.6f, -1.7f, -1.8f, -1.9f };

	private readonly float[] oppEndX = new float[] { -1.2f, -1.1f, -1.0f, -0.9f, -0.8f, -0.7f, -0.6f, -0.5f, -0.4f, -0.3f };
	private readonly float[] oppEndY = new float[] { 3.4f, 3.4f, 3.4f, 3.4f, 3.4f, 3.4f, 3.4f, 3.4f, 3.4f, 3.4f };
	private readonly float[] oppEndZ = new float[] { -2.0f, -2.1f, -2.2f, -2.3f, -2.4f, -2.5f, -2.6f, -2.7f, -2.8f, -2.9f };


	private readonly float[] oppBloodX = new float[] { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };
	private readonly float[] oppBloodY = new float[] { 2.9f, 2.9f, 2.9f, 2.9f, 2.9f, 2.9f, 2.9f, 2.9f, 2.9f, 2.9f };
	private readonly float[] oppBloodZ = new float[] { -1.0f, -1.1f, -1.2f, -1.3f, -1.4f, -1.5f, -1.6f, -1.7f, -1.8f, -1.9f };
	private readonly float[] oppBloodStackY = new float[] { 0.0f, 0.25f, 0.5f };
	private readonly float[] oppBloodStackZ = new float[] { 0.0f, -1.0f, -2.0f };

	private readonly float[] myGwangX = new float[] { -2.1f, -2.0f, -1.9f, -1.8f, -1.7f };
	private readonly float[] myGwangY = new float[] { -1.4f, -1.4f, -1.4f, -1.4f, -1.4f };
	private readonly float[] myGwangZ = new float[] { -1.0f, -1.1f, -1.2f, -1.3f, -1.4f };

	private readonly float[] myDanX = new float[] { -1.2f, -1.1f, -1.0f, -0.9f, -0.8f, -0.7f, -0.6f, -0.5f, -0.4f, -0.3f };
	private readonly float[] myDanY = new float[] { -1.4f, -1.4f, -1.4f, -1.4f, -1.4f, -1.4f, -1.4f, -1.4f, -1.4f, -1.4f };
	private readonly float[] myDanZ = new float[] { -1.0f, -1.1f, -1.2f, -1.3f, -1.4f, -1.5f, -1.6f, -1.7f, -1.8f, -1.9f };

	private readonly float[] myEndX = new float[] { -1.2f, -1.1f, -1.0f, -0.9f, -0.8f, -0.7f, -0.6f, -0.5f, -0.4f, -0.3f };
	private readonly float[] myEndY = new float[] { -0.9f, -0.9f, -0.9f, -0.9f, -0.9f, -0.9f, -0.9f, -0.9f, -0.9f, -0.9f };
	private readonly float[] myEndZ = new float[] { -2.0f, -2.1f, -2.2f, -2.3f, -2.4f, -2.5f, -2.6f, -2.7f, -2.8f, -2.9f };

	private readonly float[] myBloodX = new float[] { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };
	private readonly float[] myBloodY = new float[] { -1.4f, -1.4f, -1.4f, -1.4f, -1.4f, -1.4f, -1.4f, -1.4f, -1.4f, -1.4f };
	private readonly float[] myBloodZ = new float[] { -1.0f, -1.1f, -1.2f, -1.3f, -1.4f, -1.5f, -1.6f, -1.7f, -1.8f, -1.9f };
	private readonly float[] myBloodStackY = new float[] { 0.0f, 0.25f, 0.5f };
	private readonly float[] myBloodStackZ = new float[] { 0.0f, -1.0f, -2.0f };

	const int BOARDFIRSTINDEX = 10;
	const int HANDFIRSTINDEX = 19;

	/// <summary>
	/// member
	/// </summary>

	public static Game instance;

	GameResourceManager gameResource;
	GameNetwork gameNetwork;


	OppCardInHand oppHandCards;
	MyCardInHand MyHandCards;
	CardInGameBoard gameBoardCards;
	OppCardInScoreBoard oppScoreBoardCards;
	CardInScoreBoard scoreBoardCards;


	GAMESTATE gameState;
	public bool start;
	bool setting = false;

	//선택으로 사용된 카드 번호
	List<int> tempCards;
	List<int> tempCards1;
	List<int> tempCards2;
	List<int> tempCards3;
	int tempPos;
	int tempPos2;

	public Text oppUserScore;
	public Text UserScore;

	public static Game getInstance() {
		return instance;
	}
	/// <summary>
	/// 카드 번호를 카드 이름으로 변환해줌
	/// </summary>
	/// <param name="num"></param>
	/// <returns></returns>

	string getNumToCardName(int num) {
		if (num >= 10) {
			return ("card/pae00" + num);
		}
		else {
			return ("card/pae000" + num);
		}
	}

	void resortBoardPos(List<List<int> > boardPos) {
		for (int i = 1; i < boardPos.Count; i++) {
			for (int j = 0; j < boardPos[i].Count; j++) {
				string cardName = getNumToCardName(boardPos[i][j]);
				GameObject card = gameResource.getGameResource(cardName);
				card.transform.position = new Vector3(boardCardPointX[i] + boardCardStackX[j], boardCardPointY[i] + boardCardStackY[j], boardCardStackZ[j]);
			}
		}
	}

	void clearNextSelectPopup() {
		GameObject popup = gameResource.getGameResource("selectcardpopup");
		popup.transform.position = new Vector3(5.0f, 1.1f, -5.0f);

		string cardName1 = "popupcard" + tempCards3[0];
		string cardName2 = "popupcard" + tempCards3[1];

		GameObject card1 = gameResource.getGameResource(cardName1);
		GameObject card2 = gameResource.getGameResource(cardName2);

		card1.transform.position = new Vector3(5.0f, 1.2f, -6.0f);
		card2.transform.position = new Vector3(5.0f, 1.2f, -6.0f);
	}
	void clearSelectPopup() {
		GameObject popup = gameResource.getGameResource("selectcardpopup");
		popup.transform.position = new Vector3(5.0f, 1.1f, -5.0f);

		string cardName1 = "popupcard" + tempCards1[0];
		string cardName2 = "popupcard" + tempCards1[1];

		GameObject card1 = gameResource.getGameResource(cardName1);
		GameObject card2 = gameResource.getGameResource(cardName2);

		card1.transform.position = new Vector3(5.0f, 1.2f, -6.0f);
		card2.transform.position = new Vector3(5.0f, 1.2f, -6.0f);
	}

	void clearTenEndPopup() {
		GameObject popup = gameResource.getGameResource("tenendpopup");
		popup.transform.position = new Vector3(5.0f, 1.1f, -5.0f);
	}

	void clearGoStopPopup() {
		GameObject popup = gameResource.getGameResource("gostoppopup");
		popup.transform.position = new Vector3(5.0f, 1.1f, -5.0f);
	}

	/// <summary>
	/// 유니티 루프들
	/// </summary>

	void Awake() {
		start = false;
		instance = this;

		gameNetwork = GameNetwork.instance;
		gameResource = GameResourceManager.instance;

		gameResource.setGameResource();

		gameState = GAMESTATE.INIT;
	}

	void Start() {
		oppHandCards = new OppCardInHand();
		MyHandCards = new MyCardInHand();
		gameBoardCards = new CardInGameBoard();
		oppScoreBoardCards = new OppCardInScoreBoard();
		scoreBoardCards = new CardInScoreBoard();
	}

	// init
	void initGameObject() {
		//모든 오브젝트와 텍스트를 초기화 시킨다.
		//text
		oppUserScore.text = "";
		UserScore.text = "";

		//카드 이미지 위치 변경
		//
		gameResource.initGameResource();
	}
	
	void settingPae() {
		if (!setting) {
			setting = true;
			//network.setCard();
			gameNetwork.setCard();
			Debug.Log("패 뿌리기");
			oppHandCards.setCard(10);
			StartCoroutine(setCard());
		}
	}

	// selectturn

	void updateSelectTurn() {
		settingSelect();

		if (Input.GetMouseButtonDown(0)) {

			Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

			if (hit.collider != null) {
				Debug.Log("터치가 됨");

				if (hit.collider.tag == "overturnedCard") {
					Card card = hit.collider.GetComponent<Card>();
					int cardNum = card.getCardNum();

					if (cardNum > 10 && cardNum <= 18) {
						Debug.Log("터치가 됨");
						Card cardClass = hit.collider.GetComponent<Card>();
						int pos = cardClass.getBoardPos();
						Debug.Log("선택된 위치 : " + pos);
						//gameBoardCards.setSelectedCard(pos);
						//network.selectedTurn(pos);
						gameNetwork.selectedTurn(pos);
					}
				}
			}
		}
	}
	void settingSelect() {
		//if(false){
		if (!setting) {
			List<int> myCard = MyHandCards.getCardList();

			for (int i = 0; i < myCard.Count; i++) {
				int cardNumber = myCard[i];

				if (cardNumber >= 10) {
					GameObject obj = gameResource.getGameResource("card/pae00" + cardNumber);
					GameObject card = gameResource.getGameResource("card/pae0000" + (i + HANDFIRSTINDEX));
					obj.transform.position = card.transform.position;
					card.transform.position = new Vector3(5, 0, 0);
				}
				else {
					GameObject obj = gameResource.getGameResource("card/pae000" + cardNumber);

					GameObject card = gameResource.getGameResource("card/pae0000" + (i + HANDFIRSTINDEX));
					obj.transform.position = card.transform.position;
					card.transform.position = new Vector3(5, 0, 0);
				}
			}

			Debug.Log("선 선택하기");
			for (int i = 0; i < 8; i++) {
				GameObject obj = gameResource.getGameResource("card/Btn_패버튼002" + i);
				obj.transform.localScale = new Vector3(0.6f, 0.6f, 1);
				obj.transform.position = new Vector3(boardCardPointX[i + 1], boardCardPointY[i + 1], -1);
			}
			setting = true;
		}
	}


	//myturn 시작( 카드 선택 )

	void updateMyTurn() {
		if (Input.GetMouseButtonDown(0)) {

			Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
			if (hit.collider != null && hit.collider.tag == "Card") {
				Card card = hit.collider.GetComponent<Card>();
				int cardNum = card.getCardNum();
				if (MyHandCards.findCardInHand(cardNum)) {
					Debug.Log("터치가 됨");
					hit.collider.transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
					hit.collider.transform.position = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + 1.0f, -3.0f);
					if (cardNum > 48) {
						gameNetwork.selectBonusCardInHand(cardNum);
						gameState = GAMESTATE.SELECTEDCARD;
					}
					else {
						gameNetwork.selectCardInHand(cardNum);
						gameState = GAMESTATE.SELECTEDCARD;
					}
					// card를 선택한다.

				}
			}
		}
	}
	void updateNextSelectPopUp() {
		if (Input.GetMouseButtonDown(0)) {

			Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

			if (hit.collider != null && hit.collider.tag == "popupCard") {
				Card card = hit.collider.GetComponent<Card>();
				int cardNum = card.getCardNum();
				Debug.Log("터치가 됨");
				clearNextSelectPopup();

				//Card[1] 번째요소를 선택한 카드로 둔다.
				if (cardNum != tempCards3[0]) {
					int temp = tempCards3[0];
					tempCards3[0] = tempCards3[1];
					tempCards3[1] = temp;
				}
				gameNetwork.calcMoveCards(tempCards, tempCards1, tempCards2, tempCards3, tempPos, tempPos2);
				gameState = GAMESTATE.SELECTEDCARD;
			}
		}
	}

	void updateSelectPopUp() {
		if (Input.GetMouseButtonDown(0)) {

			Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

			if (hit.collider != null && hit.collider.tag == "popupCard") {
				Card card = hit.collider.GetComponent<Card>();
				int cardNum = card.getCardNum();
				Debug.Log("터치가 됨");
				clearSelectPopup();

				//Card[1] 번째요소를 선택한 카드로 둔다.
				if (cardNum != tempCards1[0]) {
					int temp = tempCards1[0];
					tempCards1[0] = tempCards1[1];
					tempCards1[1] = temp;
				}

				gameNetwork.selectCardInField(tempCards, tempCards1, tempPos);

				gameState = GAMESTATE.SELECTEDCARD;
			}
		}
	}

	/// <summary>
	/// todo!
	/// </summary>
	void updateTenEndPopUp() {
		if (Input.GetMouseButtonDown(0)) {

			Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
			if (hit.collider != null) {
				if (hit.collider.tag == "YesBtn") {
					//보내고 clear
					gameNetwork.tenEndMessage("yes");
					clearTenEndPopup();
				}
				else if (hit.collider.tag == "NoBtn") {
					gameNetwork.tenEndMessage("no");
					clearTenEndPopup();
				}
			}
		}
	}

	void updateGostopPopUp() {
		if (Input.GetMouseButtonDown(0)) {

			Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
			if (hit.collider != null) {
				if (hit.collider.tag == "YesBtn") {
					gameNetwork.gostopMessage("go");
					clearGoStopPopup();
				}
				else if (hit.collider.tag == "NoBtn") {
					gameNetwork.gostopMessage("stop");
					clearGoStopPopup();
				}
			}
		}
	}

	void updateOppTurn() {

	}

	//게임 업데이트
	void Update() {
		if (start) {
			// 게임 초기화
			if (gameState == GAMESTATE.INIT) {
				settingPae();
			}
			//게임 선 정하기
			else if (gameState == GAMESTATE.SELECTTURN) {
				updateSelectTurn();
			}
			// 내 턴
			else if (gameState == GAMESTATE.MYTURN) {
				updateMyTurn();
			}
			else if (gameState == GAMESTATE.SELECTEDCARD) {
				//updateSelectedCard();
			}
			else if (gameState == GAMESTATE.SELECTPOPUP) {
				//2개의 패중 하나를 선택하는 팝업을 띄운다. 선택강요.
				updateSelectPopUp();
			}
			else if (gameState == GAMESTATE.NEXTSELECTPOPUP) {
				//2개의 패중 하나를 선택하는 팝업을 띄운다. 선택강요.
				updateNextSelectPopUp();
			}
			else if (gameState == GAMESTATE.TENENDPOPUP) {
				//열끗을 옴길껀지?
				updateTenEndPopUp();
			}
			else if (gameState == GAMESTATE.GOSTOPPOPUP) {
				//고스톱 선택
				updateGostopPopUp();
			}
			else if (gameState == GAMESTATE.CALCSCORE) {
				//점수계산
			}
			//적턴
			else if (gameState == GAMESTATE.OPPTURN) {
				updateOppTurn();
			}
			else if (gameState == GAMESTATE.OPPSELECTINGCARD) {
				updateOppTurn();
			}
			else if (gameState == GAMESTATE.END) {
				//없어도됨
			}
		}
	}
	public void setScore(int score , string user) {
		//스코어 세팅
		if(user == UserInfo.getInstance().Id){
			//본인점수
			UserScore.text = score.ToString();
		}
		else {
			//상대 점수
			oppUserScore.text = score.ToString();
		}
	}

	public void endGame() {
		//모든걸 초기화 시킴

		initGameObject();

		gameState = GAMESTATE.END;
	}

	//열끗 -> 피로 이동 애니메이션
	public void movedTenEnd() {
		gameNetwork.calcScore();

		string name = getNumToCardName(33);
		GameObject obj = gameResource.getGameResource(name);

		Card card = obj.GetComponent<Card>();

		scoreBoardCards.removeEndCard(33);

		scoreBoardCards.setBloodCard(33);
		int index = scoreBoardCards.findPosBlood(33);
		int stack = index / 10;
		int pos = index % 10;

		obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
		card.moveCard(myBloodX[pos], myBloodY[pos] + myBloodStackY[stack], myBloodZ[pos] + myBloodStackZ[stack]);

	}
	public void movedOppTenEnd() {
		string name = getNumToCardName(33);
		GameObject obj = gameResource.getGameResource(name);

		Card card = obj.GetComponent<Card>();

		oppScoreBoardCards.removeEndCard(33);

		oppScoreBoardCards.setBloodCard(33);
		int index = oppScoreBoardCards.findPosBlood(33);
		int stack = index / 10;
		int pos = index % 10;

		obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
		card.moveCard(oppBloodX[pos], oppBloodY[pos] + oppBloodStackY[stack], oppBloodZ[pos] + oppBloodStackZ[stack]);
	}

	public void setTurn(string turnuser) {
		if (turnuser == UserInfo.getInstance().Id) {
			gameState = GAMESTATE.MYTURN;
		}
		else {
			gameState = GAMESTATE.OPPTURN;
		}
	}

	public void setInitTurn(string user, List<int> Card,List<int> swapCards) {
		
		gameBoardCards.setInitTurnCards(Card);
		// 카드 뒤집는 애니메이션
		// 코루틴으로 카드를 연다.
		StartCoroutine(openInitCard( user, swapCards));

	}

	public void setNextTurn(int turn) {
		//내턴
		if (turn == 1) {
			gameState = GAMESTATE.MYTURN;
		}
		else {
			gameState = GAMESTATE.OPPTURN;
		}
	}

	//Game클래스의 Update를 종료한다.
	public bool isEnd() {
		if (gameState == GAMESTATE.END) {
			gameState = GAMESTATE.INIT;
			return true;
		}
		return false;
	}

	public void setMyCardInHand(List<int> nums) {
		MyHandCards.setCard(nums);
		if(gameState == GAMESTATE.WAITING){
			gameState = GAMESTATE.SELECTTURN;
		}
		else {
			gameState = GAMESTATE.WAITING;
		}
	}

	/*
	 * 
	 * gameManager로 부터 받은 메시지
	*/

	// 선선택 메시지
	public void setSelectedTurnCard(int pos, int num) {
		if (num >= 10) {
			GameObject card = gameResource.getGameResource("card/pae00" + num);
			//int pos = gameBoardCards.getSelectedCard();
			Debug.Log("위치 :" + pos);
			GameObject obj = gameResource.getGameResource("card/pae0000" + (pos + BOARDFIRSTINDEX));
			card.transform.position = obj.transform.position;
			card.transform.localScale = new Vector3(0.6f, 0.6f, 1);
			obj.transform.position = new Vector3(5, 0, 0);
		}

		else {
			GameObject card = gameResource.getGameResource("card/pae000" + num);
			//int pos = gameBoardCards.getSelectedCard();
			Debug.Log("위치 :" + pos);
			GameObject obj = gameResource.getGameResource("card/pae0000" + (pos + BOARDFIRSTINDEX));
			card.transform.position = obj.transform.position;
			card.transform.localScale = new Vector3(0.6f, 0.6f, 1);
			obj.transform.position = new Vector3(5, 0, 0);
		}

		gameBoardCards.setCard(pos, num);
	}

	public void setoppSelectedTurnCard(int pos, int num) {

		if (num >= 10) {
			GameObject card = gameResource.getGameResource("card/pae00" + num);
			//int pos = gameBoardCards.getSelectedCard();
			Debug.Log("위치 :" + pos);
			GameObject obj = gameResource.getGameResource("card/pae0000" + (pos + BOARDFIRSTINDEX));
			card.transform.position = obj.transform.position;
			card.transform.localScale = new Vector3(0.6f, 0.6f, 1);
			obj.transform.position = new Vector3(5, 0, 0);
		}

		else {
			GameObject card = gameResource.getGameResource("card/pae000" + num);
			//int pos = gameBoardCards.getSelectedCard();
			Debug.Log("위치 :" + pos);
			GameObject obj = gameResource.getGameResource("card/pae0000" + (pos + BOARDFIRSTINDEX));
			card.transform.position = obj.transform.position;
			card.transform.localScale = new Vector3(0.6f, 0.6f, 1);
			obj.transform.position = new Vector3(5, 0, 0);
		}
		gameBoardCards.setCard(pos, num);
	}

	/// <summary>
	/// 서버를 통해 상대방이 매칭한 두개의 카드를 이동 시킨다.
	/// </summary>
	/// <param name="moveCard"> 이동할 카드 </param>
	/// <param name="targetPos"> 이동할 위치 </param>

	public void oppMatchingCards(int moveCard, int targetPos) {

		int top = gameBoardCards.findTopPos(targetPos);


		string mcard = getNumToCardName(moveCard);
		GameObject moveCardObj = gameResource.getGameResource(mcard);
		moveCardObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		moveCardObj.transform.position = new Vector3(-1.0f, 2.0f, -2);

		gameBoardCards.insertCard(targetPos, moveCard);

		StartCoroutine(oppMatchCardAni(moveCardObj, moveCard, top, targetPos));
	}

	public void oppMatchingNextCards(List<int> cards, int pos1, int pos2) {
		StartCoroutine(oppMatchNextCardAni(cards, pos1, pos2));
	}

	/// <summary>
	/// 서버로부터 매칭된 카드를 받아 이동시킨다.
	/// 다음 카드를 받았을때 점수 처리를 위해 가져 오는 카드들 목록을 모두 들고 있는다.
	/// </summary>
	/// <param name="cards">가져올 카드의 목록 cards[0]번을 pos로 이동시킨다.</param>
	/// <param name="pos">이동 시킬 위치</param>
	public void matchedCards(List<int> handCards, List<int> matchCards, int pos) {

		int top = gameBoardCards.findTopPos(pos);
		
		gameBoardCards.insertCard(pos, handCards[0]);
		StartCoroutine(matchCardAni(handCards, matchCards, top, pos));
	}

	public void matchedNextCards(List<int> handCards, List<int> getCards, List<int> drawCards, List<int> matchedCards, int pos1, int pos2) {
		StartCoroutine(matchNextCardAni(handCards, getCards, drawCards, matchedCards, pos1, pos2));
	}

	//고스톱 팝업을 보여준다.
	public void showGoStopPopup() {
		GameObject popup = gameResource.getGameResource("gostoppopup");
		popup.transform.position = new Vector3(-0.7f, 1.1f, -5.0f);
		gameState = GAMESTATE.GOSTOPPOPUP;
	}

	public void showEndPopup() {
		GameObject popup = gameResource.getGameResource("tenendpopup");
		popup.transform.position = new Vector3(-0.7f, 1.1f, -5.0f);
		gameState = GAMESTATE.TENENDPOPUP;
	}

	/// <summary>
	/// 두개의 카드중 하나의 카드를 선택한다.
	/// </summary>
	/// <param name="cards"></param>
	/// <param name="pos"></param>
	public void selectCardInField(List<int> handCards, List<int> cards, int pos) {
		//필드에서 카드를 선택한다.
		// 팝업을 띄운다
		StartCoroutine(selectCardAni(handCards, cards, pos));
	}

	public void selectNextCardInField(List<int> handCards, List<int> getCards, List<int> drawCards, List<int> matchedCards, int pos1, int pos2) {

		tempCards = handCards;
		tempCards1 = getCards;
		tempCards2 = drawCards;
		tempCards3 = matchedCards;

		tempPos = pos1;
		tempPos2 = pos2;

		StartCoroutine(selectNextCardAni(drawCards, matchedCards, pos1, pos2));
	}

	public void getOppScoreCards(List<int> gwangCards, List<int> danCards, List<int> endCards, List<int> bloodCards, List<int> recvBloodCards) {

		//패들을 이동 시킨다. -> score에 카드를 넣고 위치를 구한 뒤 이동 시킴
		StartCoroutine(moveOppScoreCardsAni(gwangCards, danCards, endCards, bloodCards, recvBloodCards));

	}

	public void getScoreCards(List<int> gwangCards, List<int> danCards, List<int> endCards, List<int> bloodCards, List<int> recvBloodCards) {
		//패들을 이동 시킨다. -> score에 카드를 넣고 위치를 구한 뒤 이동 시킴
		StartCoroutine(moveScoreCardsAni(gwangCards,danCards,endCards,bloodCards,recvBloodCards));
	}

	public void oppUseBonusCard(int bonusCard, int recvBlood){
		//보너스패 새팅
		if(bonusCard != 0){ 
			string bonusName = getNumToCardName(bonusCard);
			GameObject bonusObj = gameResource.getGameResource(bonusName);
			bonusObj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			oppScoreBoardCards.setBloodCard(bonusCard);
			int index = oppScoreBoardCards.findPosBlood(bonusCard);
			int stack = index / 10;
			int pos = index % 10;
			bonusObj.transform.position = new Vector3(oppBloodX[pos], oppBloodY[pos] + oppBloodStackY[stack], oppBloodZ[pos] + oppBloodStackZ[stack]);
		}
		//내피 빼고 적피 채워준다. -> getScoreCards 참조
		if (recvBlood != 0) {
			string name = getNumToCardName(recvBlood);
			GameObject obj = gameResource.getGameResource(name);

			Card card = obj.GetComponent<Card>();

			scoreBoardCards.removeBloodCard(recvBlood);

			oppScoreBoardCards.setBloodCard(recvBlood);
			int index = oppScoreBoardCards.findPosBlood(recvBlood);
			int stack = index / 10;
			int pos = index % 10;

			obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			// 이동 시킴
			card.moveCard(oppBloodX[pos], oppBloodY[pos] + oppBloodStackY[stack], oppBloodZ[pos] + oppBloodStackZ[stack]);
		}
	}
	public void useBonusCard(int bonusCard, int recvBlood, int swapCard) {
		// -> 카드 받기 참조
		if (bonusCard != 0) {
			string bonusName = getNumToCardName(bonusCard);
			GameObject bonusObj = gameResource.getGameResource(bonusName);
			bonusObj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			scoreBoardCards.setBloodCard(bonusCard);
			int index = scoreBoardCards.findPosBlood(bonusCard);
			int stack = index / 10;
			int pos = index % 10;
			bonusObj.transform.position = new Vector3(myBloodX[pos], myBloodY[pos] + myBloodStackY[stack], myBloodZ[pos] + myBloodStackZ[stack]);
		}
		if (recvBlood != 0) {
			string name = getNumToCardName(recvBlood);
			GameObject obj = gameResource.getGameResource(name);

			Card card = obj.GetComponent<Card>();

			oppScoreBoardCards.removeBloodCard(recvBlood);

			scoreBoardCards.setBloodCard(recvBlood);
			int index = scoreBoardCards.findPosBlood(recvBlood);
			int stack = index / 10;
			int pos = index % 10;
			// 이동 시킴
			obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			card.moveCard(myBloodX[pos], myBloodY[pos] + myBloodStackY[stack], myBloodZ[pos] + myBloodStackZ[stack]);
		}
		if(swapCard != 0){ 
			//swap 카드를 넣는다.
			string name = getNumToCardName(swapCard);
			GameObject card = gameResource.getGameResource(name);
			card.transform.position = new Vector3(boardCardPointX[0], boardCardPointY[0], -1f);
			Card cardClass = card.GetComponent<Card>();
			cardClass.speed = 10;
			MyHandCards.swapCard(bonusCard, swapCard);
			int pos = MyHandCards.posCard(swapCard);

			cardClass.moveCard(MyHandCardsPointX[pos], MyHandCardsPointY[pos], MyHandCardsPointZ[pos]);
		}
		gameState = GAMESTATE.MYTURN;
	}

	IEnumerator moveOppScoreCardsAni(List<int> gwangCards, List<int> danCards, List<int> endCards, List<int> bloodCards, List<int> recvBloodCards) {
		for (int i = 0; i < gwangCards.Count; i++) {
			gameBoardCards.removeCard(gwangCards[i]);
			

			string name = getNumToCardName(gwangCards[i]);
			GameObject obj = gameResource.getGameResource(name);

			Card card = obj.GetComponent<Card>();

			oppScoreBoardCards.setGwangCard(gwangCards[i]);
			int pos = oppScoreBoardCards.findPosGwang(gwangCards[i]);

			obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			card.moveCard(oppGwangX[pos], oppGwangY[pos], oppGwangZ[pos]);
		}
		for (int i = 0; i < danCards.Count; i++) {
			gameBoardCards.removeCard(danCards[i]);

			string name = getNumToCardName(danCards[i]);
			GameObject obj = gameResource.getGameResource(name);

			Card card = obj.GetComponent<Card>();

			oppScoreBoardCards.setDanCard(danCards[i]);
			int pos = oppScoreBoardCards.findPosDan(danCards[i]);
			// 이동 시킴
			obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			card.moveCard(oppDanX[pos], oppDanY[pos], oppDanZ[pos]);
		}
		for (int i = 0; i < endCards.Count; i++) {
			gameBoardCards.removeCard(endCards[i]);
			string name = getNumToCardName(endCards[i]);
			GameObject obj = gameResource.getGameResource(name);

			Card card = obj.GetComponent<Card>();

			oppScoreBoardCards.setEndCard(endCards[i]);
			int pos = oppScoreBoardCards.findPosEnd(endCards[i]);
			// 이동 시킴
			obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			card.moveCard(oppEndX[pos], oppEndY[pos], oppEndZ[pos]);
		}
		for (int i = 0; i < bloodCards.Count; i++) {
			gameBoardCards.removeCard(bloodCards[i]);
			string name = getNumToCardName(bloodCards[i]);
			GameObject obj = gameResource.getGameResource(name);

			Card card = obj.GetComponent<Card>();

			oppScoreBoardCards.setBloodCard(bloodCards[i]);
			int index = oppScoreBoardCards.findPosBlood(bloodCards[i]);
			int stack = index / 10;
			int pos = index % 10;
			obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			// 이동 시킴
			card.moveCard(oppBloodX[pos], oppBloodY[pos] + oppBloodStackY[stack], oppBloodZ[pos] + oppBloodStackZ[stack]);
		}
		for (int i = 0; i < recvBloodCards.Count; i++) {
			string name = getNumToCardName(recvBloodCards[i]);
			GameObject obj = gameResource.getGameResource(name);

			Card card = obj.GetComponent<Card>();

			scoreBoardCards.removeBloodCard(recvBloodCards[i]);

			oppScoreBoardCards.setBloodCard(recvBloodCards[i]);
			int index = oppScoreBoardCards.findPosBlood(recvBloodCards[i]);
			int stack = index / 10;
			int pos = index % 10;

			obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			// 이동 시킴
			card.moveCard(oppBloodX[pos], oppBloodY[pos] + oppBloodStackY[stack], oppBloodZ[pos] + oppBloodStackZ[stack]);

		}

		resortBoardPos(gameBoardCards.getBoardPos());
		yield return new WaitForSeconds(0.3f);
	}
	IEnumerator moveScoreCardsAni(List<int> gwangCards, List<int> danCards, List<int> endCards, List<int> bloodCards, List<int> recvBloodCards) {

		for (int i = 0; i < gwangCards.Count; i++) {

			gameBoardCards.removeCard(gwangCards[i]);

			string name = getNumToCardName(gwangCards[i]);
			GameObject obj = gameResource.getGameResource(name);

			Card card = obj.GetComponent<Card>();

			scoreBoardCards.setGwangCard(gwangCards[i]);
			int pos = scoreBoardCards.findPosGwang(gwangCards[i]);
			// 이동 시킴
			obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			card.moveCard(myGwangX[pos], myGwangY[pos], myGwangZ[pos]);
		}
		for (int i = 0; i < danCards.Count; i++) {

			gameBoardCards.removeCard(danCards[i]);

			string name = getNumToCardName(danCards[i]);
			GameObject obj = gameResource.getGameResource(name);

			Card card = obj.GetComponent<Card>();

			scoreBoardCards.setDanCard(danCards[i]);
			int pos = scoreBoardCards.findPosDan(danCards[i]);
			// 이동 시킴
			obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			card.moveCard(myDanX[pos], myDanY[pos], myDanZ[pos]);
		}
		for (int i = 0; i < endCards.Count; i++) {

			gameBoardCards.removeCard(endCards[i]);

			string name = getNumToCardName(endCards[i]);
			GameObject obj = gameResource.getGameResource(name);

			Card card = obj.GetComponent<Card>();

			scoreBoardCards.setEndCard(endCards[i]);
			int pos = scoreBoardCards.findPosEnd(endCards[i]);
			// 이동 시킴
			obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			card.moveCard(myEndX[pos], myEndY[pos], myEndZ[pos]);
		}
		for (int i = 0; i < bloodCards.Count; i++) {

			gameBoardCards.removeCard(bloodCards[i]);

			string name = getNumToCardName(bloodCards[i]);
			GameObject obj = gameResource.getGameResource(name);

			Card card = obj.GetComponent<Card>();

			scoreBoardCards.setBloodCard(bloodCards[i]);
			int index = scoreBoardCards.findPosBlood(bloodCards[i]);
			int stack = index / 10;
			int pos = index % 10;

			// 이동 시킴
			obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			card.moveCard(myBloodX[pos], myBloodY[pos] + myBloodStackY[stack], myBloodZ[pos] + myBloodStackZ[stack]);
		}
		for (int i = 0; i < recvBloodCards.Count; i++) {
			string name = getNumToCardName(recvBloodCards[i]);
			GameObject obj = gameResource.getGameResource(name);

			Card card = obj.GetComponent<Card>();

			oppScoreBoardCards.removeBloodCard(recvBloodCards[i]);

			scoreBoardCards.setBloodCard(recvBloodCards[i]);
			int index = scoreBoardCards.findPosBlood(recvBloodCards[i]);
			int stack = index / 10;
			int pos = index % 10;
			// 이동 시킴
			obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			card.moveCard(myBloodX[pos], myBloodY[pos] + myBloodStackY[stack], myBloodZ[pos] + myBloodStackZ[stack]);

		}
		resortBoardPos(gameBoardCards.getBoardPos());


		yield return new WaitForSeconds(0.3f);

		//세팅은 끝났다. 스코어를 받는다.

		gameNetwork.calcScore();
	}

	IEnumerator selectNextCardAni(List<int> drawCards, List<int> matchedCards, int pos1, int pos2) {

		for (int i = 0; i < drawCards.Count; i++) {
			int pos = pos2;

			if (drawCards[i] > 48) {
				pos = pos1;
			}

			int top = gameBoardCards.findTopPos(pos);
			
			gameBoardCards.insertCard(pos, drawCards[i]);

			string mcard = getNumToCardName(drawCards[i]);
			GameObject moveCardObj = gameResource.getGameResource(mcard);
			moveCardObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			moveCardObj.transform.position = new Vector3(-0.6f, 1.0f, -3);

			yield return new WaitForSeconds(0.3f);

			moveCardObj.transform.localScale = new Vector3(0.6f, 0.6f, 1.0f);
			moveCardObj.transform.position = new Vector3(boardCardPointX[pos] + boardCardStackX[top], boardCardPointY[pos] + boardCardStackY[top], boardCardStackZ[top]);
		}

		GameObject popup = gameResource.getGameResource("selectcardpopup");
		popup.transform.position = new Vector3(-0.7f, 1.1f, -5.0f);

		string cardName1 = "popupcard" + matchedCards[0];
		string cardName2 = "popupcard" + matchedCards[1];

		GameObject card1 = gameResource.getGameResource(cardName1);
		GameObject card2 = gameResource.getGameResource(cardName2);

		card1.transform.position = new Vector3(-1.3f, 1.2f, -7.0f);
		card2.transform.position = new Vector3(-0.1f, 1.2f, -7.0f);


		gameState = GAMESTATE.NEXTSELECTPOPUP;
	}

	/// <summary>
	/// 두개의 카드중 하나를 선택하는 애니메이션
	/// </summary>
	/// <param name="cards"></param>
	/// <param name="pos"></param>
	/// <returns></returns>
	IEnumerator selectCardAni(List<int> handCards, List<int> cards, int pos) {

		yield return new WaitForSeconds(0.3f);

		int top = gameBoardCards.findTopPos(pos);

		//1칸올림
		gameBoardCards.insertCard(pos, handCards[0]);

		tempCards = handCards;
		tempCards1 = cards;
		tempPos = pos;

		GameObject popup = gameResource.getGameResource("selectcardpopup");
		popup.transform.position = new Vector3(-0.7f, 1.1f, -5.0f);

		string cardName1 = "popupcard" + cards[0];
		string cardName2 = "popupcard" + cards[1];

		GameObject card1 = gameResource.getGameResource(cardName1);
		GameObject card2 = gameResource.getGameResource(cardName2);

		card1.transform.position = new Vector3(-1.3f, 1.2f, -7.0f);
		card2.transform.position = new Vector3(-0.1f, 1.2f, -7.0f);

		string mcard = getNumToCardName(handCards[0]);

		GameObject moveCardObj = gameResource.getGameResource(mcard);
		moveCardObj.transform.localScale = new Vector3(0.6f, 0.6f, 1.0f);
		moveCardObj.transform.position = new Vector3(boardCardPointX[pos] + boardCardStackX[top], boardCardPointY[pos] + boardCardStackY[top], boardCardStackZ[top]);


		gameState = GAMESTATE.SELECTPOPUP;
	}

	IEnumerator oppMatchCardAni(GameObject moveCardObj, int cardnum, int top, int pos) {
		yield return new WaitForSeconds(0.3f);
		moveCardObj.transform.localScale = new Vector3(0.6f, 0.6f, 1.0f);
		moveCardObj.transform.position = new Vector3(boardCardPointX[pos] + boardCardStackX[top], boardCardPointY[pos] + boardCardStackY[top], boardCardStackZ[top]);
	}

	/// <summary>
	/// 카드를 드로우 하는 애니메이션
	/// </summary>
	/// <param name="cards"></param>
	/// <param name="pos"></param>
	/// <returns></returns>
	IEnumerator oppMatchNextCardAni(List<int> cards, int pos1, int pos2) {

		for (int i = 0; i < cards.Count; i++) {
			int pos = pos2;
			if (cards[i] > 48) {
				pos = pos1;
			}

			int top = gameBoardCards.findTopPos(pos);
			gameBoardCards.insertCard(pos, cards[i]);

			string mcard = getNumToCardName(cards[i]);
			GameObject moveCardObj = gameResource.getGameResource(mcard);
			moveCardObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			moveCardObj.transform.position = new Vector3(-0.6f, 1.0f, -3);

			yield return new WaitForSeconds(1.0f);

			moveCardObj.transform.localScale = new Vector3(0.6f, 0.6f, 1.0f);
			moveCardObj.transform.position = new Vector3(boardCardPointX[pos] + boardCardStackX[top], boardCardPointY[pos] + boardCardStackY[top], boardCardStackZ[top]);
		}
	}

	IEnumerator matchCardAni(List<int> handCards, List<int> matchCards, int top, int pos) {
		string mcard = getNumToCardName(handCards[0]);
		GameObject moveCardObj = gameResource.getGameResource(mcard);
		moveCardObj.transform.localScale = new Vector3(0.6f, 0.6f, 1.0f);
		moveCardObj.transform.position = new Vector3(boardCardPointX[pos] + boardCardStackX[top], boardCardPointY[pos] + boardCardStackY[top], boardCardStackZ[top]);
		yield return new WaitForSeconds(0.1f);
		//보낸다.
		gameNetwork.drawNextCard(handCards, matchCards, pos);
	}

	IEnumerator matchNextCardAni(List<int> handCards, List<int> getCards, List<int> drawCards, List<int> matchedCards, int pos1, int pos2) {

		for (int i = 0; i < drawCards.Count; i++) {
			int pos = pos2;
			if (drawCards[i] > 48) {
				pos = pos1;
			}

			int top = gameBoardCards.findTopPos(pos);
			//1칸올림
			gameBoardCards.insertCard(pos, drawCards[i]);

			string mcard = getNumToCardName(drawCards[i]);
			GameObject moveCardObj = gameResource.getGameResource(mcard);
			moveCardObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			moveCardObj.transform.position = new Vector3(-0.6f, 1.0f, -3);

			yield return new WaitForSeconds(1.0f);

			moveCardObj.transform.localScale = new Vector3(0.6f, 0.6f, 1.0f);
			moveCardObj.transform.position = new Vector3(boardCardPointX[pos] + boardCardStackX[top], boardCardPointY[pos] + boardCardStackY[top], boardCardStackZ[top]);
		}
		gameNetwork.calcMoveCards(handCards, getCards, drawCards, matchedCards, pos1, pos2);
	}



	IEnumerator openInitCard(string turnuser,List<int> swapCards) {
		// 배열을 받음
		int[] cardList = gameBoardCards.getPosFirst();

		for (int i = 1; i < 9; i++) {
			Debug.Log("i : " + i + " card : " + cardList[i]);
			// 카드 위치를 이동 시킴 boardCardPointX
			string cardName = getNumToCardName(cardList[i]);
			GameObject card = gameResource.getGameResource(cardName);
			card.transform.position = new Vector3(boardCardPointX[i], boardCardPointY[i], -1);
			card.transform.localScale = new Vector3(0.6f, 0.6f, 1);
			GameObject obj = gameResource.getGameResource("card/pae0000" + (i + BOARDFIRSTINDEX));
			obj.transform.position = new Vector3(5, 0, 0);
			yield return new WaitForSeconds(0.05f);
		}

		//패버튼 초기화
		for (int i = 0; i < 8; i++) {
			GameObject obj = gameResource.getGameResource("card/Btn_패버튼002" + i);
			obj.transform.position = new Vector3(5, 0, 0);
		}

		if(swapCards.Count != 0){
			for (int i = 0; i < swapCards.Count; i++) {
				for (int j = 0; j < cardList.Length; j++) {
					if(cardList[j] > 48){
						//턴을 가진 유저에게 피를 준다.
						if (turnuser == UserInfo.getInstance().Id) {
							//내 점수로 이동
							string name = getNumToCardName(cardList[j]);
							GameObject obj = gameResource.getGameResource(name);

							Card card = obj.GetComponent<Card>();

							scoreBoardCards.setBloodCard(cardList[j]);
							int index = scoreBoardCards.findPosBlood(cardList[j]);
							int stack = index / 10;
							int pos = index % 10;

							// 이동 시킴
							obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
							card.moveCard(myBloodX[pos], myBloodY[pos] + myBloodStackY[stack], myBloodZ[pos] + myBloodStackZ[stack]);
							
						}
						else {
							//적에게 이동
							string name = getNumToCardName(cardList[j]);
							GameObject obj = gameResource.getGameResource(name);
							Card card = obj.GetComponent<Card>();
							oppScoreBoardCards.setBloodCard(cardList[j]);
							int index = oppScoreBoardCards.findPosBlood(cardList[j]);
							int stack = index / 10;
							int pos = index % 10;
							obj.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
							// 이동 시킴
							card.moveCard(oppBloodX[pos], oppBloodY[pos] + oppBloodStackY[stack], oppBloodZ[pos] + oppBloodStackZ[stack]);
						}
						cardList[j] = swapCards[i];
						string cardName = getNumToCardName(cardList[j]);
						GameObject relocationCard = gameResource.getGameResource(cardName);
						relocationCard.transform.localScale = new Vector3(0.6f, 0.6f, 1);
						relocationCard.transform.position = new Vector3(boardCardPointX[j], boardCardPointY[j], -1);

						break;
					}
				}
			}
		}
		yield return new WaitForSeconds(1.0f);
		
		gameBoardCards.sortCard(cardList);


		List<List<int> > boardPos = gameBoardCards.getBoardPos();
		resortBoardPos(boardPos);

			// 다음 상태로
		if (turnuser == UserInfo.getInstance().Id) {
			gameState = GAMESTATE.MYTURN;
		}
		else {
			gameState = GAMESTATE.OPPTURN;
		}
	}


	IEnumerator setCard() {
		GameObject cardset = gameResource.getGameResource("card/paeset");
		cardset.transform.position = new Vector3(boardCardPointX[0], boardCardPointY[0], -1f);

		for (int i = 1; i <= 28; i++) {
			// 1~ 10 상대방에게 뿌릴 카드
			if (i <= 10) {
				GameObject card = gameResource.getGameResource("card/pae0000" + i);
				card.transform.position = new Vector3(boardCardPointX[0], boardCardPointY[0], -1f);

				card.transform.localScale = new Vector3(0.15f, 0.15f, 1);

				Card cardClass = card.GetComponent<Card>();
				cardClass.setBoardPos(0);
				cardClass.moveCard(oppHandCardsPointX[i - 1], oppHandCardsPointY[i - 1], -1);

			}
			// 11 ~ 18까지 바닥에 뿌릴 카드
			else if (i > 10 && i <= 18) {
				GameObject card = gameResource.getGameResource("card/pae0000" + i);
				card.transform.position = new Vector3(boardCardPointX[0], boardCardPointY[0], -1f);
				card.transform.localScale = new Vector3(0.6f, 0.6f, 1);
				Card cardClass = card.GetComponent<Card>();
				//바닥에 번호를 입력한다.
				cardClass.setBoardPos(i - 10);
				cardClass.moveCard(boardCardPointX[i - 10], boardCardPointY[i - 10], -1);
			}
			else if (i > 18 && i <= 28) {
				GameObject card = gameResource.getGameResource("card/pae0000" + i);
				card.transform.position = new Vector3(boardCardPointX[0], boardCardPointY[0], -1f);
				Card cardClass = card.GetComponent<Card>();
				cardClass.speed = 10;
				cardClass.setBoardPos(0);
				cardClass.moveCard(MyHandCardsPointX[i - 19], MyHandCardsPointY[i - 19], MyHandCardsPointZ[i - 19]);
			}
			if (i == 5 || i == 10 || i == 14 || i == 18 || i == 23 || i == 28) {
				yield return new WaitForSeconds(0.3f);
			}
			else {
				yield return new WaitForSeconds(0.01f);
			}
		}
		setting = false;
		if (gameState == GAMESTATE.WAITING) {
			gameState = GAMESTATE.SELECTTURN;
		}
		else {
			gameState = GAMESTATE.WAITING;
		}
	}
}
