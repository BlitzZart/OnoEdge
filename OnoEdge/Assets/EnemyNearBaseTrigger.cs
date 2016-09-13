using UnityEngine;
using System.Collections;

public class EnemyNearBaseTrigger : MonoBehaviour {
    private bool enemiesInside = false;
    private SphereCollider coll;

    int layerMask;

    private float checkRate = 0.25f;

    void Start() {
        coll = GetComponent<SphereCollider>();
        InvokeRepeating("CyclicChecker", checkRate, checkRate);

        // 11 is enemy
        layerMask = 1 << 11;
        layerMask = ~layerMask;
    }

    //void OnDrawGizmos() {
    //    Gizmos.color = Color.red;
    //    print("rad " + coll.radius);
    //    Gizmos.DrawWireSphere(coll.center, coll.radius * coll.transform.localScale.x);
    //}

    void CyclicChecker() {
        // only check if enemies are inside
        if (!enemiesInside)
            return;

        enemiesInside = Physics.CheckSphere(coll.center, coll.radius * coll.transform.localScale.x, layerMask);

        //print("CC " + enemiesInside);
        if (!enemiesInside) {
            SendMessageUpwards("MsgColliderEmpty", SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Enemy>() != null) {
            enemiesInside = true;
            SendMessageUpwards("MsgEnemyEntered", SendMessageOptions.DontRequireReceiver);
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.GetComponent<Enemy>() != null) {
            enemiesInside = Physics.CheckSphere(coll.center, coll.radius * coll.transform.localScale.x, layerMask);
            SendMessageUpwards("MsgEnemyLeft", enemiesInside, SendMessageOptions.DontRequireReceiver);
        }
    }
}
