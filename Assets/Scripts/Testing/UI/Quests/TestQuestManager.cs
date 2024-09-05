using UnityEngine;

public class TestQuestManager : MonoBehaviour
{
    public QuestManager questManager;

    void Start()
    {
    }

    void Update()
    {
        // Marcar una tarea como completada al presionar una tecla (por ejemplo, 'C')
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (questManager.GetActiveQuestsCount() > 0)
            {
                questManager.CompleteQuest("TEST " + (questManager.GetActiveQuestsCount() - 1));
            }
        }

        // Mostrar la lista de tareas al presionar 'L'
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!AnimationsQuest.Instance.GetQuestsState())
                AnimationsQuest.Instance.ShowQuestsUI();
            else
                AnimationsQuest.Instance.HideQuestsUI();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            // Marcar la tarea como activa
            questManager.AddQuest("TEST " + questManager.GetActiveQuestsCount(), "Quest example " + questManager.GetActiveQuestsCount());
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            // Marcar la tarea como activa
            questManager.AddTitle("TEST " + questManager.GetActiveTitlesCount(), "Quests' title " + questManager.GetActiveTitlesCount());
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            questManager.DeleteQuest("TEST " + (questManager.GetActiveQuestsCount() - 1));
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            questManager.DeleteTitle("TEST " + (questManager.GetActiveTitlesCount() - 1));
        }
    }
}
