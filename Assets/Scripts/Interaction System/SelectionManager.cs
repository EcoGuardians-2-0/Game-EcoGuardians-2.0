using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    public bool onTarget;

    [SerializeField]
    private GameObject textBox;

    Text interaction_text;

    private void Start()
    {
        onTarget = false;
        interaction_text = textBox.GetComponentInChildren<Image>().GetComponentInChildren<Text>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool isHit = Physics.Raycast(ray, out hit);
        if (isHit)
        {
            var selectionTransform = hit.transform;

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            if (interactable && interactable.playerInRange)
            {
                SetText(interactable.GetSelectionPrompt());

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }
            else
            {
                textBox.SetActive(false);
            }

        }
        else
        {
            textBox.SetActive(false);
        }
    }

    private void SetText(string toolTiptext)
    {
        interaction_text.text = toolTiptext;
        textBox.SetActive(true);
    }
}