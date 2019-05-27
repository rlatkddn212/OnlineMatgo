using UnityEngine;
using System.Collections;

public class OppCardInHand{
	int card_;

	int spendCard() {
		return card_--;
	}
	public void setCard(int num) {
		card_ = num;
	}
}