using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Act2GameController : MonoBehaviour
{
    [SerializeField]
    private Transform GoBackButton;
    [SerializeField]
    private Transform HelpIcon;
    [SerializeField]
    private Transform HelpIconPopUp;
    [SerializeField]
    private Transform MenuField;
    [SerializeField]
    private Transform puzzleField;
    [SerializeField]
    private GameObject menuButton;
    [SerializeField]
    private Sprite Yellow_Star;
    [SerializeField]
    private Sprite Black_Star;
    [SerializeField]
    private int numberOfLevels = 2;
    public Act2LevelController levelController;
    public List<Button> btns = new List<Button>();
    public List<int> levelsProgress = new List<int>();

    // Scriptable Object progress
    private Act2ProgressSO act2ProgressSO;

    void Awake()
    {
        act2ProgressSO = Resources.Load<Act2ProgressSO>("Activity2Progress/Activity2 progressSO");
        AddMenuButtons();
        GoBackButton.GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
    }

    public void AddMenuButtons()
    {
        // Modify depeding on the number of levels
        for (int i = 1; i <= numberOfLevels; i++)
        {
            GameObject btn = Instantiate(menuButton);
            btn.name = "" + i;
            btn.transform.SetParent(MenuField, false);

            // Capture the current value of i
            int levelIndex = i;

            // Add click listener 
            btn.GetComponent<Button>().onClick.AddListener(() => OnMenuButtonClick(levelIndex));
        }
        InstantiateMenuButtons();
    }

    public void OnMenuButtonClick(int level)
    {
        puzzleField.gameObject.SetActive(true);

        HandleInfoAndBackButton();

        // Set the current Object unactive and display the puzzle field
        DisableMenu();

        // Call the levelController to add the cards first to the level
        levelController.InstantiateLevel(level);
    }

    public void InstantiateMenuButtons()
    {
        string[] starNames = { "FirstStar", "SecondStar", "ThirdStar" };

        GameObject[] objects = GameObject.FindGameObjectsWithTag("LevelSelectionButton");

        for (int i = 0; i < objects.Length; i++)
        {
            // Add the button to the list 
            btns.Add(objects[i].GetComponent<Button>());

            Transform starContainer = btns[i].transform.Find("StarContainer");
            Transform textTransform = btns[i].transform.Find("LevelNumber");
            Text starText = textTransform.GetComponent<Text>();

            starText.text = (i + 1).ToString();

            // Get the levels progress
            int starsToDisplay = GetLevelsProgress(i);
            // Calculate the number of stars to display     

            foreach (string starName in starNames)
            {
                // Get the star transform
                Transform starTransform = starContainer.transform.Find(starName);

                // Get the Image and Text components of the star
                Image starImage = starTransform.GetComponent<Image>();

                // Set the star to corresponding sprite
                if (starsToDisplay == 0)
                    starImage.sprite = Black_Star;
                else
                {
                    starImage.sprite = Yellow_Star;
                    starsToDisplay--;
                }
            }
        }
    }

    public int GetLevelsProgress(int index)
    {
        int temp = 0;
        if (index == 0)
        {
            temp = act2ProgressSO.level1Progress;
        }
        else if (index == 1)
        {
            temp = act2ProgressSO.level2Progress;
        }
        else if (index == 2)
        {
            temp = act2ProgressSO.level3Progress;
        }
        return temp;
    }

    // Handle the information icon on clik
    public void OnHelpIconClick()
    {
        HelpIconPopUp.gameObject.SetActive(true);
    }

    // Handle the back button on the help icon popup
    public void OnBackButtonClick()
    {
        HelpIconPopUp.gameObject.SetActive(false);
    }

    // Method to enable the go back button to the menu
    public void EnableBackButton()
    {
        // Set the current Object unactive and display the puzzle field
        GoBackButton.gameObject.SetActive(true);
    }

    public void DisableInfoIcon()
    {
        HelpIcon.gameObject.SetActive(false);
    }

    public void HandleInfoAndBackButton()
    {
        DisableInfoIcon();
        EnableBackButton();
    }

    // On click listener for the back button
    public void OnBackButtonClicked()
    {
        AddMenuButtons();

        // Set the current Object unactive and display the puzzle field
        MenuField.gameObject.SetActive(true);
        GoBackButton.gameObject.SetActive(false);

        if (puzzleField.gameObject.activeSelf)
        {
            puzzleField.gameObject.SetActive(false);
        }

        HelpIcon.gameObject.SetActive(true);
    }

    public void DisableMenu()
    {
        MenuField.gameObject.SetActive(false);
    }
}
