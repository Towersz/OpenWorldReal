using UnityEngine;

public class CatacumbasQuestSteps : QuestStep
{
    private int enemiesKilled = 0;
    public int enemiesToKill = 6;
    public string targetEnemyType = "skeleton"; // tipo que essa missão rastreia

    private void OnEnable()
    {
        EnemyEvent.OnEnemyKilled += OnEnemyKilled;
    }

    private void OnDisable()
    {
        EnemyEvent.OnEnemyKilled -= OnEnemyKilled;
    }

    public void OnEnemyKilled(string enemyType)
    {
        // Verifica se o inimigo morto é do tipo alvo da missão
        if (enemyType == targetEnemyType)
        {
            enemiesKilled++;

            Debug.Log($"Inimigos mortos: {enemiesKilled}/{enemiesToKill}");

            if (enemiesKilled >= enemiesToKill)
            {
                CompleteStep();
                Debug.Log("Missão de matar inimigos concluída!");
            }
        }
    }

}

