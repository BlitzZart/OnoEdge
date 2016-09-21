using UnityEngine;
using System.Collections;
using System;

public class PlayerPathManager : MonoBehaviour {

    iTweenPath path;
    iTweenEvent iEvent;


    void Start() {
        NW_GameLogic.GameStartedEvent += OnGameStarted;

    }

    float p = 0;

    void Update() {
       // iTween.PutOnPath(gameObject, iTweenPath.GetPath("Path1"), ((p += Time.deltaTime * 0.001f) % 1));
    }
    void OnDestroy() {
        NW_GameLogic.GameStartedEvent -= OnGameStarted;
    }

    private void OnGameStarted() {
        //path = FindObjectOfType<iTweenPath>();
        //iEvent = GetComponent<iTweenEvent>();

        //iEvent.enabled = true;

    }
}