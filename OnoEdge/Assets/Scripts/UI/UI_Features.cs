using UnityEngine;
using System.Collections;

public class UI_Features: MonoBehaviour {
    public void Toggle() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SelectMenuItem() {
        gameObject.SetActive(true);

        UI_Features[] elements = transform.parent.GetComponentsInChildren<UI_Features>();

        foreach (UI_Features item in elements) {
            if (item != this)
                item.gameObject.SetActive(false);
        }

    }
}
