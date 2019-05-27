
/*
 * GET home pagee.
 */

exports.index = function(req, res){
  res.render('index', { title: 'Express' });
};