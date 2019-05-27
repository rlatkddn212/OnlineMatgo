/**
 * publisher(발행자)를 통해 이벤트를 발생 시키거나 메시지 큐에 등록한다.
 */

var send = function(store, publisher, data) {
	lock = require("redis-lock")(store);

	lock("polling", function(done) {
		if (data['user'] != null) {
			//var muti = store.multi();
			store.hexists("polling", data['user'],function(err, reply){
				if(reply){
					console.log("published : " + data['user']);
					publisher.publish(data['user'], JSON.stringify(data));
					
					store.hdel("polling", data['user'],function(err, reply2){
						done(function() {
				            console.log("Lock을 푼다.");
				        });
					});
				}
				else{
					console.log("enqueued");
					store.rpush(data['user'], JSON.stringify(data));
					done(function() {
			            console.log("Lock을 푼다.");
			        });
				}
			});
		}
	});
}

exports.send = send;