using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugText : MonoBehaviour {
    private static DebugText instance;
    public static DebugText Instance {
        get { return instance; }
    }

    private Text text;

    void Awake() {
        instance = this;
        text = GetComponent<Text>();
    }



    public void AddText(string text) {
        if (this.text == null)
            return;

        this.text.text = /*this.text.text + "\n" +*/ text;
    }
}
