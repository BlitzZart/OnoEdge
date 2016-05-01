using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour {
    public GameObject explosionParticles;

    private Player player; // TODO Use Base instead !?!?!?!
    private AudioSource audioSource;
    private PlayerSounds playerSounds;
    private HealthComponent healthComponent;

    private float moveSpeed = 2;
    private Transform target;

    // Use this for initialization
    void Start() {
        target = FindObjectOfType<Base>().transform;
        player = FindObjectOfType<Player>();

        audioSource = player.GetComponent<AudioSource>();
        playerSounds = player.GetComponent<PlayerSounds>();
        healthComponent = GetComponent<HealthComponent>();
    }

    // Update is called once per frame
    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    void OnDestroy() {
        if(audioSource != null)
            audioSource.PlayOneShot(playerSounds.explodeEnemy, 1.7f);

        //CameraShake.Instance.Shake();
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet")) {

            // take dmg
            healthComponent.Hit(5); // TODO: use proper value

            if (healthComponent.Hp > 0) { // hit
                if (audioSource != null)
                    audioSource.PlayOneShot(playerSounds.explodeEnemy, 1.7f);
                player.CmdDestroyObject(other.gameObject);
            } else { // dead
                player.CmdDestroyObject(other.gameObject);
                player.CmdDestroyObject(gameObject);
            }
        }

        if (other.tag == "Base") {
            player.CmdDestroyObject(gameObject);
        }
    }
}