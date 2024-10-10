using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;  // Import TextMeshPro namespace
using UnityEngine.UI;


public class CreditsManager : MonoBehaviour
{
    public GameObject gameTitlePrefab;  // Prefab for Game_Title
    public GameObject segmentTitlePrefab;  // Prefab for Segment_Title
    public GameObject segmentPrefab;  // Prefab for Segment which has a role and names
    public GameObject additionalMessagePrefab;  // Prefab for Additional Messages
    public RectTransform creditsContainer;  // The container where the credit elements will be added

    // Path to the credits file
    public string creditsFilePath = "Assets/Resources/credits.txt";

    void Start()
    {
        LoadCreditsFromFile(creditsFilePath);
    }

    void LoadCreditsFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("Credits file not found at: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            if (line.StartsWith("game_title:"))
            {
                string titleText = line.Replace("game_title:", "").Trim();
                CreateGameTitle(titleText);
            }
            else if (line.StartsWith("segment_title:"))
            {
                string segmentTitleText = line.Replace("segment_title:", "").Trim();
                CreateSegmentTitle(segmentTitleText);
            }
            else if (line.StartsWith("segment:"))
            {
                string segmentText = line.Replace("segment:", "").Trim();
                CreateSegment(segmentText);
            }
            else if (line.StartsWith("additional_message:"))
            {
                string messageText = line.Replace("additional_message:", "").Trim();
                CreateAdditionalMessage(messageText);
            }
        }
    }

    void CreateGameTitle(string title)
    {
        GameObject gameTitle = Instantiate(gameTitlePrefab, creditsContainer);
        gameTitle.GetComponentInChildren<TextMeshProUGUI>().text = title;  // Use TextMeshProUGUI to assign the text
    }

    void CreateSegmentTitle(string title)
    {
        GameObject segmentTitle = Instantiate(segmentTitlePrefab, creditsContainer);
        segmentTitle.GetComponentInChildren<TextMeshProUGUI>().text = title;
    }

    void CreateSegment(string segmentData)
    {
        // Split the role and the names using the "|" delimiter
        string[] parts = segmentData.Split('|');
        if (parts.Length == 2)
        {
            string role = parts[0].Trim();  // The role (e.g., "Programmer")
            string names = parts[1].Trim();  // The names (e.g., "John Doe, Jane Smith, Bob Brown")

            // Replace commas with newlines so each name is on a different line
            names = names.Replace(",", "\n");

            // Instantiate the prefab and assign the role and names text
            GameObject segment = Instantiate(segmentPrefab, creditsContainer);

            // Assume the prefab has two TextMeshProUGUI components for the role and the names
            TextMeshProUGUI[] texts = segment.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = role;  // Assign the role text
            texts[1].text = names;  // Assign the names text with newlines
        }
        else
        {
            Debug.LogError("Error processing segment: " + segmentData);
        }
    }

    void CreateAdditionalMessage(string message)
    {
        // Instantiate the prefab for additional messages
        GameObject messageObject = Instantiate(additionalMessagePrefab, creditsContainer);
        // Set the message text
        messageObject.GetComponentInChildren<TextMeshProUGUI>().text = message;
    }

    IEnumerator AnimateCredits()
    {
        yield return new WaitForEndOfFrame();

        // Force rebuild of the layout to make sure we have the correct height
        RectTransform rectTransform = creditsContainer.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        // Get the current anchored position
        Vector2 startPosition = rectTransform.anchoredPosition;

        // Get the height of the RectTransform after layout rebuild
        float containerHeight = rectTransform.rect.height;

        // Calculate the end position by subtracting the container's height from the Y position
        Vector2 endPosition = new Vector2(startPosition.x, startPosition.y + containerHeight + 1080f);

        // Animate the credits to move upwards over 40 seconds (or adjust the duration as needed)
        LeanTween.move(creditsContainer.GetComponent<RectTransform>(), endPosition, 20f).setEase(LeanTweenType.linear);
    }




}
