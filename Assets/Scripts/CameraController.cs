using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public float shakeAmount = 0.7f;
    [SerializeField]
    public float shakeDuration = 0.5f;


    [SerializeField]
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Shake();
        }
    }

    public void Shake()
    {
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", shakeDuration);
    }

    void BeginShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = initialPosition + Random.insideUnitSphere * shakeAmount;
            camPos.z = -10;
            transform.position = camPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
        transform.position = initialPosition;
    }
}
