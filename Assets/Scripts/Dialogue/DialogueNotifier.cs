using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNotifier
{
    public void CheckForEventTrigger(string variableName, Ink.Runtime.Object value)
    {
        if (variableName.StartsWith("global_misiones_"))
        {
            if(((Ink.Runtime.BoolValue) value).value == true)
                GameManager.QuestAssigned();
        }
        else if(variableName == "global_mision_completada")
        {
            string missionName = value.ToString();
            GameManager.QuestCompleted(missionName);
        }
    }
}
