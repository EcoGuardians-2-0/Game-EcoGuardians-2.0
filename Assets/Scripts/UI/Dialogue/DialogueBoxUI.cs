using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxUI
{
    public TextMeshProUGUI npcDialogue { get; set; }

    public TextMeshProUGUI npcName { get; set; }

    public GameObject continueIcon { get; private set;}

    public DialogueBoxUI(GameObject dialogueBox)
    {
        npcName = dialogueBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        npcDialogue = dialogueBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        continueIcon = dialogueBox.transform.GetChild(2).gameObject;
    }
}
