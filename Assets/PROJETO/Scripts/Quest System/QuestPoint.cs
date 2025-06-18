using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoforPoint;

    [Header("Config")]
    [SerializeField] private bool StartPoint = true;
    [SerializeField] private bool EndPoint = true;


    private bool playerIsNear = false;
    private string questId;
    private QuestState CurrentQuestState;

    private void Awake()
    {
        questId = questInfoforPoint.id;
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
    }
    public void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onQuestStateChange -= QuestStateChange;
    }
    private void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.Q))
        {
            SubmitPressed(); // Chama o mesmo método que era usado no evento
        }
    }


    private void SubmitPressed()
    {
        if (!playerIsNear)
        {
            return;
        }
        if (CurrentQuestState.Equals(QuestState.CAN_START) && StartPoint)
        {
            GameEventsManager.Instance.questEvents.StartQuest(questId);
        }
        else if (CurrentQuestState.Equals(QuestState.CAN_FINISH) && EndPoint)
        {
            GameEventsManager.Instance.questEvents.FinishedQuest(questId);
        }
        
    }
    private void QuestStateChange(Quest quest)
    {
        if (quest.questInfo.id.Equals(questId))
        {
            CurrentQuestState = quest.state;
            Debug.Log($"Quest state changed: {quest.questInfo.questName} is now {CurrentQuestState}");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
            // Trigger any quest-related logic here, such as starting a quest or updating quest status
           // GameEventsManager.Instance.questEvents.QuestStateChange(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            // Optionally, you can handle logic when the player leaves the quest point area
        }
    }
}
