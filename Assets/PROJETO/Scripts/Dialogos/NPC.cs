using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public Dialogo dialogo;

    public string InteractionPrompt => throw new System.NotImplementedException();

    public bool Interact(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }

    public void TriggerDialogo()
    {
        FindAnyObjectByType<DialogoManager>().IniciarDialogo(dialogo);
    }
}
