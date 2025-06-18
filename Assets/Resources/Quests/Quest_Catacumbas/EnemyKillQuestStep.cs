using UnityEngine;

public class EnemyKillQuestStep : QuestStep
{
    private int enemiesKilled = 0;
    public int enemiesToKill = 6;
    public string targetEnemyType;

    public void OnEnemyKilled(string enemyType)
    {
        if (enemyType == targetEnemyType)
        {
            enemiesKilled++;
            UpdateState();
            Debug.Log($"Inimigos mortos: {enemiesKilled}/{enemiesToKill}");

            if (enemiesKilled >= enemiesToKill)
            {
                CompleteStep();
                Debug.Log("Missão de matar inimigos concluída!");
            }
        }
    }

    private void UpdateState()
    {
        string state = enemiesKilled.ToString();
        ChangeState(state);
    }
}
