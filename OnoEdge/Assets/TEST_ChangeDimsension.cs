using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TEST_ChangeDimsension : MonoBehaviour {
    private Text text;
    void Start() {
        text = GetComponentInChildren<Text>();
    }

    private Player GetLocalPlayer() {
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player item in players) {
            if (item.localPlayerAuthority)
                return item;
        }
        return null;
    }

    // Use this for initialization
    public void ChangeDimension() {
        NW_GameLogic.Instance.ToggleDimensions();
    }
}