using UnityEngine;
using System.Collections;

public class ApplyRotation : MonoBehaviour {
    float rotationSpeed = -40;
	
	// Update is called once per frame
	void Update () {
        float rot = rotationSpeed * Time.deltaTime;
        transform.Rotate(rot, rot, rot);
    }
}
