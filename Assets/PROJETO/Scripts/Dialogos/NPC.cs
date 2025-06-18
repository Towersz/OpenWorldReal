using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
     [SerializeField] private string _prompt;
    public Dialogo dialogo;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {

       TriggerDialogo();
        return true;
    }

    public void TriggerDialogo()
    {
        FindAnyObjectByType<DialogoManager>().IniciarDialogo(dialogo);
    }
}
