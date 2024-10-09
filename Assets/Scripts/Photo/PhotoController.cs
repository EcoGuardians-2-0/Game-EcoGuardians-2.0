using UnityEngine;
using UnityEngine.UI;

public class PhotoController : MonoBehaviour
{
    [SerializeField]
    private GameObject displayBirdCount;

    [SerializeField]
    private Text birdCountText;

    private int BirdsCount = 0;

    public void AddBird()
    {
        BirdsCount++;
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
