using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhotoController : MonoBehaviour
{
    [SerializeField]
    private GameObject displayBirdCount;
    [SerializeField]
    private Text birdCountText;
    [SerializeField]
    private Sprite azulejo_comun;
    [SerializeField]
    private Sprite Piranga_abejera;
    [SerializeField]
    private Sprite Jilguero_aliblanco;
    [SerializeField]
    private Sprite Toche_enjalmado;

    private int BirdsCount = 0;
    private Sprite birdSprite;
    private string birdReturnName;

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

            switch (birdName)
            {
                case "Azulejo Común":
                    birdReturnName = "Azulejo Común";
                    birdSprite = azulejo_comun;
                    break;
                case "Piranga Abejera":
                    birdReturnName = "Piranga Abejera";
                    birdSprite = Piranga_abejera;
                    break;
                case "Jilguero Aliblanco":
                    birdReturnName = "Jilguero Aliblanco";
                    birdSprite = Jilguero_aliblanco;
                    break;
                case "Toche Enjalmado":
                    birdReturnName = "Toche Enjalmado";
                    birdSprite = Toche_enjalmado;
                    break;
            }
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