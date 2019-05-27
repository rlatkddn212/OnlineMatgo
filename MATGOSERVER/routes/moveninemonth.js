
var pub = require('./publishmessage');

var start = function(app, store, subscriber, publisher) {
	app.post('/moveninemonth', function(req, res) {

		var room = req.param('room');
		var me = req.param('me');
		var opp = req.param('opp');
		var move = req.param('move');
		console.log("열끗 이동");
		
		if(move == 'yes'){
			//열끗에서 뺀다.
			//피에 넣는다.
			//다시 계산요청
			var multistore = store.multi();
			
			multistore.hget(room, me + "/endcards");
			multistore.hget(room, me + "/bloodcards");
			
			multistore.exec(function(err, replies){
				var end = JSON.parse(replies[0]);
				var blood =JSON.parse(replies[1]);
				
				for(var i = 0 ; i < end.length; i++){
					if(end[i] == 33){
						end.splice(i, 1);
						blood.push(33);
					}
				}
				store.hset(room, me + "/endcards", JSON.stringify(end));
				store.hset(room, me + "/bloodcards", JSON.stringify(blood));

				var senddata = {protocol : 117};
				res.send(JSON.stringify(senddata));
				var publishdata = {protocol : 17 ,user : opp};
				pub.send(store, publisher,publishdata);
			});
		}else if(move == 'no'){
			//다음 턴
			var senddata = {protocol : 110 , turn : opp};
			res.send(JSON.stringify(senddata));
			
			var publishdata = {protocol : 10 , turn : opp,user : opp};
			pub.send(store, publisher,publishdata);
		}
	});
}
exports.start = start;