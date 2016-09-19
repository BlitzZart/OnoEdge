using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLogic : MonoBehaviour {
    private static SceneLogic instance;
    public static SceneLogic Instance {
        get {
            return instance;
        }
    }

    public static bool isServer = false;

    #region unity callbacks
    void Awake() {
        instance = this;
    }

    #endregion

    #region public
    public void GoToLobby(bool asServer) {
        isServer = asServer;

        SceneManager.LoadScene("lobby");
    }

    public void GotoStartScreen() {
        SceneManager.LoadScene("start");
    }

    public void StartGame() {
        SceneManager.LoadScene("play");
    }

    #endregion
}