using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraSetup : MonoBehaviour {
    private static CameraSetup instance;
    public static CameraSetup Instance {
        get {
            return instance;
        }
    }

    // 0,5,-1
    private Vector3 fov = new Vector3(10, 70, 83);

    private List<Vector3> pos = new List<Vector3>();

    #region unity callbacks
    void Awake() {
        instance = this;
    }

    void Start() {

    }
    #endregion

    #region public
    public void ChangeDimension(int dimension) {
        if (dimension < 1 || dimension > 3)
            return;

        // set FOV
        Camera.main.fieldOfView = fov[dimension - 1];

        // set position
        InitCamPositions();
        Camera.main.transform.localPosition = pos[dimension - 1];


        //switch (dimension) {
        //    case (1):
        //        break;
        //    case (2):
        //        break;
        //    case (3):
        //        break;
        //    default:
        //        break;
        //}
    }
    #endregion

    #region private
    private void InitCamPositions() {
        if (pos.Count == 0) {
            pos = new List<Vector3>();
            pos.Add(new Vector3(0, -4, -10));
            pos.Add(new Vector3(0, -4, -10));
            pos.Add(new Vector3(0, 1, -6));
        }
    }
    #endregion
}
