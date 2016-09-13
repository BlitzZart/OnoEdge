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
    NetworkManager manager;

    void Awake() {
        instance = this;
    }

    void Start() {
        manager = NetworkManager.singleton;
    }
     
	// Use this for initialization
	public void StartHost () {
        manager.StartHost();

        NW_BroadcastingAdapter.Instance.StartBroadcasting();
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
