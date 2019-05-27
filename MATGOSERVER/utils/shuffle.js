/**
 * 카드를 셔플한다.
 */
 var knuthShuffle = function(arr) {
    var rand, temp, i;
 
    for (i = arr.length - 1; i > 0; i -= 1) {
        rand = Math.floor((i + 1) * Math.random());
        temp = arr[rand];
        arr[rand] = arr[i];
        arr[i] = temp;
    }
    return arr;
};
exports.knuthShuffle = knuthShuffle;