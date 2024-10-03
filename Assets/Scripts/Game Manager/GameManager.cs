using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private QuestManager questManager;

    private GameStage currentStage;
    private List<GameStage> questionnaireStages;
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
        // Current module at the start of the game is 1
        currentModule = 1;
        currentStage = GameStage.Module1;
        questionnaireStages = new List<GameStage>
        {
            GameStage.Questionnaire1,
            GameStage.Questionnaire2,
        };

        // Register for game start events
        ControllerScreensMenuUI.Instance.onGameStarted.AddListener(HandleGameStart);
    }

    // Quest Events to be handled by GameManager

    // Triggered by character assgining player quests
    private void HandleQuestAssigned()
    {
        Debug.Log("Assigning quests");
        LoadCurrentStageFile();
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
        StartCoroutine(ClearAndAdvance());
    }

    // Triggered when player has answered the questionnaire correctly
    private void HandleQuestionnaireCompleted()
    {
        if(currentStage < GameStage.GameComplete)
        {
            EventManager.Wall.OnDisabeWall.Invoke(currentModule);
            currentModule++;
        }
    }

    // Called in order to clear quests from UI
    private IEnumerator ClearAndAdvance()
    {
        yield return StartCoroutine(questManager.ClearCompletedQuests());
        questManager.SetNoTasksTitle();
        AdvanceToNextStage();
    }
    
    private void AdvanceToNextStage()
    {
        if(currentStage < GameStage.GameComplete)
        {
            Debug.Log("Moving to next state: " + currentStage);
            currentStage++;
            Debug.Log("Checking if the next stage is questionnaire so it loads the quests of that questionnaire");
            if(IsCurrentStageQuestionnaire())
                LoadCurrentStageFile();
        }
        else{
            Debug.Log("The game has finished");
        }
    }
    private void LoadCurrentStageFile()
    {
        if(currentStage == GameStage.GameComplete)
        {
            Debug.Log("Game has finished");
            return;
        }

        if (IsCurrentStageQuestionnaire())
        {
            Debug.Log("Enabling questionnaire for module: " + currentModule);
            DialogueManager.instance.SetVariable("global_cuestionario_"+currentModule,DialogueVariableSetter.SetVariable(true));
        }

        string fileName = currentStage.ToString().ToLower();
        Debug.Log("Loading file: " + fileName);
        new JSONReader().ReadQuests(questManager, fileName);
    }

    private bool IsCurrentStageQuestionnaire()
    {
        return questionnaireStages.Contains(currentStage);
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
