using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isCompleted = false;

    protected void CompleteStep()
    {
        if (!isCompleted)
        {
            isCompleted = true;
            Debug.Log("Step concluído: " + gameObject.name);
            Destroy(gameObject);
        }
    }

   
}
