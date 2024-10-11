using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Act2LevelController : MonoBehaviour
{
    [SerializeField]
    private Transform HelpIcon;
    [SerializeField]
    private Transform MenuField;
    [SerializeField]
    private Transform levelField;
    [SerializeField]
    private RectTransform gameBoard;
    [SerializeField]
    private Image piecePrefab;
    [SerializeField]
    private Sprite fullImage;
    [SerializeField]
    private TextMeshProUGUI timertext;
    [SerializeField]
    private Transform levelCompletePanel;
    [SerializeField]
    private Transform GoBackButton;
    [SerializeField]
    private TextMeshProUGUI ShowTime;
    [SerializeField]
    private Sprite Yellow_Star;
    [SerializeField]
    private Sprite Black_Star;
    [SerializeField]
    private Transform RestartButton;
    [SerializeField]
    private Transform GoBackLevelsButton;
    [SerializeField]
    private Transform FinishTaskButton;
    private Act2ProgressSO act2ProgressSO;
    private Act2GameController act2GameController;
    float elapsedTime = 0f;
    private List<Image> pieces = new List<Image>();
    private int emptyLocation;
    // The default puzzle size is 3x3
    private int size = 3;
    private int currentLevel = 0;
    private bool isTimerRunning = false;

    void Awake()
    {
        act2GameController = FindObjectOfType<Act2GameController>();
        FinishTaskButton.GetComponent<Button>().onClick.AddListener(FinishTask);
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            UpdateTimer();
            EnableFinishedButton();
        }
    }

    public void InstantiateLevel(int level)
    {

        // Initialize the progress SO
        act2ProgressSO = Resources.Load<Act2ProgressSO>("Activity2Progress/Activity2 progressSO");

        // Initialize the current level
        currentLevel = level;

        // Destroy any existing pieces before creating new ones
        DestroyCurrentPieces();

        // Set the puzzle size based on the level
        if (level == 1)
        {
            size = 3;
            CreateGamePieces();
            Shuffle();
            StartTimer();
        }
        else if (level == 2)
        {
            size = 4;
            CreateGamePieces();
            Shuffle();
            StartTimer();
        }
        else
        {
            Debug.LogError("Invalid level number");
        }
    }

    private void CreateGamePieces()
    {
        Texture2D fullTexture = fullImage.texture;
        int pieceWidth = fullTexture.width / size;
        int pieceHeight = fullTexture.height / size;

        // Create the pieces
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Image piece = Instantiate(piecePrefab, gameBoard);
                pieces.Add(piece);

                // Set the piece's rect transform
                RectTransform rectTransform = piece.rectTransform;
                rectTransform.anchorMin = new Vector2((float)col / size, 1 - (float)(row + 1) / size);
                rectTransform.anchorMax = new Vector2((float)(col + 1) / size, 1 - (float)row / size);
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;

                // Create a new sprite for this piece
                Rect pieceRect = new Rect(col * pieceWidth, (size - 1 - row) * pieceHeight, pieceWidth, pieceHeight);
                Sprite pieceSprite = Sprite.Create(fullTexture, pieceRect, new Vector2(0.5f, 0.5f), 100.0f);
                piece.sprite = pieceSprite;

                piece.name = $"{row * size + col}";

                // If it's the last piece, make it the empty spot
                if (row == size - 1 && col == size - 1)
                {
                    emptyLocation = pieces.Count - 1;
                    piece.color = Color.clear; // Hide the empty piece
                }

                // Add a Button component to detect clicks
                piece.gameObject.AddComponent<Button>().onClick.AddListener(() => OnPieceClicked(piece));
            }
        }
    }

    // Called when a piece is clicked
    private void OnPieceClicked(Image piece)
    {
        int clickedIndex = pieces.IndexOf(piece);
        TrySwapPiece(clickedIndex);

        // Check for completion after each move
        if (CheckCompletion())
        {
            Debug.Log("Player Wins!");
            GameFinished();
            StopTimer();
        }
    }

    // Try to swap the clicked piece with the empty space        
    private void TrySwapPiece(int index)
    {
        if (SwapIfValid(index, -size, size)) return; // Up
        if (SwapIfValid(index, size, size)) return;  // Down
        if (SwapIfValid(index, -1, 0)) return;       // Left
        if (SwapIfValid(index, 1, size - 1)) return; // Right
    }

    // Swap the pieces if the move is valid
    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        // Check if the piece can be moved
        if (((i % size) != colCheck) && ((i + offset) == emptyLocation))
        {
            Image temp = pieces[i];
            pieces[i] = pieces[i + offset];
            pieces[i + offset] = temp;

            (pieces[i].rectTransform.anchorMin, pieces[i + offset].rectTransform.anchorMin) =
                (pieces[i + offset].rectTransform.anchorMin, pieces[i].rectTransform.anchorMin);
            (pieces[i].rectTransform.anchorMax, pieces[i + offset].rectTransform.anchorMax) =
                (pieces[i + offset].rectTransform.anchorMax, pieces[i].rectTransform.anchorMax);

            emptyLocation = i;
            return true;
        }
        return false;
    }

    // Check if the puzzle is completed
    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != $"{i}")
            {
                return false;
            }
        }
        return true;
    }

    // Improved shuffle that ensures solvability by using valid random moves
    private void Shuffle()
    {
        int shuffleCount = size * size * size;  // Increase shuffle count based on puzzle size
        int[] directions = { -size, size, -1, 1 }; // Up, Down, Left, Right
        int previousEmptyLocation = emptyLocation;  // Track the previous empty location to avoid undoing the last move

        for (int i = 0; i < shuffleCount; i++)
        {
            List<int> validMoves = new List<int>();

            // Check valid directions based on emptyLocation
            foreach (int dir in directions)
            {
                int targetIndex = emptyLocation + dir;
                if (IsValidMove(targetIndex, dir) && targetIndex != previousEmptyLocation)
                {
                    validMoves.Add(targetIndex);
                }
            }
            // Pick a random valid move
            if (validMoves.Count > 0)
            {
                int randomMove = validMoves[Random.Range(0, validMoves.Count)];
                previousEmptyLocation = emptyLocation;
                TrySwapPiece(randomMove);
            }
        }
        // Ensure puzzle is not initially solved
        if (CheckCompletion())
        {
            Shuffle();
        }
    }

    // Check if the move is valid based on boundaries
    private bool IsValidMove(int targetIndex, int direction)
    {
        if (targetIndex < 0 || targetIndex >= pieces.Count)
            return false;
        if (direction == -1 && emptyLocation % size == 0) // Can't move left
            return false;
        if (direction == 1 && emptyLocation % size == size - 1) // Can't move right
            return false;

        return true;
    }

    // Destroy the current pieces
    private void DestroyCurrentPieces()
    {
        // Iterate through the list of pieces and destroy their GameObjects
        foreach (var piece in pieces)
        {
            Destroy(piece.gameObject);
        }
        // Clear the list of pieces
        pieces.Clear();
    }

    // Start the timer
    private void StartTimer()
    {
        elapsedTime = 0f;
        isTimerRunning = true;
    }

    // Stop the timer
    private void StopTimer()
    {
        isTimerRunning = false;
    }

    // Update the timer text
    private void UpdateTimer()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timertext.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private int GetElapsTimeMinutes()
    {
        return Mathf.FloorToInt(elapsedTime / 60);
    }

    public void GameFinished()
    {
        AudioManager.Instance.PlaySound(SoundType.ActivityComplete);

        GoBackButton.gameObject.SetActive(false);
        levelCompletePanel.gameObject.SetActive(true);

        // Setup the listeners to the retry and go back buttons
        RestartButton.GetComponent<Button>().onClick.AddListener(RestartGame);
        GoBackLevelsButton.GetComponent<Button>().onClick.AddListener(GoBackLevels);

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timertext.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        ShowTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        ShowResult();
    }

    public void EnableFinishedButton()
    {
        // After one minute and 30 seconds, the finish button will be enabled
        if (elapsedTime >= 90)
        {
            FinishTaskButton.gameObject.SetActive(true);
        }
    }

    // Finish task if the game is too haard
    public void FinishTask()
    {
        act2GameController.UpdateGameQuest();
    }

    public void ShowResult()
    {
        string[] starNames = { "FirstStar", "SecondStar", "ThirdStar" };

        int LevelStars = 0;
        int elapsedTimeMinutes = GetElapsTimeMinutes();

        if (currentLevel == 1)
        {
            LevelStars = FirstLevelResult(elapsedTimeMinutes);
        }
        else if (currentLevel == 2)
        {
            LevelStars = SecondLevelResult(elapsedTimeMinutes);
        }
        else
        {
            Debug.LogError("Invalid level number");
        }

        if (act2GameController != null)
        {
            act2GameController.UpdateLevelsCompleted();
            act2GameController.CheckLevelsCompleted();
        }
        else
        {
            Debug.LogError("act2GameController is null");
        }

        // Update the stars based on the level result
        Transform LevelsStarContainer = levelCompletePanel.transform.Find("LevelStarContainer");

        foreach (string starName in starNames)
        {
            Debug.Log("Star name: " + starName);
            // Get the star transform
            Transform starTransform = LevelsStarContainer.transform.Find(starName);

            // Get the Image and Text components of the star
            Image starImage = starTransform.GetComponent<Image>();

            // Set the star to corresponding sprite
            if (LevelStars == 0)
                starImage.sprite = Black_Star;
            else
            {
                starImage.sprite = Yellow_Star;
                LevelStars--;
            }
        }
    }

    // Update the progress based on the timer (hardcoded values)
    public int FirstLevelResult(int elapsedTimeMinutes)
    {
        // If the time is less than 4 minutes, the player gets 3 stars
        if (elapsedTimeMinutes <= 4)
        {
            if (act2ProgressSO.level1Progress < 3)
            {
                return act2ProgressSO.level1Progress = 3;
            }
            else // If the player already has 3 stars, return 3
            {
                return 3;
            }
        }
        else if (elapsedTimeMinutes <= 5)
        {
            if (act2ProgressSO.level1Progress < 2)
            {
                return act2ProgressSO.level1Progress = 2;
            }
            else // If the player already has 2 stars, return 2
            {
                return 2;
            }
        }
        else
        {
            if (act2ProgressSO.level1Progress < 1)
            {
                return act2ProgressSO.level1Progress = 1;
            }
            else // If the player already has 1 star, return 1
            {
                return 1;
            }
        }
    }

    // Update the progress based on the timer (hardcoded values)
    public int SecondLevelResult(int elapsedTimeMinutes)
    {
        // If the time is less than 17 minutes, the player gets 3 stars
        if (elapsedTimeMinutes <= 17)
        {
            if (act2ProgressSO.level2Progress < 3)
            {
                return act2ProgressSO.level2Progress = 3;
            }
            else
            {
                return 3;
            }
        }
        else if (elapsedTimeMinutes <= 20)
        {
            if (act2ProgressSO.level2Progress < 2)
            {
                return act2ProgressSO.level2Progress = 2;
            }
            else
            {
                return 2;
            }
        }
        else
        {
            if (act2ProgressSO.level2Progress < 1)
            {
                return act2ProgressSO.level2Progress = 1;
            }
            else
            {
                return 1;
            }
        }
    }

    // Restart the current level
    public void RestartGame()
    {
        AudioManager.Instance.PlaySound(SoundType.ActivityBtnSfx);

        levelCompletePanel.gameObject.SetActive(false);
        GoBackButton.gameObject.SetActive(true);
        InstantiateLevel(currentLevel);
    }

    public void GoBackLevels()
    {
        AudioManager.Instance.PlaySound(SoundType.ActivityBtnSfx);

        MenuField.gameObject.SetActive(true);
        // Update the menu buttons based on the level result
        act2GameController.InstantiateMenuButtons();
        // Set the current game object unactive and display the menu field
        levelField.gameObject.SetActive(false);
        // Set the level complete object unactive and display the puzzle field
        levelCompletePanel.gameObject.SetActive(false);
        HelpIcon.gameObject.SetActive(true);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}