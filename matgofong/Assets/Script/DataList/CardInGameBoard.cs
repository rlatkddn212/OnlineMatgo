using System;
using System.Collections;
using System.Collections.Generic;
public class CardInGameBoard{


	//int number;
	int[] posFirst;
	List<List<int> > boardPos;

	public CardInGameBoard() {
		posFirst = new int[9];
		boardPos = new List<List<int> >();
		for (int i = 0; i < 13; i++) {
			boardPos.Add(new List<int>());
		}
	}
	public void insertCard(int pos, int card) {
		boardPos[pos].Add(card);
	}

	public void removePosCard(int pos, int card) {
		boardPos[pos].Remove(card);
	}
	public void removeCard(int card) {
		int pos = findPos(card);
		removePosCard(pos, card);
	}

	public void setInitTurnCards(List<int> cards) {
		int k = 0;
		
		for (int i = 1; i < posFirst.Length; i++) {
			if (k == cards.Count) {
				break;
			}
			if(posFirst[i] == 0){
				posFirst[i] = cards[k];
				k++;
			}
		}
	}

	public void setCard(int pos , int num) {
		//카드를 저장한다.
		posFirst[pos] = num;
	}

	public void sortCard(int[] cardList) {
		Array.Sort(cardList);
		boardPos[1].Add(cardList[1]);
		
		int p = 1;

		for (int i = 2; i < cardList.Length; i++) {
			int pos1 = boardPos[p][0] / 4 - ((boardPos[p][0] % 4 == 0) ? 1 : 0);
			int pos2 = cardList[i] / 4 - ((cardList[i] % 4 == 0) ? 1 : 0);
			
			if (pos1 == pos2) {
				boardPos[p].Add(cardList[i]);
			}
			else {
				p++;
				boardPos[p].Add(cardList[i]);
			}
		}
	}

	public int[] getPosFirst() {
		return posFirst;
	}

	public List<List<int> > getBoardPos() {
		return boardPos;
	}

	public int findPos(int cardNum) {
		for (int i = 1; i < boardPos.Count; i++) {
			for(int j = 0 ; j < boardPos[i].Count ; j++){
				if(cardNum == boardPos[i][j]){
					return i;
				}
			}
		}
		return -1;
	}

	public int findTopPos(int pos) {
		return boardPos[pos].Count;
	}
}
