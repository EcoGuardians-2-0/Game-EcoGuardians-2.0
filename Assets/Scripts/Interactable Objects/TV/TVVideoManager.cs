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

    private double lastTime;

    private float holdTime = 0.5f;
    private float holdTimeCounterUpVolume;
    private float holdTimeCounterDownVolume;
    private float holdTimeCounterAdvanceVideo;
    private float holdTimeCounterRewindVideo;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        meshRenderer = GetComponent<MeshRenderer>();
        loaderScript = loader.GetComponent<Loader>();

        videoPlayer.playOnAwake = false;
        videoPlayer.Prepare();

        hasVideo = (videoPlayer.url != null) ? true : false;
    }

    // Update is called once per frame
    void Update()
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
            //    if (videoPlayer.time + 5 <= videoPlayer.length)
            //        videoPlayer.time += 5;

            // Up and Down keys for volume control
            if (Input.GetKeyDown(KeyCode.UpArrow))
                if (videoPlayer.GetDirectAudioVolume(0) < 1)
                {
                    float volume = videoPlayer.GetDirectAudioVolume(0);

                    volume += 0.1f;

                    if (volume > 1)
                        volume = 1;

                    videoPlayer.SetDirectAudioVolume(0, volume);              
                }

            if (Input.GetKeyDown(KeyCode.DownArrow))
                if (videoPlayer.GetDirectAudioVolume(0) > 0)
                {
                    float volume = videoPlayer.GetDirectAudioVolume(0);

                    volume -= 0.1f;

                    if (volume < 0)
                        volume = 0;

                    videoPlayer.SetDirectAudioVolume(0, volume);
                }

            // Holding keys \\

            // Up and Down keys for volume control
            if (Input.GetKey(KeyCode.UpArrow))
            {
                holdTimeCounterUpVolume -= Time.deltaTime;

                if (holdTimeCounterUpVolume < 0)
                    if (videoPlayer.GetDirectAudioVolume(0) < 1)
                    {
                        float volume = videoPlayer.GetDirectAudioVolume(0);
                        volume += 0.3f * Time.deltaTime;

                        if (volume > 1)
                            volume = 1;

                        videoPlayer.SetDirectAudioVolume(0, volume);
                    }
            }
            else
                holdTimeCounterUpVolume = holdTime;

            if (Input.GetKey(KeyCode.DownArrow))
            {
                holdTimeCounterDownVolume -= Time.deltaTime;

                if (holdTimeCounterDownVolume < 0)
                    if (videoPlayer.GetDirectAudioVolume(0) > 0)
                    {
                        float volume = videoPlayer.GetDirectAudioVolume(0);
                        volume -= 0.3f * Time.deltaTime;

                        if (volume < 0)
                            volume = 0;

                        videoPlayer.SetDirectAudioVolume(0, volume);
                    }
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

    // Function to play the video
    public void PlayVideo()
    {
        DisableObjects.Instance.ToggleControlsVideoTVUI(true);

        loaderScript.Play();
        meshRenderer.enabled = false; // Desactivate the mesh renderer to show the video

        holdTimeCounterUpVolume = holdTime;
        holdTimeCounterDownVolume = holdTime;
        holdTimeCounterAdvanceVideo = holdTime;
        holdTimeCounterRewindVideo = holdTime;

        if (videoPlayer != null && !string.IsNullOrEmpty(videoPlayer.url))
        {
            //meshRenderer.material = materials[2]; // Change material while loading

            if (lastTime != 0)
                videoPlayer.time = lastTime;

            if (videoPlayer.isPrepared)
            {
                // Change material when the video is ready
                loaderScript.Pause();

                meshRenderer.material = materials[1];
                videoPlayer.Play();
                meshRenderer.enabled = true; // Activate the mesh renderer to show the video
            }
            else
            {
                videoPlayer.prepareCompleted += OnVideoPrepared;
                videoPlayer.Prepare();
            }
        }
    }

    // Callback when the video is prepared
    private void OnVideoPrepared(VideoPlayer vp)
    {
        // Change material when the video is ready
        loaderScript.Pause();

        // Start the video after it is prepared
        meshRenderer.material = materials[1];
        videoPlayer.Play();
        meshRenderer.enabled = true;

        // Unsubscribe from the event to avoid repeated calls
        videoPlayer.prepareCompleted -= OnVideoPrepared;
    }


    // Function to pause the video
    public void PauseVideo()
    {
        loaderScript.Pause();
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            //videoPlayer.Stop();
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
