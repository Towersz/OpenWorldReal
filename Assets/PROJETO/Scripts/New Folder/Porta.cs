using UnityEngine;

public class Porta : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt; 

    public bool Interact(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }
}
