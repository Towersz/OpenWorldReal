using UnityEngine;


[RequireComponent(typeof(Collider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoforPoint;

    private bool playerIsNear = false;
    private string questId;
    private QuestState CurrentQuestState;

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
