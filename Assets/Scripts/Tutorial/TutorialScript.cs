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
    public GameObject T1;
    public GameObject T2;
    public GameObject T3;
    public GameObject T4;
    public GameObject T5;
    public GameObject T6;
    public GameObject T7;
    public GameObject T8;
    public GameObject WallCollider;
    public GameObject TvOn;
    public GameObject Pausa;
    public GameObject AlertaEmpezar;
    public Graphic Enter_UI;
    public Graphic A2_UI;
    public Graphic S2_UI;
    public Graphic D2_UI;
    public Graphic W2_UI;
    public Graphic Space_UI;
    public Graphic Shift_UI;
    public Graphic E_UI;
    public Graphic E2_UI;
    public Graphic P_UI;
    public Graphic P2_UI;
    public Graphic C_UI;
    public Graphic Ctrl_UI;
    public Graphic Tab_UI;
    public Graphic Tab2_UI;
    public Button EnterB;
    private float keyHoldTime = 0f;
    private bool isKeyHeld = false;
    private float shiftKeyStartTime = 0f; 
    private bool isHoldingShiftAndKey = false;

    private float[] keyStartTimes;
    private bool[] isColorChanged;
    void Start()
    {
        keyStartTimes = new float[keyCodes.Length];
        isColorChanged = new bool[keyCodes.Length];
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(FinishTutorial());
        }

        if (Welcome.activeSelf)
            WelcomeKeyDown();
        else if (T1.activeSelf)
            VerifyKeyDownT1();
        else if (T2.activeSelf)
            VerifyKeyDownT2();
        else if (T3.activeSelf)
            VerifyKeyDownT3();
        else if (T4.activeSelf)
            VerifyKeyDownT4();
        else if (T5.activeSelf)
            VerifyKeyDownT5();
        else if (T6.activeSelf)
            VerifyKeyDownT6();
        else if (T7.activeSelf)
            VerifyKeyDownT7();
        else if (T8.activeSelf)
            VerifyKeyDownT8();

    }

    public void WelcomeKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            Enter_UI.color = targetColor;
        if (Enter_UI.color == targetColor)
        {
            Welcome.SetActive(false);
            T1.SetActive(true);
            EnterB.onClick.Invoke();
        }
    }

    public void VerifyKeyDownT1()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKey(keyCodes[i]))
            {
                if (keyStartTimes[i] == 0)
                    keyStartTimes[i] = Time.time; 
                if (Time.time - keyStartTimes[i] >= 2f && !isColorChanged[i])
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
            T2.SetActive(true);
        }
    }
    public void VerifyKeyDownT2()
    {
        if (Input.GetKeyDown(KeyCode.P))
            P_UI.color = targetColor;
        if (Input.GetKeyDown(KeyCode.P) && P_UI.color == targetColor && Pausa.activeSelf)
            P2_UI.color = targetColor;

        if (P_UI.color == targetColor && !Pausa.activeSelf)
        {
            T2.SetActive(false);
            T3.SetActive(true);
        }
    }

    public void VerifyKeyDownT3()
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
                T4.SetActive(true);
                isHoldingShiftAndKey = false;
            }
        }
        else
        {
            isHoldingShiftAndKey = false;
        }

    }
    public void VerifyKeyDownT4()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Space_UI.color = targetColor;
        if (Space_UI.color == targetColor)
        {
            T4.SetActive(false);
            T5.SetActive(true);
        }
    }

    public void VerifyKeyDownT5()
    {

        if (Input.GetKeyDown(KeyCode.C))
            C_UI.color = targetColor;

        if (C_UI.color == targetColor)
        {
            T5.SetActive(false);
            T6.SetActive(true);
        }
    }
    public void VerifyKeyDownT6()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Ctrl_UI.color = targetColor;
            isKeyHeld = true;
            keyHoldTime = Time.time;
        }

        if (isKeyHeld && Input.GetKey(KeyCode.LeftControl))
        {
            if (Time.time - keyHoldTime >= 2f)
            {
                T6.SetActive(false);
                T7.SetActive(true);
                isKeyHeld = false;
            }
        }


        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isKeyHeld = false;
        }
    }
    public void VerifyKeyDownT7()
    {

        if (Input.GetKeyDown(KeyCode.Tab) && Tab_UI.color == targetColor)
            Tab2_UI.color = targetColor;
        if (Input.GetKeyDown(KeyCode.Tab))
            Tab_UI.color = targetColor;

        if (Tab2_UI.color == targetColor)
        {
            T7.SetActive(false);
            T8.SetActive(true);
        }
    }
    public void VerifyKeyDownT8()
    {

        if (TvOn.activeSelf)
            E_UI.color = targetColor;

        if (!TvOn.activeSelf && E_UI.color == targetColor)
            E2_UI.color = targetColor;

        if (E2_UI.color == targetColor)
        {
            T8.SetActive(false);
            StartCoroutine(FinishTutorial());
        }
    }

    private IEnumerator FinishTutorial()
    {
        WallCollider.SetActive(value: false);
        EventManager.Quest.OnQuestAssigned();
        yield return ActivateAndDeactivate();
        gameObject.SetActive(false);
    }

    private IEnumerator ActivateAndDeactivate()
    {
        AlertaEmpezar.SetActive(true);
        yield return new WaitForSeconds(3f);
        AlertaEmpezar.SetActive(false);
        tutorialUI.SetActive(false);
    }
}
