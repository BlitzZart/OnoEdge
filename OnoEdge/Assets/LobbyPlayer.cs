using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkBehaviour {
    //[SyncVar]
    //public bool playerIsReady = false;

    //public GameObject lobbyEntryPrefab;
    //public Player networkPlayer;


    //private UI_LobbyList lobbyList;

    //void OnLevelWasLoaded() {
    //    networkPlayer.gameObject.SetActive(true);
    //    Destroy(gameObject);
    //}

    //#region unity callbacks
    //void Start() {
    //    DontDestroyOnLoad(gameObject);

    //    CreateUIEntry(networkPlayer.isLocalPlayer);
    //}

    //void Update() {
    //    if (Input.GetKeyDown(KeyCode.K))
    //        print("BLUB");
    //}
    //#endregion


    //#region public
    //public void CreateUIEntry(bool isLocal) {
    //    lobbyList = FindObjectOfType<UI_LobbyList>();
    //    if (lobbyList == null)
    //        return;

    //    GameObject go = Instantiate(lobbyEntryPrefab);
    //    LobbyPlayerEntry playerEntry = go.GetComponent<LobbyPlayerEntry>();
    //    if (playerEntry == null)
    //        return;

    //    playerEntry.lobbyPlayer = this;
    //    playerEntry.playerNumber = networkPlayer.playerNumber;

    //    if (isLocal) {
    //        playerEntry.localPlayer = true;
    //    }

    //    Lobby.Instance.AddPlayer(this);
    //    lobbyList.AddEntry(go, GameData.Instance.numberOfPlayers);
    //}
    //#endregion

    //#region unet 
    //[Command]
    //public void CmdPlayerReady() {
    //    playerIsReady = !playerIsReady;
    //}
    //#endregion
}
