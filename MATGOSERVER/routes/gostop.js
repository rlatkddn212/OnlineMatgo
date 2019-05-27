
var pub = require('./publishmessage');

var start = function(app, store, subscriber, publisher) {
	app.post('/gostop', function(req, res) {

		var room = req.param('room');
		var me = req.param('me');
		var opp = req.param('opp');
		var gostop = req.param('gostop');
		console.log("고 스톱");
		
		if(gostop == "go"){
			//go 추가
			//배수 추가
			//다음턴
			
			store.hget(room, me+ "/go" , function(err, data){
				if(data == null){
					data = 0;
				}
				data++;
				var senddata = {protocol : 115 , turn : opp , gocount : data};
				res.send(JSON.stringify(senddata));
				var publishdata = {protocol : 15 , turn : opp, gocount : data,user : opp};
				pub.send(store, publisher,publishdata);
	
				store.hset(room,me+ "/go", data);
			});
					}
		else if(gostop == "stop"){
			//게임 종료
			// 게임 초기화S
			//스톱 메시지
			var senddata = {protocol : 116 , win : me};
			res.send(JSON.stringify(senddata));
			
			var publishdata = {protocol : 16 , win : me ,user : opp};
			pub.send(store, publisher,publishdata);
			
			
			store.hset(room, room + "/win", JSON.stringify(me));
		}
	});
}
exports.start = start;