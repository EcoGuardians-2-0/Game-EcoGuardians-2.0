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
        Debug.Log("Bird name: " + birdName);
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
            AudioManager.Instance.PlaySound(SoundType.foundBird);
            birdDictionary.Add(birdName, true);
            BirdsCount++;
            string normalizedBirdName = birdName.Trim().ToLower();

            switch (normalizedBirdName)
            {
                case "azulejo común":
                    birdReturnName = "Azulejo Común";
                    birdSprite = azulejo_comun;
                    break;
                case "piranga abejera":
                    birdReturnName = "Piranga Abejera";
                    birdSprite = Piranga_abejera;
                    break;
                case "jilguero aliblanco":
                    birdReturnName = "Jilguero Aliblanco";
                    birdSprite = Jilguero_aliblanco;
                    break;
                case "toche enjalmado":
                    Debug.Log("Encontrado");
                    birdReturnName = "Toche Enjalmado";
                    birdSprite = Toche_enjalmado;
                    break;
                default:
                    Debug.LogWarning("Bird name not found: " + birdName);
                    break;
            }

            EventManager.Scene.OnCatchBird.Invoke(birdReturnName, birdSprite);
            EventManager.Scene.OnUpdateBirdCaughtCount(BirdsCount);
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