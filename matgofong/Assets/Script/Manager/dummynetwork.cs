using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;

public delegate void EventHandler(string message);
public class dummynetwork : MonoBehaviour {

	public event EventHandler Recv;

	int[] CardList = new int[50];
	public static dummynetwork instance;



	void Awake() {
		instance = this;
	}
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		/*
		string uri = "http://127.0.0.1:3000";
		WebClient webClient = new WebClient();
		Stream stream = webClient.OpenRead(uri);
		string responseJSON = new StreamReader(stream).ReadToEnd();
		Debug.Log(responseJSON);
		*/
	}

	public static void knuthShuffle<T>(T[] array) {
		System.Random random = new System.Random();
		for (int i = 0; i < array.Length; i++) {
			int j = random.Next(i, array.Length);
			T temp = array[i];
			array[i] = array[j];
			array[j] = temp;
		}
	}

	//
	public void setCard() {
		for (int i = 0; i < 50; i++) {
			CardList[i] = i + 1;
		}
		knuthShuffle<int>(CardList);
		
		asynRecv();
	}



	public void selectedTurn(int pos) {
		//상대편 선택 카드
		{
			Debug.Log("selected");
			JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
			j.AddField("PROTOCOL", 1);
			j.AddField("POS", 1);
			j.AddField("NUMBER", CardList[10]);
			string encodedString = j.Print();
			Recv(encodedString);
		}
		//내가 선택 카드
		{
			JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
			j.AddField("PROTOCOL", 2);
			j.AddField("POS", pos);
			j.AddField("NUMBER", CardList[11]);
			string encodedString = j.Print();
			Recv(encodedString);
		}
		{ 
			JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
			j.AddField("PROTOCOL", 3);
			j.AddField("TURN", 0);
			JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
			j.AddField("NUMBER", arr);
			for (int i = 0; i < 6; i++) {
				arr.Add(CardList[i+12]);
			}

			string encodedString = j.Print();
			Recv(encodedString);
		}
	}
	
	public void nextTurn(){
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("PROTOCOL", 4);
		j.AddField("TURN", 0);
		string encodedString = j.Print();
		Recv(encodedString);
	}

	void asynRecv(){
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
		j.AddField("PROTOCOL", 0);
		j.AddField("NUMBER", arr);
		for (int i = 0; i < 10; i++) {
			arr.Add(CardList[i]);
		}
		string encodedString = j.Print();
		Recv(encodedString);
	}

	public void selectCardInHand(int num) {
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField("PROTOCOL", 5);
		j.AddField("SELECTEDCARD" , num);
		j.AddField("MATCHEDCARD", 5);

		string encodedString = j.Print();
		Recv(encodedString);
	}
}
