using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobbyPlayerEntry : MonoBehaviour {
    private Text text;

    public int playerNumber = 0;
    public bool localPlayer = false;

    public Player player;

    public void ChangeReadyState() {
        if (!localPlayer)
            return;

        player.CmdPlayerReady();
    }

    #region public
    #endregion



    #region unity callbacks
    void Start() {
        text = GetComponentInChildren<Text>();
    }

    void Update() {
        text.text = "Player " + (playerNumber + 1);
        if (player.playerIsReady) {
            text.text += " - READY";
        }
    }
    #endregion
}
