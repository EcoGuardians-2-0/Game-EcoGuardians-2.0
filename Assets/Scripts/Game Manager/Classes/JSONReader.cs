using UnityEngine;

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
            if (i == 0)
                questManager.AddTitle(questList.quests[i].stringid, questList.quests[i].description);
            else
                questManager.AddQuest(questList.quests[i].stringid, questList.quests[i].description);
        }
    }
}
