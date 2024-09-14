using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private QuestManager questManager;

    private bool questActive;
    private int currentModule;

    // Start is called before the first frame update
    void Start()
    {
        currentModule = 1;
        questActive = false;

        // Register for activity start events
        DialogueManager.instance.onActivityStarted.AddListener(HandleActivityStart);
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
                DialogueManager.instance.dialogueVariables.variables2["globals"]["global_mision_completada"] = mission;
            }

            if (questManager.AllQuestsCompleted())
            {
                Ink.Runtime.BoolValue questionnaire = new Ink.Runtime.BoolValue(true);
                DialogueManager.instance.dialogueVariables.variables2["globals"]["global_cuestionario_" + currentModule] = questionnaire;
            }
        }
        else
        {
            if (global_mission != null)
            {
                if (((Ink.Runtime.BoolValue)global_mission).value)
                {
                    ActivateQuests("module1");
                    questActive = true;
                }
            }
        }
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
    }


    public void ActivateQuests(string module)
    {
        new JSONReader().ReadQuests(questManager, module);
    }
}
