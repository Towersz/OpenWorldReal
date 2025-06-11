using TMPro;
using UnityEngine;

public class InteractorPrompUI : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private GameObject _UiPanel;
   [SerializeField] private TextMeshProUGUI _promptText;
    private void Start()
    {
        _camera = Camera.main;
        _UiPanel.SetActive(false);
    }

    private void LateUpdate()
    {
        var rotation = _camera.transform.root.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward,
            rotation * Vector3.up);
    }

    public bool IsDisplayed = false;
    
    public void SetUp(string promptText)
    {
        _promptText.text = promptText;
        _UiPanel.SetActive(true);
        IsDisplayed = true;
    }

    public void Close()
    {
        _UiPanel.SetActive(false);  
        IsDisplayed = false;
    }
}
