using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager 
{
    public readonly static WallEvents Wall = new WallEvents();  
    public readonly static QuestEvents Quest = new QuestEvents();
    public readonly static MapIconEvents MapIcon = new MapIconEvents();
    public readonly static LetterEvents Letter = new LetterEvents();
    public readonly static TutorialEvents Tutorial = new TutorialEvents();
    public readonly static PauseEvents Pause = new PauseEvents();
    public readonly static PhotographEvents Photograph = new PhotographEvents();
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

    public class MapIconEvents
    {
        public class IconEvent: UnityEvent<bool> { };
        private Dictionary <string, IconEvent> mapOnDisplayIcon = new Dictionary<string, IconEvent> ();

        public IconEvent OnDisplayIconFiltered(string channel = "")
        {
            mapOnDisplayIcon.TryAdd(channel, new IconEvent());
            return mapOnDisplayIcon[channel];
        }
    }

    public class LetterEvents
    {
        public UnityAction<Sprite, bool> OnDisplayLetterImage;
    }
    public class TutorialEvents
    {
        public UnityAction OnFinishedTutorialDialogue;
    }

    public class PauseEvents
    {
        public UnityAction OnUnpause;
    }

    public class PhotographEvents
    {
        public UnityAction<bool> OnActiveCamera;
    }
}

