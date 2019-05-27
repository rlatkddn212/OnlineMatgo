/**
 * long polling messages setup and remove.
 * 롱폴링 메시지를 받고 response를 저장해둔다. time out이 될 경우 롱 폴링을 취소시킨다.
 * 만약 subscriber(구독자) 이벤트가 발생하면 저장된 response 메시지를 클라이언트에 보낸다.
 * response 메시지를 보낼 동안 발생한 이벤트는 message queue에 저장되고 reqeust가 있을 경우 꺼내어 사용되어 진다.
 */


var request = new Array();

start = function(app,store,subscriber,publisher){
	
	//서버를 다시 시작할때 이전 폴링 메시지를 모두 지운다.
	store.hkeys("polling", function(err, replies){	
		console.log(replies);
		replies.forEach(function(reply , i){
			store.hdel("polling",reply);
		});
	});
	
	app.post('/longpolling', function(req, response){
		
		var name = req.param('name');
		//자신의 이름으로 된 메시지 큐에서 데이터를 가져온다.
		store.lpop(name, function(err, reply) {
		   
		    
		    //redis에 저장된 메시지가 있을 경우
		    if(reply != null){
		    	 console.log("message queue : " + reply);
		    	 response.send(reply);
		    }
		    //redis에 저장된 메시지가 없는 경우
		    else{
		    	//접속한 클라이언트의 이름과 응답메시지를 저장하고 대기한다.
		    	//request.push({name : name ,res: response});
		    	request[name] = response;
		    	var str = JSON.stringify(new Date().getTime());
		    	//내가 폴링 중이란다.
		    	store.hset("polling", name, str);
		    	subscriber.subscribe(name);
		    }
		});
		console.log(name);
	});
	
	//청취가 발생하면
	subscriber.on("message", function(channel, message){
		//채널을 청취하는 애들한테 메시지를 보낸다.
		console.log("메시지를 청취 했습니다.// "+ channel + " : " + message);
		var res = request[channel];
		res.send(message);
		delete request[channel];
		subscriber.unsubscribe(channel);
	});
	//타임 아웃 처리
	setInterval(function() {
		var expiration = new Date().getTime() - 3000;
		store.hkeys("polling", function(err, replies){	
			console.log("long polling lists : " + replies);
			replies.forEach(function(reply , i){
				store.hget("polling", reply,function(err,time){
					var time = JSON.parse(time);
					if(time < expiration){
						console.log(time);
						var res = request[reply];
						
						var data = {protocol : 0};
						res.send(JSON.stringify(data));
						
						//폴링을 중단한다.
						delete request[reply];
						store.hdel("polling", reply);
						subscriber.unsubscribe(reply);
					}
				});
			});
		});
	}, 2000);

};
exports.start = start;