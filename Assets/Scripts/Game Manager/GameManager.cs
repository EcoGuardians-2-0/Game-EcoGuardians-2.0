using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private QuestManager questManager;

    public static event Action OnQuestAssigned;
    public static event Action<string> OnQuestCompleted;
    public static event Action OnAllQuestsCompleted;

    private const string MODULE_NAME = "module";
    private const string QUESTIONNAIRE = "questionnaire";
    private int currentModule;

    private string questionnaire;

    public static void QuestCompleted(string taskName)
    {
        if(OnQuestCompleted != null)
        {
            OnQuestCompleted.Invoke(taskName);
        }
    }

    public static void QuestAssigned()
    {
        if (OnQuestAssigned != null)
        {
            OnQuestAssigned.Invoke();
        }
    }

    public static void AllQuestsCompleted()
    {
        if (OnAllQuestsCompleted != null)
        {
            OnAllQuestsCompleted.Invoke();
        }
    }

    private void OnEnable()
    {
        OnQuestAssigned += HandleQuestAssigned;
        OnQuestCompleted += HandleQuestCompleted;
        OnAllQuestsCompleted += HandleAllQuestCompleted;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentModule = 1;
        questionnaire = "global_cuestionario_";
        // Register for game start events
        ControllerScreensMenuUI.Instance.onGameStarted.AddListener(HandleGameStart);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void HandleQuestAssigned()
    {
        Debug.Log("Quests Assigned");
        ActivateQuests(MODULE_NAME + currentModule);
    }
    private void HandleQuestCompleted(string taskName)
    {
        questManager.CompleteQuest(taskName);
    }

    private void HandleAllQuestCompleted()
    {
        DialogueManager.instance.SetVariable("global_cuestionario_"+currentModule, DialogueVariableSetter.SetVariable(true));
        StartCoroutine(clearQuests());
    }

    private IEnumerator clearQuests()
    {
        yield return StartCoroutine(questManager.ClearCompletedQuests());
        ActivateQuests(QUESTIONNAIRE + currentModule);
    }
    private void HandleActivityStart(string activityNumber)
    {
        switch (activityNumber)
        {
            case "1":
                StartActivity1();
                break;
        }
    }

    private void StartActivity1()
    {
        Debug.Log("StartActivity1");
        AudioManager.Instance.PlayMusic(SoundType.Act1BackgroundMusic);
        //Hide world
        WorldManager.Instance.HideWorld();
    }

    // Handle game start event
    private void HandleGameStart(string arg0)
    {
        AudioManager.Instance.PlayMusic(SoundType.Act2BackgroundMusic);
    }

    public void ActivateQuests(string file)
    {
        new JSONReader().ReadQuests(questManager, file);
    }
}
