var pub = require('./publishmessage');
var calc = require('../utils/calc');

var start = function(app, store, subscriber, publisher) {

	app.post('/calcscore', function(req, res) {

		var room = req.param('room');
		var me = req.param('me');
		var opp = req.param('opp');

		var multistore = store.multi();

		multistore.hget(room, me + "/gwangcards");
		multistore.hget(room, me + "/dancards");
		multistore.hget(room, me + "/endcards");
		multistore.hget(room, me + "/bloodcards");

		var score = 0;

		multistore.exec(function(err, replies) {
			score += calc.calcGwang(JSON.parse(replies[0]));
			console.log(score);
			score += calc.calcDan(JSON.parse(replies[1]));
			console.log(score);
			score += calc.calcEnd(JSON.parse(replies[2]));
			console.log(score);
			score += calc.calcBlood(JSON.parse(replies[3]));
			console.log(replies);
			console.log(score);

			// 5점쯤인데 9월 열끗이 있을 경우
			var end = 0;
			var tenend =JSON.parse(replies[2]);
			for (var i = 0; i < tenend.length; i++) {
				
				if (tenend[i] === 33) {
					end = 1;
				}
			}
			if (score >= 5 && end === 1) {
				// todo 열끗 이동
				store.hset(room, me + "/score", score);
				var senddata = {
					protocol : 112
				};
				res.send(JSON.stringify(senddata));

				var publishdata = {
					protocol : 12,
					user : opp
				};
				pub.send(store, publisher, publishdata);
			} else if (score >= 7) {
				// 고스톱? 단 이전에 점수보다 낮으면 고를 못함 -> 다음턴

				// 스코어를 기록한다.

				store.hget(room, me + "/score", function(err, data) {
					if (score > data) {
						// 고 스톱
						store.hset(room, me + "/score", score);
						var senddata = {
							protocol : 111
						};
						res.send(JSON.stringify(senddata));

						var publishdata = {
							protocol : 11,
							user : opp
						};
						pub.send(store, publisher, publishdata);
					} else {
						// 다음 턴
						
						// 점수와 다음턴 보내줌
						var senddata = {
							protocol : 110,
							turn : opp
						};
						res.send(JSON.stringify(senddata));

						var publishdata = {
							protocol : 10,
							turn : opp,
							user : opp
						};
						pub.send(store, publisher, publishdata);
					}
				});
			}
			// 다음턴
			else {
				store.hset(room, me + "/score", score);
				var senddata = {
					protocol : 110,
					turn : opp
				};
				res.send(JSON.stringify(senddata));

				var publishdata = {
					protocol : 10,
					turn : opp,
					user : opp
				};
				pub.send(store, publisher, publishdata);
			}

			var publishoppdata = {
				protocol : 14,
				score : score,
				scoreuser : me,
				user : opp
			};
			pub.send(store, publisher, publishoppdata);
			var publishmydata = {
				protocol : 14,
				score : score,
				scoreuser : me,
				user : me
			};
			pub.send(store, publisher, publishmydata);
		});
	});
}
exports.start = start;