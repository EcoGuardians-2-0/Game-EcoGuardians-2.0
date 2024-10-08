using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Act1GameController : GenericActivity
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Transform GoBackButton;
    [SerializeField]
    private Transform HelpIcon;
    [SerializeField]
    private Transform HelpIconPopUp;
    [SerializeField]
    private Transform MenuField;
    [SerializeField]
    private Transform puzzleFieldLevelOne;
    [SerializeField]
    private GameObject quitButton;
    [SerializeField]
    private Transform puzzleFieldLevelTwo;
    [SerializeField]
    private Transform puzzleFieldLevelThree;
    [SerializeField]
    private GameObject menuButton;
    [SerializeField]
    private Sprite Yellow_Star;
    [SerializeField]
    private Sprite Black_Star;
    [SerializeField]
    private int numberOfLevels = 3;
    [SerializeField]
    private Act1LevelController levelController;
    public List<Button> btns = new List<Button>();
    public List<int> levelsProgress = new List<int>();
    private List<bool> levelsCompleted = new List<bool>();

    // Scriptable Object progress
    private Act1ProgressSO act1ProgressSO;

    void Awake()
    {
        // Set the current levelsCompleted to all false
        for (int i = 0; i < numberOfLevels; i++)
        {
            levelsCompleted.Add(false);
        }

        // Load the Scriptable Object
        act1ProgressSO = Resources.Load<Act1ProgressSO>("Activity1Progress/Activity1 progressSO");

        // Add the menu buttons and the listener to the back button
        AddMenuButtons();
        GoBackButton.GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
        quitButton.GetComponent<Button>().onClick.AddListener(QuitActivityOne);
    }

    public void AddMenuButtons()
    {
        // Modify depeding on the number of levels
        for (int i = 0; i < numberOfLevels; i++)
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
        switch (level)
        {
            case 0:
                BeginLevelOne();
                break;
            case 1:
                BeginLevelTwo();
                break;
            case 2:
                BeginLevelThree();
                break;
        }
    }

    public void InstantiateMenuButtons()
    {
        Debug.Log("Instantiating menu buttons");
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
            temp = act1ProgressSO.level1Progress;
        }
        else if (index == 1)
        {
            temp = act1ProgressSO.level2Progress;
        }
        else if (index == 2)
        {
            temp = act1ProgressSO.level3Progress;
        }
        return temp;
    }

    public void BeginLevelOne()
    {

        puzzleFieldLevelOne.gameObject.SetActive(true);

        HandleInfoAndBackButton();

        DisableMenu();

        levelController.InstantiateLevel(1);
    }

    public void BeginLevelTwo()
    {

        puzzleFieldLevelTwo.gameObject.SetActive(true);
        HandleInfoAndBackButton();

        // Set the current Object unactive and display the puzzle field
        DisableMenu();

        // Call the levelController to add the cards first to the level
        levelController.InstantiateLevel(2);
    }

    public void BeginLevelThree()
    {

        puzzleFieldLevelThree.gameObject.SetActive(true);

        HandleInfoAndBackButton();

        // Set the current Object unactive and display the puzzle field
        DisableMenu();

        // Call the levelController to add the cards first to the level
        levelController.InstantiateLevel(3);
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

    public void DisableQuitButton()
    {
        quitButton.SetActive(false);
    }

    public void EnableQuitButton()
    {
        quitButton.SetActive(true);
    }

    public void HandleInfoAndBackButton()
    {
        DisableQuitButton();
        DisableInfoIcon();
        EnableBackButton();
    }

    // On click listener for the back button
    public void OnBackButtonClicked()
    {
        // Display the menu field
        MenuField.gameObject.SetActive(true);
        InstantiateMenuButtons();
        GoBackButton.gameObject.SetActive(false);

        if (puzzleFieldLevelOne.gameObject.activeSelf)
        {
            puzzleFieldLevelOne.gameObject.SetActive(false);
        }
        else if (puzzleFieldLevelTwo.gameObject.activeSelf)
        {
            puzzleFieldLevelTwo.gameObject.SetActive(false);
        }
        else if (puzzleFieldLevelThree.gameObject.activeSelf)
        {
            puzzleFieldLevelThree.gameObject.SetActive(false);
        }
        
        HelpIcon.gameObject.SetActive(true);
        EnableQuitButton();
    }

    public void DisableMenu()
    {
        MenuField.gameObject.SetActive(false);
    }

    // Method to clean the Scriptable Objects
    public void CleanProgressGameTwo()
    {
        // Set the levels to false in the list
        for (int i = 0; i < numberOfLevels; i++)
        {
            levelsCompleted[i] = false;
        }

        act1ProgressSO.level1Progress = 0;
        act1ProgressSO.level2Progress = 0;
        act1ProgressSO.level3Progress = 0;
    }

    // Update the leveles completed
    public void UpdateLevelsCompleted()
    {
        // Get the current level
        int currentLevel = levelController.GetCurrentLevel();

        // Update the current level completed to true in the list
        levelsCompleted[currentLevel - 1] = true;
    }

    // Update the completed quest calling the game manager
    public void UpdateGameQuest()
    {
        gameManager.HandleQuestCompleted("quest_1.6");
    }

    //Check if the completed levels are two
    public void CheckLevelsCompleted()
    {
        // Look in the list of level completed if the two levels are completed
        if (levelsCompleted[0] && levelsCompleted[1])
        {
            // Call the game manager to complete the questionnaire
            UpdateGameQuest();
        }
    }

    public void QuitActivityOne()
    {
        Activity.Instance.activityOne.SetActive(false);
        EnableWorldMap();
    }

    private void EnableWorldMap()
    {
        DisableObjects.Instance.EnableWorld();
    }
}
