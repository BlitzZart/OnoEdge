using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStyle : MonoBehaviour {
    //private NetworkPlayer player;

    //private MeshRenderer[] meshes;

    public List<Color> colors;

    // Use this for initialization
    void Start () {
        //player = GetComponentInParent<NetworkPlayer>();
        //meshes = GetComponentsInChildren<MeshRenderer>();
    }

    public void SetColor(int id) {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in meshes) {
            mesh.material.color = colors[id % colors.Count];
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
