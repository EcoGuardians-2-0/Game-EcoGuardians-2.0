using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Show or hide the list of tasks by pressing 'Tab'
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab pressed: " + AnimationsQuest.Instance.GetQuestsState());
            if (!AnimationsQuest.Instance.GetQuestsState())
            {
                AnimationsQuest.Instance.ShowQuestsUI();
                Debug.Log("ShowQuestsUI");
            }
            else
            {
                AnimationsQuest.Instance.HideQuestsUI();
                Debug.Log("HideQuestsUI");
            }
                
        }
    }
}
