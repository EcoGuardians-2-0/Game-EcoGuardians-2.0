using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Transform pivotPoint; // El punto alrededor del cual la c�mara girar�
    public float rotationSpeed = 10f; // Velocidad de rotaci�n
    public Camera rotatingCamera;  // C�mara giratoria
    public Camera mainCamera;      // C�mara principal

    void Update()
    {
        // Rotar la c�mara alrededor del punto de pivote
        rotatingCamera.transform.RotateAround(pivotPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public void ChangeCamera(bool cam)
    {
        rotatingCamera.gameObject.SetActive(cam);
    }

}
