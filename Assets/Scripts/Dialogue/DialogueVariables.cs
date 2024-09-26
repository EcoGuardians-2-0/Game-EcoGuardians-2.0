using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;

public class DialogueVariables
{

    public Dictionary<string, Dictionary<string, Ink.Runtime.Object>> variables;
    private string currentStoryName;
    private DialogueNotifier eventNotifier;

    public DialogueVariables()
    {
        eventNotifier = new DialogueNotifier();
        variables = new Dictionary<string, Dictionary<string, Ink.Runtime.Object>>();
        variables["globals"] = new Dictionary<string, Ink.Runtime.Object>();
    }

    public void initializeVariables(TextAsset textAsset)
    {
        Story storyVariables = new Story(textAsset.text);

        currentStoryName = textAsset.name;
        Debug.Log("Script inicializado: " + currentStoryName);


        if (!variables.ContainsKey(currentStoryName))
        {
            variables[currentStoryName] = new Dictionary<string, Ink.Runtime.Object>();
            foreach (string name in storyVariables.variablesState)
            {
                Ink.Runtime.Object value = storyVariables.variablesState.GetVariableWithName(name);

                if (name.StartsWith("global"))
                {
                    if (currentStoryName.StartsWith("Load"))
                        variables["globals"][name] =  value;
                }
                else
                {
                    variables[currentStoryName][name] = value;
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

    public void setVariableValue(string variableName, Ink.Runtime.Object value)
    {
        if (variables["globals"].ContainsKey(variableName))
        {
            variables["globals"][variableName] = value;
        }
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (name.StartsWith("global"))
        {
            variables["globals"][name] = value;
            eventNotifier.CheckForEventTrigger(name, value);
        }
        else
        {
            variables[currentStoryName][name] = value;
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
                story.variablesState.SetGlobal(name, variables["globals"][name]);
            }
            else
            {
                story.variablesState.SetGlobal(name, variables[currentStoryName][name]);
            }
        }
    }
    private void printDictionaries()
    {
        foreach (KeyValuePair<string, Dictionary<string, Ink.Runtime.Object>> scripts in variables)
        {
            Debug.Log("Script: " + scripts.Key);
            foreach (KeyValuePair<string, Ink.Runtime.Object> variables in scripts.Value)
            {
                Debug.Log("Variable name: " + variables.Key + " = " + variables.Value);
            }
        }
    }
}
