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
                EventManager.Quest.OnQuestAssigned.Invoke();
        }
        else if(variableName == "global_mision_completada")
        {
            Debug.Log("Completed mission");
            string missionName = value.ToString();
            EventManager.Quest.OnQuestCompleted.Invoke(missionName);
        }
        else if (variableName.StartsWith("global_pass"))
        {
            if(((Ink.Runtime.BoolValue)value).value)
            {
                EventManager.Quest.OnQuestionnaireCompleted.Invoke();
                EventManager.Quest.OnQuestAssigned.Invoke();
            }
        }
    }
}
