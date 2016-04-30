using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    private Gun gun;
    private AudioSource audioSource;
    private PlayerSounds playerSounds;

    private float rotationSpeed = 25.0f;

    public override void OnStartLocalPlayer() {
        GetComponentInChildren<PlayerStyle>().SetColor(1);
    }

    void Start() {
        gun = GetComponentInChildren<Gun>();
        audioSource = GetComponent<AudioSource>();
        playerSounds = GetComponent<PlayerSounds>();

        if (isLocalPlayer) {
            Input.gyro.enabled = true;
            Camera.main.transform.parent = transform;
        }

    }

	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.Rotate(0, 0, 120.0f * Time.deltaTime);
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            transform.Rotate(0, 0, -120.0f * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            gun.Shoot();
        }
#endif
        DebugText.Instance.AddText("X: " + Input.gyro.attitude.eulerAngles.x);
#if UNITY_ANDROID && !UNITY_EDITOR
        transform.rotation = Quaternion.Lerp
            (transform.rotation, Quaternion.Euler
            (new Vector3(transform.rotation.x, transform.rotation.y, Input.gyro.attitude.eulerAngles.z)), rotationSpeed * Time.deltaTime);
        //transform.Rotate(new Vector3(0, 0, Input.gyro.attitude.eulerAngles.z) * Time.deltaTime);

        if (Input.GetTouch(0).phase == TouchPhase.Began) {
            gun.Shoot();
        }
#endif

    }

    //--------------------------------
    //---- Network Communication -----
    //--------------------------------
    [Command]
    public void CmdFireBullet(Vector3 pos, Vector3 dir, float speed) {
        GameObject bullet = Instantiate(gun.bulletPrefab, pos, transform.rotation) as GameObject;
        bullet.GetComponent<Rigidbody>().velocity = dir * speed;
        NetworkServer.Spawn(bullet);
        RpcPlayShootSound();
    }

    [ClientRpc]
    public void RpcPlayShootSound() {
        audioSource.PlayOneShot(playerSounds.shoot);
    }

    [Command]
    public void CmdDestroyObject(GameObject obj) {
        Destroy(obj);
    }
}
