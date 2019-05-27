var pub = require('./publishmessage');

var start = function(app, store, subscriber, publisher) {

	function counterBouns(cards) {
		var ret = 0;
		for (var i = 0; i < cards.length; i++) {
			if (cards[i] > 48) {
				ret++;
			}
		}
		return ret;
	}

	// 카드 배열 함수
	function classifyCards(getCards) {
		var Gwang = [];
		var Dan = [];
		var End = [];
		var Blood = [];

		for (var i = 0; i < getCards.length; i++) {
			var c = getCards[i];
			// 광
			if (c == 1 || c == 9 || c == 29 || c == 41 || c == 45) {
				Gwang.push(c);
			}
			// 단
			else if (c == 2 || c == 6 || c == 10 || c == 14 || c == 18
					|| c == 22 || c == 26 || c == 34 || c == 38 || c == 46) {
				Dan.push(c);
			}
			// 끗
			else if (c == 5 || c == 13 || c == 17 || c == 21 || c == 25
					|| c == 30 || c == 33 || c == 37 || c == 47) {
				End.push(c);
			}
			// 피
			else {
				Blood.push(c);
			}
		}
		var ret = {
			gwang : Gwang,
			dan : Dan,
			end : End,
			blood : Blood
		};
		return ret;
	}
	

	app.post('/movecard', function(req, res) {
		console.log("카드를 정산한다.");

		var room = req.param('room');
		var me = req.param('me');
		var opp = req.param('opp');
		var handCards = req.param('handcards');
		var matchedHandCards = req.param('getcards');
		var drawCards = req.param('drawcards');
		var matchedCards = req.param('matchedcards');
		var handPos = req.param('handpos');
		var drawPos = req.param('drawpos');

		// 광		
		function setGwang(gwangCards) {
			store.hget(room, me + "/gwangcards", function(err, gwang) {
				var data = JSON.parse(gwang);
				if(data == null){
					data = [];
				}
				
				data = data.concat(gwangCards);
				store.hset(room, me + "/gwangcards", JSON.stringify(data));
			});
		}

		function setDan(danCards) {
			store.hget(room, me + "/dancards", function(err, dan) {
				
				var data = JSON.parse(dan);
				if(data == null){
					data = [];
				}
				data = data.concat(danCards);
				store.hset(room, me + "/dancards", JSON.stringify(data));
			});
		}

		function setEnd(endCards) {
			store.hget(room, me + "/endcards", function(err, end) {
				
				var data = JSON.parse(end);
				if(data == null){
					data = [];
				}
				console.log(data);
				data = data.concat(endCards);
				store.hset(room, me + "/endcards", JSON.stringify(data));

			});
		}

		function setBlood(bloodCards, recvBloodCards) {
			store.hget(room, me + "/bloodcards", function(err, blood) {
				var data = JSON.parse(blood);
				if(data == null){
					data = [];
				}
				console.log(data);
				
				for(var i = 0 ; i < bloodCards.length; i++){
					//쌍피
					if(bloodCards[i] == 42 || bloodCards[i] == 48 || bloodCards[i] == 49 || bloodCards[i] == 50){
						data.push(bloodCards[i]);
					}else{						
						data.unshift(bloodCards[i]);
					}
				}
				for(var i = 0 ; i < recvBloodCards.length; i++){
					if(recvBloodCards[i] == 42 || recvBloodCards[i] == 48 || recvBloodCards[i] == 49 || recvBloodCards[i] == 50){
						data.push(recvBloodCards[i]);
					}else{
						data.unshift(recvBloodCards[i]);
					}
				}
				
				// todo : 쌍피 일경우
				//data = data.concat(bloodCards);
				//data = data.concat(recvBloodCards);
				
				
				store.hset(room, me + "/bloodcards", JSON.stringify(data));
			});
		}

		function getScoreCards(classifiedCards, countBlood) {
			store.hget(room, opp + "/bloodcards", function(err, oppblood) {
				var bloodCards = JSON.parse(oppblood);
				if(bloodCards == null){
					bloodCards = [];
				}
				
				var receivedBlood = [];
				// 피를 countBlood 만큼 뺀다.
				while (countBlood > 0 && bloodCards.length > 0) {
					var c = bloodCards.shift();
					receivedBlood.push(c);
					// 쌍피 일경우
					if (c == 42 && c == 48 && c == 49 && c == 33) {
						countBlood = countBlood - 2;
					} else if (c == 50) {
						countBlood = countBlood - 3;
					} else {
						countBlood--;
					}
				}
				store.hset(room, opp + "/bloodcards", JSON
						.stringify(bloodCards));

				var Gwang = classifiedCards['gwang'];
				var Dan = classifiedCards['dan'];
				var End = classifiedCards['end'];
				var Blood = classifiedCards['blood'];

				setGwang(Gwang);
				setDan(Dan);
				setEnd(End);
				setBlood(Blood, receivedBlood);
				// 클라이언트로 보낸다.
				
				
				
				var senddata = {
					protocol : 109,
					gwang : Gwang,
					dan : Dan,
					end : End,
					blood : Blood,
					recvblood : receivedBlood
				};
				console.log("get card : " + senddata);
				res.send(JSON.stringify(senddata));

				var publishdata = {
					protocol : 9,
					gwang : Gwang,
					dan : Dan,
					end : End,
					blood : Blood,
					recvblood : receivedBlood,
					user : opp
				};
				pub.send(store, publisher, publishdata);
			});
		}

		/**
		 * Logic
		 */

		var countBlood = 0;

		if (handPos == drawPos) {
			// matchedHandCards의 개수에 따라 달라진다.
			switch (matchedHandCards.length) {
			case 0:
				// 쪽
				countBlood = 1;
				countBlood = countBlood + counterBouns(drawCards);

				var getCards = [];

				getCards = getCards.concat(handCards);
				getCards = getCards.concat(drawCards);

				var classifiedCards = classifyCards(getCards);
				var delBoardCard = matchedHandCards.concat(matchedCards);
				
				console.log(classifiedCards);
				getScoreCards(classifiedCards, countBlood);
				
				break;
			case 1:
				// 뻑
				var getCards = [];
				var classifiedCards = classifyCards(getCards);
				getScoreCards(classifiedCards, countBlood);
				
				store.hget(room, room + "/boardposcards" ,function(err, boarddata){
					var boardPos = JSON.parse(boarddata);
					for(var i = 0 ; i < handCards.length ; i++){						
						boardPos[handPos].push(handCards[i]);
					}
					for(var i = 0 ; i < drawCards.length ; i++){						
						boardPos[handPos].push(drawCards[i]);
					}
					
					boardPos[handPos][0] = 3;
					store.hset(room, room + "/boardposcards", JSON.stringify(boardPos));
				});
				// setBoardCards();
				break;
			case 2:
				// 따닥
				countBlood = 1;
				countBlood = countBlood + counterBouns(drawCards);

				var getCards = [];
				getCards = getCards.concat(handCards);
				getCards = getCards.concat(drawCards);
				getCards = getCards.concat(matchedHandCards);

				var delBoardCard = matchedHandCards.concat(matchedCards);

				var classifiedCards = classifyCards(getCards);
				getScoreCards(classifiedCards, countBlood);
				// removeBoardCards(delBoardCard);
				
				store.hget(room, room + "/boardposcards" ,function(err, boarddata){
					var boardPos = JSON.parse(boarddata);
					
					delete boardPos[handPos];
					//모든 카드를 지움
					boardPos[handPos] = [0];
					store.hset(room, room + "/boardposcards", JSON.stringify(boardPos));
				});
				break;
			case 3:
			case 4:
			case 5:
				console.log("error");
				break;
			}
		} else {
			
			if (handCards.length + matchedHandCards.length >= 4) {
				countBlood++;
			}
			if (drawCards.length + matchedCards.length >= 4) {
				countBlood++;
			}
			
			countBlood = countBlood + counterBouns(matchedHandCards)
					+ counterBouns(drawCards) + counterBouns(matchedCards);

			var getCards = [];
			
			var removeHandCards = [];
			var insertHandCards = [];
			
			var removeDrawCards = [];
			var insertDrawCards	= [];
			
			// matched 개수에 따라 획득 카드가 달라진다.
			if (handCards.length == 1) {
				switch (matchedHandCards.length) {
				case 0:
					console.log("첫패는 못 먹음");
					insertHandCards.push(handCards[0]);
					break;
				case 1:
					console.log("하나 먹음");
					getCards = getCards.concat(handCards);
					getCards = getCards.concat(matchedHandCards);
					removeHandCards.push(matchedHandCards[0]);
					break;
				case 2:
					console.log("첫패는 선택해서 하나 먹음");
					getCards = getCards.concat(handCards);
					getCards.push(matchedHandCards[0]);
					removeHandCards.push(matchedHandCards[0]);
					break;
				case 3:case 4:case 5:
					console.log("다 먹음");
					getCards = getCards.concat(handCards);
					getCards = getCards.concat(matchedHandCards);
					
					removeHandCards = removeHandCards.concat(matchedHandCards);
					break;
				}
			}
			
			switch (matchedCards.length) {
			case 0:
				console.log("드로우패는 못 먹음");
				
				//여기서 드로우 카드를 변경시키니 주의 한다.
				
				while(drawCards.length > 1){
					getCards.push(drawCards.shift());
				}
				insertDrawCards.push(drawCards.pop());
				break;
			case 1:
				console.log("하나 먹음");
				getCards = getCards.concat(drawCards);
				getCards = getCards.concat(matchedCards);
				removeDrawCards.push(matchedCards[0]);
				break;
			case 2:
				console.log("드로우패는 선택해서 하나 먹음");
				getCards = getCards.concat(drawCards);
				getCards.push(matchedCards[0]);
				removeDrawCards.push(matchedCards[0]);
				break;
			case 3:case 4:case 5:
				console.log("다 먹음");
				getCards = getCards.concat(drawCards);
				getCards = getCards.concat(matchedCards);
				removeDrawCards = removeDrawCards.concat(matchedCards);
				break;
			}
			
			var classifiedCards = classifyCards(getCards);
			getScoreCards(classifiedCards, countBlood);
			
			
			store.hget(room, room + "/boardposcards" ,function(err, boarddata){
				var boardPos = JSON.parse(boarddata);
				
				console.log("첫패위치에 상태 : " + boardPos[handPos][0] +" : " + boardPos[handPos] );
				console.log("insert hand card : " + insertHandCards);
				console.log("remove hand card : " + removeHandCards);
				
				console.log("두패위치에 상태 : " + boardPos[drawPos]);
				console.log("insert draw card : " + insertDrawCards);
				console.log("remove draw card : " + removeDrawCards);
				
				if(insertHandCards.length > 0){
					boardPos[handPos].push(insertHandCards[0]);
					boardPos[handPos][0] = boardPos[handPos][0] + 1;
				}
				if(removeHandCards.length > 0){
					if(removeHandCards.length ==1){
						for(var i = boardPos[handPos].length - 1; i >= 1; i--) {
						    if(boardPos[handPos][i] === removeHandCards[0]) {
						    	boardPos[handPos].splice(i, 1);
						    }
						}
						
						boardPos[handPos][0] = boardPos[handPos][0] - 1;
					}
					else{
						delete boardPos[handPos];
						//모든 카드를 지움
						boardPos[handPos] = [0];
					}
				}
				if(insertDrawCards.length > 0){
					boardPos[drawPos].push(insertDrawCards[0]);
					boardPos[drawPos][0] = boardPos[drawPos][0] + 1;
				}
				if(removeDrawCards.length > 0){
					if(removeDrawCards.length ==1){
						for(var i = boardPos[drawPos].length - 1; i >= 1; i--) {
						    if(boardPos[drawPos][i] === removeDrawCards[0]) {
						    	boardPos[drawPos].splice(i, 1);
						    }
						}
						
						boardPos[drawPos][0] = boardPos[drawPos][0] - 1;
					}
					else{
						delete boardPos[drawPos];
						//모든 카드를 지움
						boardPos[drawPos] = [0];
					}
				}
				console.log("boardPos[handPos] : " + boardPos[handPos]);
				console.log("boardPos[drawPos] : " + boardPos[drawPos]);
				
				store.hset(room, room + "/boardposcards", JSON.stringify(boardPos));
			});
		}
	});

}
exports.start = start;