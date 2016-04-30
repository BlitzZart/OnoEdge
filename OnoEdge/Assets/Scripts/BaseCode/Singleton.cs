using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    private static T instance;
    public static T Instance;

    void Awake () {
        instance = GetComponent<T>();
	}
}
