using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkManager_B : NetworkManager {

    public GameObject enemyPrefab, baseBrefab;

    private float enemySpawnDistance = 150;

    public override void OnStartServer() {
        Invoke("SpawnOnBegin", 0.5f);
        InvokeRepeating("SpawnEnemy", 1, 2);
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
}
