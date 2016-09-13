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
        Player player = GetLocalPlayer();
        if (player != null) {
            player.dimensions = ((player.dimensions + 1) % 3) + 1; // start on 1 and cap on 3
            text.text = "Dimensions :" + player.dimensions;
        }
    }
}