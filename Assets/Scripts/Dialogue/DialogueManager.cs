using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField]
    private float typingSpeed = 0.04f;
    public static DialogueManager instance { get; private set; }

    [Header("Dialogue UI")]
    public DialogueUI dialogueUI;

    [Header("Globals Ink File")]
    [SerializeField]
    private TextAsset loadGlobalsJSON;



    public bool isTalking { get; private set; }

    private Story currentStory;
    private int currentChoiceIndex = 0;
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;

    private bool submitSkip = false;
    private bool canSkip;

    public bool canPlay { get; private set; }
    public bool hasSkipped { get; private set; }
    public bool hasFinished { get; private set; }

    public string currentSpeaker { get; private set; }
    public int currentAnimation { get; private set; }

    private const string SPEAKER_TAG = "speaker";
    private const string ANIMATION_TAG = "animation";
    private const string START_ACTIVITY_TAG = "Activity";

    public DialogueVariables dialogueVariables { get; private set; }

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
        dialogueVariables = new DialogueVariables();
        dialogueVariables.initializeVariables(loadGlobalsJSON);
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
        SelectionManager.instance.isInteracting = true;
        SelectionManager.instance.canHighlight = false;
        DisableObjects.Instance.ToggleSelectionCursor();
        DisableObjects.Instance.ToggleTooltip();
        DisableObjects.Instance.disableCharacterController();
        DisableObjects.Instance.disableCameras();
        DisableObjects.Instance.disableSwitchCamera();
        EventManager.Photograph.OnActiveCamera(false);

        currentStory = new Story(inkJSON.text);
        currentSpeaker = "";
        currentAnimation = -1;

        dialogueVariables.initializeVariables(inkJSON);
        dialogueVariables.StartListening(currentStory);

        isTalking = true;
        dialogueUI.dialogueUI.SetActive(true);
        ContinueStory();
    }

    private void EndDialogue()
    {
        dialogueVariables.StopListening(currentStory);
        isTalking = false;
        dialogueUI.dialogueUI.SetActive(false);
        SelectionManager.instance.isInteracting = false;
        SelectionManager.instance.canHighlight = true;
        EventManager.Photograph.OnActiveCamera(true);
        DisableObjects.Instance.ToggleSelectionCursor();
        DisableObjects.Instance.ToggleTooltip();
        DisableObjects.Instance.disableCharacterController();
        DisableObjects.Instance.disableCameras();
        DisableObjects.Instance.disableSwitchCamera();
        CameraUtilityManager.Instance.SetCameraOff();
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
                AudioManager.Instance.PlaySound(SoundType.dialogOptionSelection);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentChoiceIndex--;
                if (currentChoiceIndex < 0)
                    currentChoiceIndex = choices.Count - 1;
                AudioManager.Instance.PlaySound(SoundType.dialogOptionSelection);
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
                AudioManager.Instance.PlaySound(SoundType.dialogSelectedOption);
            }
            else
            {
                ContinueStory();
                AudioManager.Instance.PlaySound(SoundType.dialogForward);
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
        Debug.Log("Story: " + line);

        bool isAddingRichTextTag = false;
        dialogueUI.getDialogueBox().npcDialogue.text = "";

        canContinueToNextLine = false;
        submitSkip = false;
        canPlay = true;
        hasSkipped = false;
        hasFinished = false;

        dialogueUI.getDialogueBox().continueIcon.SetActive(false);
        dialogueUI.displayDialogueOptionBox(false);

        StartCoroutine(CanSkip());

        for (int i = 0; i < line.ToCharArray().Length; i++)
        {
            char letter = line[i];
            if (submitSkip && canSkip)
            {
                submitSkip = false;
                hasSkipped = true;
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

        canPlay = false;
        hasFinished = true;

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
                Debug.LogError("Tag could not be parsed: " + tag);
                continue;
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    Debug.Log("Speaker: " + tagValue);
                    currentSpeaker = tagValue;
                    dialogueUI.getDialogueBox().npcName.text = tagValue;
                    break;
                case ANIMATION_TAG:
                    Debug.Log("Animation: " + tagValue);
                    currentAnimation = int.Parse(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag not supported: " + tag);
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
            dialogueUI.displayDialogueOptionBox(true);
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

    public void SetVariable(string name, Ink.Runtime.Object value)
    {
        dialogueVariables.setVariableValue(name, value);
    }
}
