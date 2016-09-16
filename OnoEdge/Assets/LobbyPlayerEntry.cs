using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobbyPlayerEntry : MonoBehaviour {
    private Text text;

    public int playerNumber = 0;
    public bool localPlayer = false;

    public LobbyPlayer lobbyPlayer;

    public void ChangeReadyState() {
        if (!localPlayer)
            return;

        lobbyPlayer.CmdPlayerReady();
    }

    #region public
    #endregion



    #region unity callbacks
    void Start() {
        text = GetComponentInChildren<Text>();
    }

    void Update() {
        text.text = "Player " + playerNumber;
        if (lobbyPlayer.playerIsReady) {
            text.text += " - READY";
        }

    }
    #endregion


}
