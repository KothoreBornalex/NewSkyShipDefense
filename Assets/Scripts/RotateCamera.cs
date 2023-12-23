using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    private float rotationSpeed = 0f;
    public float smoothness = 5.0f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Stationary:
                    rotationSpeed = 0.0f;
                    break;

                case TouchPhase.Began:
                    break;

                case TouchPhase.Moved:
                    // Adjust the rotation speed based on touch delta position
                    rotationSpeed += touch.deltaPosition.x * Time.deltaTime * 3.5f;
                    break;

                case TouchPhase.Ended:
                    break;
            }
        }

        // Gradually decrease rotation speed for smooth deceleration
        rotationSpeed = Mathf.Lerp(rotationSpeed, 0f, Time.deltaTime * smoothness);

        // Rotate the object based on the rotation speed
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
