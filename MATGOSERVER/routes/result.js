

var pub = require('./publishmessage');
var start = function(app, store, subscriber, publisher) {	
	
	app.post('/result', function(req, res) {
		var room = req.param('room');
		var me = req.param('me');
		var opp = req.param('opp');
		
		store.hget(room, room + "/win",function(err, data){
			var username = JSON.parse(data);
			
			store.hget(room, me + "/score", function(err, score) {
				var senddata = {protocol : 118 , win : username, score : score};
				res.send(JSON.stringify(senddata));
				
				//var publishdata = {protocol : 18 , win : username , score : score,user : opp};
				//pub.send(store, publisher,publishdata);
			});
		});
		
		
		//게임 데이터 초기화 todo
		
		
		
	});
}
exports.start = start;