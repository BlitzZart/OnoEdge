using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NetworkManager_B : NetworkManager {

    public static int numberOfPlayers = 0;

    private static NetworkManager_B instance;
    public static NetworkManager_B Instance {
        get {
            if (instance == null)
                instance = instance.gameObject.GetComponent<NetworkManager_B>(); // wtf^^
            return instance;
        }
    }

    public GameObject enemyPrefab, baseBrefab;

    private float enemySpawnDistance = 150;
    private float spawnRate = 2;


    //this awake causes strange network errors / maybe because this class derives from NetworkManager(which also uses Awake() to init singleton)
    //void Awake() {
    //    instance = this;
    //}

    void Start() {
        instance = this;
    }

    public override void OnStartServer() {
        //Invoke("SpawnOnBegin", 0.5f);
        //InvokeRepeating("SpawnEnemy", spawnRate, spawnRate);
    }

    void SpawnOnBegin() {
        GameObject theBase = Instantiate(baseBrefab);
        NetworkServer.Spawn(theBase);
    }

    void SpawnEnemy() {
        GameObject enemy = Instantiate(
            enemyPrefab, 
            Random.insideUnitCircle * enemySpawnDistance,
            Quaternion.identity) as GameObject;

        NetworkServer.Spawn(enemy);
    }

    //public void SpawnObject(GameObject go) {
    //    NetworkServer.Spawn(go);
    //}

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

        float rate = spawnRate;

        if (numPlayers + offset > 0)
            rate = spawnRate / (numPlayers + offset);

        //InvokeRepeating("SpawnEnemy", spawnRate, rate);
    }


    public override void OnStopHost() {
        base.OnStopHost();

        // Stop enemy spawning
        CancelInvoke("SpawnEnemy");
    }

}
