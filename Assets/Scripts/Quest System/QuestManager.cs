using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

/*
 * Class QuestManager
 * 
 * Description: manage the quests of the game, providing methods to add, complete and clear tasks
 * 
 */
public class QuestManager : MonoBehaviour
{
    // Variables

    [SerializeField]
    private GameObject questPrefab;
    [SerializeField]
    private GameObject titlePrefab;
    [SerializeField]
    private Transform QuestListParent;
    [SerializeField]
    private Sprite spriteCompleted;

    // Dictionary to store the active quests
    private Dictionary<string, GameObject> activeQuests = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> activeTitles = new Dictionary<string, GameObject>();

    private GameObject noQuestsTitle;

    // Methods

    /*
     * Method Start
     * 
     * Description: Start the QuestManager
     */
    public void Start()
    {
        GameObject newTitle = Instantiate(titlePrefab, QuestListParent);
        newTitle.name = "Title: " + "NoQuests";

        GameObject Content = newTitle.transform.Find("Content").gameObject;
        GameObject Text_Content = Content.transform.Find("Text Content").gameObject;
        GameObject Title_Title = Text_Content.transform.Find("Tittle Text").gameObject;
        TextMeshProUGUI textMeshPro = Title_Title.GetComponent<TextMeshProUGUI>();

        if (textMeshPro != null)
        {
            textMeshPro.text = "No hay tareas por realizar.";
            noQuestsTitle = newTitle;

            for (int i = 0; i < 2; i++)
                LayoutRebuilder.ForceRebuildLayoutImmediate(QuestListParent.GetComponent<RectTransform>());
        }
        else
        {
            Debug.LogWarning("Can't add title: " + "NoQuests" + " because textMeshPro couldn't be found.");
            Destroy(newTitle);
        }
    }

    /*
     * Method GetActiveQuestsCount
     * 
     * Description: returns the number of active quests
     * 
     * Returns: 
     *      -> int - the number of active quests
     */
    public int GetActiveQuestsCount()
    {
        return activeQuests.Count;
    }

    /*
     * Method GetActiveTitlesCount
     * 
     * Description: returns the number of active titles
     * 
     * Returns: 
     *      -> int - the number of active titles
     */
    public int GetActiveTitlesCount()
    {
        return activeTitles.Count;
    }

    /*
     * Method AddQuest
     * 
     * Description: add a new quest to the dictionary of active quests
     * 
     * Parameters: 
     *      -> string questId - the unique identifier for the quest
     *      -> string questDescription - the description of the quest
     */
    public void AddQuest(string questId, string questDescription)
    {
        if (!activeQuests.ContainsKey(questId))
        {
            SetOffTasksTitle();

            GameObject newQuest = Instantiate(questPrefab, QuestListParent);
            newQuest.name = "Quest: " + questId;

            GameObject Content = newQuest.transform.Find("Content").gameObject;
            GameObject Text_Content = Content.transform.Find("Text Content").gameObject;
            GameObject Quest_Description = Text_Content.transform.Find("Quest Description").gameObject;
            TextMeshProUGUI textMeshPro = Quest_Description.GetComponent<TextMeshProUGUI>();

            if (textMeshPro != null)
            {
                textMeshPro.text = questDescription;
                activeQuests.Add(questId, newQuest);
                AnimationsQuest.Instance.ShowQuestAnimation(newQuest);
                LayoutRebuilder.ForceRebuildLayoutImmediate(QuestListParent.GetComponent<RectTransform>());
            }
            else
            {
                Debug.LogWarning("Can't add quest: " + questId + " because textMeshPro couldn't be found.");
                Destroy(newQuest);
            }
        }
        else
            Debug.LogWarning("Can't add quest: " + questId + " because already exists.");
    }

    /*
     * Method CompleteQuest
     * 
     * Description: Marks a quest as completed in the dictionary of active quests
     * 
     * Parameters: 
     *      -> string questId - the unique identifier for the quest
     */
    public void CompleteQuest(string questId)
    {
        Debug.Log("Quest id: " + questId);
        if (activeQuests.ContainsKey(questId))
        {
            GameObject quest = activeQuests[questId];
            GameObject checkBox = quest.transform.Find("Checkbox").gameObject;
            checkBox.AddComponent<Toggle>();
            checkBox.GetComponent<Toggle>().isOn = true;
            RawImage rawImage = checkBox.GetComponent<RawImage>();

            if (rawImage.texture != spriteCompleted.texture)
                AnimationsQuest.Instance.CompleteQuestAnimation(checkBox);

            rawImage.texture = spriteCompleted.texture;
        }
        else
            Debug.LogWarning("Quest with ID " + questId + " does not exist.");
    }

