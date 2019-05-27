/**
 * SELECT TURN CARD 선(turn)을 정하기 위해 카드를 선택한다.
 * 
 */
var pub = require('./publishmessage');

var start = function(app, store, subscriber, publisher) {

	store.del("abc");
	store.del("abc2");
	
	function setPosInBoard(room,cards){
		var pea = [];
		console.log(pea);
		for(var i = 0; i < cards.length ; i++){
			pea.push(cards[i]);
		}
		pea.sort(function(a,b){return a-b;});
		var pos = {};
		var info = [];
		var p = 1;
		info.push(pea[0]);
		for(var i = 1 ; i < pea.length; i++){
			console.log(pea[i]);
			
			var month1 = Math.floor(info[0] / 4) - ((info[0] % 4 == 0) ? 1 : 0);
			var month2 = Math.floor(pea[i] / 4) - ((pea[i] % 4 == 0) ? 1 : 0);
			
			console.log (info[0] + " " + pea[i] + " : " + month1 + " " + month2);
			if (month1 == month2) {
				info.push(pea[i]);
			}
			else {
				pos[p] = info;
				//console.log(info);
				info = [];
				p++;
				info.push(pea[i]);
			}
			if( i == (pea.length - 1)){
				pos[p] = info;
			}
		}
		console.log(pos);
		
		for(var i = 1 ; i <= 12 ; i++){
			var cardlist = pos[i];
			if(cardlist != undefined){
				cardlist.unshift(cardlist.length);	
			}else{
				pos[i] = [0];
			}
		}
		store.hset(room, room+"/boardposcards", JSON.stringify(pos));
	}
	function setCardInBoard(room,card,cards,swapcards){
		store.hget(room,room + "/boardcards" , function(err, data){
			var boardcards = [];
			
			boardcards = cards;
			
			boardcards.push(card);
			boardcards.push(JSON.parse(data));
			
			for(var i = 0 ; i < swapcards.length ; i++){
				for(var j = 0 ; j < boardcards.length ; j++){
					if(boardcards[j]> 48){
						boardcards[j] = swapcards[i];
						break;
					}
				}
			}
			
			//로그해보자.
			console.log("cards : " +cards + " data : " + data);
			
			store.hset(room,room + "/boardcards",JSON.stringify(boardcards),function(err, success){
				
				store.hget(room,room + "/boardcards" , function(err, pea){
					setPosInBoard(room, JSON.parse(pea));
				});
			});
		});
	}
	
	
	app.post('/selectturn', function(req, res) {
		var room = req.param('room');
		var me = req.param('me');
		var opp = req.param('opp');
		var pos = req.param('pos');
		console.log(" 카드 선택 메시지를 받음 ");
		
		store.hget(room ,room+"/cardset",function(err, data){
			console.log(" card set을 불러옴 ");
			
			store.hget(room ,room+"/selectedpos", function(err,position){
				console.log(" 이전에 선택된 위치를 불러옴 ");
				
				if(position == null){
					console.log(" 상대방이 아직 선택안함 ");
					
					var cards = [];
					var getcards;
					cards = JSON.parse(data);
					//getcards = cards.slice(1, 10);
					
					var selectcard = cards.shift();
					console.log(" 카드를 한장 뽑았다 : " + selectcard + " 카드이다.");
					while(selectcard > 48){
						cards.push(selectcard);
						console.log("다시 카드를 한장 뽑았다 : " + selectcard + " 카드이다.");
						selectcard = cards.shift();
					}
					
					store.hset(room, room+ "/boardcards",JSON.stringify(selectcard));
					
					store.hset(room ,me+"/selectturn", JSON.stringify(selectcard));
					console.log("선택한 카드를 저장해 둔다. ");
					store.hset(room ,room+"/selectedpos", pos);
					console.log("선택한 위치를 저장해 둔다. ");
					store.hset(room ,room+"/cardset", JSON.stringify(cards));
					console.log(" 카드 뭉치를 redis에 다시 저장해둔다. ");
					
					var senddata = {protocol : 104 , card : selectcard , pos: pos};
					
					res.send(JSON.stringify(senddata));
					console.log(" 자신에게 선택한 카드를 보낸다. ");
					
					var publishdata = {protocol : 4 , card : selectcard , pos: pos ,user : opp};
					pub.send(store, publisher,publishdata);
					console.log(" 상대방에게 선택한 카드를 보낸다. ");
					
				}
				else{
					// 위치가 같지 않다면
					if(position != pos){
						console.log(" 상대방이 이미 선택했음 ");
						var cards = [];
						var getcards;
						cards = JSON.parse(data);
						//getcards = cards.slice(1, 10);
						
						var selectcard = cards.shift();
						
						while(selectcard > 48){
							cards.push(selectcard);
							selectcard = cards.shift();
						}
						var getcards = [];
						getcards = cards.splice(0, 6);
						
						var bonusCards = [];
						var bonusSwapCards = [];
						for(var i = 0 ; i < getcards.length; i++){
							if(getcards[i] > 48){
								//카드를 한장 뽑음
								bonusSwapCards.push(cards.shift());
								bonusCards.push(getcards[i]);
								if(bonusSwapCards[0] > 48){
									bonusCards.push(bonusSwapCards[0]);
									bonusSwapCards.push(cards.shift());
								}
							}
						}
						
						//카드를 board에 저장한다.
						setCardInBoard(room,selectcard, getcards, bonusSwapCards);
						
						var senddata = {protocol : 104 , card : selectcard , pos: pos};
						res.send(JSON.stringify(senddata));
						
						var publishdata = {protocol : 4 , card : selectcard , pos: pos ,user : opp};
						pub.send(store, publisher,publishdata);
						
						//누가 선인지 정하고 방에 있는 유저들에게 패들을 보낸다.	
						store.hget(room ,opp+"/selectturn", function(err,turncard){
							var id;
							//누구턴인지 알아본다.
							if(turncard > selectcard){
								id = opp;
							}else{
								id = me;
							}
							console.log("누가 선인지 정한다.");
							store.hdel(room ,room+"/selectedpos");
							store.hdel(room ,opp+"/selectturn");
							console.log(" 선택 기록을 지운다. ");
							var publishdata = {protocol : 3 , cards : getcards, swapcards : bonusSwapCards , turn : id  ,user : opp};
							pub.send(store, publisher,publishdata);
							var publishdataToMe = {protocol : 3 , cards : getcards , swapcards : bonusSwapCards, turn : id  ,user : me};
							pub.send(store, publisher,publishdataToMe);
							
							store.hset(room, id + "/bloodcards" , JSON.stringify(bonusCards));
						});
						store.hset(room ,room+"/cardset", JSON.stringify(cards));
						
						
					}else{
						console.log(" 상대방이 선택한 카드를 또 선택함 ");
						res.writeHead(204);
					}
				}
				
			});
			
		});
	});
}
exports.start = start;