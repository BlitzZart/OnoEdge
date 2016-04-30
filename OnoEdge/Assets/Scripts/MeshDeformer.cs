using System.Collections.Generic;
using UnityEngine;

public class MeshDeformer : MonoBehaviour {
    static Vector3 unInitVec = new Vector3(-9999.1f, -9999.2f, -9999.3f); // TODO: make clean initialisation - this init value is just a hack
    class Vertex {
        public Vector3 tVertex;
        public Vector3 oVertex;
        public Vector3 normal;
        public List<int> indices;

        public Vertex() {
            indices = new List<int>();
            tVertex = new Vector3(unInitVec.x, unInitVec.y, unInitVec.z);
        }

        public Vertex(Vector3 vertex, Vector3 normal, int index) {
            this.tVertex = vertex;
            this.oVertex = vertex;
            this.normal = normal;

            indices = new List<int>();
            indices.Add(index);
        }

        public void AddIndex(int index, Vector3 normal) {
            indices.Add(index);
            this.normal = (this.normal + normal) / 2;
        }

        public override string ToString() {
            string text = "@ Vertex: ";
            text += tVertex + "\n";
            text += " Indices: ";
            foreach (int item in indices) {
                text += item + " ";
            }
            return text;
        }
    }

    class VertexList {
        public List<Vertex> vertices;
        public VertexList() {
            vertices = new List<Vertex>();
        }

        public void Add(Vertex vertex) {
            vertices.Add(vertex);
        }

        public void TryAddIndex(Vector3 vertex, Vector3 normal, int index) {
            foreach (Vertex item in vertices)
                if (item.tVertex == vertex)
                    item.AddIndex(index, normal);

        }

        public bool Contains(Vector3 vertex) {
            foreach (Vertex item in vertices)
                if (item.tVertex == vertex)
                    return true;
            return false;
        }

        public override string ToString() {
            string text = "Length " + vertices.Count + "  \n";
            foreach (Vertex item in vertices) {
                text += item.ToString();
            }
            return text;
        }
    }

    public bool solidDeformation = true;
    bool positiveDeformation = true;

    Mesh mesh;
    Vector3[] targetVertices;
    Vector3[] oV;


    Vertex[] unique;
    VertexList uList;

    float[] offsets;

    [Header("Overall power of deformation")]
    [Range(0.0f, 10)]
    public float power = 1.0f;
    [Header("Influence of FFT")]
    [Range(0.0f, 2)]
    public float fftPower = 1.0f;
    [Header("Noise floor")]
    [Range(0.0f, 10.0f)]
    public float randomPower = 1.0f;
    [Header("Speed of linear interpolation")]
    [Range(0.0f, 25.0f)]
    public float speed = 1.0f;
    [Header("Update rate of animation")]
    [Range(0.0f, 1.0f)]
    public float rate = 0.1f; // for music visualization beat detection would be nice


    private float defaultVibration = 0.02f;

    // Use this for initialization
    void Start() {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.MarkDynamic();

        // prepare datastructure for solid mesh deformation
        List<Vector3> rV = new List<Vector3>();
        rV.Add(mesh.vertices[0]);
        for (int i = 1; i < mesh.vertices.Length; i++) {
            if (!rV.Contains(mesh.vertices[i])) {
                rV.Add(mesh.vertices[i]);
            }
        }
        uList = new VertexList();
        for (int i = 0; i < mesh.vertexCount; i++) {
            if (!uList.Contains(mesh.vertices[i])) {
                uList.Add(new Vertex(mesh.vertices[i], mesh.normals[i], i));
            }
            else {
                uList.TryAddIndex(mesh.vertices[i], mesh.normals[i], i);
            }
        }
        unique = uList.vertices.ToArray();

        //print(uList.ToString());

        oV = mesh.vertices;
        targetVertices = mesh.vertices;
        offsets = new float[] { 0 };
    }

    public void ArrayOffests(float[] offsets) {
        this.offsets = offsets;
    }

    private float current = 0;
    // Update is called once per frame
    void Update() {
        if (current < rate) {
            current += Time.deltaTime;
        }
        else {
            current = 0;

            if (solidDeformation)
                UpdateMeshSolid();
            else
                UpdateMeshBreaking();
        }

        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < oV.Length; i++) {
            //if (positiveDeformation & targetVertices[i].sqrMagnitude < 0)
            //    targetVertices[i] *= -1;
            vertices[i] = Vector3.Lerp(vertices[i], targetVertices[i], Time.deltaTime * speed);
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }

    void UpdateMeshBreaking() {
        for (int i = 0; i < oV.Length; i++) {
            targetVertices[i] = oV[i] + mesh.normals[i] * Random.Range(-randomPower, randomPower) * power *
                (defaultVibration * Mathf.Clamp01(power) + offsets[i % offsets.Length] * fftPower);
        }
    }

    void UpdateMeshSolid() {
        int i = 0;
        foreach (Vertex item in unique) {
            item.tVertex = item.oVertex + item.normal * Random.Range(-randomPower, randomPower) * power *
                    (defaultVibration * Mathf.Clamp01(power) + offsets[i++ % offsets.Length] * fftPower);

            foreach (int index in item.indices) {
                targetVertices[index] = item.tVertex;
            }
        }
    }
}