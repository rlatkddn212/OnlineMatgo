# OnlineMatgo
this project is Online Matgo game.

리소스가 없고 소스 파일만 있습니다. 참고바랍니다 :)

-----------------------------
## 개발 도구
클라이언트 : unity

서버 : Node.js, http long polling, redis   

----------------------------
## 게임 코드를 보려면?
클라이언트 : matgofong/Assets/Script/  
서버 : MATGOSERVER/  

-----------------------------
## 게임 영상을 보려면?
https://www.youtube.com/watch?v=zbqMfbKU3hc

----------------------------
## 설계

### 기능 명세

   

   

|기능| 상세 기능 | 추가 설명 | 기타 |
| -------------- | ------------------------------------------------------------ | :----------------------------------------------------------- | ---- |
| 게임화면       | 카드 패                                                      | 칸에 맞춤   먹을 수 있는 패를 구분 시켜줌                    |      |
| 상대편 카드 패 | 개수만 보여줌(패는 서버에서   관리됨)                        |                                                              |      |
| 게임판         | 선턴 정하기   카드 패가 깔림   카드 뭉치에서 한장씩 드로우 가능 |                                                              |      |
| 점수판         | 점수 계산   상대방의 피를 뺏음                               |                                                              |      |
| 상대 점수판    | 점수 계산   자신의 피를 상대가 뺏음                          |                                                              |      |
| 유저 정보      | 유저의 케릭터, 아이디, 레벨 표시                             |                                                              |      |
| 맞고의 게임룰  | 대통령, 따닥, 쪽등                                           |                                                              |      |
| 패             | 1월에서 12월까지 모양을 가짐                                 |                                                              |      |
| 고 스톱 팝업   | 7점   이상일 시 고 스톱 팝업                                 |                                                              |      |
| 선택 팝업      | 여러 패중 하나를 선택해야 할 때   피로 사용할지 구분할 때    |                                                              |      |
| 승패 팝업      | 점수와 획득한 코인을 보여줌   나가기   계속하기              |                                                              |      |
| 설정           | 사운드 조절                                                  |                                                              |      |
| 사운드         | 게임 중 소리                                                 |                                                              |      |
| 재접속         | 나갔지만 아직 게임 실행 중일 경우                            |                                                              |      |



### 네트워크 구성

