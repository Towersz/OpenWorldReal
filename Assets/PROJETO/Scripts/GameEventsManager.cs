using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
   public static GameEventsManager Instance { get; private set; }

    public QuestEvents questEvents;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of GameEventsManager detected. Destroying the new instance.");
        }
        Instance = this;

        questEvents = new QuestEvents();
    }
}
