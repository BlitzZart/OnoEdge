﻿using UnityEngine;
using System.Collections;
using System;

public class Base : MonoBehaviour {
    private Light baseLight;
    private float speed = 80;

    public Color saveColor, dangerColor;




	// Use this for initialization
	void Start () {
        baseLight = GetComponentInChildren<Light>();
        if (baseLight != null)
            baseLight.color = saveColor;

        Player[] players = FindObjectsOfType<Player>();
        foreach (Player item in players) {
            item.AssignBase(this);
        }
    }

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

    void OnDestroy () {
    }
}
