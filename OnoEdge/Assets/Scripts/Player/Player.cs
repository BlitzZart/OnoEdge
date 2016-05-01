﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    private Gun gun;
    private AudioSource audioSource;
    private PlayerSounds playerSounds;



    // rotation of player
    private float gyroSpeed = 25.0f;
    private float keyboardRotationSpeed = 120.0f;

    // rotation of camera
    private Transform cameraMounting;
    private float cameraRotationSpeed = 19.0f;

    // rolling of player model
    private Transform bodyTransform;
    private float maxRoll = 61;
    private float rollThreshold = 0.1f;
    private float rollSpeed = 4.0f;

    public override void OnStartLocalPlayer() {
        GetComponentInChildren<PlayerStyle>().SetColor(1);
    }

    void Start() {
        gun = GetComponentInChildren<Gun>();
        audioSource = GetComponent<AudioSource>();
        playerSounds = GetComponent<PlayerSounds>();

        if (isLocalPlayer) {
            Input.gyro.enabled = true;
            cameraMounting = Camera.main.transform.parent;
            bodyTransform = GetComponentInChildren<PlayerOrbit>().transform;
        }

    }

	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;

        float rotationDirection = 0;

        // transform camera
        Vector3 inter = new Vector3(cameraMounting.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
        cameraMounting.rotation = Quaternion.Lerp(cameraMounting.rotation, Quaternion.Euler(inter), cameraRotationSpeed * Time.deltaTime);

        rotationDirection = transform.rotation.eulerAngles.z;
#if UNITY_EDITOR || UNITY_STANDALONE

        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.Rotate(0, 0, keyboardRotationSpeed * Time.deltaTime);
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            transform.Rotate(0, 0, -keyboardRotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            gun.Shoot();
        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        transform.rotation = Quaternion.Lerp
            (transform.rotation, Quaternion.Euler
            (new Vector3(transform.rotation.x, transform.rotation.y, Input.gyro.attitude.eulerAngles.z)), gyroSpeed * Time.deltaTime);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            gun.Shoot();
        }
#endif

        rotationDirection -= transform.rotation.eulerAngles.z;
        Roll(rotationDirection);
    }

    // roll ship
    private void Roll(float roll) {
        Vector3 inter;
        float speed = 1;
        if (Mathf.Abs(roll) > rollThreshold) {
            inter = new Vector3(0, maxRoll * Mathf.Sign(-roll), 0);
        }
        else {
            inter = Vector3.zero;
            speed = 17.0f;
        }

        //DebugText.Instance.AddText("° " + roll + " time " + Time.deltaTime.ToString() + " fps " + Time.renderedFrameCount);

        bodyTransform.localRotation = Quaternion.Lerp(bodyTransform.localRotation, Quaternion.Euler(inter), Time.deltaTime * speed);
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
        //Destroy(obj);
        NetworkServer.Destroy(obj);
    }
    //--------------------------------
    //--------------------------------
}