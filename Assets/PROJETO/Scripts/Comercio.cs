using UnityEngine;

public class Comercio : MonoBehaviour
{
    [SerializeField] private GameObject loja;
    private bool Dento = false;
    private void Update()
    {
        if (Dento && Input.GetKeyDown(KeyCode.E))
        {
            loja.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Dento = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            loja.SetActive(false);
            Dento = false;
        }
    }
}
