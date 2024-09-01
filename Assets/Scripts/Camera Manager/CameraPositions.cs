using Cinemachine;
using System;
using System.Reflection;
using UnityEngine;

public class CameraPositions
{
    public static void GetRandomConfig(CinemachineVirtualCamera camera)
    {
        // Get the type of the class CameraPositions
        Type type = typeof(CameraPositions);

        // Filter the methods that start with "Config", are public, non-static, and declared only in this class
        MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        MethodInfo[] configMethods = Array.FindAll(methods, m => m.Name.StartsWith("Config"));

        // Check if there are any methods matching the criteria
        if (configMethods.Length == 0)
        {
            Debug.LogError("No 'Config' methods found in the CameraPositions class.");
            return;
        }

        // Select a random index from the array of 'Config' methods
        int randomIndex = UnityEngine.Random.Range(0, configMethods.Length);

        // Create an instance of CameraPositions to invoke the selected method
        CameraPositions instance = new CameraPositions();

        // Invoke the randomly selected method
        configMethods[randomIndex].Invoke(instance, new object[] { camera});
    }


    public void Config1(CinemachineVirtualCamera camera)
    {
        var cinemachine3RdPersonFollow = camera.GetCinemachineComponent<Cinemachine3rdPersonFollow>() ??
                                         camera.AddCinemachineComponent<Cinemachine3rdPersonFollow>();

        var cinemachineComposer = camera.GetCinemachineComponent<CinemachineComposer>() ??
                                  camera.AddCinemachineComponent<CinemachineComposer>();

        cinemachine3RdPersonFollow.CameraDistance = 0;
        cinemachine3RdPersonFollow.ShoulderOffset = new Vector3(1.3f, 1.5f, 1.3f);
        cinemachine3RdPersonFollow.Damping = new Vector3(0f, 0f, 0f);
        cinemachineComposer.m_TrackedObjectOffset = new Vector3(1.2f, 0.3f, -0.1f);
    }

    public void Config2(CinemachineVirtualCamera camera)
    {
        var cinemachine3RdPersonFollow = camera.GetCinemachineComponent<Cinemachine3rdPersonFollow>() ??
                                         camera.AddCinemachineComponent<Cinemachine3rdPersonFollow>();

        var cinemachineComposer = camera.GetCinemachineComponent<CinemachineComposer>() ??
                                  camera.AddCinemachineComponent<CinemachineComposer>();

        cinemachine3RdPersonFollow.CameraDistance = 0;
        cinemachine3RdPersonFollow.ShoulderOffset = new Vector3(-1.2f, 1.5f, 1.3f);
        cinemachine3RdPersonFollow.Damping = new Vector3(0f, 0f, 0f);
        cinemachineComposer.m_TrackedObjectOffset = new Vector3(0.2f, 1f, 0.4f);
    }


}
