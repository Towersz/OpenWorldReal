using System;

public class QuestEvents 
{
  public event Action <string> onQuestStarted;

    public void StartQuest(string id)
    {
        if (onQuestStarted != null)
        {
            onQuestStarted(id);
        }

    }

    public event Action<string> onQuestAdvanced;

    public void AdvanceQuest(string id)
    {
        if (onQuestAdvanced != null)
        {
            onQuestAdvanced(id);
        }

    }

    public event Action<string> onQuestFinished;

    public void FinishedQuest(string id)
    {
        if (onQuestFinished != null)
        {
            onQuestFinished(id);
        }

    }

    public event Action<Quest> onQuestStateChange;

    public void QuestStateChange(Quest quest)
    {
        if (onQuestStateChange != null)
        {
            onQuestStateChange(quest);
        }

    }

}
