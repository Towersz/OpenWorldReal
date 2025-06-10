using UnityEngine;

public class Bau : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        Debug.Log(message: "abrir bau!");
            return true;
    }
}
