using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconHandler : MonoBehaviour
{
    private MiniMapComponent miniMapComponent;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private string questID;

    void Start()
    {
        miniMapComponent = GetComponent<MiniMapComponent>();
        miniMapComponent.icon = icon;
        miniMapComponent.size = new Vector2(35, 35);
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

}
