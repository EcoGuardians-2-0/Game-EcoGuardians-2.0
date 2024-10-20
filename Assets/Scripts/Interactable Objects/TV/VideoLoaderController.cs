using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class VideoLoaderController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> tvObjects = new List<GameObject>();
    private JSONReader jsonReader;

    void Start()
    {
        jsonReader = new JSONReader();
        LoadVideos();
    }

    void LoadVideos()
    {
        VideoList videoList = jsonReader.ReadVideos(); 

        if (videoList != null)
        {
            for (int i = 0; i < videoList.videos.Length; i++)
            {
                Video video = videoList.videos[i];

                if (i < tvObjects.Count)
                {
                    GameObject tvObject = tvObjects[i];
                    VideoPlayer videoPlayer = tvObject.GetComponent<VideoPlayer>();

                    if (videoPlayer != null)
                    {
                        videoPlayer.url = video.url;
                        tvObject.GetComponent<TVVideoManager>().Init();
                        Debug.Log($"Playing {video.screen_name}: {video.url}");
                    }
                    else
                        Debug.LogWarning("No VideoPlayer found on " + tvObject.name);
                }
                else
                    Debug.LogWarning("No TV object found for index: " + i);
            }
        }
    }
}

