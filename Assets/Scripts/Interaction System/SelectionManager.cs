using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{

    public static SelectionManager instance;
    public bool isInteracting;
    public bool onTarget;
    public bool canHighlight;
    public bool oneTimeInteraction;

    [SerializeField]
    private GameObject textBox;

    TextMeshProUGUI interaction_text;

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
        canHighlight = true;
        interaction_text = textBox.GetComponentInChildren<Image>().GetComponentInChildren<TextMeshProUGUI>();
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

            if (interactable != null && (interactable.playerInRange || oneTimeInteraction))
            {
                if (interactable != lastInteractable)
                {
                    lastInteractable = interactable;
                }


                interactable.GetComponent<Outline>().enabled = canHighlight;
                SetText(interactable.GetSelectionPrompt());

                if (Input.GetKeyDown(KeyCode.E) && !isInteracting )
                {
                    Debug.Log("Interacting with " + interactable.name);
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