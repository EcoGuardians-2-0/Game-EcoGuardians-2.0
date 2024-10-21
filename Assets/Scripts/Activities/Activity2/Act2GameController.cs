using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Act2GameController : GenericActivity
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Transform GoBackButton;
    [SerializeField]
    private Transform ReferenceImagebtn;
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
    private GameObject quitButton;
    [SerializeField]
    private Sprite Yellow_Star;
    [SerializeField]
    private Sprite Black_Star;
    [SerializeField]
    private Transform FinishTaskButton;
    [SerializeField]
    private int numberOfLevels = 2;
    public Act2LevelController levelController;
    public List<Button> btns = new List<Button>();
    public List<int> levelsProgress = new List<int>();

    // Scriptable Object progress
    private Act2ProgressSO act2ProgressSO;

    private List<bool> levelsCompleted = new List<bool>();

    void Awake()
    {
        // Set the current levelsCompleted to all false
        for (int i = 0; i < numberOfLevels; i++)
        {
            levelsCompleted.Add(false);
        }

        // Load the Scriptable Object
        act2ProgressSO = Resources.Load<Act2ProgressSO>("Activity2Progress/Activity2 progressSO");

        // Add the menu buttons and the listeners
        AddMenuButtons();

        GoBackButton.GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
        quitButton.GetComponent<Button>().onClick.AddListener(QuitActivityTwo);
        HelpIcon.GetComponent<Button>().onClick.AddListener(OnHelpIconClick);
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
        AudioManager.Instance.PlaySound(SoundType.ActivityBtnSfx);

        puzzleField.gameObject.SetActive(true);

        HandleInfoAndBackButton();

        // Set the current Object unactive and display the puzzle field
        DisableMenu();

        // Call the levelController to add the cards first to the level
        levelController.InstantiateLevel(level);
    }

    public void InstantiateMenuButtons()
    {
        if (FinishTaskButton.gameObject.activeSelf)
        {
            FinishTaskButton.gameObject.SetActive(false);
        }
   
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
        AudioManager.Instance.PlaySound(SoundType.ActivityBtnSfx);
        HelpIconPopUp.gameObject.SetActive(true);
    }

    // Handle the back button on the help icon popup
    public void OnBackButtonClick()
    {
        AudioManager.Instance.PlaySound(SoundType.ActivityBtnSfx);
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

    public void EnableQuitButton()
    {
        quitButton.SetActive(true);
    }

    public void HandleInfoAndBackButton()
    {
        EnableQuitButton();
        DisableInfoIcon();
        EnableBackButton();
    }

    // On click listener for the back button
    public void OnBackButtonClicked()
    {
        AudioManager.Instance.PlaySound(SoundType.ActivityBtnSfx);

        // Display the menu field
        MenuField.gameObject.SetActive(true);
        InstantiateMenuButtons();
        GoBackButton.gameObject.SetActive(false);
        ReferenceImagebtn.gameObject.SetActive(false);

        if (puzzleField.gameObject.activeSelf)
        {
            puzzleField.gameObject.SetActive(false);
        }        
        HelpIcon.gameObject.SetActive(true);
        quitButton.SetActive(true);
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

        // Set the levels progress to 0 in the SO
        act2ProgressSO.level1Progress = 0;
        act2ProgressSO.level2Progress = 0;
        act2ProgressSO.level3Progress = 0;
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
        gameManager.HandleQuestCompleted("quest_3.3");
    }

    //Check if the completed levels are two
    public void CheckLevelsCompleted()
    {
        // Look in the list of level completed if the two levels are completed
        if (levelsCompleted[0] || levelsCompleted[1])
        {
            // Call the game manager to complete the questionnaire
            UpdateGameQuest();
        }
    }

    public void QuitActivityTwo()
    {
        AudioManager.Instance.PlaySound(SoundType.ActivityBtnSfx);

        Activity.Instance.activityThree.SetActive(false);
        EnableWorldMap();
    }

    private void EnableWorldMap()
    {
        DisableObjects.Instance.EnableWorld();
    }
}
