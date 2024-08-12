using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    public static SelectionManager instance;
    public bool isInteracting;
    public bool onTarget;

    [SerializeField]
    private GameObject textBox;

    Text interaction_text;

    private InteractableObject interactable;
    private InteractableObject lastInteractable;

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


        if (lastInteractable != null && (!isHit || lastInteractable != hit.transform.GetComponent<InteractableObject>() || !lastInteractable.playerInRange))
        {
            lastInteractable.GetComponent<Outline>().enabled = false;
            lastInteractable = null;
        }

        if (isHit)
        {
            var selectionTransform = hit.transform;

            interactable = selectionTransform.GetComponent<InteractableObject>();

            if (interactable != null && interactable.playerInRange)
            {
                if (interactable != lastInteractable)
                {
                    lastInteractable = interactable;
                }


                interactable.GetComponent<Outline>().enabled = !isInteracting;
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