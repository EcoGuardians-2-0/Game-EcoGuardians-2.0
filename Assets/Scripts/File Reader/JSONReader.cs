using UnityEngine;
using System.IO;

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
                EventManager.MapIcon.OnDisplayIconFiltered(questList.quests[i].stringid).Invoke(true);
                questManager.AddQuest(questList.quests[i].stringid, questList.quests[i].description);

            }
        }
    }

    /*
     * 
     * Method: ReadLinks
     * Description: Reads the links from a JSON file located in StreamingAssets and returns a LinksContainer object
     * Returns: 
     *  -> LinksContainer: The deserialized object containing the list of links
     * 
     */
    public LinksContainer ReadLinks()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "links.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return null;
        }

        string jsonData = File.ReadAllText(filePath);

        LinksContainer linksContainer = JsonUtility.FromJson<LinksContainer>(jsonData);

        if (linksContainer == null || linksContainer.links.Count == 0)
        {
            Debug.LogError("Error deserializing JSON or no links found.");
            return null;
        }

        return linksContainer;
    }

}
