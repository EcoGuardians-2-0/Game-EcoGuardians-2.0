using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconHandler : MonoBehaviour
{
    private MiniMapComponent miniMapComponent;

    [SerializeField]
    private Sprite iconAbovePlayer;

    [SerializeField]
    private Sprite iconAtPlayerLevel;

    [SerializeField]
    private Sprite iconBelowPlayer;

    [SerializeField]
    private string questID;

    [SerializeField]
    private float gap;

    void Start()
    {
        miniMapComponent = GetComponent<MiniMapComponent>();
        miniMapComponent.icon = iconAtPlayerLevel;
        miniMapComponent.size = new Vector2(35, 35);
        gap = 2.0f;

    }

    void Update()
    {
        if (!miniMapComponent.enabled)
            return;

        PositionController positionController = PositionController.instance;

        if (positionController == null)
            return;

        float playerY = positionController.GetYPosition();
        float iconY = transform.position.y;

        if (playerY - iconY > gap)
            miniMapComponent.icon = iconBelowPlayer;
        else if (iconY - playerY > gap)
            miniMapComponent.icon = iconAbovePlayer;
        else
            miniMapComponent.icon = iconAtPlayerLevel;

        UpdateIcon();
    }

    void OnEnable()
    {
        EventManager.MapIcon.OnDisplayIconFiltered(questID).AddListener(HandleDisplayIcon);
    }

    void OnDisable()
    {
        EventManager.MapIcon.OnDisplayIconFiltered(questID).RemoveListener(HandleDisplayIcon);
    }

    public void HandleDisplayIcon(bool activate)
    {
        miniMapComponent.enabled = activate;
    }

    private void UpdateIcon()
    {
        HandleDisplayIcon(false);
        HandleDisplayIcon(true);
    }

}
