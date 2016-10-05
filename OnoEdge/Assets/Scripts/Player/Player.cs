using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class Player : NetworkBehaviour, IFireBullet {
    [SyncVar]
    public bool playerIsReady = false;

    [SyncVar(hook = "PlayerNumberChanged")]
    public int playerNumber = 0;

    [SyncVar]
    public float rotationZ = 0;

    private Base playerBase;
    private Gun gun;
    private Shield shield;
    private AudioSource audioSource;
    private PlayerSounds playerSounds;

    private int dimension = 2;
    /// <summary>
    /// set dimension (1-3)
    /// </summary>
    public int Dimension {
        get {
            return dimension;
        }
        set {
            if (value < 1 || value > 3)
                return;
            dimension = value;
        }
    }

    public LobbyPlayerEntry lobbyEntryPrefab;

    //// player activation
    //private bool activated = false;
    //public bool Activated {
    //    get {
    //        return activated;
    //    }
    //}

    // rotation of player
    private float gyroSpeed = 25.0f;
    private float keyboardRotationSpeed = 90.0f;
    private float precisionMoveFactor = 0.5f;

    // rotation of camera
    private Transform cameraMounting;
    private float cameraRotationSpeed = 19.0f;

    // rolling of player model
    private Transform bodyTransform;
    private float maxRoll = 61;
    private float rollThreshold = 0.1f;
    private float rollSpeed = 4.0f;

    #region unity callbacks
    void Start() {
        DontDestroyOnLoad(gameObject);

        NW_GameLogic.ChangeDimensionEvent += OnChangeDimension;
        //NW_GameLogic.GameStartedEvent += OnGameStarted;

        gun = GetComponentInChildren<Gun>();
        shield = GetComponentInChildren<Shield>();
        audioSource = GetComponent<AudioSource>();
        playerSounds = GetComponent<PlayerSounds>();

        if (isLocalPlayer) {
            Input.gyro.enabled = true;
            cameraMounting = Camera.main.transform.parent;

            //transform.SetParent(cameraMounting);

            bodyTransform = GetComponentInChildren<PlayerOrbit>().transform;
            CmdCountPlayers();

        }
        CreateUIEntry(isLocalPlayer);

        //gameObject.SetActive(false);
    }

    void Update () {
        FollowBase();

        if (isLocalPlayer) {
            LocalPlayerMovement();
            CmdSetRotation(transform.localRotation.eulerAngles.z);
        } else {
            OtherPlayerMovement();
        }
    }

    void OnDestroy() {
        if (Lobby.Instance)
            Lobby.Instance.RemovePlayer(this);


        NW_GameLogic.ChangeDimensionEvent -= OnChangeDimension;
        //NW_GameLogic.GameStartedEvent -= OnGameStarted;
    }
    #endregion

    #region delegates
    private void OnChangeDimension(int value) {
        dimension = value;
    }
    //private void OnGameStarted() {
    //}
    #endregion

    #region public 
    public void Activate() {
        gameObject.SetActive(true);
        //activated = true;
        cameraMounting = Camera.main.transform.parent;
    }
    public void Deactivate() {
        gameObject.SetActive(false);
        //activated = false;
    }

    public void AssignBase(Base b) {
        playerBase = b;
    }

    public void CreateUIEntry(bool isLocal) {
        GameObject go = Instantiate(lobbyEntryPrefab.gameObject);
        lobbyEntryPrefab = go.GetComponent<LobbyPlayerEntry>();
        if (lobbyEntryPrefab == null)
            return;

        lobbyEntryPrefab.player = this;
        lobbyEntryPrefab.playerNumber = playerNumber;

        if (isLocal) {
            lobbyEntryPrefab.localPlayer = true;
        }

        Lobby.Instance.AddPlayer(this);
    }
    #endregion

    #region private
    private void LocalPlayerMovement() {

        float rotationDirection = 0;

        //FollowBase(); // do @ both, local player and other clients
        MoveCamera();
        //// transform camera 2D
        //Vector3 inter = new Vector3(cameraMounting.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
        //cameraMounting.rotation = Quaternion.Lerp(cameraMounting.rotation, Quaternion.Euler(inter), cameraRotationSpeed * Time.deltaTime);

        rotationDirection = transform.rotation.eulerAngles.z;
//#if UNITY_EDITOR || UNITY_STANDALONE

        float moveBy = keyboardRotationSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            moveBy *= precisionMoveFactor;


        if (dimension >= 2)
            if (Input.GetKey(KeyCode.LeftArrow)) {
                transform.Rotate(0, 0, moveBy * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.RightArrow)) {
                transform.Rotate(0, 0, -moveBy * Time.deltaTime);
            }

        if (dimension >= 3)
            if (Input.GetKey(KeyCode.UpArrow)) {
                transform.Rotate(-moveBy * Time.deltaTime, 0, 0);
            }
            else if (Input.GetKey(KeyCode.DownArrow)) {
                transform.Rotate(moveBy * Time.deltaTime, 0, 0);
            }


        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            gun.Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            shield.SetShieldState(true);
            CmdSetShieldState(true);
        } else if (Input.GetKeyUp(KeyCode.Space)) {
            shield.SetShieldState(false);
            CmdSetShieldState(false);
        }
//#endif

#if UNITY_ANDROID && !UNITY_EDITOR // do // && !UNITY_EDITOR to test with remote
        if (dimension == 2) {
            transform.rotation = Quaternion.Lerp
                (transform.rotation, Quaternion.Euler
                (new Vector3(transform.rotation.x, transform.rotation.y, Input.gyro.attitude.eulerAngles.z)), gyroSpeed * Time.deltaTime);
        }
        else
        if (dimension == 3) {
            transform.rotation = Quaternion.Lerp
                (transform.rotation, Quaternion.Euler
                (new Vector3(transform.rotation.x, -Input.gyro.attitude.eulerAngles.y, Input.gyro.attitude.eulerAngles.z)), gyroSpeed * Time.deltaTime);
        }


        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            gun.Shoot();
        }
