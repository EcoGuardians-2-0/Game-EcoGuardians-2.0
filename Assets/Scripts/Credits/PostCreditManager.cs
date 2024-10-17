using UnityEngine;
using TMPro;
using System.Collections;

public class PostCreditManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PostCreditUI;
    [SerializeField]
    private GameObject timePanel; // Panel that contains the time
    [SerializeField]
    private GameObject messagePanel; // Panel that contains the recognition message
    [SerializeField]
    private GameObject buttonPanel; // Panel that contains the buttons
    [SerializeField]
    private GameObject socialPanel;

    private TextMeshProUGUI timeText; // Reference to the TextMeshProUGUI for the time
    private TextMeshProUGUI messageText; // Reference to the TextMeshProUGUI for the message

    private float startTime; // Variable to store the start time

    void OnEnable()
    {
        EventManager.Scene.OnFinishCredit += ShowResults;
    }

    void OnDisable()
    {
        EventManager.Scene.OnFinishCredit -= ShowResults;
    }

    void Start()
    {
        // Record the start time of the game
        startTime = Time.time;

        timeText = timePanel.GetComponentInChildren<TextMeshProUGUI>();
        messageText = messagePanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void ShowResults()
    {
        DisableObjects.Instance.showCursor();
        StartCoroutine(ShowPanels());
    }
    private IEnumerator ShowPanels()
    {
        // Show the time panel
        ShowTimePanel();
        yield return new WaitForSeconds(3f); // Wait for 3 seconds

        // Show the message panel
        ShowMessagePanel();
        yield return new WaitForSeconds(3f); // Wait for 3 seconds

        // Show the button panel
        ShowButtonPanel();

        yield return new WaitForSeconds(3f); // Wait for 3 seconds

        // Show the social media panel
        ShowSocialPanel();
    }

    private void ShowTimePanel()
    {
        // Format the time and update the text
        string formattedTime = FormatTime(Time.time - startTime);
        timeText.text = $"¡Exploraste la estación en <color=yellow>{formattedTime}</color>!";

        // Make the panel appear using LeanTween
        LeanTween.alpha(timePanel.GetComponent<RectTransform>(), 1f, 2f).setEase(LeanTweenType.easeInOutQuad);
        timePanel.SetActive(true); // Ensure the panel is active
    }

    private void ShowMessagePanel()
    {
        // Set up the message based on the elapsed time
        string message = GenerateMessage(Time.time - startTime);
        messageText.text = message;

        // Make the panel appear using LeanTween
        LeanTween.alpha(messagePanel.GetComponent<RectTransform>(), 1f, 2f).setEase(LeanTweenType.easeInOutQuad);
        messagePanel.SetActive(true); // Ensure the panel is active
    }

    private void ShowButtonPanel()
    {
        // Make the button panel appear using LeanTween
        LeanTween.alpha(buttonPanel.GetComponent<RectTransform>(), 1f, 2f).setEase(LeanTweenType.easeInOutQuad);
        buttonPanel.SetActive(true); // Ensure the panel is active
    }

    private void ShowSocialPanel()
    {
        // Make the social media panel appear using LeanTween
        LeanTween.alpha(socialPanel.GetComponent<RectTransform>(), 1f, 2f).setEase(LeanTweenType.easeInOutQuad);
        socialPanel.SetActive(true); // Ensure the panel is active
    }

    private string FormatTime(float seconds)
    {
        int hours = Mathf.FloorToInt(seconds / 3600); // Calculate hours
        int minutes = Mathf.FloorToInt((seconds % 3600) / 60); // Calculate minutes
        int secs = Mathf.FloorToInt(seconds % 60); // Calculate seconds
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, secs); // Format the time as hh:mm:ss
    }

    private string GenerateMessage(float totalTime)
    {
        if (totalTime < 1800) 
        {
            return $"¡Increíble! Completaste la exploración en un tiempo récord de menos de 30 minutos.";
        }
        else if (totalTime < 3600)
        {
            return $"¡Bien hecho! Has explorado la estación en menos de una hora, ¡parece que estabas decidido!";
        }
        else if (totalTime < 5400)
        {
            return $"¡Buen trabajo! Completaste la exploración en 1 hora y 30 minutos, lo que muestra tu dedicación.";
        }
        else
        {
            return $"¡Impresionante! Pasaste más de 1 hora y 30 minutos explorando la estación, ¡tu paciencia es admirable!";
        }
    }
}

