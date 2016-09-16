using UnityEngine;
using System.Collections;

public class Base : MonoBehaviour {
    private Light baseLight;
    private float speed = 80;

    public Color saveColor, dangerColor;

	// Use this for initialization
	void Start () {
        baseLight = GetComponentInChildren<Light>();
        if (baseLight != null)
            baseLight.color = saveColor;
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

    // Update is called once per frame
    void Update () {

	}
}
