using UnityEngine;

public class Porta : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private Item Chave;
    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<MenuGame>();

        if (inventory != null) return false;

        if (inventory.HasItemInInventory(Chave))
        {
            Debug.Log(message: "abrir porta!");
            return true;
        } 
        Debug.Log(message: "sem chave!");
        return false;   
        
    }
}
