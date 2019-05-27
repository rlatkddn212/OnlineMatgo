using UnityEngine;
using System.Collections;

public class OppInfo {
	private static OppInfo instance;
	int character_;
	// 레벨
	int level_;
	// 배수
	int multiple_;
	// id
	string id_;


	public static OppInfo getInstance() {
		if (instance == null) {
			instance = new OppInfo();
		}
		return instance;
	}


	public int Character {
		get { return this.character_; }
		set {
			this.character_ = value;
		}
	}
	public int Level {
		get { return this.level_; }
		set {
			this.level_ = value;
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
