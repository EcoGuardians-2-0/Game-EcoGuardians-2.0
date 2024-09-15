using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButtons : MonoBehaviour
{   
    [SerializeField] 
    private Transform puzzleField;

    [SerializeField]
    private GameObject button;

    void Awake()
    {
        // Fill the puzzle field with buttons
        for(int i = 0; i < 8; i++)
        {
            GameObject btn = Instantiate(button);
            btn.name = "" + i;
            btn.transform.SetParent(puzzleField, false);
        }
    }
}
