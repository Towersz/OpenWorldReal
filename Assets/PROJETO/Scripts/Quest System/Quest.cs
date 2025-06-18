using System.Security.Cryptography;
using UnityEngine;

public class Quest 
{
    public QuestInfoSO questInfo;

    public QuestState state;

    private int currentStepIndex;
    private QuestStepState[] questStepStates;

    public Quest(QuestInfoSO questInfo)
    {
        this.questInfo = questInfo;
        this.state = QuestState.CAN_START;
        this.currentStepIndex = 0;
        this.questStepStates = new QuestStepState[questInfo.questStepsPrefabs.Length];
        for (int i = 0; i < questStepStates.Length; i++)
        {
            questStepStates[i] = new QuestStepState();
        }
    }

    public void MoveToNextStep()
    {
        currentStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return currentStepIndex < questInfo.questStepsPrefabs.Length;
    }

    public void InstantiateCurrentStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentStepPrefab(); 

        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform)
                 .GetComponent<QuestStep>();
            questStep.InitializeQuestStep(questInfo.id, currentStepIndex);
        }
    }

    private GameObject GetCurrentStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (CurrentStepExists())
        {
            questStepPrefab = questInfo.questStepsPrefabs[currentStepIndex];
        }
        else
        {
            Debug.LogWarning($"Tried to get a step prefab, but stepIndex was out of range indicating that " +
                             $"there's no current step: QuestId = {questInfo.id}, stepIndex = {currentStepIndex}");
        }

        return questStepPrefab;
    }

    public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
    {
        if (stepIndex < questStepStates.Length)
        {
            questStepStates[stepIndex].state = questStepState.state;
        }
    }

    public QuestData GetQuestData()
    {
        return new QuestData(state, currentStepIndex, questStepStates);
    }
}
