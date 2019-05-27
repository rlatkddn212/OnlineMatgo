using UnityEngine;
using System.Collections;

public class UserInfo {
	private static UserInfo instance;
	// 케릭터
	int character_;
	// 레벨
	int level_;
	// 배수
	int multiple_;
	// id
	string id_;
	// custom number
	string cn_;
	string room_;

	STATE userState_;

	private UserInfo() {

	}
	public static UserInfo getInstance() {
		if (instance == null) {
			instance = new UserInfo();
		}
		return instance;
	}

	public enum STATE {
		LOBBY,
		WAIT,
		BALLOON,
		GAME,
		RESULT
	}
	public string Room {
		get { return this.room_; }
		set {
			this.room_ = value;
		}
	}
	public int Character {
		get { return this.character_; }
		set {
			this.character_ = value;
		}
	}
	public STATE UserState {
		get { return this.userState_; }
		set {
			this.userState_ = value;
		}
	}
	public int Level {
		get { return this.level_; }
		set {
			this.level_ = value;
		}
	}
	public string Cn {
		get { return this.cn_; }
		set {
			this.cn_ = value;
		}
	}
	public string Id {
		get { return this.id_; }
		set {
			this.id_ = value;
		}
	}
	public int Multiple {
		get { return this.multiple_; }
		set {
			this.multiple_ = value;
		}
	}

}
