using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField]
    private float typingSpeed = 0.04f;
    public static DialogueManager instance { get; private set; }

    [Header("Dialogue UI")]
    public DialogueUI dialogueUI;

    public bool isTalking { get; private set; }

    private Story currentStory;
    private int currentChoiceIndex = 0;
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;

    private bool submitSkip = false;
    private bool canSkip;

    private const string SPEAKER_TAG = "speaker";



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
        isTalking = false;
        currentChoiceIndex = 0;
        dialogueUI.dialogueUI.SetActive(false);
    }

    void Update()
    {
        if (isTalking)
        {
            HandleInput();
        }
    }


    public void StartConversation(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        isTalking = true;
        dialogueUI.dialogueUI.SetActive(true);
        ContinueStory();
    }

    private void EndDialogue()
    {
        isTalking = false;
        dialogueUI.dialogueUI.SetActive(false);
        SelectionManager.instance.isInteracting = false;
        DisableObjects.Instance.disableCharacterController();
        DisableObjects.Instance.disableCameras();
        DisableObjects.Instance.disableSwitchCamera();
    }

    private void HandleInput()
    {
        List<Choice> choices = currentStory.currentChoices;


        if (choices.Count > 0 && canContinueToNextLine)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentChoiceIndex++;
                if (currentChoiceIndex >= choices.Count)
                    currentChoiceIndex = 0;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentChoiceIndex--;
                if (currentChoiceIndex < 0)
                    currentChoiceIndex = choices.Count - 1;
            }

            dialogueUI.changeOption(currentChoiceIndex);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            submitSkip = true;
        }


        if (canContinueToNextLine && submitSkip)
        {
            if (choices.Count > 0)
            {
                MakeChoice();
            }
            else
            {
                ContinueStory();
            }
        }
    }

    private IEnumerator CanSkip()
    {
        canSkip = false;
        yield return new WaitForSeconds(0.05f);
        canSkip = true;
    }

    private IEnumerator DisplayLine(string line)
    {
        bool isAddingRichTextTag = false;
        dialogueUI.getDialogueBox().npcDialogue.text = "";

        canContinueToNextLine = false;
        submitSkip = false;
        dialogueUI.getDialogueBox().continueIcon.SetActive(false);
        dialogueUI.displayDialogueOptionBox(false);

        StartCoroutine(CanSkip());

        for (int i = 0; i < line.ToCharArray().Length; i++)
        {
            char letter = line[i];
            if (submitSkip && canSkip)
            {
                submitSkip = false;
                dialogueUI.getDialogueBox().npcDialogue.text = line;
                break;
            }

            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                dialogueUI.getDialogueBox().npcDialogue.text += letter;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                dialogueUI.getDialogueBox().npcDialogue.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        dialogueUI.getDialogueBox().continueIcon.SetActive(true);
        DisplayChoices();
        canContinueToNextLine = true;
        canSkip = false;
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            HandleTags(currentStory.currentTags);
        }
        else
        {
            EndDialogue();
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could  not be parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    dialogueUI.getDialogueBox().npcName.text = tagValue;
                    break;
                default:
                    Debug.LogWarning("Tag  came in but not supporte" + tag);
                    break;
            }
        }
    }

    public void DisplayChoices()
    {
        List<Choice> choices = currentStory.currentChoices;
        if (choices.Count > 0)
        {
            if (choices.Count > dialogueUI.getChoiceCount())
            {
                Debug.LogError("More choices than UI can support");
            }
            dialogueUI.displayDialogueOptionBox(true); ;
            dialogueUI.setOptions(choices);
        }

    }



    private void MakeChoice()
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(currentChoiceIndex);
            currentChoiceIndex = 0;
            ContinueStory();
        }
    }

}
