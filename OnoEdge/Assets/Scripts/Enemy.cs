using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour {
    public delegate void KilledEnemyDel(Enemy enemy, int playerNumber);
    public static event KilledEnemyDel KilledEnemyEvent;

    [SyncVar]
    private int killedByPlayerNumber;

    public GameObject explosionParticles;

    private Player player; // TODO Use Base instead !?!?!?!
    private AudioSource audioSource;
    private PlayerSounds playerSounds;
    private HealthComponent healthComponent;

    private float moveSpeed = 2;
    private Transform target;

    void Start() {
        target = FindObjectOfType<Base>().transform;
        player = FindObjectOfType<Player>();

        audioSource = player.GetComponent<AudioSource>();
        playerSounds = player.GetComponent<PlayerSounds>();
        healthComponent = GetComponent<HealthComponent>();
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    void OnDestroy() {
        if (KilledEnemyEvent != null)
            KilledEnemyEvent(this, killedByPlayerNumber);

        if (!NetworkManager.singleton.isNetworkActive)
            return; // don't spawn stuff if game shuts down

        if (audioSource != null)
            audioSource.PlayOneShot(playerSounds.explodeEnemy, 1.7f);

        //CameraShake.Instance.Shake();
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet")) {
            Bullet bullet = other.GetComponent<Bullet>();
            // take dmg
            healthComponent.Hit(bullet.damage);

            if (healthComponent.Hp > 0) { // hit
                if (audioSource != null)
                    audioSource.PlayOneShot(playerSounds.explodeEnemy, 1.7f);

                NetworkServer.Destroy(other.gameObject);
            } else { // dead
                CmdKilledEnemy(bullet.playerNumber);
                NW_GameLogic.Instance.DestroyedEnemy();
                NetworkServer.Destroy(other.gameObject);
                NetworkServer.Destroy(gameObject);
            }
        }

        if (other.tag == "Base") {
            CmdKilledEnemy(-1); ; // -1 means it hits the base
            NetworkServer.Destroy(gameObject);
        }
    }

    [Command]
    private void CmdKilledEnemy(int playerNumber) {
        killedByPlayerNumber = playerNumber;
    }
}