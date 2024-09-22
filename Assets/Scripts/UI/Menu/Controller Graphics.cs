using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControllerGraphics : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int quality;

    private static ControllerGraphics _Instance;
    public static ControllerGraphics Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<ControllerGraphics>();
                // name it for easy recognition
                _Instance.name = _Instance.GetType().ToString();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

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
