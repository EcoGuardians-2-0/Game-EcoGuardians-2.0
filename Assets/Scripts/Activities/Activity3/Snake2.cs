using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake2 : MonoBehaviour
{
    [SerializeField]
    private int xSize, ySize;
    [SerializeField]
    private GameObject block;
    [SerializeField]
    private Sprite headSprite, tailSprite, foodSprite;
    [SerializeField]
    public GameObject startPanel;
    [SerializeField]
    private Text points;
    [SerializeField]
    private Text HighScoreinGame;
    [SerializeField]
    private Act3GameController act3GameController;
    private Vector2 blockSize;
    GameObject food;
    bool isAlive;
    GameObject head;
    List<GameObject> tail = new List<GameObject>();
    Vector2 dir;
    Vector2 nextDir;
    float timeBetweenMovements;
    float moveTimer;
    bool gameStarted = false;

    void Start()
    {
        timeBetweenMovements = 0.24f;
        dir = Vector2.right;
        nextDir = dir;
        createGrid();
        createPlayer();
        spawnFood();
        isAlive = true;
        startPanel.SetActive(true);
        DisplayHighScoreInGame();
    }

    private void createPlayer()
    {
        head = Instantiate(block) as GameObject;
        RectTransform headRect = head.GetComponent<RectTransform>();
        headRect.SetParent(transform.parent, false);
        headRect.sizeDelta = blockSize;
        head.GetComponent<Image>().sprite = headSprite;
        tail = new List<GameObject>();
    }

    private void spawnFood()
    {
        List<Vector2> availablePositions = new List<Vector2>();

        for (int x = -xSize / 2 + 1; x < xSize / 2; x++)
        {
            for (int y = -ySize / 2 + 1; y < ySize / 2; y++)
            {
                Vector2 pos = new Vector2(x, y);
                if (!containedInSnake(pos))
                {
                    availablePositions.Add(pos);
                }
            }
        }

        if (availablePositions.Count > 0)
        {
            Vector2 spawnPos = availablePositions[Random.Range(0, availablePositions.Count)];
            food = Instantiate(block);
            RectTransform foodRect = food.GetComponent<RectTransform>();
            foodRect.SetParent(transform.parent, false);
            foodRect.sizeDelta = blockSize;
            foodRect.anchoredPosition = new Vector3(spawnPos.x * blockSize.x, spawnPos.y * blockSize.y, 0);
            food.GetComponent<Image>().sprite = foodSprite;
            food.tag = "Food";
            food.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No available positions for food!");
        }
    }

    private bool containedInSnake(Vector2 pos)
    {
        Vector2 headPos = new Vector2(
            Mathf.Round(head.GetComponent<RectTransform>().anchoredPosition.x / blockSize.x),
            Mathf.Round(head.GetComponent<RectTransform>().anchoredPosition.y / blockSize.y)
        );

        if (pos == headPos)
        {
            return true;
        }

        foreach (var item in tail)
        {
            Vector2 tailPos = new Vector2(
                Mathf.Round(item.GetComponent<RectTransform>().anchoredPosition.x / blockSize.x),
                Mathf.Round(item.GetComponent<RectTransform>().anchoredPosition.y / blockSize.y)
            );
            if (pos == tailPos)
            {
                return true;
            }
        }

        return false;
    }

    private Vector2 getRandomPos()
    {
        return new Vector2(Random.Range(-xSize / 2 + 1, xSize / 2), Random.Range(-ySize / 2 + 1, ySize / 2));
    }

    private void createGrid()
    {
        Transform parentTransform = transform.parent;
        RectTransform rectTransform = GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(300, 300);
        Vector2 canvasSize = rectTransform.sizeDelta;

        float blockWidth = (canvasSize.x - (xSize + 1)) / xSize;
        float blockHeight = (canvasSize.y - (ySize + 1)) / ySize;
        blockSize = new Vector2(blockWidth, blockHeight);

        for (int x = 0; x <= xSize; x++)
        {
            GameObject borderBottom = Instantiate(block) as GameObject;
            RectTransform borderBottomRect = borderBottom.GetComponent<RectTransform>();
            borderBottomRect.SetParent(parentTransform, false);
            borderBottomRect.sizeDelta = blockSize;
            borderBottomRect.anchoredPosition = new Vector2((x - xSize / 2) * blockSize.x, -(ySize / 2) * blockSize.y);

            GameObject borderTop = Instantiate(block) as GameObject;
            RectTransform borderTopRect = borderTop.GetComponent<RectTransform>();
            borderTopRect.SetParent(parentTransform, false);
            borderTopRect.sizeDelta = blockSize;
            borderTopRect.anchoredPosition = new Vector2((x - xSize / 2) * blockSize.x, (ySize - ySize / 2) * blockSize.y);
        }

        for (int y = 0; y <= ySize; y++)
        {
            GameObject borderRight = Instantiate(block) as GameObject;
            RectTransform borderRightRect = borderRight.GetComponent<RectTransform>();
            borderRightRect.SetParent(parentTransform, false);
            borderRightRect.sizeDelta = blockSize;
            borderRightRect.anchoredPosition = new Vector2(-(xSize / 2) * blockSize.x, (y - (ySize / 2)) * blockSize.y);

            GameObject borderLeft = Instantiate(block) as GameObject;
            RectTransform borderLeftRect = borderLeft.GetComponent<RectTransform>();
            borderLeftRect.SetParent(parentTransform, false);
            borderLeftRect.sizeDelta = blockSize;
            borderLeftRect.anchoredPosition = new Vector2((xSize - (xSize / 2)) * blockSize.x, (y - (ySize / 2)) * blockSize.y);
        }
    }

    void Update()
    {
        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                gameStarted = true;
                startPanel.SetActive(false);
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.S) && dir != Vector2.up)
        {
            AudioManager.Instance.PlaySound(SoundType.SnakeMove);
            nextDir = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.W) && dir != Vector2.down)
        {
            AudioManager.Instance.PlaySound(SoundType.SnakeMove);
            nextDir = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.D) && dir != Vector2.left)
        {
            AudioManager.Instance.PlaySound(SoundType.SnakeMove);
            nextDir = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.A) && dir != Vector2.right)
        {
            AudioManager.Instance.PlaySound(SoundType.SnakeMove);
            nextDir = Vector2.left;
        }

        moveTimer += Time.deltaTime;
        if (moveTimer >= timeBetweenMovements && isAlive)
        {
            moveTimer = 0;
            MoveSnake();
        }
    }

    private void MoveSnake()
    {
        dir = nextDir;

        Vector2 newPosition = head.GetComponent<RectTransform>().anchoredPosition + new Vector2(dir.x * blockSize.x, dir.y * blockSize.y);

        if (CheckCollision(newPosition))
        {
            GameOver();
            isAlive = false;
            return;
        }

        if (food != null && Vector2.Distance(newPosition, food.GetComponent<RectTransform>().anchoredPosition) < 0.1f)
        {
            EatFood(newPosition);
        }

        MoveTail();

        head.GetComponent<RectTransform>().anchoredPosition = newPosition;

        RotateHead();
    }

    private bool CheckCollision(Vector2 position)
    {
        if (position.x >= xSize / 2 * blockSize.x
            || position.x <= -xSize / 2 * blockSize.x
            || position.y >= ySize / 2 * blockSize.y
            || position.y <= -ySize / 2 * blockSize.y)
        {
            return true;
        }

        foreach (var item in tail)
        {
            if (Vector2.Distance(item.GetComponent<RectTransform>().anchoredPosition, position) < 0.1f)
            {
                return true;
            }
        }

        return false;
    }

    private void EatFood(Vector2 newPosition)
    {   
        AudioManager.Instance.PlaySound(SoundType.CollectApple);
        
        GameObject newTile = Instantiate(block);
        RectTransform newTileRect = newTile.GetComponent<RectTransform>();
        newTileRect.SetParent(transform.parent, false);
        newTileRect.sizeDelta = blockSize;
        newTileRect.anchoredPosition = head.GetComponent<RectTransform>().anchoredPosition;
        newTile.GetComponent<Image>().sprite = tailSprite;
        tail.Insert(0, newTile);
        Destroy(food);
        spawnFood();
        points.text = tail.Count.ToString();
    }

    private void MoveTail()
    {
        if (tail.Count > 0)
        {
            tail[tail.Count - 1].GetComponent<RectTransform>().anchoredPosition = head.GetComponent<RectTransform>().anchoredPosition;
            tail.Insert(0, tail[tail.Count - 1]);
            tail.RemoveAt(tail.Count - 1);
        }
    }

    private void RotateHead()
    {
        if (dir == Vector2.up)
        {
            head.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (dir == Vector2.down)
        {
            head.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (dir == Vector2.left)
        {
            head.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (dir == Vector2.right)
        {
            head.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }

    private void GameOver()
    {
        act3GameController.levelController.GameFinished(points.text);
        gameStarted = false;
    }

    public void DisplayHighScoreInGame()
    {
        HighScoreinGame.text = act3GameController.GetHighScore().ToString();
    }

    public void Restart()
    {
        foreach (var tile in tail)
        {
            Destroy(tile);
        }

        tail.Clear();
        Destroy(food);
        Destroy(head);
        createPlayer();
        spawnFood();

        isAlive = true;
        dir = Vector2.right;
        nextDir = dir;
        points.text = "0";
        gameStarted = true;
        DisplayHighScoreInGame();
    }

    public int GetPoints()
    {
        return tail.Count;
    }
}