using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraUtilityManager : MonoBehaviour
{
    public static CameraUtilityManager Instance { get; private set; }

    private CinemachineVirtualCamera utilityCamera;

    [SerializeField]
    private float secondsToChangeCamera;

    private void Awake()
    {
        // If there is already an instance of this class and it is not this one, destroy this instance
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            utilityCamera = GetComponent<CinemachineVirtualCamera>();
        }
    }

    public void SetCameraOn(Transform target, float cameraDistance, Vector3 shoulderOffset, Vector3 Damping)
    {
        utilityCamera.Priority = 12;
        utilityCamera.Follow = target;
        utilityCamera.LookAt = target;

        var cinemachine3RdPersonFollow = utilityCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>() ??
                                         utilityCamera.AddCinemachineComponent<Cinemachine3rdPersonFollow>();

        cinemachine3RdPersonFollow.CameraDistance = cameraDistance;
        cinemachine3RdPersonFollow.Damping = Damping;
        cinemachine3RdPersonFollow.ShoulderOffset = shoulderOffset;

        var cinemachineComposer = utilityCamera.GetCinemachineComponent<CinemachineComposer>() ??
                                  utilityCamera.AddCinemachineComponent<CinemachineComposer>();
    }

    public void SetCameraOff()
    {
        utilityCamera.Priority = 9;
        utilityCamera.Follow = null;
        utilityCamera.LookAt = null;

        var cinemachine3RdPersonFollow = utilityCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>() ??
                                         utilityCamera.AddCinemachineComponent<Cinemachine3rdPersonFollow>();

        cinemachine3RdPersonFollow.Damping = new Vector3(0, 0, 0);
        cinemachine3RdPersonFollow.ShoulderOffset = new Vector3(0, 0, 0);
        cinemachine3RdPersonFollow.CameraDistance = 0;

        var cinemachineComposer = utilityCamera.GetCinemachineComponent<CinemachineComposer>() ??
                                  utilityCamera.AddCinemachineComponent<CinemachineComposer>();

        cinemachineComposer.m_TrackedObjectOffset = new Vector3(0, 0, 0);
    }

    public void SetCameraOnCharacter(Transform target)
    {
        utilityCamera.Priority = 12;
        utilityCamera.Follow = target;
        utilityCamera.LookAt = target;

        StartCoroutine(ChangeCameraValues());
    }

    private IEnumerator ChangeCameraValues()
    {
        while (DialogueManager.instance.isTalking)
        {
            CameraPositions.GetRandomConfig(utilityCamera);
            yield return new WaitForSeconds(secondsToChangeCamera);
        }
    }
}
