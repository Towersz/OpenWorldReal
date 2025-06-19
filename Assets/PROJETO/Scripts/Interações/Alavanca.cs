using UnityEngine;
using UnityEngine.UI; // Para manipular o texto na tela

public class Alavanca : MonoBehaviour
{
    public GameObject objetoParaDestruir; // O objeto que será destruído
    public GameObject Mensagem;
    public Text mensagemTexto; // O texto na tela para exibir a mensagem

    private bool playerDentro = false;

    void Start()
    {
        if (mensagemTexto != null)
        {
            mensagemTexto.text = ""; // Limpa a mensagem no início
        }
    }

    void Update()
    {
        if (playerDentro && Input.GetKeyDown(KeyCode.Q))
        {
            if (objetoParaDestruir != null)
            {
                Destroy(objetoParaDestruir);
            }
            if (mensagemTexto != null)
            {
                mensagemTexto.text = "Algo foi feito...";
                // Opcional: você pode querer esconder a mensagem após alguns segundos
                Invoke("LimparMensagem", 2f);
                
            }
        }
    }

    void LimparMensagem()
    {
        if (mensagemTexto != null)
        {
            mensagemTexto.text = "";
            Destroy(Mensagem);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDentro = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDentro = false;
        }
    }
}
