/**
 * user init game and get card in hand 게임을 시작하기 전 손패를 받는다.
 */

var pub = require('./publishmessage');
var start = function(app, store, subscriber, publisher) {

	lock = require("redis-lock")(store);
	
	
	app.post('/getcard', function(req, res) {
		console.log("request : getcard");
		var room = req.param('room');
		var me = req.param('me');
		var opp = req.param('opp');
		
		lock("myLock", function(done) {
			store.hget(room, room + "/cardset", function(err, data) {
				console.log(3333);
				var cards = [];
				var getcards = [];
				cards = JSON.parse(data);
				// getcards = cards.slice(1, 10);
				getcards = cards.splice(0, 10);
				getcards.sort(function(a, b) {
					return a - b
				});

				store.hset(room, room + "/cardset", JSON.stringify(cards),function(e,d){
					done(function() {
			            console.log("Lock has been released, and is available for others to use");
			        });

				});
				store.hset(room, me + "/hand", JSON.stringify(getcards));

				// 카드 10장 받음
				var senddata = {
					protocol : 103,
					cards : getcards
				};
				res.send(JSON.stringify(senddata));
			});
			
		});	
		
	});
}
exports.start = start;