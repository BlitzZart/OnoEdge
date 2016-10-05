using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public GameObject bulletPrefab;
    public GameObject nozzle;
    private float speed = 50f;
    private IFireBullet player;
    public IFireBullet Player {
        get {
            return player;
        }
    }

    // Use this for initialization
    void Start () {
        player = GetComponentInParent<IFireBullet>();
	}

    public void Shoot() {
        Shoot(transform.up);
    }
    public void Shoot(Vector3 direction) {
        player.CmdFireBullet(nozzle.transform.position, direction, speed);
    }
}
