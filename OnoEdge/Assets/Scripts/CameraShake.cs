using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;

    public static CameraShake Instance { get { return CameraShake.instance; } }

    private bool isShaking = false;
    private bool isPermaShakeing = false;
    private Vector3 originPosition;
    //private Quaternion originRotation;

    [HideInInspector]
    public float shake_decay;

    [HideInInspector]
    public float shake_intensity;

    void Start()
    {
        originPosition = Camera.main.transform.position;
    }

    void Awake()
    {
        CameraShake.instance = this;
    }

    void Update()
    {
        if (isPermaShakeing)
            UpdatePermaShake();
        else
            UpdateShotShake();
    }

    void UpdateShotShake()
    {
        if (shake_intensity > 0)
        {
            float newX = originPosition.x + Random.Range(0, shake_intensity / 2.0f); // half in x axis
            float newY = originPosition.y + Random.Range(0, shake_intensity);
            Camera.main.transform.position = new Vector3(newX, newY, Camera.main.transform.position.z);

            shake_intensity -= shake_decay;
        }
        else if (isShaking)
        {
            Camera.main.transform.position = originPosition;
            isShaking = false;
        }
    }

    void UpdatePermaShake()
    {
        float newX = originPosition.x + Random.Range(0, shake_intensity);
        float newY = originPosition.y + Random.Range(0, shake_intensity);
        Camera.main.transform.position = new Vector3(newX, newY, Camera.main.transform.position.z);
    }

    public void StartPermaShake(float intensity = 0.05f)
    {
        shake_intensity = intensity; // .2f;
        shake_decay = 0.04f;
        isPermaShakeing = true;
    }

    public void StopPermaShake()
    {
        Camera.main.transform.position = originPosition;
        isPermaShakeing = false;
    }

    public void Shake(float intensity = 0.05f, float decay = 0.01f)
    {
        //var not used
        if (!isShaking)
        {
            isShaking = true;
        }

        if (intensity > shake_intensity)
            shake_intensity = intensity; // .2f;
        shake_decay = decay; // 0.012f;
    }
}
