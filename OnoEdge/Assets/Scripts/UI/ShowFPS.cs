using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour {

    private Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = (1.0f / Time.smoothDeltaTime).ToString("f0");
	}
}
