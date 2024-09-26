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

    private bool questActive;

    private string module;
    private int currentModule;

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
    private void OnEnable()
    {
        OnQuestAssigned += HandleQuestAssigned;
        OnQuestCompleted += HandleQuestCompleted;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentModule = 1;
        questActive = false;
        module = "module";

        // Register for activity start events
        DialogueManager.instance.onActivityStarted.AddListener(HandleActivityStart);
        // Register for game start events
        ControllerScreensMenuUI.Instance.onGameStarted.AddListener(HandleGameStart);
    }

    // Update is called once per frame
    void Update()
    {
        // Retrieve Ink variable
        Ink.Runtime.Object global_mission = DialogueManager.instance.GetVariableState("global_misiones_" + currentModule);

        if (questActive)
        {
            string mission_name = "";

            Ink.Runtime.Object completed_mission = DialogueManager.instance.GetVariableState("global_mision_completada");

            if (completed_mission != null)
            {
                mission_name = ((Ink.Runtime.StringValue)completed_mission).value;
            }

            if (mission_name != "")
            {
                questManager.CompleteQuest(mission_name);
                Ink.Runtime.StringValue mission = new Ink.Runtime.StringValue("");
                DialogueManager.instance.dialogueVariables.variables["globals"]["global_mision_completada"] = mission;
            }

            if (questManager.AllQuestsCompleted())
            {
                Ink.Runtime.BoolValue questionnaire = new Ink.Runtime.BoolValue(true);
                DialogueManager.instance.dialogueVariables.variables["globals"]["global_cuestionario_" + currentModule] = questionnaire;
            }
        }
    }

    private void HandleQuestAssigned()
    {
        Debug.Log("Quests Assigned");
        ActivateQuests(module + currentModule);
    }
    private void HandleQuestCompleted(string taskName)
    {
        questManager.CompleteQuest(taskName);
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

    public void ActivateQuests(string module)
    {
        new JSONReader().ReadQuests(questManager, module);
    }
}
