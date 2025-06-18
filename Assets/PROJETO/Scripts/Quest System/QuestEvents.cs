using System;
using UnityEngine;

public class QuestEvents
{
    public event Action<string> onQuestStarted;
    public event Action<string> onQuestAdvanced;
    public event Action<string> onQuestFinished;
    public event Action<Quest> onQuestStateChange;
    public event Action<string, int,QuestStepState> onQuestStepStateChange;

    public void StartQuest(string id)
    {
        Debug.Log($"[QuestEvents] Iniciando a quest: {id}");
        if (onQuestStarted != null)
        {
            onQuestStarted(id);
        }
    }

    public void AdvanceQuest(string id)
    {
        Debug.Log($"[QuestEvents] Avançando a quest: {id}");
        if (onQuestAdvanced != null)
        {
            onQuestAdvanced(id);
        }
    }

    public void FinishedQuest(string id)
    {
        Debug.Log($"[QuestEvents] Finalizando a quest: {id}");
        if (onQuestFinished != null)
        {
            onQuestFinished(id);
        }
    }

    public void QuestStateChange(Quest quest)
    {
        Debug.Log($"[QuestEvents] Mudança de estado na quest: {quest.questInfo.questName} para {quest.state}");
        if (onQuestStateChange != null)
        {
            onQuestStateChange(quest);
        }
    }
    public void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        
        if (onQuestStepStateChange != null)
        {
            onQuestStepStateChange(id, stepIndex,questStepState);
        }
    }
}
