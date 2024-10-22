using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    private Light flickerLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;

    private void Awake()
    {
        flickerLight = GetComponent<Light>();
    }

    private void Update()
    {
        flickerLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * flickerSpeed, 0f));
    }
}
