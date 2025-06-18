using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public CatacumbasQuestSteps catacumbasQuest;

    private void Awake()
    {
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
}
