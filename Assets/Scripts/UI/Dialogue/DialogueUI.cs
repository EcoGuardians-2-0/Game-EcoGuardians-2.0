using Ink.Runtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialogueUI;

    [SerializeField]
    private List<GameObject> buttonOptionsUI;
    [SerializeField]
    private GameObject dialogueOptionBox;
    [SerializeField]
    private GameObject dialogueBox;


    private List<DialogueButtonUI> buttonsUI;
    private DialogueBoxUI dialogueBoxUI { get; set; }

    private int lastIndex;


    public void Start()
    {
        buttonsUI = new List<DialogueButtonUI>();
        dialogueOptionBox.SetActive(false);
        dialogueBox.SetActive(true);
        foreach(GameObject button in buttonOptionsUI)
        {
            buttonsUI.Add(new DialogueButtonUI(button));
        }

        dialogueBoxUI = new DialogueBoxUI(dialogueBox);
    }

    public DialogueBoxUI getDialogueBox() {
        return dialogueBoxUI;
    }

    public void changeOption(int optionIndex)
    {
        buttonsUI[lastIndex].deactivateButton();
        buttonsUI[optionIndex].activateButton();
        lastIndex = optionIndex;
    }

    public void setOptions(List<Choice> choice)
    {
        for (int i = 0; i < buttonOptionsUI.Count; i++)
        {
            if (i < choice.Count)
            {
                buttonOptionsUI[i].SetActive(true);
                buttonsUI[i].setText(choice[i].text);

                if (i != 0)
                {
                    buttonsUI[i].deactivateButton();
                }
                else
                {
                    buttonsUI[i].activateButton();
                }
            }
            else
            {
                buttonOptionsUI[i].SetActive(false);
            }
        }
    }

    public void displayDialogueOptionBox(bool status)
    {
        dialogueOptionBox.SetActive(status);
    }

    public int getChoiceCount()
    {
        return buttonsUI.Count;
    }

}

