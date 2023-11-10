using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smootSpeed = 0.03f;
    public Vector3 offset;

    private void Update()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smootSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }
}