![1560471835672](https://github.com/rlatkddn212/OnlineMatgo/blob/master/assets/1560471835672.png)

(mongo.db는 사용 못함)

Restful API을 사용, 유니티 클라이언트에서 HTTP 메시지를 보냅니다.

Redis에 데이터를 저장하게 됩니다.



**restful** **구조**

Json 형식으로 데이터 통신이 가능함 (Node.js단에서 파싱에 오버헤드가 줄어듬)

상태 저장이 없음. 분산 서버 구성이 유리해짐

HTTP 기본 메소드 ( get, push, put, delete)로 접근. 순수 웹 기술들을 사용할 수 있음



**long polling** **구조**

http는 클라이언트에서 요청하면 응답을 해주는 서버라서 서버가 실시간으로 중계할 수 없음

클라이언트가 서버에 계속 요청을 보내는 폴링으로 서버를 구성할 수 있지만 서버에 부하가 많이 발생하게 되고 이런 HTTP의 단방향 통신을 해결한 방법으로 **long polling(comet)**을 활용 할 수 있음


### 1) 액티비티 다이어그램

 

**게임 진행 큰 그림**

![1560472035020](https://github.com/rlatkddn212/OnlineMatgo/blob/master/assets/1560472035020.png)

시작 시 배수를 결정하고 선을 정합니다. 그리고 한 턴씩 주고 받으면서 게임을 진행합니다.



**게임 진행**

![1560472055040](https://github.com/rlatkddn212/OnlineMatgo/blob/master/assets/1560472055040.png)

게임 진행 구조입니다 턴을 받으면 자신이 가지고 있는 패들이 바닥에 있는 패들과의 관계를 계산하여 특정 모양으로 보여줍니다. 그리고 패를 선택 후 패 더미에서 패를 하나 더 꺼내 자신의 점수판으로 가져옵니다. 그런 뒤 점수를 계산하고 상대방에게 턴을 넘깁니다.

(특정 패를 먹었을 경우 피를 빼앗는 경우는 점수계산에 포함됩니다.)



### 2) 상태 다이어그램

![1560472085898](https://github.com/rlatkddn212/OnlineMatgo/blob/master/assets/1560472085898.png)

유저의 상태를 나타냅니다. 게임에 접속하면 로비로 가거나 게임 중 재접속하여 진행 중이던 게임을
연결하여 진행할 수 있습니다. 그리고 방에 입장하면 대기 상태가 되고 게임을 자동 혹은 수동으로 진행하게
됩니다. 



### 3) **클래스 다이어그램 & 시퀀스 다이어그램**

 

**3-1)** **클라이언트 부분**

- 게임관리자

![1560472146236](https://github.com/rlatkddn212/OnlineMatgo/blob/master/assets/1560472146236.png)

게임 매니저는 게임 로직들을 관리합니다. 게임을 진행하는데 필요한 요소들을 모두
포함관계로 가지고 있거나 직접 생성합니다.

- 게임진행에 필요한 클래스

![1560472163339](https://github.com/rlatkddn212/OnlineMatgo/blob/master/assets/1560472163339.png)

![1560472192757](https://github.com/rlatkddn212/OnlineMatgo/blob/master/assets/1560472192757.png)

보드 판, 핸드, 점수 판들이 화투 패를 가질 수 있습니다. 어떻게 관리할지는 개발을 진행하면서 정하는 게 좋을 것 같습니다



- 유저정보 저장

![1560472213956](https://github.com/rlatkddn212/OnlineMatgo/blob/master/assets/1560472213956.png)

![1560472217781](https://github.com/rlatkddn212/OnlineMatgo/blob/master/assets/1560472217781.png)

자신의 정보는 로그인 세션을 받았을 때 생성됩니다. 사용자는 로비에 있든지 게임 안에 있던지 유일 해야 하기 때문에 싱글턴으로 구현합니다. 그리고 라이프 사이클을 공유하지 않습니다.

상대방의 정보는 게임 속에만 존재하고 상대방이 방에 입장하고 나갈 경우 생성 삭제됩니다.

유저 정보들은 케릭터나 레벨을 보여주기도 하지만 지금까지 계산으로 몇 배의 점수를 받는지 계산해둡니다.



- 클라이언트 네트워크 클래스

![1560472241300](https://github.com/rlatkddn212/OnlineMatgo/blob/master/assets/1560472241300.png)

네트워크에서 데이터를 받을 경우 network클래스에서 비동기 recv를 통해 데이터를 받을 것입니다. (제가 사용할 모델은 콜백하여 Thread pool에서 쓰레드를 할당받아 recv를 처리 합니다. )

그런 후 string parser로 전송되어 게임매니저에서 활용 할 수 있는 데이터로 만들어줍니다.

네트워크에 데이터를 보낼 경우는 게임 매니저에서 보내고 싶은 데이터가 있을 경우 json 형식으로 데이터를 만들어 network클래스에 send메시지로 보내집니다.



**3-2)** **서버 부분**

- 큰 구성

![1560472285538](https://github.com/rlatkddn212/OnlineMatgo/blob/master/assets/1560472285538.png)

Node.js로 HTTP 서버를 구성하기 위해 express.js를 활용합니다.

Redis는 저장소에 저장하기 위한 store 객체 발행자/구독자를 사용하기 위한 publisher/subscri ber객체를 만들어 활용합니다.

 클라이언트에서 받은 요청메시지는 routes를 통해 처리됩니다.



- Routes 처리 후 메시지 보내기

![1560472303987](https://github.com/rlatkddn212/OnlineMatgo/blob/master/assets/1560472303987.png)

routes에서 데이터를 처리하고 나면
publishMessage를 에서 publish할지 메시지 큐에 넣을지 결정하고 이벤트를
발생시켜 구독자를 통해 받은 메시지를 클라이언트로 보냅니다.



- Route 처리들

![1560472328385](https://github.com/rlatkddn212/OnlineMatgo/blob/master/assets/1560472328385.png)

roomRoutes.js : 요청을 나누는 역할을 합니다.

Enterroom.js : 유저가 방에 입장했을 때 처리를 합니다.

Selectballoon.js :  풍선껌을 불 것 인지 선택합니다.

Getcard.js : 패들을 나눠줍니다.

Selectturn.js : 선을 정합니다.

Selectcardinhand.js : 손 패에서 패를 냅니다.

Selectbonuscard.js : 손 패에서 보너스 패를 냅니다.

Selectboomcard.js : 손 패에서 폭탄을 냅니다.

Nextcard.js : 화투 패 뭉치에서 패를 한 장을 뒤집습니다.

Movecard.js : 획득할 카드를 계산하고 이동 시킵니다.

Calcscore.js : 점수를 계산합니다.

Moveninemonth.js : 구월 열끗을 이동 시킬지 판단합니다.

Gostop.js : 고 스톱을 판단합니다.

Result.js : 결과를 보내줍니다.

Restart.js : 재 시작 합니다.

Outroom.js : 방을 나갑니다.



