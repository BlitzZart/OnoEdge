using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lobby : MonoBehaviour {
    private static Lobby instance;
    public static Lobby Instance {
        get {
            return instance;
        }
    }



    private List<Player> players;
    public List<Player> Players {
        get {
            return players;
        }
    }

    private bool loadingDone;
    private UI_LobbyList lobbyList;

    #region unity callbacks
    void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start() {
        players = new List<Player>();
        lobbyList = FindObjectOfType<UI_LobbyList>();


        if (SceneLogic.isServer) {
            NW_ManagerAdapter.Instance.StartHost();
        }
        else {
            NW_BroadcastingAdapter.Instance.StartListening();
        }
    }

    void Update() {
        CheckPlayersReady();
    }
    #endregion

    #region public
    public void AddPlayer(Player newPlayer) {
        players.Add(newPlayer);

        if (lobbyList == null)
            lobbyList = FindObjectOfType<UI_LobbyList>();

        lobbyList.AddEntry(newPlayer.lobbyEntryPrefab);
    }

    public void RemovePlayer(Player leaving) {
        players.Remove(leaving);
        lobbyList.AddEntry(leaving.lobbyEntryPrefab);
    }

    public void LeaveLobby() {
        SceneLogic.Instance.GotoStartScreen();
    }
    #endregion

    #region private
    private void CheckPlayersReady () {
        if (loadingDone)
            return;

        if (players.Count < 1)
            return;

        foreach (Player item in players) {
            if (!item.playerIsReady)
                return;
        }
        loadingDone = true;
        lobbyList.HideUI();
        SceneLogic.Instance.StartGame();
    }
    #endregion
}
