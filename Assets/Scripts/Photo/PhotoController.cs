using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhotoController : MonoBehaviour
{
    [SerializeField]
    private GameObject displayBirdCount;

    [SerializeField]
    private Text birdCountText;

    private int BirdsCount = 0;
    private Dictionary<string, bool> birdDictionary;

    void Start()
    {
        birdDictionary = new Dictionary<string, bool>();
    }

    public bool AddBird(string birdName)
    {
        if (birdDictionary.ContainsKey(birdName))
        {
            if (!birdDictionary[birdName])
            {
                birdDictionary[birdName] = true;
                BirdsCount++;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            birdDictionary.Add(birdName, true);
            BirdsCount++;
            return true;
        }
    }

    public void DisplayBirdCount()
    {
        birdCountText.text = BirdsCount.ToString();
        displayBirdCount.SetActive(true);
    }

    public void HideBirdCount()
    {
        displayBirdCount.SetActive(false);
    }
}