using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog",menuName = "Dialog System/DialogData")]
public class DialogueData : ScriptableObject
{
    [SerializeField]
    public List<DialogueNode> dialogNodes;
}
