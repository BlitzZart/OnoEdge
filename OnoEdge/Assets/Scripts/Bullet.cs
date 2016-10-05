using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
    public GameObject hitParticles;
    public float lifeTime = 5;
    public int damage = 5;
    // player number == -1 indicats enemy bullets
    [SyncVar]
    public int playerNumber;

    #region unity callbacks
    // Use this for initialization
    void Start() {
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, lifeTime);
        StartCoroutine(ShowDelayed());
    }

    void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
    }

    void OnDestroy() {
        Instantiate(hitParticles, transform.position, Quaternion.identity);
    }

    #endregion

    // TODO: this is a workaround. Bullets show up rotated on clients.
    IEnumerator ShowDelayed() {
        yield return new WaitForSeconds(0.04f);
        GetComponent<MeshRenderer>().enabled = true;
    }
}