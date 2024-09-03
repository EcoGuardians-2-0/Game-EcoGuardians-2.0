using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;

public class DialogueVariables
{

    public Dictionary<string, Ink.Runtime.Object> variables;


    public DialogueVariables()
    {
        variables = new Dictionary<string, Ink.Runtime.Object>();
    }
    public DialogueVariables(TextAsset loadGlobalsJSON)
    {
        variables = new Dictionary<string, Ink.Runtime.Object>();
        initializeVariables(loadGlobalsJSON);
    }
    public void initializeVariables(TextAsset textAsset)
    {
        Story globalVariablesStory = new Story(textAsset.text);

        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            if (!variables.ContainsKey(name))
            {
                Debug.Log("Initialized global dialogue variable " + name + " = " + value);
                variables.Add(name, value);
            }
        }
    }

    public void StartListening(Story story)
    {
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (variables.ContainsKey(name))
        {
            Debug.Log("Variable changed " + " name: " + name + " = " + value);
            variables.Remove(name);
            variables.Add(name, value);
        }
    }

    private void VariablesToStory(Story story)
    {
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
