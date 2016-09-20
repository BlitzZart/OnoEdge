using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NW_GameLogic : NetworkBehaviour {
    private static NW_GameLogic instance;
    public static NW_GameLogic Instance {
        get {
            return instance;
        }
    }

    [SyncVar]
    public int Score = 0;

    private Text ui_score;

    public void DestroyedEnemy() {
        if (isServer) {
            Score++;
        }
        else {
            if(hasAuthority)
                CmdDestroyedEnemy();
        }
    }

    [Command]
    public void CmdDestroyedEnemy() {
        Score++;
    }

    void Awake() {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        UI_Score s = FindObjectOfType<UI_Score>();
        if (s != null)
            ui_score = s.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (ui_score == null)
            return;

        ui_score.text = Score.ToString();
	}
}
