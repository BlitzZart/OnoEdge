using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NW_BroadcastingAdapter : NetworkDiscovery {
    private static NW_BroadcastingAdapter instance;
    public static NW_BroadcastingAdapter Instance {
        get {
            return instance;
        }
    }

    [SerializeField]
    public string data = "OnNoEdge";

    void Awake() {
        instance = this;
    }

    public override void OnReceivedBroadcast(string fromAddress, string data) {
        // wrong data/game
        if (data.Equals(this.data))
            return;
        //if (NetworkManager.singleton.client.isConnected)
        //    return;

        StopBroadcast();

        // is null when the client is in the offline scene
        // after changing to lobby scene this code will be executed a second time
        //if (NetworkManager.singleton == null) {
        //    SceneLogic.Instance.GoToLobby(false);
        //    return;
        //}


        NetworkManager.singleton.networkAddress = fromAddress;
        NetworkManager.singleton.StartClient();
    }

    public void StopAll() {
        //Initialize();
        StopBroadcast();
    }

    public void StartListening() {
        Initialize();
        StartAsClient();
    }

    public void StartBroadcasting() {
        broadcastData = data;
        Initialize();
  
        StartAsServer();
    }
}
