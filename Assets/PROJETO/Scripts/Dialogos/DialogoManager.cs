using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
public class DialogoManager : MonoBehaviour
{
    // This class manages the dialog system, including the queue of dialogues.
    private Queue<string> falas;
    void Start()
    {
        // Initialize the queue of dialogues.
        falas = new Queue<string>();
    }
    public void IniciarDialogo(Dialogo dialogo)
    {
        Debug.Log("Iniciando diálogo com " + dialogo.nomePersonagem);

        falas.Clear();
        foreach (string fala in dialogo.falas)
        {
            falas.Enqueue(fala);
        }
        DisplayProximaFala();
    }
    public void DisplayProximaFala()
    {
        if (falas.Count == 0)
        {
            FimDialogo();
            return;
        }
        string falaAtual = falas.Dequeue();
        Debug.Log(falaAtual);
    }
    public void FimDialogo()
    {
        Debug.Log("Fim do diálogo.");
    }


    // Update is called once per frame

}
