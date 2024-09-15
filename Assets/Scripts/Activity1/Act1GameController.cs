using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Act1GameController : MonoBehaviour
{
    [SerializeField]
    private Sprite bgImage;
    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new List<Sprite>();
    public List<Button> btns = new List<Button>();
    private bool firstGuess, secondGuess;
    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;
    private int firstGuessIndex, secondGuessIndex;
    private string firstGuessPuzzle, secondGuessPuzzle;

    [SerializeField]
    private float flipDuration = 0.5f;  // Duration of flip animation

    void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Sprites/Activity1/Cards");
    }

    void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
    }

    // Get all the buttons in the game and add them to the list
    public void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    // Add the puzzles to the game 
    void AddGamePuzzles()
    {
        // Add each puzzle twice
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
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }

    public void PickAPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        int buttonIndex;

        if (int.TryParse(name, out buttonIndex) && buttonIndex >= 0 && buttonIndex < gamePuzzles.Count)
        {
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
                secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
                StartCoroutine(FlipCard(btns[secondGuessIndex], gamePuzzles[secondGuessIndex]));

                countGuesses++;

                StartCoroutine(CheckIfPuzzleMatch());
            }
        }
        else
        {
            Debug.LogError("Invalid button index: " + name);
        }
    }

    IEnumerator FlipCard(Button btn, Sprite targetSprite)
    {
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
    }

    IEnumerator CheckIfPuzzleMatch()
    {
        yield return new WaitForSeconds(flipDuration);  

        if (firstGuessPuzzle == secondGuessPuzzle && firstGuessIndex != secondGuessIndex)
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
            // Wait for a short time before fading out
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
            Debug.Log("Game Finished!");
            Debug.Log("It took you " + countGuesses + " guesses to finish the game.");
        }
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
}