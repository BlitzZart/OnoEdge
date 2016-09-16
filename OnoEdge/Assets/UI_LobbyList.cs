using UnityEngine;
using System.Collections;

public class UI_LobbyList : MonoBehaviour {
    private int startingHeight = 256;
    private int gap = 128;

    

	void Start () {
	
	}

	void Update () {
	
	}

    public void AddEntry(GameObject entry, int number) {
        RectTransform trans = entry.GetComponent<RectTransform>();

        entry.transform.parent = transform;
        trans.anchoredPosition = new Vector2(1, startingHeight + gap * number);
        trans.localScale = Vector2.one;
    }

    public void RemoveEntry() {

    }
}
