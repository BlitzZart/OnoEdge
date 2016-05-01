using UnityEngine;
using System.Collections;

public class UI_Setup : MonoBehaviour {
    public void Toggle() {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
