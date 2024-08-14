using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialogueUI;
    public GameObject dialogueOptionBox;

    [SerializeField]
    private List<GameObject> buttonOptionsUI;
    private List<DialogueButtonUI> buttonsUI;

    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDialogue;

    private int lastIndex;

    public void Start()
    {
        buttonsUI = new List<DialogueButtonUI>();
        dialogueOptionBox.SetActive(false);

        Debug.Log("Hijos de buttonOptionsUI " +  buttonOptionsUI.Count);
        foreach(GameObject button in buttonOptionsUI)
        {
            buttonsUI.Add(new DialogueButtonUI(button));
        }

        Debug.Log("Hijos de buttonUI " + buttonsUI.Count);


    }


    public void SetDialogueBox(string npcName, string npcDialogue)
    {
        this.npcName.text = npcName;
        this.npcDialogue.text = npcDialogue;
    }

    public void changeOption(int optionIndex)
    {
        buttonsUI[lastIndex].deactivateButton();
        buttonsUI[optionIndex].activateButton();
        lastIndex = optionIndex;
    }

    public void setOptions(List<string> options)
    {
        for (int i = 0; i < buttonOptionsUI.Count; i++)
        {
            Debug.Log("Indice es " + i + " y la cantidad de opciones son " + options.Count);
            if (i < options.Count)
            {
                buttonOptionsUI[i].SetActive(true);
                buttonsUI[i].setText(options[i]);

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


}

