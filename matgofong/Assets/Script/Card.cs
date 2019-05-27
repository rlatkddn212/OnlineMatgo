using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

	int cardnum;
	int boardPos;

	Vector3 target;
	Vector3 prevPos;
	public float speed; 
	float ratio; 
    float deltaRatio;

	enum STATE{ 
        IDLE, 
        MOVE 
    };

	STATE cardState;

	// Use this for initialization
	void Start () {
		cardState = STATE.IDLE;
	}
	
	// Update is called once per frame

	void Update () {
		if (cardState == STATE.MOVE) {
			ratio += deltaRatio * Time.deltaTime;
			if (ratio > 1.0f) {
				ratio = 1.0f;
				cardState = STATE.IDLE;
			}
			transform.position = prevPos + target * ratio;
		}
	}

	public void setCardNum(int number) {
		cardnum = number;
	}

	public void setBoardPos(int pos) {
		boardPos = pos;
	}

	public int getBoardPos() {
		return boardPos;
	}
	public int getCardNum() {
		return cardnum;
	}

	public void setCardScale() {
		
	}

	public void moveCard(float x, float y, float z){
		//이동 시킴
		cardState = STATE.MOVE; 
        ratio = 0; 
        prevPos = transform.position;
		target = new Vector3(x, y, z);
		//target과 prevPos의 거리차
        target = target - prevPos;
        deltaRatio = (speed * speed) / target.sqrMagnitude; 
	}
}
