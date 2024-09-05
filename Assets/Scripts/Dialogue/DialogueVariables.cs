using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;

public class DialogueVariables
{

    public Dictionary<string, Dictionary<string, Ink.Runtime.Object>> variables2;

    private string currentStoryName;

    public Dictionary<string, Ink.Runtime.Object> variables;


    public DialogueVariables()
    {
        variables2 = new Dictionary<string, Dictionary<string, Ink.Runtime.Object>>();
        variables2["globals"] = new Dictionary<string, Ink.Runtime.Object>();
    }

    public void initializeVariables(TextAsset textAsset)
    {
        Story storyVariables = new Story(textAsset.text);

        currentStoryName = textAsset.name;
        Debug.Log("Script inicializado: " + currentStoryName);


        if (!variables2.ContainsKey(currentStoryName))
        {
            variables2[currentStoryName] = new Dictionary<string, Ink.Runtime.Object>();
            foreach (string name in storyVariables.variablesState)
            {
                Ink.Runtime.Object value = storyVariables.variablesState.GetVariableWithName(name);

                if (name.StartsWith("global"))
                {
                    if (currentStoryName.StartsWith("Load"))
                        variables2["globals"][name] =  value;
                }
                else
                {
                    variables2[currentStoryName][name] = value;
                }
            }
        }
    }

    public void StartListening(Story story)
    {
        VariablesToStory(story);
        printDictionaries();
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
        printDictionaries();
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (name.StartsWith("global"))
        {
            variables2["globals"][name] = value;
        }
        else
        {
            variables2[currentStoryName][name] = value;
        }
    }

    public Ink.Runtime.Object searchVariable(string variableName)
    {
        Ink.Runtime.Object variableValue = null;

        if (variableName.StartsWith("global"))
        {
            variables2["globals"].TryGetValue(variableName, out variableValue);
        }
        else
        {
            variables2[currentStoryName].TryGetValue(variableName, out variableValue);
        }
        return variableValue;
    }

    private void printDictionaries()
    {
        foreach(KeyValuePair<string, Dictionary<string, Ink.Runtime.Object>> scripts in variables2)
        {
            Debug.Log("Script: " + scripts.Key);
            foreach(KeyValuePair<string, Ink.Runtime.Object> variables in scripts.Value)
            {
                Debug.Log("Variable name: " + variables.Key + " = " +  variables.Value);
            }
        }
    }

    private void VariablesToStory(Story story)
    {
        Debug.Log(story.variablesState);

        List<string> variableNames = new List<string>(story.variablesState);

        foreach (string name in variableNames)
        {
            if (name.StartsWith("global"))
            {
                story.variablesState.SetGlobal(name, variables2["globals"][name]);
            }
            else
            {
                story.variablesState.SetGlobal(name, variables2[currentStoryName][name]);
            }
        }
    }
}
