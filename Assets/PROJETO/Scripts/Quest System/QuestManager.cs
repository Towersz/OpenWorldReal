using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public Inventory playerInventory; // arraste no Inspector OU use FindObjectOfType no Start


    private Dictionary<string, Quest> questMap;

    private void Awake()
    {
        questMap = CreateQuestMap();

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void EnemyKilled(string enemyType)
    {
        var enemySteps = FindObjectsOfType<EnemyKillQuestStep>();

        foreach (var step in enemySteps)
        {
            step.OnEnemyKilled(enemyType);
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
        playerInventory = FindObjectOfType<Inventory>(); // localiza o inventário na cena

        foreach (Quest quest in questMap.Values)
        {
            GameEventsManager.Instance.questEvents.QuestStateChange(quest);
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.Instance.questEvents.QuestStateChange(quest);
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        if (quest == null) return;

        quest.InstantiateCurrentStep(this.transform);
        ChangeQuestState(quest.questInfo.id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);

        quest.MoveToNextStep();

        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentStep(this.transform);
        }
        else
        {
            ChangeQuestState(quest.questInfo.id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
       Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.questInfo.id, QuestState.FINISHED);
    }

    private void ClaimRewards(Quest quest)
    {
        if (quest.questInfo.itens == null || quest.questInfo.itens.Length == 0)
        {
            Debug.Log($"Nenhuma recompensa definida para a quest {quest.questInfo.questName}");
            return;
        }

        if (playerInventory == null)
        {
            Debug.LogWarning("Inventário do jogador não encontrado.");
            return;
        }

        foreach (GameObject rewardPrefab in quest.questInfo.itens)
        {
            if (rewardPrefab != null)
            {
                // Instancia o item sem colocá-lo na cena  
                GameObject rewardInstance = Instantiate(rewardPrefab);
                rewardInstance.SetActive(false); // opcional: desativa visualmente se necessário  

                // Obtém o script Item associado ao prefab  
                Item item = rewardInstance.GetComponent<Item>();
                if (item != null)
                {
                    // Envia para o inventário com quantidade padrão de 1  
                    playerInventory.AddItem(item, 1);
                }
                else
                {
                    Debug.LogWarning($"O prefab {rewardPrefab.name} não possui um componente Item.");
                }
            }
        }

        Debug.Log($"Recompensas da quest '{quest.questInfo.questName}' adicionadas ao inventário.");
    }



    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning($"Duplicate quest ID found:" + questInfo.id);
                continue;
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        if (questMap.TryGetValue(id, out Quest quest))
        {
            return quest;
        }

        Debug.LogWarning($"Quest with ID {id} not found.");
        return null;
    }

    public void TriggerQuestFromPoint(string questId)
    {
        GameEventsManager.Instance.questEvents.StartQuest(questId);
    }

    private void OnApplicationQuit()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.IN_PROGRESS)
            {
                ChangeQuestState(quest.questInfo.id, QuestState.CAN_START);
            }
        }
    }
}
