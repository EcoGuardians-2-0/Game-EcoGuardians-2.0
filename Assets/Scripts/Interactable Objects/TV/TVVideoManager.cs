using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVVideoManager : MonoBehaviour
{
    [SerializeField] private List<Material> materials = new List<Material>();

    private VideoPlayer videoPlayer;
    private MeshRenderer meshRenderer;
    private Loader loaderScript;
    [SerializeField] private GameObject loader;

    public bool hasVideo = false;
    private bool isPrepared = false;

    private double lastTime;

    private float holdTime = 0.5f;
    private float holdTimeCounterUpVolume = 0.5f;
    private float holdTimeCounterDownVolume = 0.5f;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        meshRenderer = GetComponent<MeshRenderer>();
        loaderScript = loader.GetComponent<Loader>();

        ToggleMeshRenderer();
        isPrepared = false;
        videoPlayer.playOnAwake = false;
        videoPlayer.prepareCompleted += OnVideoPreparedFirstTime;
        videoPlayer.Prepare();

        hasVideo = (videoPlayer.url != null) ? true : false;
    }

    // Callback when the video is prepared for first time
    private void OnVideoPreparedFirstTime(VideoPlayer vp)
    {
        videoPlayer.Pause();
        isPrepared = true;
        Debug.Log("Video: " + videoPlayer.url + " is prepared for first time");
        // Unsubscribe from the event to avoid repeated calls
        videoPlayer.prepareCompleted -= OnVideoPreparedFirstTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (meshRenderer.enabled)
        {
            // Pause video
            if (Input.GetKeyDown(KeyCode.Space))
                TogglePlayPause();

            if (videoPlayer.isPlaying)
            {
                // Advance and rewind video - Suspended functionality
                //if (Input.GetKeyDown(KeyCode.LeftArrow))
                //    videoPlayer.time = Mathf.Max(0, (float)videoPlayer.time - 5);

                //if (Input.GetKeyDown(KeyCode.RightArrow))
                //    videoPlayer.time = Mathf.Min((float)videoPlayer.length, (float)videoPlayer.time + 5);

                // Up and Down keys for volume control
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    videoPlayer.SetDirectAudioVolume(0, Mathf.Min(1, (float)videoPlayer.GetDirectAudioVolume(0) + 0.1f));


                if (Input.GetKeyDown(KeyCode.DownArrow))
                    videoPlayer.SetDirectAudioVolume(0, Mathf.Max(0, (float)videoPlayer.GetDirectAudioVolume(0) - 0.1f));

                // Holding keys \\

                // Up and Down keys for volume control
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    holdTimeCounterUpVolume -= Time.deltaTime;

                    if (holdTimeCounterUpVolume < 0)
                        videoPlayer.SetDirectAudioVolume(0, Mathf.Min(1, (float)videoPlayer.GetDirectAudioVolume(0) + 0.3f * Time.deltaTime));
                }
                else
                    holdTimeCounterUpVolume = holdTime;

                if (Input.GetKey(KeyCode.DownArrow))
                {
                    holdTimeCounterDownVolume -= Time.deltaTime;

                    if (holdTimeCounterDownVolume < 0)
                        videoPlayer.SetDirectAudioVolume(0, Mathf.Max(0, (float)videoPlayer.GetDirectAudioVolume(0) - 0.3f * Time.deltaTime));
                }
                else
                    holdTimeCounterDownVolume = holdTime;

                // Save time to continue from there
                if (videoPlayer.time > 0.1)
                    lastTime = videoPlayer.time;
            }

            // Hide or show the controls video TV UI
            if (Input.GetKeyDown(KeyCode.Q))
                DisableObjects.Instance.ToggleControlsVideoTVUI(false);
        }
    }

    // Function to play the video
    public void PlayVideo()
    {
        loaderScript.Play();
        DisableObjects.Instance.ToggleControlsVideoTVUI(true);

        if (videoPlayer != null && !string.IsNullOrEmpty(videoPlayer.url))
        {
            if (lastTime != 0)
                videoPlayer.time = lastTime;

            if (isPrepared)
            {
                Debug.Log("Video is prepared");
                // Change material when the video is ready
                meshRenderer.material = materials[1];
                loaderScript.Pause();
                videoPlayer.Play();
                ToggleMeshRenderer();
            }
            else if (!isPrepared)
            {
                Debug.Log("Video is not prepared");
                videoPlayer.prepareCompleted += OnVideoPrepared;
                videoPlayer.Prepare();
            }
        }
    }

    // toggle mesh renderer
    public void ToggleMeshRenderer()
    {
        meshRenderer.enabled = !meshRenderer.enabled;
        Debug.Log("Mesh Renderer is: " + meshRenderer.enabled);
    }

    // Callback when the video is prepared
    private void OnVideoPrepared(VideoPlayer vp)
    {
        // Change material when the video is ready
        loaderScript.Pause();

        // Start the video after it is prepared
        meshRenderer.material = materials[1];
        videoPlayer.Play();
        ToggleMeshRenderer();
        isPrepared = true;
        Debug.Log("Video: " + videoPlayer.url + " is prepared");

        // Unsubscribe from the event to avoid repeated calls
        videoPlayer.prepareCompleted -= OnVideoPrepared;
    }


    // Function to pause the video
    public void PauseVideo()
    {
        loaderScript.Pause();
        videoPlayer.Pause();
        ToggleMeshRenderer();
        meshRenderer.material = materials[0];
    }

    // Funtion to toggle play/pause the video
    public void TogglePlayPause()
    {
        if (videoPlayer != null)
            if (videoPlayer.isPlaying)
                videoPlayer.Pause();
            else
                videoPlayer.Play();
    }
}
