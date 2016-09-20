using UnityEngine;
using System.Collections;

public class DisableOnLoad : MonoBehaviour {

    void OnLevelWasLoaded() {
        gameObject.SetActive(false);
    }
}
