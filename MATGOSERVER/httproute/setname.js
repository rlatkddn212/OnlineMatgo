/**
 * 미 사용
 */
var start = function(store) {
	app.post('/setname', function(req, res) {
		var name = req.param('name');
		
		//존재하는지 검사
		store.exists("username/" + name, function(err, reply){
			if(reply){
				//존재하는 아이디입니다.
			}
			else{
				store.set("username/" + name , name);
			}
		});
	});
}