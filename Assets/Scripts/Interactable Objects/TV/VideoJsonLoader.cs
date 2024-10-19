using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class VideoJsonLoader : MonoBehaviour
{
    // Name of the JSON file with the video URLs
    public string jsonFileName = "videos.json";

    [SerializeField]
    private List<GameObject> tvObjects = new List<GameObject>();

    void Start()
    {
        // Start the coroutine to load the JSON file
        StartCoroutine(LoadJson());
    }

    IEnumerator LoadJson()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        // If the path is using HTTP or HTTPS, use UnityWebRequest
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            using (UnityWebRequest request = UnityWebRequest.Get(filePath))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error al acceder al archivo JSON: " + request.error);
                }
                else
                {
                    ProcessJson(request.downloadHandler.text);
                }
            }
        }
        else
        {
            // For local files, use File.ReadAllText
            if (File.Exists(filePath))
            {
                string jsonText = File.ReadAllText(filePath);
                ProcessJson(jsonText);
            }
            else
            {
                Debug.LogError("El archivo JSON no se encontró en StreamingAssets: " + filePath);
            }
        }
    }

    void ProcessJson(string jsonText)
    {
        // Convert the JSON text to a VideoList object
        VideoList videoList = JsonUtility.FromJson<VideoList>(jsonText);

        // Assigns the URL of each video to the corresponding VideoPlayer
        for (int i = 0; i < videoList.videos.Length; i++)
        {
            Video video = videoList.videos[i];

            Debug.Log("Screen Name from JSON: " + video.screen_name);
            if (i < tvObjects.Count) // Verify that the TV object exists
            {
                GameObject tvObject = tvObjects[i];
                VideoPlayer videoPlayer = tvObject.GetComponent<VideoPlayer>();

                if (videoPlayer != null)
                {
                    videoPlayer.url = video.url; // Assign the video URL
                    TVVideoManager tvVideoManager = tvObject.GetComponent<TVVideoManager>();
                    tvVideoManager.Init(); // Initialize the VideoPlayer
                    Debug.Log($"Playing {video.screen_name}: {video.url}"); // Mesage to show the video being played
                }
                else
                {
                    Debug.LogWarning("No VideoPlayer found on " + tvObject.name);
                }
            }
            else
            {
                Debug.LogWarning("No TV object found for index: " + i);
            }
        }
    }

    // Class for representing a video
    [System.Serializable]
    public class Video
    {
        public string screen_name;
        public string url;
    }

    // Class for representing a list of videos
    [System.Serializable]
    public class VideoList
    {
        public Video[] videos;
    }
}
