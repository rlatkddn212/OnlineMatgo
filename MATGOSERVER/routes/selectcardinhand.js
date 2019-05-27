var pub = require('./publishmessage');

var start = function(app, store, subscriber, publisher) {
	app.post('/selectcardinhand', function(req, res) {
		var room = req.param('room');
		var me = req.param('me');
		var opp = req.param('opp');
		var card = req.param('card');
		
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
		
		store.hget(room, me + "/hand", function(err, myHandCard){
			var handInCard = [];
			handInCard = JSON.parse(myHandCard);
			for(var i = handInCard.length - 1; i >= 0; i--) {
			    if(handInCard[i] === card) {
			    	handInCard.splice(i, 1);
			    }
			}
			store.hset(room,me +"/hand",JSON.stringify(handInCard));
		});
		
		store.hget(room, room + "/boardposcards", function(err, cards){
			var boardPosCards = JSON.parse(cards);
			//console.log(boardPosCards);
			var matched = false;
			
			for(var pos = 1 ; pos <= 12 ; pos++ ){
				var cardInfo = boardPosCards[pos];
				if(cardInfo.length > 1 && isSameCard(card,cardInfo[1])){
					
					switch (cardInfo[0]) {
					case 1:
						var getcard = [cardInfo[1]];
						
						var senddata = {protocol : 105 , handcard : [card] ,matchcard : getcard, pos : pos };
						res.send(JSON.stringify(senddata));
						
						var publishdata = {protocol : 5 , takeoutcard : card , pos: pos ,user : opp};
						pub.send(store, publisher,publishdata);
						
						matched = true;
						break;
					case 2:
						var selectcard = [cardInfo[1],cardInfo[2]];
						
						//카드 선택
						var senddata = {protocol : 106 ,handcard : [card], select : selectcard, pos : pos };
						res.send(JSON.stringify(senddata));
						var publishdata = {protocol : 6 , takeoutcard : card , pos: pos ,user : opp};
						pub.send(store, publisher,publishdata);
						
						matched = true;
						break;
					//3장
					case 3:
						var getcard = [cardInfo[1],cardInfo[2],cardInfo[3]];
						
						var senddata = {protocol : 105 ,handcard : [card], matchcard : getcard, pos : pos };
						res.send(JSON.stringify(senddata));
						
						var publishdata = {protocol : 5 , takeoutcard : card , pos: pos ,user : opp};
						pub.send(store, publisher,publishdata);
						
						matched = true;
						break;
					//뻑
					case 4:
						
						var getcard = [];
						for(var i = 1 ; i < cardInfo.length ; i++){
							getcard.push(cardInfo[i]);
						}
						
						var senddata = {protocol : 105 ,handcard : [card], matchcard : getcard, pos : pos };
						res.send(JSON.stringify(senddata));
						
						var publishdata = {protocol : 5 , takeoutcard : card , pos: pos ,user : opp};
						pub.send(store, publisher,publishdata);
						matched = true;
						break;
					}
				}
			}
			// 매칭 되는 패가 없는 경우
			if(!matched){
				for(var pos = 1 ; pos <= 12 ; pos++ ){
					var cardInfo = boardPosCards[pos];
					if(cardInfo.length == 1){
						cardInfo.push(card);
						cardInfo[0] = 1;

						var getcard = [];
						
						var senddata = {protocol : 105 ,handcard : [card], matchcard : getcard, pos : pos };
						res.send(JSON.stringify(senddata));
						
						console.log(" 카드 매칭 ");
						
						var publishdata = {protocol : 5 , takeoutcard : card , pos: pos ,user : opp};
						pub.send(store, publisher,publishdata);
						break;
					}
				}
			}
		});
	});
}
exports.start = start;