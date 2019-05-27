/**
 * GameUser was entered to room ( 맞낭? )
 * 유저가 방에 입장함
 */

var pub = require('./publishmessage');

var start = function(app, store, subscriber, publisher) {
	
	var clearRoom = function(roomname){
		store.hkeys(roomname, function(err, replies){	
			console.log(replies);
			replies.forEach(function(reply , i){
				store.hdel(roomname,reply);
			});
		});
	}
	
	app.post('/enterroom', function(req, res) {
		var name = req.param('name');
		console.log("player enter room : " + name);

		//방에 빈공간이 있는 경우
		store.hkeys("room", function(err, replies) {
			var userIn = false;
			replies.forEach(function(reply, i) {
				//store.hdel("room", reply);
				
				store.hget("room", reply, function(err, user) {
					var u = JSON.parse(user);
					
					if (u.u1 == undefined && !userIn) {
						userIn = true;
						u['u1'] = name;
						store.hdel("user", reply);
						store.hset("room", reply, JSON.stringify(u));
						//방이름을 저장한다.
						var senddata;
						if(u.u2 != undefined){
							//적의 이름을 알린다.
							senddata = {protocol : 101 ,room : reply , user : u['u2']};
							//적에게 나의 이름을 알린다.
							var publishdata = {protocol : 1 , room : reply , user : u['u2'] , opp :name};
							pub.send(store, publisher,publishdata);
							clearRoom(reply);
						}else{
							senddata = {protocol : 101 ,room : reply , user : null};
							//var publishdata = {protocol : 1 , room : reply , user : name};
							//publishmessage(publishdata);
						}
						
						
						res.send(JSON.stringify(senddata));
						console.log("send message : " +senddata);
						//상대방에게 메시지를 보냄
					} else if (u.u2 == undefined && !userIn) {
						userIn = true;
						u['u2'] = name;
						store.hdel("user", reply);
						store.hset("room", reply, JSON.stringify(u));
						//방이름을 저장한다.
						var senddata;
						
						if(u.u1 != undefined){
							senddata = {protocol : 101 ,room : reply , user : u['u1']};
							var publishdata = {protocol : 1 , room : reply , user : u['u1'] ,opp : name};
							pub.send(store, publisher,publishdata);
							//방청소
							clearRoom(reply);
						}else{
							senddata = {protocol : 101 ,room : reply , user : null};
							//var publishdata = {protocol : 1 , room : reply , user : u['u1']};
							//publishmessage(publishdata);
						}
						res.send(JSON.stringify(senddata));
						console.log("send message : " +senddata);
					}
				});
			});
		});

		//자신의 채널을 청취한다.
	});
}

exports.start = start;
	