using UnityEngine;
using UnityEngine.UI;

public class Act3GameController : GenericActivity
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Transform StartGameBtn;
    [SerializeField]
    public Transform MenuField;
    public Act3LevelController levelController;

    // Scriptable Object progress
    private Act3ProgressSO act3ProgressSO;

    void Awake()
    {
        // Load the Scriptable Object
        act3ProgressSO = Resources.Load<Act3ProgressSO>("Activity3Progress/Activity3 progressSO");
        StartGameBtn.GetComponent<Button>().onClick.AddListener(StartActivityThree);
    }
    public void StartActivityThree()
    {
        AudioManager.Instance.PlaySound(SoundType.ActivityBtnSfx);

        MenuField.gameObject.SetActive(false);
        levelController.InstantiateLevel();
    }

    public void SetHighScore(int score)
    {
        int temp = act3ProgressSO.HighScore;
        if (score > temp)
        {
            act3ProgressSO.HighScore = score;
        }
    }

    public int GetHighScore()
    {
        return act3ProgressSO.HighScore;
    }

    // Method to clean the Scriptable Objects
    public void CleanProgressGameThree()
    {
        act3ProgressSO.HighScore = 0;
    }

    // Update the completed quest calling the game manager
    public void UpdateGameQuest()
    {
        gameManager.HandleQuestCompleted("quest_2.2");
    }

    public void QuitActivityThree()
    {
        AudioManager.Instance.PlaySound(SoundType.ActivityBtnSfx);
        Activity.Instance.activityTwo.SetActive(false);
        SelectionManager.instance.isInteracting = false;
        SelectionManager.instance.canHighlight = true;
        EnableWorldMap();
    }

    private void EnableWorldMap()
    {
        DisableObjects.Instance.EnableWorld();
    }
}
