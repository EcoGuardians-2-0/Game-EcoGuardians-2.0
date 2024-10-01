using UnityEngine.Events;

public class EventManager 
{
    public readonly static WallEvents Wall = new WallEvents();  
    public readonly static QuestEvents Quest = new QuestEvents();
    public class WallEvents
    {
        public UnityAction<int, bool> OnDisplayText;
        public UnityAction<int> OnDisabeWall;
    }

    public class QuestEvents
    {
        public UnityAction OnQuestAssigned;
        public UnityAction<string> OnQuestCompleted;
        public UnityAction OnAllQuestsCompleted;
        public UnityAction OnQuestionnaireCompleted;
    } 
}

