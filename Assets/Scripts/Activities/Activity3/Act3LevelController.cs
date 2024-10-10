using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Act3LevelController : MonoBehaviour
{   
    [SerializeField]
    private Transform HelpIcon;
    [SerializeField]
    private Transform HelpIconPopUp;
    [SerializeField]
    private Transform GoBackPopUp;
    [SerializeField]
    private Transform levelField;
    [SerializeField]
    private Transform highScoreContainer;
    [SerializeField]
    private Transform completedQuitGameBtn;
    [SerializeField]
    private Transform quitGameBtn;
    [SerializeField]
    private Transform restartButton;
    [SerializeField]
    private Text finalScoreText;
    [SerializeField]
    private Text highScoreText;
    [SerializeField]
    private Transform levelCompletePanel;
    [SerializeField]
    private Act3GameController act3GameController;
    [SerializeField]
    private Snake2 snake;

    void Awake()
    {
        HelpIcon.GetComponent<Button>().onClick.AddListener(DisplayHelpIconPopUp);
        GoBackPopUp.GetComponent<Button>().onClick.AddListener(() => 
        {
            AudioManager.Instance.PlaySound(SoundType.ActivityBtnSfx);
            HelpIconPopUp.gameObject.SetActive(false);
            Time.timeScale = 1; // Resume the game
        });
        quitGameBtn.GetComponent<Button>().onClick.AddListener(QuitGame);
        completedQuitGameBtn.GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    public void InstantiateLevel()
    {
        levelField.gameObject.SetActive(true);
    }

    public void DisplayHelpIconPopUp()
    {
        AudioManager.Instance.PlaySound(SoundType.ActivityBtnSfx);
        HelpIconPopUp.gameObject.SetActive(true);
        Time.timeScale = 0; // Pause the game
    }

    public void GameFinished(string score)
    {
        AudioManager.Instance.PlaySound(SoundType.ActivityComplete);

        // Set the high score
        act3GameController.SetHighScore(int.Parse(score));

        // hide the btns and the score
        quitGameBtn.gameObject.SetActive(false);
        HelpIcon.gameObject.SetActive(false);
        highScoreContainer.gameObject.SetActive(false);

        // Show the final score
        levelCompletePanel.gameObject.SetActive(true);
        finalScoreText.text = score;
        highScoreText.text = act3GameController.GetHighScore().ToString();

        // Update the related task in the task manager
        if (act3GameController.GetHighScore() >= 10)
            act3GameController.UpdateGameQuest();

        // Setup the listeners to the retry and go back buttons
        restartButton.GetComponent<Button>().onClick.AddListener(RestartGame);
        completedQuitGameBtn.GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    // Restart the current level
    public void RestartGame()
    {
        snake.Restart();
        levelCompletePanel.gameObject.SetActive(false);
        HelpIcon.gameObject.SetActive(true);
        quitGameBtn.gameObject.SetActive(true);
        highScoreContainer.gameObject.SetActive(true);   
    }

    private void QuitGame()
    {
        Time.timeScale = 1; // Resume the game
        
        // Clean the progress
        snake.Restart();

        if(levelCompletePanel.gameObject.activeSelf)
        {
            levelCompletePanel.gameObject.SetActive(false);
        }

        // Set up the game objects
        HelpIcon.gameObject.SetActive(true);
        quitGameBtn.gameObject.SetActive(true);
        highScoreContainer.gameObject.SetActive(true);
        

        // Set the level field inactive
        levelField.gameObject.SetActive(false);
        act3GameController.QuitActivityThree();
        
        // Set the menu field active
        act3GameController.MenuField.gameObject.SetActive(true);
    }
}