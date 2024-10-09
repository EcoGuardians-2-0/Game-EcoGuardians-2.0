using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;
    [SerializeField] private GameObject cameraUI;

    [Header("Flash Effect")]
    [SerializeField] private GameObject cameraFlash;
    [SerializeField] private float flashTime;

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;

    [Header("Photo Delay Effect")]
    [SerializeField] private Animator ActivatePhotoDelayAnimation;

    [Header("Capture Delay")]
    [SerializeField] private float captureDelay = 2f;

    [Header("Auto Remove Photo")]
    [SerializeField] private float autoRemoveDelay = 3f;

    private Texture2D screenCapture;
    private bool viewingPhoto;
    private bool isCapturing;
    private bool isInFirstCamera;
    private bool canTakePhoto;

    private void Start()
    {
        canTakePhoto = false;
        isInFirstCamera = false;
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    private void OnEnable()
    {
        EventManager.Photograph.OnActiveCamera += HandleTakePhoto;
        EventManager.Photograph.OnFirstPerson += HandleOnFirstCamera;
    }

    private void OnDisable()
    {
        EventManager.Photograph.OnActiveCamera -= HandleTakePhoto;
        EventManager.Photograph.OnFirstPerson -= HandleOnFirstCamera;
    }

    private void Update()
    {
        if (canTakePhoto && isInFirstCamera)
        {
            if (Input.GetMouseButtonDown(1) && !viewingPhoto && !isCapturing)
            {
                StartCoroutine(PrepareCapture());
            }
            else if (Input.GetMouseButtonUp(1) && isCapturing)
            {
                StopCoroutine(PrepareCapture());
                isCapturing = false;
                cameraUI.SetActive(false);
            }
        }
    }

    IEnumerator PrepareCapture()
    {
        isCapturing = true;
        cameraUI.SetActive(true);

        ActivatePhotoDelayAnimation.Play("CameraShow");

        float elapsedTime = 0f;
        while (elapsedTime < captureDelay)
        {
            if (!Input.GetMouseButton(1))
            {
                cameraUI.SetActive(false);
                isCapturing = false;
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(CapturePhoto());
    }

    IEnumerator CapturePhoto()
    {
        isCapturing = false;
        cameraUI.SetActive(false);
        viewingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();
        ShowPhoto();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("BlueBird"))
            {
                Debug.Log("BlueBird detected!");
            }

            if (hit.collider.CompareTag("RedBird"))
            {
                Debug.Log("RedBird detected!");
            }

            if (hit.collider.CompareTag("YellowBird"))
            {
                Debug.Log("YellowBird detected!");
            }

            if (hit.collider.CompareTag("BlackBird"))
            {
                Debug.Log("BlackBird detected!");
            }
        }

        StartCoroutine(AutoRemovePhoto());
    }

    void ShowPhoto()
    {
        StartCoroutine(CameraFlashEffect());

        Sprite photoSprite = Sprite.Create(screenCapture,
            new Rect(0, 0, screenCapture.width, screenCapture.height),
            new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;

        photoFrame.SetActive(true);

        fadingAnimation.Play("PhotoFade");
    }

    IEnumerator CameraFlashEffect()
    {
        AudioManager.Instance.PlaySound(SoundType.TakePhoto);

        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);
    }

    IEnumerator AutoRemovePhoto()
    {
        yield return new WaitForSeconds(autoRemoveDelay);
        RemovePhoto();
    }

    void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.SetActive(false);
        cameraUI.SetActive(false);
    }

    private void HandleTakePhoto(bool canTakePhoto)
    {
        this.canTakePhoto = canTakePhoto;
    }

    private void HandleOnFirstCamera(bool isInFirstCamera)
    {
        this.isInFirstCamera = isInFirstCamera;
    }
}