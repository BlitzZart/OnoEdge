using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetFinder : MonoBehaviour {
    private Dictionary<Transform, LineRenderer> targetLines;


    public GameObject laserLinePrefab;

    


    #region unity callbacks
    void Start() {
        targetLines = new Dictionary<Transform, LineRenderer>();
    }

    void Update() {
        DebugText.Instance.AddText(targetLines.Count.ToString());
        UpdateLines();
    }
	
    void OnTriggerEnter(Collider other) {
        if (!targetLines.ContainsKey(other.transform))
            AddTarget(other.transform);
    }

    void OnTriggerExit(Collider other) {
        //if (enemies.Contains(other.transform)) no check needed
        RemoveTarget(other.transform);
    }
    #endregion;

    #region private
    private void AddLine(Transform target) {
        GameObject go = Instantiate(laserLinePrefab);
        go.transform.SetParent(transform);

        LineRenderer lr = go.GetComponent<LineRenderer>();
        

    }
    
    private void RemoveLine(Transform target) {

    }

    void UpdateLines() {
        foreach (KeyValuePair<Transform, LineRenderer> item in targetLines) {
            if (item.Key != null) // TODO: get rid of those null guys
                {
                item.Value.SetPosition(0, item.Key.position);
                item.Value.SetPosition(1, transform.position);
            }


        }

        // was list iteration
        //for (int i = 0; i < targetLines.Count; i++) {
        //    targetLines[i].Value.SetPosition(0, targetLines[i].Key.position);
        //    targetLines[i].Value.SetPosition(1, transform.position);
        //}
    }

    private void AddTarget(Transform target) {


        LineRenderer lr = Instantiate(laserLinePrefab).GetComponent<LineRenderer>();
        //lr.transform.SetParent(transform);
        targetLines.Add(target, lr);

        //targets.Add(target);
        //AddLine(target);
    }
    private void RemoveTarget(Transform target) {
        LineRenderer lr;
        targetLines.TryGetValue(target, out lr);

        Destroy(lr.gameObject);

        targetLines.Remove(target);

        //targets.Remove(target);
        //RemoveLine(target);
    }
    #endregion

}
