using Unity.Cinemachine;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    private CinemachineBasicMultiChannelPerlin cinemachineCameraPerlin;

    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    private void Awake()
    {
        Instance = this;

        cinemachineCameraPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        cinemachineCameraPerlin.AmplitudeGain = intensity;

        shakeTimer = time;
        shakeTimerTotal = time;
        startingIntensity = intensity;
    }

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            cinemachineCameraPerlin.AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
        }
    }
}
