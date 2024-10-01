using UnityEngine.Events;

public class EventManager 
{
    public readonly static WallEvents Wall = new WallEvents();  

    public class WallEvents
    {
        public UnityAction<int, bool> OnDisplayText;
        public UnityAction<int> OnDisabeWall;
    }
}

