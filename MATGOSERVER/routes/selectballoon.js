/**
 * 풍선껌을 불어 배수를 조절 시킨다.
 * 카드를 셔플하고 클라이언트들에게 게임 시작 메시지를 보낸다.
 */
var pub = require('./publishmessage');
var shuffle = require('../utils/shuffle');

var start = function(app, store, subscriber, publisher) {
	app.post('/selectballoon', function(req, res) {
		var room = req.param('room');
		var me = req.param('me');
		var opp = req.param('opp');
		var dec = req.param('dec');
		console.log("room : " + room  + " me : " + me + " opp : " + opp + " dec :" + dec);
		if(dec == "yes"){
			var rand =[];
			rand = shuffle.knuthShuffle([2,3,5,7,10]);
			console.log(rand[0]);
			
			store.hset(room , me + "/muti" , rand[0]);
			//자신
			var senddata = {protocol : 102 , muti :rand[0],balloon : dec};
			res.send(JSON.stringify(senddata));
			//적
			var publishdata = {protocol : 2 , balloon : dec , muti :rand[0]  ,user : opp};
			pub.send(store, publisher,publishdata);
		}
		else{
			var senddata = {protocol : 102 , balloon : dec ,muti : 1};
			res.send(JSON.stringify(senddata));
			//적
			var publishdata = {protocol : 2 , balloon : dec , muti : 1  ,user : opp};
			pub.send(store, publisher,publishdata);

			store.hset(room, me + "/muti" , 1);
		}
		if(store.exists(opp+"/muti")){
			
			//게임 시작 카드를 장전한다.
			store.hset(room ,room +"/start", 1);
			var cards = [];
			for(var i = 1 ; i <= 50 ; i++){
				cards.push(i);
			}
			cards = shuffle.knuthShuffle(cards);
			console.log(cards);
			store.hset(room, room+"/cardset" ,JSON.stringify(cards));
		}else{
			//일단 ok
			store.hset(room, room +"/start", 0);
		}
	});
	
}
exports.start = start;