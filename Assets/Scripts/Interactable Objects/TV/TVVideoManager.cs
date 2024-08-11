using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVVideoManager : MonoBehaviour
{
    [SerializeField] private VideoClip video;
    [SerializeField] private List<Material> materials = new List<Material>();

    private VideoPlayer videoPlayer;
    private MeshRenderer meshRenderer;

    public bool hasVideo = false;

    private double lastTime;

    private float holdTime = 0.5f;
    private float holdTimeCounterUpVolume;
    private float holdTimeCounterDownVolume;
    private float holdTimeCounterAdvanceVideo;
    private float holdTimeCounterRewindVideo;

    // Start is called before the first frame update
    public void Init()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        meshRenderer = GetComponent<MeshRenderer>();

        if (video != null)
            hasVideo = true;

        DisableObjects.Instance.ToggleControlsVideoTVUI(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Pause video
        if (Input.GetKeyDown(KeyCode.Space))
            TogglePlayPause();

        if (videoPlayer.isPlaying)
        {
            // Advance and rewind video
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                videoPlayer.time -= 5;
            
            if (Input.GetKeyDown(KeyCode.RightArrow))
                videoPlayer.time += 5;

            // Up and Down keys for volume control
            if (Input.GetKeyDown(KeyCode.UpArrow))
                if (videoPlayer.GetDirectAudioVolume(0) < 1)
                {
                    videoPlayer.SetDirectAudioVolume(0, videoPlayer.GetDirectAudioVolume(0) + 0.1f);

                    if (videoPlayer.GetDirectAudioVolume(0) > 1)
                        videoPlayer.SetDirectAudioVolume(0, 1);
                }

            if (Input.GetKeyDown(KeyCode.DownArrow))
                if (videoPlayer.GetDirectAudioVolume(0) > 0)
                {
                    videoPlayer.SetDirectAudioVolume(0, videoPlayer.GetDirectAudioVolume(0) - 0.1f);

                    if (videoPlayer.GetDirectAudioVolume(0) < 0)
                        videoPlayer.SetDirectAudioVolume(0, 0);
                }

            // Holding keys \\

            // Up and Down keys for volume control
            if (Input.GetKey(KeyCode.UpArrow))
            {
                holdTimeCounterUpVolume -= Time.deltaTime;

                if (holdTimeCounterUpVolume < 0)
                    if (videoPlayer.GetDirectAudioVolume(0) < 1)
                    {
                        videoPlayer.SetDirectAudioVolume(0, videoPlayer.GetDirectAudioVolume(0) + 0.3f * Time.deltaTime);
                        if (videoPlayer.GetDirectAudioVolume(0) > 1)
                            videoPlayer.SetDirectAudioVolume(0, 1);
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
                        videoPlayer.SetDirectAudioVolume(0, videoPlayer.GetDirectAudioVolume(0) - 0.3f * Time.deltaTime);

                        if (videoPlayer.GetDirectAudioVolume(0) < 0)
                            videoPlayer.SetDirectAudioVolume(0, 0);
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
        // Assign the video to the video player
        if (video != null)
            videoPlayer.clip = video;

        holdTimeCounterUpVolume = holdTime;
        holdTimeCounterDownVolume = holdTime;
        holdTimeCounterAdvanceVideo = holdTime;
        holdTimeCounterRewindVideo = holdTime;

        if (videoPlayer != null && videoPlayer.clip != null)
        {
            meshRenderer.material = materials[0];

            if (lastTime != 0)
                videoPlayer.time = lastTime;

            videoPlayer.Play();
            StartCoroutine(ChangeMaterial());
        }
    }

    // Function to change the material of the TV
    private IEnumerator ChangeMaterial()
    {
        yield return new WaitForSeconds(0.4f);
        meshRenderer.material = materials[1];
    }

    // Function to pause the video
    public void PauseVideo()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            videoPlayer.Stop();
            videoPlayer.clip = null;
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
