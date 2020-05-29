using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform target;            // The position that that camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.

    Vector3 offset;                     // The initial offset from the target.

    public Transform camtarg;

    void Start()
    {
        // Calculate the initial offset.
        offset = transform.position - target.position;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        // Create a postion the camera is aiming for based on the offset from the target.
        Vector3 targetCamPos = target.position + offset;

        // Make the camera look at the player
        transform.LookAt(target.position);


        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp(transform.position, camtarg.position, smoothing * Time.deltaTime);
    }
}
