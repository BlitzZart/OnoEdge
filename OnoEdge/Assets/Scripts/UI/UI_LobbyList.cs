using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_LobbyList : MonoBehaviour {
    private int startingHeight = 384;
    private int gap = 128;

    private List<LobbyPlayerEntry> playerEntries;
    

	void Start () {
        playerEntries = new List<LobbyPlayerEntry>();
	}

    #region public

    public void HideUI() {
        transform.parent.parent.gameObject.SetActive(false);
    }

    public void UpdateEntries() {
        foreach(LobbyPlayerEntry item in playerEntries) {
            item.playerNumber = item.player.playerNumber;
            UpdateEntry(item);
        }
	}

    private void UpdateEntry(LobbyPlayerEntry entry) {
        RectTransform trans = entry.GetComponent<RectTransform>();

        entry.transform.SetParent(transform);// = transform;
        trans.anchoredPosition = new Vector2(1, startingHeight + gap * -entry.playerNumber);
        trans.localScale = Vector2.one;
    }

    public void AddEntry(LobbyPlayerEntry entry) {
        playerEntries.Add(entry);
        UpdateEntries();
    }

    public void RemoveEntry(LobbyPlayerEntry entry) {
        playerEntries.Remove(entry);
    }
    #endregion

}
