using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public int dialogueID;
    public string npcName;
    [TextArea(1, 3)]
    public string npcDialogue;
    public List<DialogOption> options;
    public int nextDialogueID;


    public DialogueNode(int dialogueID, string npcName, string npcDialogue, List<DialogOption> options = null, int nextDialogueID = -1)
    {
        this.dialogueID = dialogueID;
        this.npcName = npcName;
        this.npcDialogue = npcDialogue;
        this.options = options ?? new List<DialogOption>();
        this.nextDialogueID = nextDialogueID;
    }

    public bool hasOptions()
    {
        return options != null && options.Count > 0;
    }

    public bool dialogueHasEnded()
    {
        return nextDialogueID < 0;
    }

    [Serializable]
    public class DialogOption
    {
        public int optionId;
        [TextArea(1, 1)]
        public string optionText;
        public int nextDialogId;

        public DialogOption(int optionId, string optionText, int nextDialogID)
        {
            this.optionId = optionId;
            this.optionText = optionText;
            this.nextDialogId = nextDialogID;
        }
    }
}