    /*
     * Method DeleteQuest
     * 
     * Description: Deletes a quest from the dictionary of active quests
     * 
     * Parameters: 
     *      -> string questId - the unique identifier for the quest
     */
    public void DeleteQuest(string questId)
    {
        if (activeQuests.ContainsKey(questId))
        {
            GameObject quest = activeQuests[questId];
            AnimationsQuest.Instance.DeleteQuestAnimation(quest);
            activeQuests.Remove(questId);
            SetNoTasksTitle();
        }
        else
            Debug.LogWarning("Quest with ID " + questId + " does not exist.");
    }

    /*
     * Method AddTitle
     * 
     * Description: add a new title to the dictionary of active titles
     * 
     * Parameters: 
     *      -> string titleId - the unique identifier for the title
     *      -> string titleDescription - the description of the title
     */
    public void AddTitle(string titleId, string titleDescription)
    {
        if (!activeTitles.ContainsKey(titleId))
        {
            SetOffTasksTitle();

            GameObject newTitle = Instantiate(titlePrefab, QuestListParent);
            newTitle.name = "Title: " + titleId;

            GameObject Content = newTitle.transform.Find("Content").gameObject;
            GameObject Text_Content = Content.transform.Find("Text Content").gameObject;
            GameObject Title_Title = Text_Content.transform.Find("Tittle Text").gameObject;

            TextMeshProUGUI textMeshPro = Title_Title.GetComponent<TextMeshProUGUI>();

            if (textMeshPro != null)
            {
                textMeshPro.text = titleDescription;

                if (!titleId.Equals("NoQuests"))
                {
                    activeTitles.Add(titleId, newTitle);
                    AnimationsQuest.Instance.ShowQuestAnimation(newTitle);
                }
                else
                    noQuestsTitle = newTitle;

                for (int i = 0; i < 2; i++)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(QuestListParent.GetComponent<RectTransform>());

            }
            else
            {
                Debug.LogWarning("Can't add title: " + titleId + " because textMeshPro couldn't be found.");
                Destroy(newTitle);
            }
        }
        else
            Debug.LogWarning("Can't add title: " + titleId + " because already exists.");
    }

    /*
     * Method DeleteTitle
     * 
     * Description: Deletes a title from the dictionary of active titles
     * 
     * Parameters: 
     *      -> string titleId - the unique identifier for the title
     */
    public void DeleteTitle(string titleId)
    {
        if (activeTitles.ContainsKey(titleId))
        {
            GameObject title = activeTitles[titleId];
            activeTitles.Remove(titleId);

            AnimationsQuest.Instance.DeleteQuestAnimation(title);

            SetNoTasksTitle();
        }
        else if (titleId.Equals("NoQuests"))
        {
            Destroy(noQuestsTitle);
            noQuestsTitle = null;
        }
        else
            Debug.LogWarning("Title with ID " + titleId + " does not exist.");
    }

    public bool AllQuestsCompleted()
    {
        int questsToDelete = 0;
        foreach (var questEntry in activeQuests)
        {
            Toggle toggle = questEntry.Value.GetComponentInChildren<Toggle>();
            if (toggle != null && toggle.isOn)
            {
                questsToDelete++;
            }
        }
        return activeQuests.Count == questsToDelete;
    }

    /*
     * Method ClearCompletedQuests
     * 
     * Description: Clears all completed quests from the dictionary of active quests
     */
    public void ClearCompletedQuests()
    {
        List<string> questsToRemove = new List<string>();

        foreach (var questEntry in activeQuests)
        {
            Toggle toggle = questEntry.Value.GetComponentInChildren<Toggle>();
            if (toggle != null && toggle.isOn)
            {
                Destroy(questEntry.Value);
                questsToRemove.Add(questEntry.Key);
            }
        }

        foreach (string questId in questsToRemove)
        {
            activeQuests.Remove(questId);
        }
    }

    /*
     * Method SetNoTasksTitle
     * 
     * Description: Adds a title to the dictionary of active titles if there are no active quests
     * 
     */
    public void SetNoTasksTitle()
    {
        if (GetActiveQuestsCount() == 0 && GetActiveTitlesCount() == 0 && !activeTitles.ContainsKey("NoQuests"))
            AddTitle("NoQuests", "No hay tareas por realizar.");
    }

    /*
     * Method SetOffTasksTitle
     * 
     * Description: Removes the title from the dictionary of active titles if there are active quests
     */
    public void SetOffTasksTitle()
    {
        if (noQuestsTitle != null)
            DeleteTitle("NoQuests");
    }
}