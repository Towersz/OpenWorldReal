using System.Security.Cryptography;
using UnityEngine;

public class Quest 
{
    public QuestInfoSO questInfo;

    public QuestState state;

    private int currentStepIndex;

    public Quest(QuestInfoSO questInfo)
    {
        this.questInfo = questInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentStepIndex = 0;
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
            Object.Instantiate<GameObject>(questStepPrefab, parentTransform);
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
}
