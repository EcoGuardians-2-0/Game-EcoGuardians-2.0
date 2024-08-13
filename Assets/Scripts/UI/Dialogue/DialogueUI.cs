using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialogueUI;
    public GameObject dialogueOptionBox;

    public List<GameObject> buttonOptionsUI;

    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDialogue;



    public void SetDialogueBox(string npcName, string npcDialogue)
    {
        this.npcName.text = npcName;
        this.npcDialogue.text = npcDialogue;
    }

    public void setOptions(List<string> options)
    {
        for (int i = 0; i < buttonOptionsUI.Count; i++)
        {
            Debug.Log("Indice es " + i + " y la cantidad de opciones son " + options.Count);
            if (i < options.Count)
            {
                buttonOptionsUI[i].SetActive(true);
                Transform button = buttonOptionsUI[i].transform;

                TextMeshProUGUI text = button.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
                text.text = options[i];

                if (i != 0)
                {
                    button.GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    button.GetChild(1).gameObject.SetActive(true);
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

