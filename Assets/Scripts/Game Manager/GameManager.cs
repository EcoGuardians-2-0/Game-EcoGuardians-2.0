using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private QuestManager questManager;

    private const string MODULE_NAME = "module";
    private const string QUESTIONNAIRE = "questionnaire";
    private static int currentModule;
    private bool doingTasks;
    private bool doingQuestionnaire;


    private void OnEnable()
    {
        EventManager.Quest.OnQuestAssigned += HandleQuestAssigned;
        EventManager.Quest.OnQuestCompleted += HandleQuestCompleted;
        EventManager.Quest.OnAllQuestsCompleted += HandleAllQuestCompleted;
        EventManager.Quest.OnQuestionnaireCompleted += HandleQuestionnaireCompleted;
    }

    private void OnDisable()
    {
        EventManager.Quest.OnQuestAssigned -= HandleQuestAssigned;
        EventManager.Quest.OnQuestCompleted -= HandleQuestCompleted;
        EventManager.Quest.OnAllQuestsCompleted -= HandleAllQuestCompleted;
        EventManager.Quest.OnQuestionnaireCompleted -= HandleQuestionnaireCompleted;

    }

    private void OnDestroy()
    {
        EventManager.Quest.OnQuestAssigned -= HandleQuestAssigned;
        EventManager.Quest.OnQuestCompleted -= HandleQuestCompleted;
        EventManager.Quest.OnAllQuestsCompleted -= HandleAllQuestCompleted;
        EventManager.Quest.OnQuestionnaireCompleted -= HandleQuestionnaireCompleted;

    }


    // Start is called before the first frame update
    void Start()
    {
        currentModule = 1;
        // Register for game start events
        ControllerScreensMenuUI.Instance.onGameStarted.AddListener(HandleGameStart);
    }

    // Quest Events to be handled by GameManager

    // Triggered by character assgining player quests
    private void HandleQuestAssigned()
    {
        Debug.Log("Quests Assigned");
        doingQuestionnaire = true;
        ActivateQuests(MODULE_NAME + currentModule);
    }

    // Triggered when player completes a quest
    public void HandleQuestCompleted(string taskName)
    {
        EventManager.MapIcon.OnDisplayIconFiltered(taskName).Invoke(false);
        questManager.CompleteQuest(taskName);
    }

    // Triggered when player has completed all quests
    private void HandleAllQuestCompleted()
    {
        StartCoroutine(clearQuests());
    }

    private void HandleQuestionnaireCompleted()
    {
        doingQuestionnaire = false;
        EventManager.Wall.OnDisabeWall.Invoke(currentModule);
        currentModule++;
    }

    // Called in order to clear quests from UI
    private IEnumerator clearQuests()
    {
        yield return StartCoroutine(questManager.ClearCompletedQuests());
        if (doingQuestionnaire)
        {
            DialogueManager.instance.SetVariable("global_cuestionario_" + currentModule, DialogueVariableSetter.SetVariable(true));
            ActivateQuests(QUESTIONNAIRE + currentModule);
        }
        else
        {
            questManager.SetNoTasksTitle();
        }
    }

    // Handle game start event
    private void HandleGameStart(string arg0)
    {
        AudioManager.Instance.PlayMusic(SoundType.EnviromentBackgroundMusic);
    }

    public void ActivateQuests(string file)
    {
        new JSONReader().ReadQuests(questManager, file);
    }
}
