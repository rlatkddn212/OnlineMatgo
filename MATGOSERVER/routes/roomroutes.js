/**
 * 방들을 미리 만들어두고 방을 모두 초기화 시켜 둔다.
 * 게임에 관련된 http 메시지를 listen 시키는 역할을 하고있다.
 */

var enterroom = require('./enterroom');
var selectballoon = require('./selectballoon');
var getcard = require('./getcard');
var selectturn = require('./selectturn');
var selectcardinhand = require('./selectcardinhand');
var nextcard = require('./nextcard');
var movecard = require('./movecard');
var calcscore = require('./calcscore');
var selectbonuscard = require('./selectbonuscard');
var moveninemonth = require('./moveninemonth');
var gostop = require('./gostop');
var result = require('./result');


var start = function(app, store, subscriber, publisher) {
	console.log("router setup");
	
	var createRoom = function(size, addsize) {
		for (var i = size; i < addsize; i++) {
			var voidUser = {};
			console.log("room create : " + "room" + i);
			store.hset("room", "room" + i, JSON.stringify(voidUser));
		}
	}
	createRoom(1, 100);
	
	enterroom.start(app,store,subscriber, publisher);
	selectballoon.start(app,store,subscriber, publisher);
	getcard.start(app,store,subscriber, publisher);
	selectturn.start(app,store,subscriber, publisher);
	selectcardinhand.start(app,store,subscriber, publisher);
	nextcard.start(app,store,subscriber, publisher);
	movecard.start(app,store,subscriber, publisher);
	calcscore.start(app,store,subscriber, publisher);
	selectbonuscard.start(app,store,subscriber, publisher);
	moveninemonth.start(app,store,subscriber, publisher);
	gostop.start(app,store,subscriber, publisher);	
	result.start(app,store,subscriber, publisher);
	
	//콩콩!
	//콩콩코오옹!
	
	app.post('/nextturn', function(req, res) {
	});
	app.post('/selectcard', function(req, res) {
	});
	app.post('/outroom', function(req, res) {
	});
};

exports.start = start;