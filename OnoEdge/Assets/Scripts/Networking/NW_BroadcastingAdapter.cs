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
        if (data.Equals(this.data))
            return;

        //if (NetworkManager.singleton.client.isConnected)
        //    return;

        NetworkManager.singleton.networkAddress = fromAddress;
        NetworkManager.singleton.StartClient();

        StopBroadcast();

    }

    public void StopAll() {
        StopBroadcast();
    }

    public void StartListening() {
        Initialize();
        StartAsClient();
    }

    public void StartBroadcasting() {
        Initialize();

        broadcastData = data;
        StartAsServer();
    }
}
