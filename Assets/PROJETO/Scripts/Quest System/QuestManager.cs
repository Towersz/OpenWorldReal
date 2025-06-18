using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public CatacumbasQuestSteps catacumbasQuest;

    private void Awake()
    {
        questMap = CreateQuestMap();

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void EnemyKilled(string enemyType)
    {
        if (catacumbasQuest != null)
        {
            catacumbasQuest.OnEnemyKilled(enemyType);
        }
    }
    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onQuestStarted += StartQuest;
        GameEventsManager.Instance.questEvents.onQuestAdvanced += AdvanceQuest;
        GameEventsManager.Instance.questEvents.onQuestFinished += FinishQuest;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onQuestStarted -= StartQuest;
        GameEventsManager.Instance.questEvents.onQuestAdvanced -= AdvanceQuest;
        GameEventsManager.Instance.questEvents.onQuestFinished -= FinishQuest;
    }

    private void Start()
    {
        // Initialize or start any quests here if needed
        // For example, you can start a quest directly:
        // StartQuest("quest_id_here");
        foreach(Quest quest in questMap.Values)
        {
           GameEventsManager.Instance.questEvents.QuestStateChange(quest);
            
        }
    }

    private void StartQuest(string id)
    {

    }


    private void AdvanceQuest(string id)
    {

    }

    private void FinishQuest (string id)
    {

    }
    private Dictionary<string, Quest> questMap;

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning($"Duplicate quest ID found:" + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogWarning($"Quest with ID {id} not found.");
        }
        return quest;
    }


}