#endif

        rotationDirection -= transform.rotation.eulerAngles.z;
        Roll(rotationDirection);
    }

    private void OtherPlayerMovement() {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rotationZ), Time.deltaTime * 17);
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

    private void FollowBase() {
        if (playerBase != null)
            transform.localPosition = playerBase.transform.position;
    }

    private void MoveCamera() {
        if (cameraMounting == null)
            return;

        cameraMounting.rotation = Quaternion.Lerp(cameraMounting.rotation, this.transform.rotation, Time.deltaTime * 10);
        cameraMounting.position = transform.position;
    }
    #endregion

    //--------------------------------
    //---- Network Communication -----
    //--------------------------------
    #region unet
    // [SyncVar] hook -> playerNumber
    void PlayerNumberChanged(int value) {
        GetComponentInChildren<PlayerStyle>().SetColor(playerNumber);

        playerNumber = value;
        UI_LobbyList lobbyList = FindObjectOfType<UI_LobbyList>();
        if (lobbyList == null)
            return;
        lobbyList.UpdateEntries();
    }

    [Command]
    private void CmdSetShieldState(bool state) {
        RpcSetShieldState(state);
    }
    [ClientRpc]
    private void RpcSetShieldState(bool state) {
        if (localPlayerAuthority)
            return;
        shield.SetShieldState(state);
    }

    [Command]
    private void CmdSetRotation(float rotation) {
        rotationZ = rotation;
    }

    [Command]
    private void CmdCountPlayers() {
        playerNumber = GameData.Instance.numberOfPlayers++;
    }

    [Command]
    public void CmdFireBullet(Vector3 pos, Vector3 dir, float speed) {
        GameObject bullet = Instantiate(gun.bulletPrefab, pos, transform.rotation) as GameObject;
        bullet.GetComponent<Rigidbody>().velocity = dir * speed;
        NetworkServer.Spawn(bullet);
        RpcSpawnBullet();

        bullet.GetComponent<Bullet>().playerNumber = playerNumber;
    }

    [ClientRpc]
    public void RpcSpawnBullet() {
        audioSource.PlayOneShot(playerSounds.shoot);
    }

    [Command]
    public void CmdDestroyObject(GameObject obj) {
        NetworkServer.Destroy(obj);
    }

    [Command]
    public void CmdPlayerReady() {
        playerIsReady = !playerIsReady;
    }
    //--------------------------------
    //--------------------------------
    #endregion
}