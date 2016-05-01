using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

    private float lifeTime = 5;

    // Use this for initialization
    void Start () {
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, lifeTime);
        StartCoroutine(ShowDelayed());
	}

    // TODO: this is a workaround. Bullets show up rotated on clients.
    IEnumerator ShowDelayed() {
        yield return new WaitForSeconds(0.04f);
        GetComponent<MeshRenderer>().enabled = true;
    }
    

    void OnDestroy() {
        // make some noise
    }
}
