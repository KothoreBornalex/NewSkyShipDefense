using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform cameraTransform; // The main camera's transform
    public float swayAmount = 1.0f; // Adjust this value to control the intensity of the sway
    public float swaySpeed = 1.0f; // Adjust this value to control the speed of the sway
    public Vector2 swayDirection = new Vector2(1.0f, 1.0f); // Adjust these values to control the direction of the sway
    public float rotationAmountX = 10.0f; // Adjust this value to control the X-axis rotation
    public float rotationAmountY = 5.0f; // Adjust this value to control the Y-axis rotation
    public float rotationAmountZ = 0.0f; // Adjust this value to control the Z-axis rotation
    public float rotationSpeed = 2.0f; // Adjust this value to control the speed of the rotation

    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;
    private float timeCounter = 0.0f;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        initialCameraPosition = cameraTransform.localPosition;
        initialCameraRotation = cameraTransform.localRotation;
    }

    void Update()
    {
        // Calculate the sway motion using Perlin noise
        float xOffset = Mathf.PerlinNoise(timeCounter, 0) * 2.0f - 1.0f;
        float yOffset = Mathf.PerlinNoise(0, timeCounter) * 2.0f - 1.0f;

        // Calculate the rotation using Perlin noise
        float xRotation = Mathf.PerlinNoise(timeCounter, 0) * 2.0f - 1.0f;
        float yRotation = Mathf.PerlinNoise(0, timeCounter) * 2.0f - 1.0f;
        float zRotation = Mathf.PerlinNoise(timeCounter * 2, timeCounter * 2) * 2.0f - 1.0f;

        // Apply the sway and rotation to the camera's local position and rotation
        Vector3 sway = new Vector3(xOffset, yOffset, 0) * swayAmount;
        Quaternion rotation = Quaternion.Euler(xRotation * (rotationAmountX * Mathf.Cos(Time.time)), yRotation * (rotationAmountY * Mathf.Cos(Time.time)), zRotation * (rotationAmountZ * Mathf.Cos(Time.time))) * initialCameraRotation;

        cameraTransform.localPosition = initialCameraPosition + sway;
        cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, rotation, Time.deltaTime * rotationSpeed);

        // Update the time counter for Perlin noise
        timeCounter += Time.deltaTime * swaySpeed;
    }
}
