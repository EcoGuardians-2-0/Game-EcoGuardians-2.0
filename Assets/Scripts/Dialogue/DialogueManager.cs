using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager instance { get; private set;}

    public DialogueUI dialogueUI;

    private int currentNodeIndex = 0;
    private int currentResponseTracker = 0;
    public bool isTalking;
    public bool isDone;

    private DialogueData npc;
    private DialogueNode currentNode;
    private Character character;

   
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        dialogueUI.dialogueUI.SetActive(false);
    }

    void Update()
    {
        if (isTalking)
        {
            dialogueUI.SetDialogueBox(currentNode.npcName, currentNode.npcDialogue);

            if (currentNode.hasOptions())
            {

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentResponseTracker++;
                    if (currentResponseTracker >= currentNode.options.Count - 1)
                    {
                        currentResponseTracker = currentNode.options.Count - 1;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentResponseTracker--;
                    if (currentResponseTracker < 0)
                    {
                        currentResponseTracker = 0;
                    }
                }

                dialogueUI.changeOption(currentResponseTracker);

            }


            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (currentNode.hasOptions())
                {
                    currentNodeIndex = currentNode.options[currentResponseTracker].nextDialogId-1;
                    currentResponseTracker = 0;
                }
                else
                {
                    if (!currentNode.dialogueHasEnded())
                    {
                        currentNodeIndex = currentNode.nextDialogueID-1;
                    }
                    else
                    {
                        isDone = true;
                        character.Interact();
                    }
                }

                currentNode = npc.dialogNodes[currentNodeIndex];
                checkHasDialogueOptions(currentNode);
            } 
        }
    }

    public List<string> obtainDialogOptions(DialogueNode node)
    {
        List<string> options = new List<string>();
        for(int i =  0; i < node.options.Count; i++)
        {
            options.Add(node.options[i].optionText);
        }
        return options;

    }

    public void displayDialogueOptions(bool activate)
    {
        dialogueUI.displayDialogueOptionBox(activate);
    }

    public void checkHasDialogueOptions(DialogueNode node)
    {
        if (node.hasOptions())
        {
            displayDialogueOptions(true);
            dialogueUI.setOptions(obtainDialogOptions(node));
        }
        else
        {
            displayDialogueOptions(false);
        }
    }

    public void StartConversation(DialogueData npc, Character character)
    {
        currentResponseTracker = 0;
        currentNodeIndex = 0;
        isTalking = true;
        isDone = false;

        // Activate UI Dialogue
        dialogueUI.dialogueUI.SetActive(true);
        this.npc = npc;
        this.character = character;

        currentNode = npc.dialogNodes[currentNodeIndex];

        checkHasDialogueOptions(currentNode);


    }

    public void EndDialogue()
    {
        isTalking = false;

        dialogueUI.dialogueUI.SetActive(false);
    }
}
