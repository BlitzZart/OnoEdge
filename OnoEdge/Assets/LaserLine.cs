using UnityEngine;
using System.Collections;

public class LaserLine : MonoBehaviour {
    private MeshRenderer laserDot;
    private LineRenderer laserLine;
    
	// Use this for initialization
	void Start () {
        laserLine = GetComponentInChildren<LineRenderer>();
        laserDot = GetComponentInChildren<MeshRenderer>();
    }
    RaycastHit hitInfo;
	// Update is called once per frame
	void Update () {
        Physics.Raycast(transform.position, transform.up, out hitInfo, 256, LayerMask.GetMask("Enemy"));

        if (hitInfo.collider != null) {
            float distance = Vector3.Distance(hitInfo.point, transform.position - transform.up * 2);
            laserLine.SetPosition(0, Vector3.up * distance);

            laserDot.enabled = true;
            laserDot.transform.position = hitInfo.point;
        }
        else {
            laserLine.SetPosition(0, Vector3.up * 256);
            laserDot.enabled = false;
        }


        //Debug.DrawRay(transform.position, transform.up * 256, Color.green);
    }
}
