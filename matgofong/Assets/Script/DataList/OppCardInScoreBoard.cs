using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class OppCardInScoreBoard {
	List<int> gwangCards = new List<int>();
	List<int> danCards = new List<int>();
	List<int> endCards = new List<int>();
	List<int> bloodCards = new List<int>();

	public void setBloodCard(int card) {
		bloodCards.Add(card);
	}

	public void setEndCard(int card) {
		endCards.Add(card);
	}

	public void setDanCard(int card) {
		danCards.Add(card);
	}

	public void setGwangCard(int card) {
		gwangCards.Add(card);
	}

	public void removeBloodCard(int card) {
		bloodCards.Remove(card);
	}
	public void removeEndCard(int card) {
		endCards.Remove(card);
	}

	public void init() {
		gwangCards.Clear();
		danCards.Clear();
		endCards.Clear();
		bloodCards.Clear();
	}

	public int findPosGwang(int card) {
		for (int i = 0; i < gwangCards.Count; i++) {
			if (gwangCards[i] == card) {
				return i;
			}
		}
		return -1;
	}

	public int findPosDan(int card) {
		for (int i = 0; i < danCards.Count; i++) {
			if (danCards[i] == card) {
				return i;
			}
		}
		return -1;
	}
	public int findPosEnd(int card) {
		for (int i = 0; i < endCards.Count; i++) {
			if (endCards[i] == card) {
				return i;
			}
		}
		return -1;
	}
	public int findPosBlood(int card) {
		for (int i = 0; i < bloodCards.Count; i++) {
			if (bloodCards[i] == card) {
				return i;
			}
		}
		return -1;
	}
}
