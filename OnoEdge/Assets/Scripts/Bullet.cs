using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

    private float lifeTime = 5;

    // Use this for initialization
    void Start () {
        Destroy(gameObject, lifeTime);
	}
    void OnDestroy() {
        // make some noise
    }
}
