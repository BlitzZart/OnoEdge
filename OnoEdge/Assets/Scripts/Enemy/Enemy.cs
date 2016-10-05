using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

/// <summary>
/// simple enemy base class
/// </summary>
public class Enemy : NetworkBehaviour, IFireBullet {
    public delegate void KilledEnemyDel(Enemy enemy, int playerNumber);
    public static event KilledEnemyDel KilledEnemyEvent;

    [SyncVar]
    private int killedByPlayerNumber;

    public GameObject explosionParticles;

    private Gun gun;

    private Player player; // TODO Use Base instead !?!?!?!
    private AudioSource audioSource;
    private PlayerSounds playerSounds;
    private HealthComponent healthComponent;

    private float moveSpeed = 3;
    [HideInInspector]
    public Transform target;

    #region unity callbacks
    void Start() {
        target = FindObjectOfType<Base>().transform;
        player = FindObjectOfType<Player>();

        gun = GetComponentInChildren<Gun>();

        audioSource = GetComponent<AudioSource>();
        playerSounds = player.GetComponent<PlayerSounds>();
        healthComponent = GetComponent<HealthComponent>();
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(target.position);
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
            }
            else { // dead
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
    #endregion

    #region unet
    [Command]
    private void CmdKilledEnemy(int playerNumber) {
        killedByPlayerNumber = playerNumber;
    }
    [Command]
    public void CmdFireBullet(Vector3 pos, Vector3 dir, float speed) {
        Vector3 direction;
        if (dir != Vector3.zero)
            direction = dir;
        else
            direction = (target.position - transform.position).normalized;

        GameObject bullet = Instantiate(gun.bulletPrefab, pos, transform.rotation) as GameObject;
        bullet.GetComponent<Rigidbody>().velocity = direction * speed;

        NetworkServer.Spawn(bullet);
        RpcSpawnBullet();

        bullet.GetComponent<Bullet>().playerNumber = -1;
    }
    [ClientRpc]
    public void RpcSpawnBullet() {
        audioSource.PlayOneShot(playerSounds.shoot);
    }
    #endregion
}