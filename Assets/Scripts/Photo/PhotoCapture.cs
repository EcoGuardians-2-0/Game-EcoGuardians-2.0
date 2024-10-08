using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;
    // [SerializeField] private GameObject cameraUI;

    [Header("Flash Effect")]
    [SerializeField] private GameObject cameraFlash;
    [SerializeField] private float flashTime;

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;

    private Texture2D screenCapture;
    private bool viewingPhoto;

    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    private void Update()
    {
        // Get the right click of the mouse
        if (Input.GetMouseButtonDown(1))
        {
            if(!viewingPhoto)
            {
                StartCoroutine(CapturePhoto());
            }
            else
            {
                RemovePhoto();
            }  
        }
    }

    IEnumerator CapturePhoto()
    {
        // cameraUI.SetActive(false);
        // Camera UI set false
        viewingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();
        ShowPhoto();

        // Send a raycast that occupies all the size
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("BlueBird"))
            {
                Debug.Log("BlueBird detected!");
            }
        }
    }

    void ShowPhoto()
    {
        //Do flash
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
        // Play some audio
        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);
    }

    void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.SetActive(false);
        // Camera UI
        // cameraUI.SetActive(true);
    }
}