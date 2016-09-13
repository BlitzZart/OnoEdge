using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {
    private Transform cam;
	// Use this for initialization
	void Start () {
        cam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(cam);
	}
}
