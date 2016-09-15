using UnityEngine;
using System.Collections;

public class Lobby : MonoBehaviour {
    private static Lobby instance;
    public static Lobby Instance {
        get {
            return instance;
        }
    }

    // Use this for initialization
    void Start () {
        if (SceneLogic.isServer)
            NW_ManagerAdapter.Instance.StartHost();

        NW_ManagerAdapter.Instance.AddLobbyPlayer();
    }

	// Update is called once per frame
	void Update () {
	
	}
}
