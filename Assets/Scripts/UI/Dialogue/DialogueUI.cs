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

    private int lastIndex;

    public void Start()
    {
        dialogueOptionBox.SetActive(false);
    }


    public void SetDialogueBox(string npcName, string npcDialogue)
    {
        this.npcName.text = npcName;
        this.npcDialogue.text = npcDialogue;
    }

    public void changeOption(int optionIndex)
    {
        Debug.Log("Received index " + optionIndex);
        deactivateButton(buttonOptionsUI[lastIndex].transform);
        activateButton(buttonOptionsUI[optionIndex].transform);
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
                Transform button = buttonOptionsUI[i].transform;

                TextMeshProUGUI text = button.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
                text.text = options[i];

                if (i != 0)
                {
                    deactivateButton(button);
                }
                else
                {
                    lastIndex = i;
                    activateButton(button);
                }
            }
            else
            {
                buttonOptionsUI[i].SetActive(false);
            }
        }
    }

    private void activateButton(Transform button)
    {
        Image panel1 = button.GetChild(0).GetComponent<Image>();
        panel1.color = new Color(242f / 255f, 1f, 1f, 1f);
        button.GetChild(1).gameObject.SetActive(true);
    }

    private void deactivateButton(Transform button)
    {
        Image panel1 = button.GetChild(0).GetComponent<Image>();
        panel1.color = new Color(242f / 255f, 211 / 255f, 211 / 255f, 122f / 255f);
        button.GetChild(1).gameObject.SetActive(false);
    }

    public void displayDialogueOptionBox(bool status)
    {
        dialogueOptionBox.SetActive(status);
    }


}

