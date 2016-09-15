using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLogic : MonoBehaviour {

    public static bool isServer = false;

    void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void GoToLobby(bool asServer) {
        if (asServer)
            isServer = true;

        SceneManager.LoadScene("lobby");
    }
}
