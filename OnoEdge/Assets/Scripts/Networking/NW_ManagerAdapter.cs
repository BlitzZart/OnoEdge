using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NW_ManagerAdapter : MonoBehaviour {
    private static NW_ManagerAdapter instance;
    public static NW_ManagerAdapter Instance {
        get {
            return instance;
        }
    }
    private NetworkManager manager;
    public GameObject lobbyPlayerPrefab;


    void Awake() {
        instance = this;
    }

    void Start() {
        manager = NetworkManager.singleton;
    }
     
	// Starts host and broadcasting
	public void StartHost () {
        manager.StartHost();

        NW_BroadcastingAdapter.Instance.StartBroadcasting();
    }

    public void StopHost() {
        manager.StopHost();
    }

    public void AddLobbyPlayer(GameObject nwPlayer) {
        GameObject go = Instantiate(lobbyPlayerPrefab);
        LobbyPlayer player = go.GetComponent<LobbyPlayer>();
        if (player == null)
            return;

        player.networkPlayer = nwPlayer;

        //manager.SpawnObject(go);
        NetworkServer.Spawn(go);
    }


    public void QuitApplication() {
        NW_ManagerAdapter.Instance.StopHost();
        NW_BroadcastingAdapter.Instance.StopAll();
        Application.Quit();
    }
}
