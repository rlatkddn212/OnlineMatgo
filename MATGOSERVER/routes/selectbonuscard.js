var pub = require('./publishmessage');

var start = function(app, store, subscriber, publisher) {

	app.post('/selectbonuscard' , function(req, res){
		var room = req.param('room');
		var me = req.param('me');
		var opp = req.param('opp');
		var card = req.param('card');
		
		//피를 한장 뺏는다.
		//내 스코어 보드에 보너스 피를 한장 넣는다.
		//card set에서 카드 한장을 뽑고 내 카드에 넣는다.
		
		var storeMulti = store.multi();
		
		//피를 뺏음
		storeMulti.hget(room, opp + "/bloodcards");
		storeMulti.hget(room, me + "/bloodcards");
		storeMulti.hget(room, me + "/hand");
		storeMulti.hget(room, room + "/cardset");
		
		storeMulti.exec(function(err, replies){
			
			//적 피를 한장 뺀다.
			var oppBlood = JSON.parse(replies[0]);
			var myBlood = JSON.parse(replies[1]);
			var myHand = JSON.parse(replies[2]);
			var cardSet = JSON.parse(replies[3]);

			var swapcard;
			var recvblood = 0;
			
			//내 보너스피와 적피를 내 피에 등록
			
			if(oppBlood == null){
				oppBlood = [];
			}
			if(myBlood == null){
				myBlood = [];
			}
			
			myBlood.push(card);
			
			if(oppBlood.length > 0){
				
				recvblood = oppBlood.shift();
				if(recvblood > 47 || recvblood == 42){
					myBlood.push(recvblood);
				}
				else{
					myBlood.unshift(recvblood);
				}
			}
			
			
			//새로운 카드를 cardset에서 받는다.
			swapcard = cardSet.shift();
			//보너스카드를 내 카드에서 삭제하고 새로운 카드를 등록한다.
			
			for (var i = 0 ; i < myHand.length ; i++){
				if(myHand[i] == card){
					myHand[i] = swapcard;
				}
			}
			
			store.hset(room, opp + "/bloodcards",JSON.stringify(oppBlood));
			store.hset(room, me + "/bloodcards",JSON.stringify(myBlood));
			store.hset(room, me + "/hand",JSON.stringify(myHand));
			store.hset(room, room + "/cardset",JSON.stringify(cardSet));
			
			//보낼 메시지
			var senddata = {protocol : 113 , swapcard : swapcard , recvblood : recvblood, bonuscard : card };
			res.send(JSON.stringify(senddata));
			
			var publishdata = {protocol : 13, recvblood : recvblood, bonuscard : card ,user : opp};
			pub.send(store, publisher,publishdata);
		});
		
		//내 스코어 보드에 보너스 피를 한장 넣음
		
	});

}
exports.start = start;