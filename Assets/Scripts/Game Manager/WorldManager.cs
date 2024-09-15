using UnityEngine;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToHide = new List<GameObject>();
    [SerializeField] private List<MonoBehaviour> scriptsToDisable = new List<MonoBehaviour>();

    private List<GameObject> hiddenObjects = new List<GameObject>();
    private List<MonoBehaviour> disabledScripts = new List<MonoBehaviour>();

    private static WorldManager _Instance;
    public static WorldManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<WorldManager>();
                if (_Instance == null)
                {
                    GameObject go = new GameObject("WorldManager");
                    _Instance = go.AddComponent<WorldManager>();
                }
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void HideWorld()
    {
        if (objectsToHide == null || objectsToHide.Count == 0)
        {
            Debug.LogWarning("No objects to hide. Make sure to assign objects in the Inspector.");
            return;
        }

        foreach (var obj in objectsToHide)
        {
            if (obj != null)
            {
                HideObjectAndChildren(obj);
            }
        }
        DisableScripts();
    }

    public void ShowWorld()
    {
        foreach (var obj in hiddenObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        hiddenObjects.Clear();

        EnableScripts();
    }

    private void HideObjectAndChildren(GameObject obj)
    {
        if (obj.activeSelf)
        {
            obj.SetActive(false);
            hiddenObjects.Add(obj);
        }

        foreach (Transform child in obj.transform)
        {
            HideObjectAndChildren(child.gameObject);
        }
    }

    private void DisableScripts()
    {
        if (scriptsToDisable == null || scriptsToDisable.Count == 0)
        {
            Debug.LogWarning("No scripts to disable. Make sure to assign scripts in the Inspector if needed.");
            return;
        }

        foreach (var script in scriptsToDisable)
        {
            if (script != null && script.enabled)
            {
                script.enabled = false;
                disabledScripts.Add(script);
            }
        }
    }

    private void EnableScripts()
    {
        foreach (var script in disabledScripts)
        {
            if (script != null)
            {
                script.enabled = true;
            }
        }
        disabledScripts.Clear();
    }
}