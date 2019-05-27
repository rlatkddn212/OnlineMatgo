using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyCardInHand {
	List<int> cardNum = new List<int>();

	public MyCardInHand() {
	}
	
	public void setCard(List<int> nums) {
		cardNum = nums;
		
	}

	public void spendCard(int num) {
		cardNum.Remove(num);	
	}

	public List<int> getCardList() {
		return cardNum;
	}

	public int posCard(int card) {
		for (int i = 0; i < cardNum.Count; i++) {
			if (cardNum[i] == card) {
				return i;
			}
		}
		return -1;
	}

	public void swapCard(int card, int swap) {
		for (int i = 0; i < cardNum.Count; i++) {
			if (cardNum[i] == card) {
				cardNum[i] = swap;
			}
		}
	}
	public bool findCardInHand(int num) {
		for (int i = 0; i < cardNum.Count; i++) {
			if(cardNum[i] == num){
				return true;
			}
		}
		return false;
	}
}
