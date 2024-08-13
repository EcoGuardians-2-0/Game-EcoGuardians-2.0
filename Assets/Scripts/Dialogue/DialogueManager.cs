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
    private bool isTalking;

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
            currentNode = npc.dialogNodes[currentNodeIndex];
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
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentResponseTracker--;
                    if (currentResponseTracker < 0)
                    {
                        currentResponseTracker = 0;
                    }
                }
            }
            else
            {
                dialogueUI.displayDialogueOptionBox(false);
            }


            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (currentNode.hasOptions())
                {
                    Debug.Log("La opcion seleccionada fue " + currentNode.options[currentResponseTracker].optionText);
                    currentNodeIndex = currentNode.options[currentResponseTracker].nextDialogId-1;
                    Debug.Log("Current index"+currentNodeIndex);
                    currentResponseTracker = 0;
                }
                else
                {
                    Debug.Log("Dialogo no tiene opciones");
                    if (!currentNode.dialogueHasEnded())
                    {
                        currentNodeIndex = currentNode.nextDialogueID-1;
                        if (npc.dialogNodes[currentNodeIndex].hasOptions())
                        {
                            Debug.Log("Cargar las opciones");
                            dialogueUI.setOptions(obtainDialogOptions(npc.dialogNodes[currentNodeIndex]));
                            dialogueUI.displayDialogueOptionBox(true);
                        }
                        
                    }
                    else
                    {
                        character.Interact();
                    }
                }
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

    public void StartConversation(DialogueData npc, Character character)
    {
        currentResponseTracker = 0;
        currentNodeIndex = 0;
        isTalking = true;

        dialogueUI.dialogueUI.SetActive(true);

        this.npc = npc;
        this.character = character;
    }

    public void EndDialogue()
    {
        isTalking = false;

        dialogueUI.dialogueUI.SetActive(false);
    }
}
