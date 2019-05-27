var pub = require('./publishmessage');

var start = function(app, store, subscriber, publisher) {
	
	//같은 달인지 검사한다.
	function isSameCard(card1, card2){
		var month1 = Math.floor(card1 / 4) - ((card1 % 4 == 0) ? 1 : 0);
		var month2 = Math.floor(card2 / 4) - ((card2 % 4 == 0) ? 1 : 0);
		if(month1 == month2){
			return true;
		}
		else{
			return false;
		}
	}
	
	app.post('/nextcard', function(req, res) {
		console.log("다음카드를 꺼낸다.");
		var room = req.param('room');
		var me = req.param('me');
		var opp = req.param('opp');
		var handcards = req.param('handcards');
		var card = req.param('matchcards');
		var delpos = req.param('pos');
		
		store.hget( room, room+"/cardset", function(err, data){
			var drawcards = [];
			var cardset = [];
			cardset = JSON.parse(data);
			
			var drawcard = cardset.shift();
			drawcards.push(drawcard);
			
			while(drawcard > 48){
				drawcard = cardset.shift();
				drawcards.push(drawcard);
			}
			
			store.hset(room, room + "/cardset" , JSON.stringify(cardset));
			
			//손패에서 낸카드와 드로우한 카드가 같은 카드이면 쪽,뻑,따닥 처리
			if(isSameCard(handcards[0],drawcard)){
				console.log("쪽, 뻑 , 따닥 이다!" + drawcard);
				var senddata = {protocol : 107 , handcards : handcards , getcards : card , drawcards : drawcards ,
						matchedcards : [], handpos : delpos , drawpos : delpos};
				res.send(JSON.stringify(senddata));
				var publishdata = {protocol : 7 , drawcards : drawcards ,handpos : delpos , drawpos : delpos ,user : opp};
				pub.send(store, publisher, publishdata);
			}
			//매칭 시킴
			else{
				store.hget(room, room + "/boardposcards", function(err, cards){
					var boardPosCards = JSON.parse(cards);
					//console.log(boardPosCards);
					var matched = false;
					
					for(var pos = 1 ; pos <= 12 ; pos++ ){
						var cardInfo = boardPosCards[pos];
						if(cardInfo.length > 1 && isSameCard(drawcard,cardInfo[1])){
							
							console.log("매칭시켜보니 : " + pos + " 위치이고 카드는 : " + drawcard);
							
							switch (cardInfo[0]) {
							case 1:
								console.log("하나짜리 먹음");
								var getcard = [cardInfo[1]];
								
								var senddata = {protocol : 107 , handcards : handcards , getcards : card, drawcards : drawcards ,
										matchedcards : getcard, handpos : delpos , drawpos : pos};
								res.send(JSON.stringify(senddata));
								var publishdata = {protocol : 7 , drawcards : drawcards , handpos : delpos , drawpos : pos ,user : opp};
								pub.send(store, publisher,publishdata);
								
								matched = true;
								break;
							case 2:
								console.log("선택을 통해 먹음");
								var getcard = [cardInfo[1],cardInfo[2]];
								
								var senddata = {protocol : 108 , handcards : handcards , getcards : card, drawcards : drawcards ,
										matchedcards : getcard, handpos : delpos , drawpos : pos};
								res.send(JSON.stringify(senddata));
								var publishdata = {protocol : 8 , drawcards : drawcards , handpos : delpos , drawpos : pos ,user : opp};
								pub.send(store, publisher,publishdata);
								
								matched = true;
								break;
							
							case 3:
								console.log("3칸짜리를 먹음");
								var getcard = [cardInfo[1],cardInfo[2],cardInfo[3]];
								
								var senddata = {protocol : 107 , handcards : handcards , getcards : card, drawcards : drawcards ,
										matchedcards : getcard, handpos : delpos , drawpos : pos};
								res.send(JSON.stringify(senddata));
								var publishdata = {protocol : 7 , drawcards : drawcards , handpos : delpos , drawpos : pos ,user : opp};
								pub.send(store, publisher,publishdata);
								
								matched = true;
								break;
							case 4:
								console.log("뻑을 먹음");
								var getcard = [];
								for(var i = 1 ; i < cardInfo.length ; i++){
									getcard.push(cardInfo[i]);
								}
								
								var senddata = {protocol : 107 , handcards : handcards , getcards : card, drawcards : drawcards ,
										matchedcards : getcard, handpos : delpos , drawpos : pos};
								res.send(JSON.stringify(senddata));
								var publishdata = {protocol : 7 , drawcards : drawcards , handpos : delpos , drawpos : pos ,user : opp};
								pub.send(store, publisher,publishdata);
								
								matched = true;
								break;
							}
						}
					}
					// 매칭 되는 패가 없는 경우
					if(!matched){
						console.log("매칭 되는 카드가 없습니다.");
						for(var pos = 1 ; pos <= 12 ; pos++ ){
							var cardInfo = boardPosCards[pos];
							if(delpos != pos && cardInfo.length == 1){
								var getcard = [];
								var senddata = {protocol : 107 , handcards : handcards , getcards : card, drawcards : drawcards ,
										matchedcards : getcard, handpos : delpos , drawpos : pos};
								res.send(JSON.stringify(senddata));
								var publishdata = {protocol : 7 , drawcards : drawcards , handpos : delpos , drawpos : pos ,user : opp};
								pub.send(store, publisher,publishdata);
								break;
							}
						}
					}
				});	
			}
		});
	});
}
exports.start = start;