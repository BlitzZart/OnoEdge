using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NetworkManager_B : NetworkManager {
    private static NetworkManager_B instance;
    public static NetworkManager_B Instance {
        get {
            if (instance == null)
                instance = instance.gameObject.GetComponent<NetworkManager_B>(); // wtf^^
            return instance;
        }
    }

    public GameObject enemyPrefab, baseBrefab, gameLogicPrefab;

    public bool gameRunning = false;

    private float enemySpawnDistance = 150;
    private float stdSpawnRate = 3;
    private float spawnRate = 3;


    //this awake causes strange network errors / maybe because this class derives from NetworkManager(which also uses Awake() to init singleton)
    //void Awake() {
    //    instance = this;
    //}

    void OnLevelWasLoaded() {
        print("active scene: " + SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name.Equals("play")) {
            if (!NetworkServer.active)
                return;

            GameObject gl = Instantiate(gameLogicPrefab);
            NetworkServer.Spawn(gl);
        }
    }

    void Start() {
        instance = this;
    }

    public void StartGame() {
        gameRunning = true;
        Invoke("SpawnOnBegin", 0.5f);
        InvokeRepeating("SpawnEnemy", spawnRate, spawnRate);
    }

    void SpawnOnBegin() {
        if (!NetworkServer.active)
            return;
        GameObject theBase = Instantiate(baseBrefab);
        NetworkServer.Spawn(theBase);
    }

    void SpawnEnemy() {
        if (!NetworkServer.active)
            return;

        GameObject enemy = Instantiate(
            enemyPrefab, 
            Random.insideUnitCircle * enemySpawnDistance,
            Quaternion.identity) as GameObject;

        NetworkServer.Spawn(enemy);
    }

    public void SpawnObject(GameObject go) {
        NetworkServer.Spawn(go);
    }

    public override void OnServerConnect(NetworkConnection conn) {
        base.OnServerConnect(conn);
        AdjustRate(1);
    }

    public override void OnServerDisconnect(NetworkConnection conn) {
        base.OnServerDisconnect(conn);      
        AdjustRate(0);
    }

    /// <summary>
    /// enemy spawn rate relays to number of players
    /// </summary>
    /// <param name="offset">+1 on connect 0 on disconnect</param>
    private void AdjustRate(int offset) {
        CancelInvoke("SpawnEnemy");

        Player[] players = FindObjectsOfType<Player>();

        spawnRate = stdSpawnRate;

        if (numPlayers + offset > 0)
            spawnRate = stdSpawnRate / (numPlayers + offset);

        if (!gameRunning)
            return;

        InvokeRepeating("SpawnEnemy", stdSpawnRate, spawnRate);
    }


    public override void OnStopHost() {
        base.OnStopHost();

        // Stop enemy spawning
        CancelInvoke("SpawnEnemy");
    }

}
