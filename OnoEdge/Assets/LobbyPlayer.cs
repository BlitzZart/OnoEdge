using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkBehaviour {
    [SyncVar]
    public int numberOfPlayers = 0;

    [SyncVar]
    public bool playerIsReady = false;

    public GameObject lobbyEntryPrefab;
    public GameObject networkPlayer;


    private UI_LobbyList lobbyList;

    void OnLevelWasLoaded() {
        networkPlayer.SetActive(true);
        Destroy(gameObject);
    }

    #region unity callbacks
    void Start() {
        DontDestroyOnLoad(gameObject);
        lobbyList = FindObjectOfType<UI_LobbyList>();
        if (lobbyList == null)
            return;

        CreateUIEntry();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.K))
            print("BLUB");
    }
    #endregion

    #region private
    private void CreateUIEntry() {
        GameObject go = Instantiate(lobbyEntryPrefab);

        LobbyPlayerEntry playerEntry = go.GetComponent<LobbyPlayerEntry>();
        if (playerEntry == null)
            return;

        playerEntry.lobbyPlayer = this;

        if (hasAuthority) {
            playerEntry.localPlayer = true;

            playerEntry.playerNumber = numberOfPlayers;
            CmdCntPlayer();
        }
        Lobby.Instance.AddPlayer(this);
        lobbyList.AddEntry(go, numberOfPlayers);
    }
    #endregion

    #region unet 
    [Command]
    private void CmdCntPlayer() {
        numberOfPlayers++;
    }
    [Command]
    public void CmdPlayerReady() {
        playerIsReady = !playerIsReady;
    }
    #endregion
}
