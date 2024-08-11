using System.Collections.Generic;
using UnityEngine;

public class TVImagesManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private List<Sprite> images = new List<Sprite>();

    private int currentIndex = 0;

    // Start is called before the first frame update
    public void Init()
    {
        // Get component reference
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Asure there are images in the list and show the first image
        if (images.Count > 0)
            spriteRenderer.sprite = images[currentIndex];

        DisableObjects.Instance.ToggleControlsImageTVUI(true);
    }

    // Turn off the TV
    public void TurnOff()
    {
       spriteRenderer.sprite = null;
    }

    // Update is called once per frame
    private void Update()
    {
        // Change to the next image if the right arrow is pressed
        if (Input.GetKeyDown(KeyCode.RightArrow))
            NextImage();

        // Change to the previous image if the left arrow is pressed
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            PreviousImage();

        // Hide or show the controls image TV UI
        if (Input.GetKeyDown(KeyCode.Q))
            DisableObjects.Instance.ToggleControlsImageTVUI(false);
    }

    // Change to the next image
    private void NextImage()
    {
        currentIndex = (currentIndex + 1) % images.Count;
        spriteRenderer.sprite = images[currentIndex];
    }

    // Change to the previous image
    private void PreviousImage()
    {
        currentIndex--;

        if (currentIndex < 0)
            currentIndex = images.Count - 1;

        spriteRenderer.sprite = images[currentIndex];
    }
}
