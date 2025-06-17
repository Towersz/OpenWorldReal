using System.Collections.Generic;
using UnityEngine;

public class DialogoManager : MonoBehaviour
{
    // This class manages the dialog system, including the queue of dialogues.
    private Queue <string> falas;
    void Start()
    {
        // Initialize the queue of dialogues.
        falas = new Queue<string>();
    }
    public void IniciarDialogo(Dialogo dialogo)
    {
        Debug.Log("Iniciando diálogo com " + dialogo.nomePersonagem);
    }

    // Update is called once per frame

}
