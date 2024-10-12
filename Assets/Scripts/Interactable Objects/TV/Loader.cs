using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Loader : MonoBehaviour
{
    VideoPlayer videoPlayer;
    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.enabled = false;

        if (videoPlayer)
        { 
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "loader.mp4");
            videoPlayer.url = videoPath;
            videoPlayer.Prepare();
        }
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
