using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameData : NetworkBehaviour {
    private static GameData instance;
    public static GameData Instance {
        get {
            return instance;
        }
    }

    [SyncVar]
    public int numberOfPlayers = 0;

    #region unity callbacks
    void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    #endregion
}
