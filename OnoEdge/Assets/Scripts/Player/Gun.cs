using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public GameObject bulletPrefab;
    public GameObject nozzle;
    private float speed = 50;
    private Player player;
    public Player Player {
        get {
            return player;
        }
    }

    // Use this for initialization
    void Start () {
        player = GetComponentInParent<Player>();
	}
	
    public void Shoot() {
        //if (player.Activated)
            player.CmdFireBullet(nozzle.transform.position, transform.up, speed);
    }
}
