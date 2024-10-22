using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class Loader : MonoBehaviour
{
    VideoPlayer videoPlayer;
    MeshRenderer meshRenderer;

    bool waitingPrepared = true;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.enabled = false;

        if (videoPlayer)
        {
            string videoPath = Path.Combine(Application.streamingAssetsPath, "loader.mp4");
            videoPlayer.url = videoPath;
            videoPlayer.errorReceived += OnVideoError;
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare();
        }
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("Video: " + videoPlayer.url + " is prepared");
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

    public void Play()
    {
        if (videoPlayer)
        {
            videoPlayer.Play();
            meshRenderer.enabled = true;
        }
    }

    public void Pause()
    {
        if (videoPlayer)
        {
            meshRenderer.enabled = false;
            videoPlayer.Pause();
        }
    }
}
