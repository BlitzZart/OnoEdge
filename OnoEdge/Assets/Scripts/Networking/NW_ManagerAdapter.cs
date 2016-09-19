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

    void Awake() {
        instance = this;
    }

    void Start() {
        manager = NetworkManager.singleton;
    }
     
	// Starts host and broadcasting
	public void StartHost () {

        NW_BroadcastingAdapter.Instance.StartBroadcasting();
        manager.StartHost();
    }

    public void StopHost() {
        manager.StopHost();
    }


    public void QuitApplication() {
        NW_ManagerAdapter.Instance.StopHost();
        NW_BroadcastingAdapter.Instance.StopAll();
        Application.Quit();
    }
}
