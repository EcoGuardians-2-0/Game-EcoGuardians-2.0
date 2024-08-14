using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueButtonUI
{
    private TextMeshProUGUI optionText;
    private Image panel1;
    private GameObject hand;
    private GameObject buttonPanel;

    public DialogueButtonUI(GameObject buttonPanel)
    {
        optionText = buttonPanel.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        panel1 = buttonPanel.transform.GetChild(0).GetComponent<Image>();
        hand = buttonPanel.transform.GetChild(1).gameObject;

    }

    public void setText(string text)
    {
        optionText.text = text;
    }

    public void activateButton()
    {
        panel1.color = new Color(242f / 255f, 1f, 1f, 1f);
        hand.SetActive(true);
    }

    public void deactivateButton()
    {
        panel1.color = new Color(242f / 255f, 211 / 255f, 211 / 255f, 122f / 255f);
        hand.SetActive(false);
    }
}
