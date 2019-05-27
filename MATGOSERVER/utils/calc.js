/**
 * 계산
 */

function calcGwang(gwang){
	var ret = 0;
	console.log("광의 길이  : " + gwang.length);
	if(gwang.length === 3){
		ret = 3;
		for(var i = 0 ; i < 3 ; i++){
			//비광 처리
			if(gwang[i] === 45){
				ret--;
			}
		}
	}
	else if(gwang.length == 4){
		ret = 4;
	}
	else if(gwang.length == 5){
		ret = 15;
	}else{
		//없쩡
		ret = 0;
	}
	return ret;
}
function calcDan(dan){
	//초단
	//청단
	//홍단
	var ret = 0;
	if(dan.length > 4){
		ret = ret + dan.length - 4;
	}
	var hongDan = 0;
	var chungDan = 0;
	var choDan = 0;
	
	for(var i = 0 ; i < dan.length ; i++){
		if(dan[i] == 2 ||dan[i] ==  6|| dan[i] ==  10){
			hongDan++;
		}
		else if(dan[i] == 22 ||dan[i] ==  34||dan[i] ==  38){
			chungDan++;
		}
		else if(dan[i] == 14|| dan[i] == 18 ||dan[i] == 26){
			choDan++;
		}
	}
	
	if(hongDan == 3){
		ret += 3;
	}
	if(chungDan == 3){
		ret += 3;
	}
	if(choDan == 3){
		ret += 3;
	}
	
	return ret;
}
function calcEnd(end){
	//고도리
	var ret = 0;
	
	if(end.length > 4){
		ret = ret + end.length - 4;
	}
	
	var godori = 0;
	
	for(var i = 0 ; i < end.length ; i++){
		if(end[i] == 5|| end[i] ==13 || end[i] == 30){
			godori++;
		}
	}	
	if(godori == 3){
		ret += 6;
	}
	return ret;
}

function calcBlood(blood){
	var value = 0;
	
	for(var i = 0 ; i < blood.length ; i++){
		//쌍피 2점 추가
		if(blood[i] == 48 || blood[i] == 42 || blood[i] == 49 || blood[i] == 33){
			value += 2;
		}
		//보너스 피
		else if(blood[i] == 50){
			value += 3;
		}
		else{
			value += 1;
		}
	}
	
	if(value > 9){
		return value - 9;
	}
	else{
		return 0;		
	}
}
exports.calcBlood = calcBlood;
exports.calcGwang = calcGwang;
exports.calcEnd = calcEnd;
exports.calcDan = calcDan;
