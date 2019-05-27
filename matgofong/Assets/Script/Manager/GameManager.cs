using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

	Balloon balloon;
	UserInfo user;
	OppInfo opp;
	Game game;
	Result result;
	GameNetwork gameNetwork;

	public Text oppUserName;


	void Awake() {
		gameNetwork = GameNetwork.instance;
		user = UserInfo.getInstance();
		opp = OppInfo.getInstance();

		//월래는 대기 상태로 시작하고 INIT 상태로 변경 되어야함
		user.UserState = UserInfo.STATE.WAIT;

		gameNetwork.Recv += new GameHandler(gameRouter);
	}

	void Start() {
		Camera.main.aspect = 480f / 800f;
		//Screen.SetResolution(270, 450, false);
		balloon = Balloon.getInstance();
		game = Game.getInstance();
		result = Result.getInstance();
		gameNetwork.enterRoom();

		gameNetwork.setLongPolling();
	}

	void Update() {
		// todo : 기본 메뉴 세팅 나가기, 케릭터 얼굴
		if (user.UserState == UserInfo.STATE.WAIT) {
			//만약 나가기 예약 상태이면 종료 시킨다.
		}

		//풍선껌
		if (user.UserState == UserInfo.STATE.BALLOON) {
			balloon.setBalloon();
			balloon.balloonAnimation();
			balloon.enableTouch();
			//balloon.oppSelect();

			if (balloon.isEnd()) {
				user.UserState = UserInfo.STATE.GAME;
			}
		}
		//게임 상태
		else if (user.UserState == UserInfo.STATE.GAME) {
			if (!game.start) {
				game.start = true;
			}
			//게임을 정지합니다. 앙 안되자나.
			if (game.isEnd()) {
				game.start = false;
				user.UserState = UserInfo.STATE.RESULT;
			}
		}
		// 결과 화면
		//선택지 -> 풍선껌
		//        -> 대기
		//        -> 메뉴
		else if (user.UserState == UserInfo.STATE.RESULT) {
			//결과 팝업
			if (!result.start) {
				result.start = true;
				result.getResult();
			}

			if (result.isEnd()) {
				game.start = false;
				user.UserState = UserInfo.STATE.RESULT;
			}
		}
	}



	/// <summary>
	/// 메시지를 Recv해서 상태에 맞게 데이터를 세팅합니다.
	/// </summary>
	/// <param name="message">서버에서 받은 JSON OBJECT 메시지</param>

	void gameRouter(string message) {
		JSONObject j = new JSONObject(message);
		int protocol = (int)j["protocol"].n;
		Debug.Log("Routing :" + message);
		switch (protocol) {
			//time-out
			case 0: {
					gameNetwork.setLongPolling();
				}
				break;

			//상대방 입장
			case 1: {
					opp.Id = j["opp"].str;
					oppUserName.text = opp.Id;
					user.Room = j["room"].str;
					user.UserState = UserInfo.STATE.BALLOON;
					gameNetwork.setLongPolling();
				}
				break;
			//내가 입장
			case 101: {
					if (j["user"].str == null) {
						Debug.Log("기다림");
					}
					else {
						Debug.Log("드루옴");
						opp.Id = j["user"].str;
						oppUserName.text = opp.Id;
						user.Room = j["room"].str;
						user.UserState = UserInfo.STATE.BALLOON;
					}
				}
				break;
			case 2: {
					int m = (int)j["muti"].n;
					balloon.oppSelect(m, j["balloon"].str);
					gameNetwork.setLongPolling();
				}
				break;
			case 102: {
					int m = (int)j["muti"].n;
					balloon.select(m, j["balloon"].str);
				}
				break;
			//턴 확정을 받는다. 103과 무관
			case 3: {
					gameNetwork.setLongPolling();
					//누구 턴인지
					//어떤 카드를 받았는지
					string turn = j["turn"].str;
					JSONObject arr = j["cards"];
					JSONObject arr2 = j["swapcards"];
					List<int> cards = new List<int>();
					for (int i = 0; i < arr.Count; i++) {
						cards.Add((int)arr[i].n);
					}
					List<int> swapCards = new List<int>();
					for (int i = 0; i < arr2.Count; i++) {
						swapCards.Add((int)arr2[i].n);
					}
					game.setInitTurn(turn, cards, swapCards);

				}
				break;
			//카드를 받는다.
			case 103: {
					JSONObject arr = j["cards"];
					List<int> nums = new List<int>();
					for (int i = 0; i < arr.Count; i++) {
						nums.Add((int)arr[i].n);
					}

					game.setMyCardInHand(nums);
				}
				break;
			case 4: {
					gameNetwork.setLongPolling();
					//상대방이 카드를 선택했는지
					int pos = (int)j["pos"].n;
					int oppSelect = (int)j["card"].n;
					game.setoppSelectedTurnCard(pos, oppSelect);
				}
				break;
			case 104: {
					//내가 어떤 카드를 선택했는지
					Debug.Log("선택된 패 보여주기");
					int selected = (int)j["card"].n;
					int pos = (int)j["pos"].n;
					game.setSelectedTurnCard(pos, selected);
				}
				break;
			//매칭된 카드
			case 5: {
					gameNetwork.setLongPolling();
					Debug.Log("매칭된 카드로 이동");
					int takeoutcard = (int)j["takeoutcard"].n;
					int pos = (int)j["pos"].n;
					game.oppMatchingCards(takeoutcard, pos);
				}
				break;
			case 105: {
					Debug.Log("매칭된 카드로 이동");
					
					//매치시킨 카드
					JSONObject arr1 = j["matchcard"];
					JSONObject arr2 = j["handcard"];
					List<int> matchCards = new List<int>();
					List<int> handCards = new List<int>();
					for (int i = 0; i < arr1.Count; i++) {
						matchCards.Add((int)arr1[i].n);
					}

					for (int i = 0; i < arr2.Count; i++) {
						handCards.Add((int)arr2[i].n);
					}

					int pos = (int)j["pos"].n;
					game.matchedCards(handCards, matchCards, pos);
				}
				break;
			//카드 2장중 선택
			case 6: {
					gameNetwork.setLongPolling();
					Debug.Log("매칭된 카드로 이동");
					int takeoutcard = (int)j["takeoutcard"].n;
					int pos = (int)j["pos"].n;
					game.oppMatchingCards(takeoutcard, pos);
				}
				break;

			case 106: {
					Debug.Log("매칭된 카드로 이동");
					JSONObject arr1 = j["select"];
					JSONObject arr2 = j["handcard"];
					List<int> cards = new List<int>();
					List<int> handcards = new List<int>();	
					for (int i = 0; i < arr1.Count; i++) {
						cards.Add((int)arr1[i].n);
					}

					for (int i = 0; i < arr2.Count; i++) {
						handcards.Add((int)arr2[i].n);
					}
					int pos = (int)j["pos"].n;
					game.selectCardInField(handcards,cards, pos);
				}
				break;
			//다음카드
			case 7: {
					gameNetwork.setLongPolling();
					Debug.Log("매칭된 카드로 이동");
					JSONObject arr = j["drawcards"];

					List<int> cards = new List<int>();
					for (int i = 0; i < arr.Count; i++) {
						cards.Add((int)arr[i].n);
					}
					int pos1 = (int)j["handpos"].n;
					int pos2 = (int)j["drawpos"].n;
					game.oppMatchingNextCards(cards, pos1,pos2);
				}
				break;

			case 107: {
					Debug.Log("매칭된 카드로 이동");
					JSONObject arr1 = j["handcards"];
					JSONObject arr2 = j["getcards"];
					JSONObject arr3 = j["drawcards"];
					JSONObject arr4 = j["matchedcards"];

					List<int> handCards = new List<int>();
					List<int> getCards = new List<int>();
					List<int> drawCards = new List<int>();
					List<int> matchedCards = new List<int>();

					for (int i = 0; i < arr1.Count; i++) {
						handCards.Add((int)arr1[i].n);
					}
					for (int i = 0; i < arr2.Count; i++) {
						getCards.Add((int)arr2[i].n);
					}
					for (int i = 0; i < arr3.Count; i++) {
						drawCards.Add((int)arr3[i].n);
					}
					for (int i = 0; i < arr4.Count; i++) {
						matchedCards.Add((int)arr4[i].n);
					}
					int pos1 = (int)j["handpos"].n;
					int pos2 = (int)j["drawpos"].n;
					game.matchedNextCards(handCards,getCards,drawCards,matchedCards, pos1,pos2);
				}
				break;
			case 8: {
					gameNetwork.setLongPolling();
					Debug.Log("매칭된 카드로 이동");
					JSONObject arr = j["drawcards"];

					List<int> cards = new List<int>();
					for (int i = 0; i < arr.Count; i++) {
						cards.Add((int)arr[i].n);
					}
					int pos1 = (int)j["handpos"].n;
					int pos2 = (int)j["drawpos"].n;

					game.oppMatchingNextCards(cards, pos1,pos2);
				}
				break;
			case 108: {
					JSONObject arr1 = j["handcards"];
					JSONObject arr2 = j["getcards"];
					JSONObject arr3 = j["drawcards"];
					JSONObject arr4 = j["matchedcards"];

					List<int> handCards = new List<int>();
					List<int> getCards = new List<int>();
					List<int> drawCards = new List<int>();
					List<int> matchedCards = new List<int>();

					for (int i = 0; i < arr1.Count; i++) {
						handCards.Add((int)arr1[i].n);
					}
					for (int i = 0; i < arr2.Count; i++) {
						getCards.Add((int)arr2[i].n);
					}
					for (int i = 0; i < arr3.Count; i++) {
						drawCards.Add((int)arr3[i].n);
					}
					for (int i = 0; i < arr4.Count; i++) {
						matchedCards.Add((int)arr4[i].n);
					}
					int pos1 = (int)j["handpos"].n;
					int pos2 = (int)j["drawpos"].n;
					game.selectNextCardInField(handCards, getCards, drawCards, matchedCards, pos1, pos2);
				}
				break;
			case 9: {
					gameNetwork.setLongPolling();
					JSONObject arr1 = j["gwang"];
					JSONObject arr2 = j["dan"];
					JSONObject arr3 = j["end"];
					JSONObject arr4 = j["blood"];
					JSONObject arr5 = j["recvblood"];
					List<int> gwangCards = new List<int>();
					List<int> danCards = new List<int>();
					List<int> endCards = new List<int>();
					List<int> bloodCards = new List<int>();
					List<int> recvBloodCards = new List<int>();

					for (int i = 0; i < arr1.Count; i++) {
						gwangCards.Add((int)arr1[i].n);
					}
					for (int i = 0; i < arr2.Count; i++) {
						danCards.Add((int)arr2[i].n);
					}
					for (int i = 0; i < arr3.Count; i++) {
						endCards.Add((int)arr3[i].n);
					}
					for (int i = 0; i < arr4.Count; i++) {
						bloodCards.Add((int)arr4[i].n);
					}
					for (int i = 0; i < arr5.Count; i++) {
						recvBloodCards.Add((int)arr5[i].n);
					}

					game.getOppScoreCards(gwangCards, danCards, endCards, bloodCards, recvBloodCards);
				}
				break;
			case 109: {
					JSONObject arr1 = j["gwang"];
					JSONObject arr2 = j["dan"];
					JSONObject arr3 = j["end"];
					JSONObject arr4 = j["blood"];
					JSONObject arr5 = j["recvblood"];

					List<int> gwangCards = new List<int>();
					List<int> danCards = new List<int>();
					List<int> endCards = new List<int>();
					List<int> bloodCards = new List<int>();
					List<int> recvBloodCards = new List<int>();

					for (int i = 0; i < arr1.Count; i++) {
						gwangCards.Add((int)arr1[i].n);
					}
					for (int i = 0; i < arr2.Count; i++) {
						danCards.Add((int)arr2[i].n);
					}
					for (int i = 0; i < arr3.Count; i++) {
						endCards.Add((int)arr3[i].n);
					}
					for (int i = 0; i < arr4.Count; i++) {
						bloodCards.Add((int)arr4[i].n);
					}
					for (int i = 0; i < arr5.Count; i++) {
						recvBloodCards.Add((int)arr5[i].n);
					}

					game.getScoreCards(gwangCards,danCards,endCards,bloodCards,recvBloodCards);
				}
				break;
				
			case 10: {
					gameNetwork.setLongPolling();
					Debug.Log("");
					string user = j["turn"].str;
					game.setTurn(user);

				}
				break;
			case 110:{
					string user = j["turn"].str;
					game.setTurn(user);

				}
				break;

			// 고 스 톱
			case 11: {
					gameNetwork.setLongPolling();
					//game.showGoStopPopup();
				}
				break;
			case 111: {
					
					game.showGoStopPopup();
				}
				break;
				//열끗
			case 12: {
					gameNetwork.setLongPolling();
				}
				break;
			case 112: {
					game.showEndPopup();
				}
				break;
			case 13:{
					gameNetwork.setLongPolling();
					//보너스 피
					int bonuscard = (int)j["bonuscard"].n;
					//뺏긴피
					int recvblood = (int)j["recvblood"].n;
					game.oppUseBonusCard(bonuscard, recvblood);	
				}
				break;
			case 113:{
					//보너스 피
					int bonuscard = (int)j["bonuscard"].n;
					//뺏은 피
					int recvblood = (int)j["recvblood"].n;
					int swapcard = (int)j["swapcard"].n;

					game.useBonusCard(bonuscard, recvblood, swapcard);

				}
				break;
				//점수 받고 세팅
			case 14: {
					gameNetwork.setLongPolling();
					int score = (int)j["score"].n;
					string scoreuser = j["scoreuser"].str;
					game.setScore(score, scoreuser);
				}
				break;
			
			//고
			case 15: {
					gameNetwork.setLongPolling();
					string user = j["turn"].str;
					int gocount = (int)j["gocount"].n;
					game.setTurn(user);
				}
				break;
			
			case 115: {
					string user = j["turn"].str;
					int gocount = (int)j["gocount"].n;
					game.setTurn(user);
				}
				break;
			//스톱
			case 16: {
					gameNetwork.setLongPolling();
					//게임 종료
					game.endGame();
				}
				break;
			case 116: {
					//게임 종료
					game.endGame();
				}
				break;
			
			// 열끗으로 인한 재 계산 요청
			case 17: {
					gameNetwork.setLongPolling();
					game.movedOppTenEnd();
				}
				break;
			case 117: {
					game.movedTenEnd();
				}
				break;
			//필요없음
			case 18: {
					gameNetwork.setLongPolling();
					string name = j["name"].str;
					int score = (int)j["score"].n;

					result.setResult( name, score);
				}
				break;
			//자신의 점수
			case 118: {
					string name = j["win"].str;
					int score = (int)j["score"].n;
					result.setResult(name, score);
				}
				break;
		}
						
	}
}
