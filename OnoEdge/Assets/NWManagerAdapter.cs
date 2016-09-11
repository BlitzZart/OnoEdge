using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NWManagerAdapter : MonoBehaviour {
    NetworkManager manager;
    

    void Start() {
        manager = FindObjectOfType<NetworkManager>();
    }
     
	// Use this for initialization
	public void StartHost () {
        manager.StartHost();
    }
	
}
