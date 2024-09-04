using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private QuestManager questManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Obtén la variable desde Ink
         Ink.Runtime.Object global_mission = DialogueManager.instance.GetVariableState("global_misiones_1");

        if (global_mission != null)
        {
            if (((Ink.Runtime.BoolValue)global_mission).value)
            {
                if (questManager.GetActiveQuestsCount() == 0)
                    ActivateQuests("module1");
            }
        }

        Ink.Runtime.Object completed_mission = DialogueManager.instance.GetVariableState("global_mision_completada");

        if (completed_mission != null)
        {
            string mission_name = ((Ink.Runtime.StringValue)completed_mission).value;
            if (mission_name != "")
            {
                questManager.CompleteQuest(mission_name);
                mission_name = "";
            }
        }

    }

    public void ActivateQuests(string module)
    {
        new JSONReader().ReadQuests(questManager, module);
    }
}
