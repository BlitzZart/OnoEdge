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

    private List<LobbyPlayer> players;

    void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start() {
        players = new List<LobbyPlayer>();

        if (SceneLogic.Instance.isServer) {
            NW_ManagerAdapter.Instance.StartHost();
        }
        else {
            NW_BroadcastingAdapter.Instance.StartListening();
            
        }
    }

    void Update() {
        CheckPlayersReady();

    }

    public void AddPlayer(LobbyPlayer newPlayer) {
        players.Add(newPlayer);
    }

    public void RemovePlayer(LobbyPlayer leaving) {
        players.Remove(leaving);
    }

    // Update is called once per frame
    private void CheckPlayersReady () {
        if (players.Count < 1)
            return;

        foreach (LobbyPlayer item in players) {
            if (!item.playerIsReady)
                return;
        }
        SceneLogic.Instance.StartGame();
    }
}
