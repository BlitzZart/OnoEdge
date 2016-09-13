using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Scoring : NetworkBehaviour {
    private static Scoring instance;
    public static Scoring Instance {
        get {
            return instance;
        }
    }

    [SyncVar]
    public int Score = 0;



    [Command]
    public void CmdDestroyedEnemy() {
        Score++;
    }

    void Awake() {
        instance = this;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
