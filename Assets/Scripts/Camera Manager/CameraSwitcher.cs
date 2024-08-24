using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Transform pivotPoint; // El punto alrededor del cual la cámara girará
    public float rotationSpeed = 10f; // Velocidad de rotación
    public Camera rotatingCamera;  // Cámara giratoria
    public Camera mainCamera;      // Cámara principal

    void Update()
    {
        // Rotar la cámara alrededor del punto de pivote
        rotatingCamera.transform.RotateAround(pivotPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public void ChangeCamera(bool cam)
    {
        rotatingCamera.gameObject.SetActive(cam);
    }

}
