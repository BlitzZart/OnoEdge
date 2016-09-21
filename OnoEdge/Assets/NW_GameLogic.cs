using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NW_GameLogic : NetworkBehaviour {
    public delegate void GameLogicIntDelInt(int value);
    public static event GameLogicIntDelInt ChangeDimensionEvent;
    public delegate void GameLogicDelSimple();
    public static event GameLogicDelSimple GameStartedEvent;



    private static NW_GameLogic instance;
    public static NW_GameLogic Instance {
        get {
            return instance;
        }
    }

    [SyncVar]
    public int Score = 0;
    [SyncVar]
    public int Dimension;


    private Text ui_score;


    #region unity callbacks
    void Awake() {
        instance = this;
    }
    void Start() {
        UI_Score s = FindObjectOfType<UI_Score>();
        if (s != null)
            ui_score = s.GetComponent<Text>();
    }
    void Update() {
        if (isServer) {
            DebugSwitchDimensions();
        }

        if (isClient) {
            if (ui_score == null)
                return;

            ui_score.text = Score.ToString();
        }
    }
    #endregion

    #region private
    private void DebugSwitchDimensions() {
        if (!Input.anyKeyDown)
            return;

        int newDimension = Dimension;

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            newDimension = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            newDimension = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            newDimension = 3;
        }

        if (newDimension == Dimension)
            return;

        Dimension = newDimension;

        RpcChangeDimension(Dimension);
    }
    #endregion

    #region public
    public void StartGame() {
        if (GameStartedEvent != null)
            GameStartedEvent();
    }

    public void DestroyedEnemy() {
        if (isServer) {
            Score++;
        }
        else {
            if(hasAuthority)
                CmdDestroyedEnemy();
        }
    }

    public void ToggleDimensions() {
        int d = Dimension;
        d = ((d + 1) % 3) + 1;

        print("new d " + d);
        Dimension = d;
        RpcChangeDimension(d);
    }

    #endregion

    #region network
    [ClientRpc]
    private void RpcChangeDimension(int d) {
        CameraSetup.Instance.ChangeDimension(d);
        if (ChangeDimensionEvent != null)
            ChangeDimensionEvent(d);
    }
    [Command]
    public void CmdDestroyedEnemy() {
        Score++;
    }
    #endregion

}
