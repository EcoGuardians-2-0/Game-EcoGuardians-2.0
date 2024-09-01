using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControllerGraphics : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int quality;
    // Start is called before the first frame update
    void Start()
    {
        quality = PlayerPrefs.GetInt("QualityNumber", 1);
        dropdown.value = quality;
        AdjustQuality();
    }

    public void AdjustQuality()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("QualityNumber", dropdown.value);
        quality = dropdown.value;
    }
}
