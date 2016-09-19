using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameLogic : MonoBehaviour {


    void Start () {
        Lobby.Instance.ActivatePlayers();
        NetworkManager_B.Instance.StartGame();
        Destroy(Lobby.Instance.gameObject);
	}
}
