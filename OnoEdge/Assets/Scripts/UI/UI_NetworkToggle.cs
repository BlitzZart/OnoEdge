using UnityEngine;
using System.Collections;

public class UI_NetworkToggle : MonoBehaviour {

    NetworkUI ui;

    public void Toggle() {
        if (ui == null)
            ui = FindObjectOfType<NetworkUI>();

        if (ui != null)
            ui.gameObject.SetActive(!ui.gameObject.activeSelf);
    }
}