using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;
using System.Collections;

/*
 * 
 * Class: JSONReader
 * Description: Reads JSON files and adds the data to the game manager
 * 
 */
public class JSONReader
{
    /*
     * 
     * Method: ReadQuests
     * Description: Reads the quests from a JSON file and adds them to the quest manager
     * Parameters: 
     *  -> QuestManager questManager: The quest manager to add the quests to
     *  -> string module: The name of the JSON file to read
     * 
     */
    public void ReadQuests(QuestManager questManager, string module)
    {
        TextAsset jsonData = Resources.Load<TextAsset>("JSONs/Quests/" + module);

        if (jsonData == null)
        {
            Debug.LogError("File not found: " + module);
            return;
        }

        Quests questList = JsonUtility.FromJson<Quests>(jsonData.text);

        if (questList == null)
        {
            Debug.LogError("Error deserializing JSON file: " + module);
            return;
        }

        // Add quests to the quest manager, the first quest is the title
        for (int i = 0; i < questList.quests.Length; i++)
        {
            Debug.Log("Loading quests");
            if (i == 0)
                questManager.AddTitle(questList.quests[i].stringid, questList.quests[i].description);
            else
            {
                questManager.AddQuest(questList.quests[i].stringid, questList.quests[i].description);

            }
        }
    }

    /*
     * 
     * Method: ReadLinks
     * Description: Reads the links from a JSON file and adds them to the links container
     * Parameters: 
     *  -> Action<LinksContainer> callback: The callback to execute after reading the links
     *  -> int maxRetries: The maximum number of retries to read the file
     * Returns: IEnumerator
     *  -> The coroutine to read the links
     * 
     */
    public IEnumerator ReadLinks(System.Action<LinksContainer> callback, int maxRetries = 100)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "links.json");
        int attempt = 0;

        while (attempt < maxRetries)
        {
            UnityWebRequest request = UnityWebRequest.Get(filePath);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonData = request.downloadHandler.text;
                LinksContainer linksContainer = JsonUtility.FromJson<LinksContainer>(jsonData);

                if (linksContainer != null && linksContainer.links.Count > 0)
                {
                    callback(linksContainer);
                    yield break; 
                }
                else
                {
                    Debug.LogError("Error deserializing JSON or no links found.");
                }
            }
            else
            {
                Debug.LogError($"Error loading links.json: {request.error}");
            }

            attempt++;
            if (attempt < maxRetries)
            {
                Debug.Log($"Retrying... Attempt {attempt + 1} of {maxRetries}");
                yield return new WaitForSeconds(2);
            }
        }

 
        Debug.LogError("Failed to load links after maximum retries.");
        callback(null);
    }


    /*
     * 
     * Method: ReadVideos
     * Description: Reads the videos from a JSON file and adds them to the video manager
     * Parameters: 
     *  -> Action<VideoList> callback: The callback to execute after reading the videos
     * Returns: IEnumerator
     *  -> The coroutine to read the videos
     * 
     */
    public IEnumerator ReadVideos(System.Action<VideoList> callback, int maxRetries = 100)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "videos.json");
        int attempt = 0;

        while (attempt < maxRetries)
        {
            UnityWebRequest request = UnityWebRequest.Get(filePath);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonData = request.downloadHandler.text;
                VideoList videoList = JsonUtility.FromJson<VideoList>(jsonData);

                if (videoList != null && videoList.videos != null && videoList.videos.Length > 0)
                {
                    callback(videoList);
                    yield break;
                }
                else
                {
                    Debug.LogError("Error deserializing JSON or no videos found.");
                }
            }
            else
            {
                Debug.LogError($"Error loading videos.json: {request.error}");
            }

            attempt++;
            if (attempt < maxRetries)
            {
                Debug.Log($"Retrying... Attempt {attempt + 1} of {maxRetries}");
                yield return new WaitForSeconds(2);
            }
        }

        Debug.LogError("Failed to load videos after maximum retries.");
        callback(null);
    }
}
