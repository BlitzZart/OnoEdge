using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class Base : NetworkBehaviour {
    private Light baseLight;
    private float speed = 5;

    public Color saveColor, dangerColor;


    #region unity callbacks
    void Start() {
        baseLight = GetComponentInChildren<Light>();
        if (baseLight != null)
            baseLight.color = saveColor;

        Transform[] currentChildren = GetComponentsInChildren<Transform>();

        //foreach(Transform item in currentChildren)
        //    if (item != transform)
        //        item.SetParent(transform);

        Player[] players = FindObjectsOfType<Player>();
        foreach (Player item in players) {
            item.AssignBase(this);

            //item.transform.SetParent(transform);
        }
    }


    void Update() {
        if (!isServer)
            return;
        
    }

    void OnDestroy() {
    }
    #endregion

    #region unity messages
    void MsgColliderEmpty() {
        //print("empty ");
        if (baseLight != null)
            baseLight.color = saveColor;
    }

    void MsgEnemyEntered() {
        //print("entered");
        if (baseLight != null)
            baseLight.color = dangerColor;
    }

    void MsgEnemyLeft(bool enemiesInside) {
        //print("left " + enemiesInside);
        if (!enemiesInside)
            if (baseLight != null)
                baseLight.color = saveColor;
    }
    #endregion
}