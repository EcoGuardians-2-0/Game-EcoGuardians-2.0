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
        GoBackPopUp.GetComponent<Button>().onClick.AddListener(() => HelpIconPopUp.gameObject.SetActive(false));
        quitGameBtn.GetComponent<Button>().onClick.AddListener(QuitGame);
        completedQuitGameBtn.GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    public void InstantiateLevel()
    {
        levelField.gameObject.SetActive(true);
    }

    public void DisplayHelpIconPopUp()
    {
        HelpIconPopUp.gameObject.SetActive(true);
        finalScoreText.text = act3GameController.GetHighScore().ToString();
    }

    public void GameFinished(string score)
    {
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
        if(levelCompletePanel.gameObject.activeSelf)
        {
            levelCompletePanel.gameObject.SetActive(false);
        }

        levelField.gameObject.SetActive(false);
        act3GameController.QuitActivityThree();
    }


}