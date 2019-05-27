using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;

public delegate void GameHandler(string message);
public class GameNetwork : MonoBehaviour {

	//이벤트
	public event GameHandler Recv;

	public static GameNetwork instance;


	private const string URL = "http://localhost:3000";
	HttpWebRequest request;

	UserInfo user;
	OppInfo opp;
	Queue<string> q;

	void Awake() {
		instance = this;
		q = new Queue<string>();
	}
	
	

	void Start () {
		user = UserInfo.getInstance();
		opp = OppInfo.getInstance();
		//request = (HttpWebRequest)WebRequest.Create(URL + "/user");
		//string data = @"{""name"":""123"", ""region"":""region213""}";
		//enterRoom();
		//setLongPolling();
	}
	public void setLongPolling() {
		request = (HttpWebRequest)WebRequest.Create(URL + "/longpolling");
		//id
		string data = @"{""name"":""" + user.Id + @"""}";


		request.Method = "POST";
		request.ContentType = "application/json";
		request.ContentLength = data.Length;
		using (Stream webStream = request.GetRequestStream())
		using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII)) {
			requestWriter.Write(data);
		}
		Debug.Log(data);
		request.BeginGetResponse(new AsyncCallback(FinishWebRequest), null);

	}
	void Update() {
		if(q.Count != 0){
			string message = q.Dequeue();
			Recv(message);
		}
	}
	void sendPostMessage( string data) {
		request.Method = "POST";
		request.ContentType = "application/json";
		request.ContentLength = data.Length;
		using (Stream webStream = request.GetRequestStream())
		using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII)) {
			requestWriter.Write(data);
		}
		Debug.Log(data);
		request.BeginGetResponse(new AsyncCallback(FinishWebRequest), null);
	}

	//비동기로 Polling  메시지를 받음
	void FinishWebRequest(IAsyncResult result) {
		try {
			WebResponse webResponse = request.EndGetResponse(result);
			
			using (Stream webStream = webResponse.GetResponseStream()) {
				if (webStream != null) {
					using (StreamReader responseReader = new StreamReader(webStream)) {
						string response = responseReader.ReadToEnd();
						Debug.Log("message : " + response);
						q.Enqueue(response);
						//Recv(response);
					}
				}
			}
		}
		catch (Exception e) {
			Debug.Log(e);
		}
	}

	public void enterRoom() {
		/*
		WebClient webClient = new WebClient();
		Stream stream = webClient.OpenRead(URL + "/enterroom");


		string responseJSON = new StreamReader(stream).ReadToEnd();
		//Recv(responseJSON);
		 * */

		request = (HttpWebRequest)WebRequest.Create(URL + "/enterroom");
		//id
		string data = @"{""name"":""" + user.Id + @"""}";

		sendPostMessage(data);
	}

	public void balloonBtn(string s) {
		request = (HttpWebRequest)WebRequest.Create(URL + "/selectballoon");
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("room", user.Room);
		j.AddField("me", user.Id);
		j.AddField("opp", opp.Id);
		j.AddField("dec", s);
		string encodedString = j.Print();
		sendPostMessage(encodedString);
	}
	public void setCard() {
		request = (HttpWebRequest)WebRequest.Create(URL + "/getCard");
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("room", user.Room);
		j.AddField("me", user.Id);
		j.AddField("opp", opp.Id);
		string encodedString = j.Print();
		sendPostMessage(encodedString);
	}
	public void selectedTurn(int pos) {
		request = (HttpWebRequest)WebRequest.Create(URL + "/selectturn");
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("room", user.Room);
		j.AddField("me", user.Id);
		j.AddField("opp", opp.Id);
		j.AddField("pos", pos);
		string encodedString = j.Print();
		sendPostMessage(encodedString);
	}

	public void selectBonusCardInHand(int cardNum) {
		request = (HttpWebRequest)WebRequest.Create(URL + "/selectbonuscard");
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("room", user.Room);
		j.AddField("me", user.Id);
		j.AddField("opp", opp.Id);
		j.AddField("card", cardNum);
		string encodedString = j.Print();
		sendPostMessage(encodedString);
	}

	public void selectCardInHand(int cardNum) {
		request = (HttpWebRequest)WebRequest.Create(URL + "/selectcardinhand");
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("room", user.Room);
		j.AddField("me", user.Id);
		j.AddField("opp", opp.Id);
		j.AddField("card", cardNum);
		string encodedString = j.Print();
		sendPostMessage(encodedString);
	}

	public void calcMoveCards(List<int> handCards,List<int> getCards, List<int> drawCards, List<int> matchedCards, int pos1, int pos2) {
		request = (HttpWebRequest)WebRequest.Create(URL + "/movecard");
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("room", user.Room);
		j.AddField("me", user.Id);
		j.AddField("opp", opp.Id);
		j.AddField("handpos", pos1);
		j.AddField("drawpos", pos2);
		
		JSONObject arr1 = new JSONObject(JSONObject.Type.ARRAY);
		JSONObject arr2 = new JSONObject(JSONObject.Type.ARRAY);
		JSONObject arr3 = new JSONObject(JSONObject.Type.ARRAY);
		JSONObject arr4 = new JSONObject(JSONObject.Type.ARRAY);


		for (int i = 0; i < handCards.Count; i++) {
			arr1.Add(handCards[i]);
		}
		j.AddField("handcards", arr1);

		for (int i = 0; i < getCards.Count; i++) {
			arr2.Add(getCards[i]);
		}
		j.AddField("getcards", arr2);

		for (int i = 0; i < drawCards.Count; i++) {
			arr3.Add(drawCards[i]);
		}
		j.AddField("drawcards", arr3);

		for (int i = 0; i < matchedCards.Count; i++) {
			arr4.Add(matchedCards[i]);
		}
		j.AddField("matchedcards", arr4);

		string encodedString = j.Print();
		sendPostMessage(encodedString);
	}

	public void drawNextCard(List<int> handCards, List<int> matchCards, int pos) {
		request = (HttpWebRequest)WebRequest.Create(URL + "/nextcard");
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("room", user.Room);
		j.AddField("me", user.Id);
		j.AddField("opp", opp.Id);
		j.AddField("pos", pos);
		JSONObject arr1 = new JSONObject(JSONObject.Type.ARRAY);
		JSONObject arr2 = new JSONObject(JSONObject.Type.ARRAY);

		for (int i = 0; i < handCards.Count; i++) {
			arr1.Add(handCards[i]);
		}
		for (int i = 0; i < matchCards.Count; i++) {
			arr2.Add(matchCards[i]);
		}
		j.AddField("handcards", arr1);
		j.AddField("matchcards", arr2);

		string encodedString = j.Print();
		sendPostMessage(encodedString);
	}

	public void selectCardInField(List<int> handCards,List<int> cards,int pos){
		request = (HttpWebRequest)WebRequest.Create(URL + "/nextcard");
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("room", user.Room);
		j.AddField("me", user.Id);
		j.AddField("opp", opp.Id);
		j.AddField("pos", pos);
		JSONObject arr1 = new JSONObject(JSONObject.Type.ARRAY);
		JSONObject arr2 = new JSONObject(JSONObject.Type.ARRAY);
		
		for (int i = 0; i < handCards.Count; i++) {
			arr1.Add(handCards[i]);
		}
		j.AddField("handcards", arr1);

		for (int i = 0; i < cards.Count; i++) {
			arr2.Add(cards[i]);
		}
		j.AddField("matchcards", arr2);

		string encodedString = j.Print();
		sendPostMessage(encodedString);
	}

	public void calcScore() {
		request = (HttpWebRequest)WebRequest.Create(URL + "/calcscore");
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("room", user.Room);
		j.AddField("me", user.Id);
		j.AddField("opp", opp.Id);
		string encodedString = j.Print();
		sendPostMessage(encodedString);
	}

	public void gostopMessage(string s) {
		request = (HttpWebRequest)WebRequest.Create(URL + "/gostop");
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("room", user.Room);
		j.AddField("me", user.Id);
		j.AddField("opp", opp.Id);
		j.AddField("gostop", s);
		string encodedString = j.Print();
		sendPostMessage(encodedString);

	}
	public void tenEndMessage(string s) {
		request = (HttpWebRequest)WebRequest.Create(URL + "/moveninemonth");
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("room", user.Room);
		j.AddField("me", user.Id);
		j.AddField("opp", opp.Id);
		j.AddField("move", s);
		string encodedString = j.Print();
		sendPostMessage(encodedString);

	}

	public void getResult() {
		request = (HttpWebRequest)WebRequest.Create(URL + "/result");
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("room", user.Room);
		j.AddField("me", user.Id);
		j.AddField("opp", opp.Id);
		string encodedString = j.Print();
		sendPostMessage(encodedString);
	}


	public void reGameMessage() {
		request = (HttpWebRequest)WebRequest.Create(URL + "/regame");
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("room", user.Room);
		j.AddField("me", user.Id);
		j.AddField("opp", opp.Id);
		string encodedString = j.Print();
		sendPostMessage(encodedString);
	}
	public void outMessage() {
		request = (HttpWebRequest)WebRequest.Create(URL + "/outroom");
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("room", user.Room);
		j.AddField("me", user.Id);
		j.AddField("opp", opp.Id);
		string encodedString = j.Print();
		sendPostMessage(encodedString);
	}
}

