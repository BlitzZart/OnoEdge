using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {
    private Player[] players;

    #region unity callbacks
    void Start() {
        InitializeGame();
    }
    #endregion


    #region public
    public void ActivatePlayers() {
        foreach (Player item in players) {
            item.Activate();
        }
    }

    public void ChangeDimension(int dimension) {
        Player player = GetLocalPlayer();
        if (player != null) {
            player.Dimensions = dimension;
        }
    }
    #endregion

    #region private
    private Player GetLocalPlayer() {
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player item in players) {
            if (item.localPlayerAuthority)
                return item;
        }
        return null;
    }

    void InitializeGame() {
        // copy player list
        players = new Player[Lobby.Instance.Players.Count];
        Lobby.Instance.Players.CopyTo(players);
        // destroy lobby
        Destroy(Lobby.Instance.gameObject);
        // activate players
        ActivatePlayers();
        // start game
        NetworkManager_B.Instance.StartGame();
    }
    #endregion
}