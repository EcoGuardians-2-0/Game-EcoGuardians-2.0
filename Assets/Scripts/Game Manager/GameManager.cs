using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private QuestManager questManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ActivateQuests("module1");
        }
    }
    
    public void ActivateQuests(string module)
    {
        new JSONReader().ReadQuests(questManager, module);
    }
}
