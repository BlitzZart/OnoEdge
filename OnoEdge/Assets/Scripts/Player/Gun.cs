using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public GameObject bulletPrefab;
    public GameObject nozzle;
    private float speed = 50;
    private Player player;

	// Use this for initialization
	void Start () {
        player = GetComponentInParent<Player>();
	}
	
    public void Shoot() {
        player.CmdFireBullet(nozzle.transform.position, transform.up, speed);
    }
}
