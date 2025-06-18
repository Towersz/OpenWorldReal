using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private InteractorPrompUI _interactorPrompUI;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;

    [SerializeField] private MenuGame Menugame;

    
    public MenuGame GetMenuGame()
    {
        return Menugame;
    }

    private IInteractable _interactable;
    private void Update()
    {
        if (_interactable != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Interacting with: " + _interactable);
                if (_interactable.Interact(this))
                {
                    _interactorPrompUI.Close();
                    _interactable = null;
                }
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            _interactable = other.GetComponent<IInteractable>();
            Debug.Log("OnTriggerEnter: " + _interactable);

            if (_interactable != null)
            {
                if (!_interactorPrompUI.IsDisplayed)
                {
                    _interactorPrompUI.SetUp(_interactable.InteractionPrompt);

                }

            }
        }
    }
       
}

