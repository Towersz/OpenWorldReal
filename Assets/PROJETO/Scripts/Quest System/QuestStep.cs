using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isCompleted = false;

    protected void CompleteStep()
    {
        if (!isCompleted)
        {
            isCompleted = true;

            Destroy(this.gameObject);
        }
    }
}
