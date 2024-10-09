using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public Image[] keyUI;
    public GameObject tutorialUI;
    public KeyCode[] keyCodes = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.W };
    private Color targetColor = new Color32(44, 87, 41, 255);
    private Color principalColor = new Color32(0, 0, 0, 255);
    public GameObject Welcome;
    public GameObject Tutorial_Start;
    public GameObject Movement;
    public GameObject Pause;
    public GameObject Run;
    public GameObject Jump;
    public GameObject Camera;
    public GameObject Camera_Control;
    public GameObject Task;
    public GameObject Interactable;
    public GameObject Minimap;
    public GameObject NPC;
    public GameObject NPC2;
    public GameObject NPC3;
    public GameObject T1;
    public GameObject T2;
    public GameObject T3;
    public GameObject T4;
    public GameObject T5;
    public GameObject T6;
    public GameObject T7;
    public GameObject T8;
    public GameObject T9;
    public GameObject T10;
    public GameObject WallCollider;
    public GameObject TvOn;
    public GameObject Pausa;
    public GameObject AlertStart;
    public Graphic Enter_UI;
    public Graphic Enter_UI_T10;
    public Graphic A2_UI;
    public Graphic S2_UI;
    public Graphic D2_UI;
    public Graphic W2_UI;
    public Graphic Space_UI;
    public Graphic Shift_UI;
    public Graphic E_UI;
    public Graphic E2_UI;
    public Graphic M_UI;
    public Graphic M2_UI;
    public Graphic P_UI;
    public Graphic P2_UI;
    public Graphic C_UI;
    public Graphic C2_UI;
    public Graphic Ctrl_UI;
    public Graphic Tab_UI;
    public Graphic Tab2_UI;
    public Graphic Left_UI;
    public Graphic Right_UI;
    private float keyHoldTime = 0f;
    private bool isKeyHeld = false;
    private float shiftKeyStartTime = 0f; 
    private bool isHoldingShiftAndKey = false;
    private float[] keyStartTimes;
    private bool[] isColorChanged;
    private bool isTransitioning = false;
    // private bool isTimerActive = false;
    private bool isCharacterActive = false;
    // private float timer = 0f;
    // private float alertDuration = 3f;
    
    void Start()
    {
        keyStartTimes = new float[keyCodes.Length];
        isColorChanged = new bool[keyCodes.Length];
    }
    
    private void OnEnable()
    {
        EventManager.Tutorial.OnFinishedTutorialDialogue += HandleOnFinishedTutorial;
    }

    private void OnDisable()
    {
        EventManager.Tutorial.OnFinishedTutorialDialogue -= HandleOnFinishedTutorial;
    }

    void Update()
    {
        if(!isCharacterActive && T1.activeSelf){
            DisableObjects.Instance.disableCharacterController();
            isCharacterActive = true;
        }    
        if (Input.GetKeyDown(KeyCode.L))
            StartCoroutine(FinishTutorial());
        if (!isTransitioning)
        {
            if (Welcome.activeSelf)
                HandleEnterKey(Welcome, Tutorial_Start);
            else if (Tutorial_Start.activeSelf)
                HandleEnterKey(Tutorial_Start, Movement);
            else if (Movement.activeSelf)
                HandleEnterKey(Movement, T1);
        }
        if (T1.activeSelf)
            VerifyKeyT1();
        else if (Pause.activeSelf)
            HandleEnterKey(Pause, T2);
        else if (T2.activeSelf)
            VerifyKeyT2();
        else if (Run.activeSelf)
            HandleEnterKey(Run, T3);
        else if (T3.activeSelf)
            VerifyKeyT3();
        else if (Jump.activeSelf)
            HandleEnterKey(Jump, T4);
        else if (T4.activeSelf)
            VerifyKeyT4();
        else if (Camera.activeSelf)
            HandleEnterKey(Camera, T5);
        else if (T5.activeSelf)
            VerifyKeyT5();
        else if (Camera_Control.activeSelf)
            HandleEnterKey(Camera_Control, T6);
        else if (T6.activeSelf)
            VerifyKeyT6();
        else if (Task.activeSelf)
            HandleEnterKey(Task, T7);
        else if (T7.activeSelf)
            VerifyKeyT7();
        else if (Interactable.activeSelf)
            HandleEnterKey(Interactable, T8);
        else if (T8.activeSelf)
            VerifyKeyT8();
        else if (Minimap.activeSelf)
            HandleEnterKey(Minimap, T9);
        else if (T9.activeSelf)
            VerifyKeyT9();
        else if (NPC.activeSelf)
            HandleEnterKey(NPC, NPC2);
        else if (NPC2.activeSelf)
            HandleEnterKey(NPC2, NPC3);
        else if (NPC3.activeSelf)
        {
            DialogueManager.instance.SetVariable("global_tutorial", DialogueVariableSetter.SetVariable(true));
            HandleEnterKey(NPC3, T10);
        }
        else if (T10.activeSelf)
            VerifyKeyT10();
    }

    public void HandleEnterKey(GameObject currentScreen, GameObject nextScreen)
    {
        if (Enter_UI.color == targetColor && !isTransitioning)
        {
            currentScreen.SetActive(false);
            nextScreen.SetActive(true);
            isTransitioning = true;
        }
        
        if (Input.GetKey(KeyCode.Return) && Enter_UI.color == principalColor && !isTransitioning)
        {
            Enter_UI.color = targetColor;
            StartCoroutine(RestoreColorAfterDelay());
        }
    }

    private IEnumerator RestoreColorAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        Enter_UI.color = principalColor;
        isTransitioning = false;
    }

    public void VerifyKeyT1()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKey(keyCodes[i]))
            {
                if (keyStartTimes[i] == 0)
                    keyStartTimes[i] = Time.time; 
                if (Time.time - keyStartTimes[i] >= 0.5f && !isColorChanged[i])
                {
                    isColorChanged[i] = true;
                    keyUI[i].color = targetColor;
                }
            }
            else
            {
                if (!isColorChanged[i])
                {
                    keyUI[i].color = principalColor;
                    keyStartTimes[i] = 0; 
                }
            }
        }
    
        if (keyUI[0].color == targetColor && keyUI[1].color == targetColor && keyUI[2].color == targetColor && keyUI[3].color == targetColor)
        {
            T1.SetActive(false);
            Pause.SetActive(true);
        }
    }
    public void VerifyKeyT2()
    {
        if (Input.GetKeyDown(KeyCode.P))
            P_UI.color = targetColor;
        if (Input.GetKeyDown(KeyCode.P) && P_UI.color == targetColor && Pausa.activeSelf)
            P2_UI.color = targetColor;

        if (P_UI.color == targetColor && !Pausa.activeSelf)
        {
            T2.SetActive(false);
            Run.SetActive(true);
        }
    }

    public void VerifyKeyT3()
    {
        Shift_UI.color = Input.GetKey(KeyCode.LeftShift) ? targetColor : principalColor;
        A2_UI.color = Input.GetKey(KeyCode.A) ? targetColor : principalColor;
        S2_UI.color = Input.GetKey(KeyCode.S) ? targetColor : principalColor;
        D2_UI.color = Input.GetKey(KeyCode.D) ? targetColor : principalColor;
        W2_UI.color = Input.GetKey(KeyCode.W) ? targetColor : principalColor;
        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            if (!isHoldingShiftAndKey)
            {
                shiftKeyStartTime = Time.time;
                isHoldingShiftAndKey = true; 
            }

            if (Time.time - shiftKeyStartTime >= 2f)
            {
                T3.SetActive(false);
                Jump.SetActive(true);
                isHoldingShiftAndKey = false;
            }
        }
        else
        {
            isHoldingShiftAndKey = false;
        }

    }
    public void VerifyKeyT4()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Space_UI.color = targetColor;
        if (Space_UI.color == targetColor)
        {
            T4.SetActive(false);
            Camera.SetActive(true);
        }
    }

    public void VerifyKeyT5()
    {
        if (Input.GetKeyDown(KeyCode.C) && C_UI.color == targetColor)
            C2_UI.color = targetColor;
        if (Input.GetKeyDown(KeyCode.C))
            C_UI.color = targetColor;

        if (C2_UI.color == targetColor)
        {
            T5.SetActive(false);
            Camera_Control.SetActive(true);
        }
    }
    public void VerifyKeyT6()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Ctrl_UI.color = targetColor;
            isKeyHeld = true;
            keyHoldTime = Time.time;
        }

        if (isKeyHeld && Input.GetKey(KeyCode.LeftControl))
        {
            if (Time.time - keyHoldTime >= 0.5f)
            {
                T6.SetActive(false);
                Task.SetActive(true);
                isKeyHeld = false;
            }
        }


        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isKeyHeld = false;
        }
    }
    public void VerifyKeyT7()
    {

        if (Input.GetKeyDown(KeyCode.Tab) && Tab_UI.color == targetColor)
            Tab2_UI.color = targetColor;
        if (Input.GetKeyDown(KeyCode.Tab))
            Tab_UI.color = targetColor;

        if (Tab2_UI.color == targetColor)
        {
            T7.SetActive(false);
            Interactable.SetActive(true);
        }
    }
    public void VerifyKeyT8()
    {

        if (TvOn.activeSelf)
            E_UI.color = targetColor;

        if (!TvOn.activeSelf && E_UI.color == targetColor)
            E2_UI.color = targetColor;

        if (E2_UI.color == targetColor)
        {
            T8.SetActive(false);
            Minimap.SetActive(true);
        }
    }
    public void VerifyKeyT9()
    {

        if (Input.GetKeyDown(KeyCode.M)  && M_UI.color == targetColor)
            M2_UI.color = targetColor;

        if (Input.GetKeyDown(KeyCode.M))
            M_UI.color = targetColor;

        if (M2_UI.color == targetColor)
        {
            T9.SetActive(false);
            NPC.SetActive(true);
        }
    }

    public void HandleOnFinishedTutorial()
    {
        T10.SetActive(false);
        StartCoroutine(FinishTutorial());    
    }

    public void VerifyKeyT10()
    {
        if (DialogueManager.instance.isTalking)
        {
            if (Input.GetKeyDown(KeyCode.Return))
                Enter_UI_T10.color = targetColor;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Left_UI.color = targetColor;
            if (Input.GetKeyDown(KeyCode.RightArrow))
                Right_UI.color = targetColor;
        }
    }

    public IEnumerator FinishTutorial()
    {
        WallCollider.SetActive(false);
        AlertStart.SetActive(true);

        yield return StartCoroutine(ActivateAndDeactivate());
        EventManager.Minimap.OnUnlockMiniMap.Invoke();
        EventManager.Photograph.OnActiveCamera(true);
        EventManager.Quest.OnQuestAssigned();
        gameObject.SetActive(false);
    }

    public IEnumerator ActivateAndDeactivate()
    {
        AlertStart.SetActive(true);

        yield return new WaitForSeconds(3f);
    
        AlertStart.SetActive(false);
    }

    public void StartGame(){
        StartCoroutine(FinishTutorial());
    }
}
