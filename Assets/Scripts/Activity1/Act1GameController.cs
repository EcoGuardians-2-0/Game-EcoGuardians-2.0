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
    
    void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Sprites/Activity1/Cards");
    }

    void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
    }

    public void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

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
                btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
            }
            else if (!secondGuess)
            {
                secondGuess = true;
                secondGuessIndex = buttonIndex;
                secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
                btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

                if (firstGuessPuzzle == secondGuessPuzzle)
                {
                    Debug.Log("The puzzles match!");
                }
                else
                {
                    Debug.Log("The puzzles don't match!");
                }
            }
        }
        else
        {
            Debug.LogError("Invalid button index: " + name);
        }
    }
}
