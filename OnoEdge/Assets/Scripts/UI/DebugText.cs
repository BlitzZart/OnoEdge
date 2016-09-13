using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugText : MonoBehaviour {
    private static DebugText instance;
    public static DebugText Instance {
        get { return instance; }
    }

    void Awake() {
        instance = this;
    }

    private Text text;

    void Start() {
        text = GetComponent<Text>();
    }

    public void AddText(string text) {
        this.text.text = /*this.text.text + "\n" +*/ text;
    }

}
