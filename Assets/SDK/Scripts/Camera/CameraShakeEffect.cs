using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeEffect : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform = default;
    [SerializeField] private float shakeFrequency = default;

    private Vector3 originalPosition = default;
    public static bool ActivateCameraShake;

    void Awake()
    {
        originalPosition = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (ActivateCameraShake)
        {
            CameraShake();
        }
    }

    private void CameraShake()
    {
        cameraTransform.localPosition += Random.insideUnitSphere * shakeFrequency;
    }
}
