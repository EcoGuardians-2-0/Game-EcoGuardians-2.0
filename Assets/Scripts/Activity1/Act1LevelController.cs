using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Act1LevelController : MonoBehaviour
{
    [SerializeField]
    private Sprite bgImage;
    [SerializeField]
    private GameObject button;
    [SerializeField]
    private Transform HelpIcon;
    [SerializeField]
    private Transform MenuField;
    [SerializeField]
    private Transform puzzleFieldLevelOne;
    [SerializeField]
    private Transform puzzleFieldLevelTwo;
    [SerializeField]
    private Transform puzzleFieldLevelThree;
    [SerializeField]
    private Transform levelCompletePanel;
    [SerializeField]
    private Sprite Yellow_Star;
    [SerializeField]
    private Sprite Black_Star;
    [SerializeField]
    private Transform GoBackButton;
    [SerializeField]
    private Text ShowAttemps;
    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new List<Sprite>();
    public List<Button> btns = new List<Button>();
    private bool firstGuess, secondGuess;
    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;
    private int firstGuessIndex, secondGuessIndex;
    private string firstGuessPuzzle, secondGuessPuzzle;
    private int currentLevel;
    private bool isAnimating = false;
    // Scriptable Object progress
    private Act1ProgressSO act1ProgressSO;

    [SerializeField]
    private float flipDuration = 0.5f; 

    void Awake()
    {
        act1ProgressSO = Resources.Load<Act1ProgressSO>("Activity1Progress/Activity1 progressSO");
    }

    public void InstantiateLevel(int level)
    {
        currentLevel = level;

        CleanupCurrentLevel(level);
        LoadPuzzles(level);
        AddButtons(level);
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
    }

    public void AddButtons(int level)
    {
        if (level == 1){
            // Add the buttons to the list
            for (int i = 0; i < 8; i++)
            {
                GameObject btn = Instantiate(button);
                btn.name = "" + i;
                btn.transform.SetParent(puzzleFieldLevelOne, false); 
            }
        }
        else if (level == 2)
        {
            // Add the buttons to the list
            for (int i = 0; i < 12; i++)
            {
                GameObject btn = Instantiate(button);
                btn.name = "" + i;
                btn.transform.SetParent(puzzleFieldLevelTwo, false);
            }
        }
        else if (level == 3)
        {
            // Add the buttons to the list
            for (int i = 0; i < 16; i++)
            {
                GameObject btn = Instantiate(button);
                btn.name = "" + i;
                btn.transform.SetParent(puzzleFieldLevelThree, false); 
            }
        }

    }

    public void CleanupCurrentLevel(int level)
    {
        // Reset game state
        firstGuess = secondGuess = false;
        countGuesses = 0;
        countCorrectGuesses = 0;
        gameGuesses = 0;
        firstGuessIndex = secondGuessIndex = -1;
        firstGuessPuzzle = secondGuessPuzzle = null;
        isAnimating = false;
    
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");
        // Debug.Log("objects count before cleanup: " + objects.Length);
    
        // Destroy the objects tagged with "PuzzleButton"
        foreach (GameObject obj in objects)
        {
            DestroyImmediate(obj);
        }
    
        // Destroy the child objects of the puzzle fields depending on the level
        if (level == 1)
        {
            foreach (Transform child in puzzleFieldLevelOne)
            {
                DestroyImmediate(child.gameObject);
            }
        }
        else if (level == 2)
        {
            foreach (Transform child in puzzleFieldLevelTwo)
            {
                DestroyImmediate(child.gameObject);
            }
        }
        else if (level == 3)
        {
            foreach (Transform child in puzzleFieldLevelThree)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    
        // Debug.Log("objects count after cleanup: " + GameObject.FindGameObjectsWithTag("PuzzleButton").Length);
    
        btns.Clear();
    
        // Clear the list of game puzzles
        gamePuzzles.Clear();
    }

    public void LoadPuzzles(int level)
    {


        // Load resources based on level
        if (level < 1 || level > 3)
        {
            Debug.LogError("Invalid level: " + level);
            return;
        }

        string resourcePath = "Sprites/Activity1/Cards";
        if (level > 1)
        {
            resourcePath += level.ToString();
        }

        puzzles = Resources.LoadAll<Sprite>(resourcePath);

        if (puzzles == null || puzzles.Length == 0)
        {
            Debug.LogError("Failed to load puzzles for level " + level);
        }
    }

    // Get all the buttons based on the level
    public void GetButtons()
    {
        // Debug.Log("Getting buttons");
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");
        // Debug.Log("Found " + objects.Length + " buttons");

        for (int i = 0; i < objects.Length; i++)
        {
            Button btn = objects[i].GetComponent<Button>();
            btns.Add(btn);
            btn.image.sprite = bgImage;
        }
    }

    // Add the puzzles to the game 
    void AddGamePuzzles()
    {
        int looper = btns.Count;
        int index = 0;

        for (int i = 0; i < looper; i++)
        {
            if (index == puzzles.Length)
            {
                index = 0;
            }

            gamePuzzles.Add(puzzles[index]);

            index++;
        }

    }

    // Add listeners to the buttons to pick a puzzle
    public void AddListeners()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => PickAPuzzle(btn));
        }
    }

    public void PickAPuzzle(Button selectedButton)
    {   
        if (isAnimating)  // Block interaction if an animation is still running
        {
            return;
        }

        // Find the index of the selected button
        int buttonIndex = btns.IndexOf(selectedButton);


        if (buttonIndex < 0 || buttonIndex >= gamePuzzles.Count)
        {
            Debug.LogError("Invalid button index: " + selectedButton.name);
            return;
        }

        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = buttonIndex;
            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;
            StartCoroutine(FlipCard(btns[firstGuessIndex], gamePuzzles[firstGuessIndex]));
        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = buttonIndex;
            if (secondGuessIndex != firstGuessIndex)
            {
                secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
                StartCoroutine(FlipCard(btns[secondGuessIndex], gamePuzzles[secondGuessIndex]));

                countGuesses++;
                StartCoroutine(CheckIfPuzzleMatch());
            }
            else
            {
                secondGuess = false; // Reset second guess if the same card is clicked
            }
        }
    }


    IEnumerator FlipCard(Button btn, Sprite targetSprite)
    {
        isAnimating = true;  // Start of animation

        btn.interactable = false;  // Prevent multiple clicks during animation

        // Flip animation
        float elapsedTime = 0f;
        Quaternion startRotation = btn.transform.rotation;
        Quaternion middleRotation = Quaternion.Euler(0f, 90f, 0f);
        Quaternion endRotation = Quaternion.Euler(0f, 0f, 0f);

        // First half of the flip
        while (elapsedTime < flipDuration / 2)
        {
            btn.transform.rotation = Quaternion.Slerp(startRotation, middleRotation, elapsedTime / (flipDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Change the sprite
        btn.image.sprite = targetSprite;

        elapsedTime = 0f;

        // Second half of the flip
        while (elapsedTime < flipDuration / 2)
        {
            btn.transform.rotation = Quaternion.Slerp(middleRotation, endRotation, elapsedTime / (flipDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Re-enable button after animation
        btn.transform.rotation = endRotation;
        btn.interactable = true;

        isAnimating = false;  
    }

    IEnumerator CheckIfPuzzleMatch()
    {
        yield return new WaitForSeconds(flipDuration);

        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            // Wait for a short time before fading out
            yield return new WaitForSeconds(0.5f);

            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            // Fade out animation
            StartCoroutine(FadeOutCard(btns[firstGuessIndex]));
            StartCoroutine(FadeOutCard(btns[secondGuessIndex]));

            CheckIfGameIsFinished();
        }
        else
        {
            // Wait for a short time before flipping back
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(FlipCard(btns[firstGuessIndex], bgImage));
            StartCoroutine(FlipCard(btns[secondGuessIndex], bgImage));
        }

        firstGuess = secondGuess = false;
    }

    IEnumerator FadeOutCard(Button btn)
    {
        float elapsedTime = 0f;
        Color startColor = btn.image.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < flipDuration)
        {
            btn.image.color = Color.Lerp(startColor, endColor, elapsedTime / flipDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        btn.image.color = endColor;
    }

    public void CheckIfGameIsFinished()
    {
        countCorrectGuesses++;

        if (countCorrectGuesses == gameGuesses)
        {
            GoBackButton.gameObject.SetActive(false);
            levelCompletePanel.gameObject.SetActive(true);
            ShowAttemps.text = countGuesses.ToString();
            ShowResult();
        }
    }

    public void ShowResult()
    {
        string[] starNames = { "FirstStar", "SecondStar", "ThirdStar" };

        int LevelStars = 0;

        if (currentLevel == 1)
        {
            LevelStars = FirstLevelResult();
        }
        else if (currentLevel == 2)
        {
            LevelStars = SecondLevelResult();
        }
        else if (currentLevel == 3)
        {
            LevelStars = ThirdLevelResult();
        }

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

    // Update the progress based on the number of guesses (hardcoded values)
    public int FirstLevelResult()
    {
        int result = 0;
        if (countGuesses <= 7)
        {   
            result = 3;
            if (act1ProgressSO.level1Progress < 3)
            {
                act1ProgressSO.level1Progress = 3;
            }
        }
        else if (countGuesses <= 9)
        {
            result = 2;
            if (act1ProgressSO.level1Progress < 2)
            {
                act1ProgressSO.level1Progress = 2;
            }
        }
        else
        {
            result = 1;
            if (act1ProgressSO.level1Progress < 1)
            {
                return act1ProgressSO.level1Progress = 1;
            }
        }
        return result;
    }

    // Update the progress based on the number of guesses (hardcoded values)
    public int SecondLevelResult()
    {
        if (countGuesses <= 10)
        {
            if (act1ProgressSO.level2Progress < 3)
            {

                return act1ProgressSO.level2Progress = 3;
            }
        }
        else if (countGuesses <= 12)
        {
            if (act1ProgressSO.level2Progress < 2)
            {

                return act1ProgressSO.level2Progress = 2;
            }
        }
        else
        {
            if (act1ProgressSO.level2Progress < 1)
            {

                return act1ProgressSO.level2Progress = 1;
            }
        }
        return 0;
    }

    // Update the progress based on the number of guesses (hardcoded values)
    public int ThirdLevelResult()
    {
        if(countGuesses <= 15)
        {
            if (act1ProgressSO.level3Progress < 3)
            {

                return act1ProgressSO.level3Progress = 3;
            }
        }
        else if (countGuesses <= 18)
        {
            if (act1ProgressSO.level3Progress < 2)
            {

                return act1ProgressSO.level3Progress = 2;
            }
        }
        else
        {
            if (act1ProgressSO.level3Progress < 1)
            {

                return act1ProgressSO.level3Progress = 1;
            }
        }
        return 0;
    }

    public void LoadProgress(int progress)
    {
        // Load the progress from the scriptable object
        act1ProgressSO.level1Progress = progress;
    }

    // Shuffle the puzzles in the game
    public void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void RestartGame()
    {
        // Set the current Object unactive and display the puzzle field
        levelCompletePanel.gameObject.SetActive(false);
        InstantiateLevel(currentLevel);
    }

    public void GoBackLevels()
    {
        // Set the current Object unactive and display the puzzle field
        levelCompletePanel.gameObject.SetActive(false);
        HelpIcon.gameObject.SetActive(true);
        MenuField.gameObject.SetActive(true);
    }
}