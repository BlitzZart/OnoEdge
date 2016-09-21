using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;

public class TargetFinder : MonoBehaviour {
    private Dictionary<Transform, LineRenderer> targetLines;
    public Player player;
    public GameObject laserLinePrefab;

    #region unity callbacks
    void Start() {
        targetLines = new Dictionary<Transform, LineRenderer>();
        Enemy.KilledEnemyEvent += OnKilledEnemy;
    }

    void Update() {
        DebugText.Instance.AddText(targetLines.Count.ToString());
        UpdateLines();
    }

    void OnTriggerEnter(Collider other) {
        if (!player.isLocalPlayer)
            return;

        if (!targetLines.ContainsKey(other.transform))
            AddTarget(other.transform);
    }

    void OnTriggerExit(Collider other) {
        RemoveTarget(other.transform);
    }

    void OnDestroy() {
        Enemy.KilledEnemyEvent -= OnKilledEnemy;
    }
    #endregion;

    #region delegates
    private void OnKilledEnemy(Enemy enemy, int playerNumber) {
        RemoveTarget(enemy.transform);
        //print("KILLER " + playerNumber);
    }

    #endregion

    #region private
    private void AddLine(Transform target) {
        GameObject go = Instantiate(laserLinePrefab);
        go.transform.SetParent(transform);
    }

    void UpdateLines() {
        foreach (KeyValuePair<Transform, LineRenderer> item in targetLines) {
            item.Value.SetPosition(0, item.Key.position);
            item.Value.SetPosition(1, transform.position);
        }
    }

    private void AddTarget(Transform target) {
        LineRenderer lr = Instantiate(laserLinePrefab).GetComponent<LineRenderer>();
        //lr.transform.SetParent(transform);
        targetLines.Add(target, lr);
    }

    private void RemoveTarget(Transform target) {
        LineRenderer lr;
        if (targetLines.TryGetValue(target, out lr) == false)
            return; // already destroyed

        Destroy(lr.gameObject);
        targetLines.Remove(target);
    }
    #endregion
}