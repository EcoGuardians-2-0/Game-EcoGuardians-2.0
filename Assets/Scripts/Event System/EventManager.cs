using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EventManager;

public class EventManager 
{
    public readonly static WallEvents Wall = new WallEvents();  
    public readonly static QuestEvents Quest = new QuestEvents();
    public readonly static MapIconEvents MapIcon = new MapIconEvents();
    public readonly static LetterEvents Letter = new LetterEvents();
    public readonly static TutorialEvents Tutorial = new TutorialEvents();
    public readonly static PauseEvents Pause = new PauseEvents();
    public readonly static PhotographEvents Photograph = new PhotographEvents();
    public readonly static CameraViewEvents CameraView = new CameraViewEvents();
    public readonly static MinimapEvents Minimap = new MinimapEvents();
    public readonly static QuestUIEvents QuestUI = new QuestUIEvents();
    public readonly static SceneEvents Scene = new SceneEvents();
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
        public UnityAction<bool> OnFirstPerson;
    }

    public class CameraViewEvents
    {
        public UnityAction OnChangeCameraView;
    }

    public class MinimapEvents
    {
        public UnityAction OnDisplayMinimap;
        public UnityAction OnLockMiniMap;
        public UnityAction OnUnlockMiniMap;
        public UnityAction OnGeneralUnlockMiniMap;
    }
    public class QuestUIEvents
    {
        public UnityAction OnDisplayQuestUI;
        public UnityAction OnLockQuestUI;
        public UnityAction OnUnlockQuestUI;
        public UnityAction OnGeneralUnlockQuestUI;

    }

    public class SceneEvents
    {
        public UnityAction OnPlayCredit;
        public UnityAction OnFinishCredit;
        public UnityAction<string, Sprite> OnCatchBird;
        public UnityAction<int> OnUpdateBirdCaughtCount;
    }
}

