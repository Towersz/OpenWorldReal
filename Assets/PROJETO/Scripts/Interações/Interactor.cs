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
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if (_numFound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractable>();

            if (_interactable != null)
            {
                if(!_interactorPrompUI.IsDisplayed)
                {
                    _interactorPrompUI.SetUp(_interactable.InteractionPrompt);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (_interactable.Interact(this))
                    {
                        _interactorPrompUI.Close();
                    }
                }
            }
        }
        else
        {
            if (_interactable != null)
            {
                _interactable = null;
                _interactorPrompUI.Close();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
