using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

    public float radius = 5;
    public GameObject plane;
    private int numberOfPlayers;

	// Use this for initialization
	void Start () {
        float scale = radius / 10 * 2;
        plane.transform.localScale = new Vector3(scale, scale, scale);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
