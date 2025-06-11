using UnityEngine;

public class Porta : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private Item Chave; // Atribua no Inspector

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        // Correção: usar método GetMenuGame() corretamente
        var inventory = interactor.GetMenuGame();

        if (inventory == null)
        {
            Debug.LogError(" MenuGame não foi atribuído no Interactor.");
            return false;
        }

        if (Chave == null)
        {
            Debug.LogError(" Nenhuma chave foi atribuída na Porta.");
            return false;
        }

        if (inventory.HasItemInInventory(Chave))
        {
            Debug.Log(" Porta aberta!");
            return true;
        }

        Debug.Log(" Você não tem a chave!");
        return false;
    }
}
