using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private QuestManager questManager;

    private GameStage currentStage = GameStage.None;
    private List<GameStage> questionnaireStages;
    private const string MODULE_NAME = "module";
    private const string QUESTIONNAIRE = "questionnaire";
    private static int currentModule;
    private bool doingTasks;
    private bool doingQuestionnaire;
    private bool advancingLevel = false;


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

    void Update()
    {
        // Detectar tecla 'U' para saltar de nivel
        if (Input.GetKeyDown(KeyCode.U) && !advancingLevel)
        {
            if(currentStage < GameStage.GameComplete)
                StartCoroutine(SkipLevel());
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // Current module at the start of the game is 1
        currentModule = 1;
        currentStage++;
        questionnaireStages = new List<GameStage>
        {
            GameStage.Questionnaire1,
            GameStage.Questionnaire2,
            GameStage.Questionnaire3,
        };
        

        // Register for game start events
        ControllerScreensMenuUI.Instance.onGameStarted.AddListener(HandleGameStart);
    }

    // Quest Events to be handled by GameManager

    // Triggered by character assgining player quests
    private void HandleQuestAssigned()
    {
        Debug.Log("Assigning quests");
        StartCoroutine(LoadCurrentStageFile());
    }

    // Triggered when player completes a quest
    public void HandleQuestCompleted(string taskName)
    {
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

    private IEnumerator SkipLevel()
    {
        advancingLevel = true;

        // 1. Limpiar todas las tareas con el QuestManager
        Debug.Log("Clearing all tasks before advancing to the next stage...");
        yield return StartCoroutine(questManager.ClearAllQuests());

        // 2. Verificar si el nivel anterior era un cuestionario y manejarlo
        GameStage previousStage = currentStage;
        currentStage++;  // Avanzar al siguiente nivel

        if (IsStageQuestionnaire(previousStage))
        {
            HandleQuestionnaireCompleted();  // Llamar al m�todo para manejar la finalizaci�n del cuestionario
        }

        StartCoroutine(LoadCurrentStageFile());

        advancingLevel = false;
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
            GameStage prevStage = currentStage;
            Debug.Log("Previous state: " + currentStage);
            currentStage++;
            Debug.Log("Moving to next state: " + currentStage);
            if(IsStageQuestionnaire(currentStage))
                StartCoroutine(LoadCurrentStageFile());
        }
        else{
            Debug.Log("The game has finished");
        }
    }
    private IEnumerator LoadCurrentStageFile()
    {
        while (questManager.isClearingQuests)
        {
            yield return null;
        }

        if (currentStage == GameStage.GameComplete)
        {
            Debug.Log("Game has finished");
            questManager.SetNoTasksTitle();
            DialogueManager.instance.SetVariable("global_pass_" + currentModule, DialogueVariableSetter.SetVariable(true));
            yield break;
        }

        if (IsStageQuestionnaire(currentStage))
        {
            Debug.Log("Enabling questionnaire for module: " + currentModule);
            DialogueManager.instance.SetVariable("global_cuestionario_"+currentModule,DialogueVariableSetter.SetVariable(true));
        }

        string fileName = currentStage.ToString().ToLower();
        Debug.Log("Loading file: " + fileName);
        new JSONReader().ReadQuests(questManager, fileName);
    }

    private bool IsStageQuestionnaire(GameStage currentStage)
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
