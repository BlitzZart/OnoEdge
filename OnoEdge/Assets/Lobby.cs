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
    public void ActivatePlayers() {
        foreach (Player item in players) {
            item.Activate();
        }
    }

    public void AddPlayer(Player newPlayer) {
        players.Add(newPlayer);
        lobbyList.AddEntry(newPlayer.playerEntry);
    }

    public void RemovePlayer(Player leaving) {
        players.Remove(leaving);
        lobbyList.AddEntry(leaving.playerEntry);
    }

    public void LeaveLobby() {
        SceneLogic.Instance.GotoStartScreen();
    }
    #endregion

    #region private
    private void CheckPlayersReady () {
        if (players.Count < 1)
            return;

        foreach (Player item in players) {
            if (!item.playerIsReady)
                return;
        }
        lobbyList.HideUI();
        SceneLogic.Instance.StartGame();
    }
    #endregion
}
