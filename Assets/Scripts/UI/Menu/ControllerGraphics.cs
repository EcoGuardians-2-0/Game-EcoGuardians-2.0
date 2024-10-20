using System.Collections;
using UnityEngine;
using TMPro;

public class ControllerGraphics : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public GameObject AlertChangesSaved;
    private int quality;
    private int originalQuality;
    private static ControllerGraphics _Instance;
    
    public static ControllerGraphics Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<ControllerGraphics>();
                _Instance.name = _Instance.GetType().ToString();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    void Start()
    {
        // Cambiar el valor por defecto a 1 para iniciar en el segundo nivel
        quality = PlayerPrefs.GetInt("QualityNumber", 1);
        originalQuality = quality; // Guardar el valor original de calidad

        // Añadir el listener del dropdown antes de establecer el valor
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        // Configurar el valor del dropdown
        dropdown.value = quality; // Establecer el valor actual

        AdjustQuality(); // Ajustar la calidad según el valor inicial
    }

    public void AdjustQuality()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        quality = dropdown.value; // Actualizar la calidad actual
    }

    public void ApplyChanges()
    {
        // Verificar si hubo cambios antes de aplicar
        if (HasQualityChanged())
        {
            PlayerPrefs.SetInt("QualityNumber", quality);
            PlayerPrefs.Save();
            originalQuality = quality; // Actualizar el valor original al nuevo valor guardado
            AdjustQuality();
            StartCoroutine(ActivateAndDeactivate());
        }
    }

    public void CancelChanges()
    {
        dropdown.value = originalQuality;
        quality = originalQuality; // Asegurarse de que quality también se actualice
    }

    public IEnumerator ActivateAndDeactivate()
    {
        if (!AlertChangesSaved.activeSelf)
        {
            AlertChangesSaved.SetActive(true);

            yield return new WaitForSeconds(3f);

            AlertChangesSaved.SetActive(false);
        }
    }

    public bool HasQualityChanged()
    {
        return quality != originalQuality; // Verificar si la calidad ha cambiado
    }

    private void OnDropdownValueChanged(int value)
    {
        AdjustQuality();
    }
}
