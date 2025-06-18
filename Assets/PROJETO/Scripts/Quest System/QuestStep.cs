using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isCompleted = false;
    private string questId;
    private int stepIndex;

    public void InitializeQuestStep(string questId, int stepIndex)
    {
        this.questId= questId; 
        this.stepIndex = stepIndex;
    }

    protected void CompleteStep()
    {
        if (!isCompleted)
        {
            isCompleted = true;
            GameEventsManager.Instance.questEvents.AdvanceQuest(questId);
            Debug.Log("Step concluído: " + gameObject.name);
            Destroy(gameObject);
        }
    }
     protected void ChangeState(string newState)
     {
        GameEventsManager.Instance.questEvents.QuestStepStateChange(questId, stepIndex, new QuestStepState(newState));
    }
   
}
