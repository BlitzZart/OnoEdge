using UnityEngine;
using System.Collections;

public class CameraSetup : MonoBehaviour {

    

    private Color background;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        RenderSettings.fogColor = Camera.main.backgroundColor;
	}
}
