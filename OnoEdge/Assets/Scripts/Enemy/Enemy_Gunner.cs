using UnityEngine;
using System.Collections;

public class Enemy_Gunner : MonoBehaviour {

    public float shootRate = 3;
    private Gun gun;

    #region unity callbacks
    void Start() {
        gun = GetComponent<Gun>();
        InvokeRepeating("Shoot", shootRate, shootRate);
    }
    #endregion

    #region private
    private void Shoot() {
        gun.Shoot(Vector3.zero);
    }
    #endregion
}
