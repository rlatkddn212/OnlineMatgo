/**
 * 서버의 시작 부분
 * 서버 설정을 한다.
 */

/////////////module//////////////////
var express = require('express');
var routes = require('./routes');
var user = require('./routes/user');
var longpolling = require('./routes/longpolling');
var roomroutes = require('./routes/roomroutes');
var http = require('http');
var path = require('path');
var redis = require('redis');

var app = express();

///////////////redis////////////////////
var store = redis.createClient();
var subscriber = redis.createClient();
var publisher = redis.createClient();


//////////////////setting//////////////////
app.configure(function(){
  app.set('port', process.env.PORT || 3000);
  app.set('views', __dirname + '/views');
  app.set('view engine', 'jade');
  app.use(express.favicon());
  app.use(express.logger('dev'));
  app.use(express.bodyParser());
  app.use(express.methodOverride());
  app.use(app.router);
  app.use(express.static(path.join(__dirname, 'public')));
});

app.configure('development', function(){
  app.use(express.errorHandler());
});


////////////////// route ///////////////////
app.get('/', routes.index);
app.get('/users', user.list);
longpolling.start(app,store,subscriber, publisher);
roomroutes.start(app,store,subscriber, publisher);



///////////////////serverstart//////////////

http.createServer(app).listen(app.get('port'), function(){
  console.log('MATGO SERVER listening on port ' + app.get('port'));
});
