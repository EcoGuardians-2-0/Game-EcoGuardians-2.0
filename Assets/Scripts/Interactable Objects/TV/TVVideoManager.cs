using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVVideoManager : MonoBehaviour
{
    [SerializeField] private List<Material> materials = new List<Material>();

    private VideoPlayer videoPlayer;
    private MeshRenderer meshRenderer;
    private bool visibility = true;
    
    private Loader loaderScript;
    [SerializeField] private GameObject loader;

    public bool hasVideo = false;

    private double lastTime;

    private float holdTime = 0.5f;
    private float holdTimeCounterUpVolume = 0.5f;
    private float holdTimeCounterDownVolume = 0.5f;

    private bool waitingPreparedFirstTime = true;
    private bool waitingPrepared = false;

    public void Init()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        meshRenderer = GetComponent<MeshRenderer>();
        loaderScript = loader.GetComponent<Loader>();

        SetVisibility(false);
        videoPlayer.playOnAwake = false;
        videoPlayer.errorReceived += OnVideoError;
        videoPlayer.prepareCompleted += OnVideoPreparedFirstTime;
        videoPlayer.Prepare();

        hasVideo = (videoPlayer.url != null) ? true : false;
    }

    // Callback when the video is prepared for first time
    private void OnVideoPreparedFirstTime(VideoPlayer vp)
    {
        videoPlayer.Pause();
        Debug.Log("Video: " + videoPlayer.url + " is prepared for first time");
        // Unsubscribe from the event to avoid repeated calls
        videoPlayer.prepareCompleted -= OnVideoPreparedFirstTime;
        waitingPreparedFirstTime = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (visibility)
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
        if (waitingPreparedFirstTime)
        {
            videoPlayer.prepareCompleted -= OnVideoPreparedFirstTime;
            waitingPreparedFirstTime = false;
        }

        loaderScript.Play();
        DisableObjects.Instance.ToggleControlsVideoTVUI(true);

        if (videoPlayer != null && !string.IsNullOrEmpty(videoPlayer.url))
        {
            if (lastTime != 0)
                videoPlayer.time = lastTime;

            if (videoPlayer.isPrepared)
            {
                Debug.Log("Video is prepared");
                // Change material when the video is ready
                loaderScript.Pause();
                SetVisibility(true);
                videoPlayer.Play();
            }
            else if (!videoPlayer.isPrepared)
            {
                Debug.Log("Video is not prepared");
                videoPlayer.prepareCompleted += OnVideoPrepared;
                waitingPrepared = true;
                videoPlayer.Prepare();
            }
        }
    }

    // toggle mesh renderer
    public void SetVisibility(bool state)
    {
        visibility = state;
        meshRenderer.material = visibility ? materials[1] : materials[0];
        Debug.Log("Mesh Renderer is: " + meshRenderer.material + " state: " + state);
    }

    // Callback when the video is prepared
    private void OnVideoPrepared(VideoPlayer vp)
    {
        // Change material when the video is ready
        loaderScript.Pause();

        // Start the video after it is prepared
        videoPlayer.Play();
        SetVisibility(true);
        Debug.Log("Video: " + videoPlayer.url + " is prepared");

        // Unsubscribe from the event to avoid repeated calls
        videoPlayer.prepareCompleted -= OnVideoPrepared;
        waitingPrepared = false;
    }

    private void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogError($"Error playing video: {message}, waiting for prepared: {waitingPrepared}, isprepared: {videoPlayer.isPrepared}");
        if (waitingPrepared && !videoPlayer.isPrepared)
            StartCoroutine(ReloadVideo());
    }

    private IEnumerator ReloadVideo()
    {
        yield return new WaitForSeconds(3);
        if (waitingPrepared && !videoPlayer.isPrepared)
            videoPlayer.Prepare();
    }

    // Function to pause the video
    public void PauseVideo()
    {
        loaderScript.Pause();
        if (visibility)
        {
            videoPlayer.Pause();
            SetVisibility(false);
        }

        if (waitingPrepared)
        {
            videoPlayer.prepareCompleted -= OnVideoPrepared;
            waitingPrepared = false;
        }
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
