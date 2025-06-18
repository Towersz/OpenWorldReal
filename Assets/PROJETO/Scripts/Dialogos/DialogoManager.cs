using System.Collections.Generic;
using UnityEngine;

public class DialogoManager : MonoBehaviour
{
    private Queue<string> falas;
    private Dialogo dialogoAtual;

    void Start()
    {
        falas = new Queue<string>();
    }

    public void IniciarDialogo(Dialogo dialogo)
    {
        Debug.Log("Iniciando diálogo com " + dialogo.nomePersonagem);

        dialogoAtual = dialogo;
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

        if (!string.IsNullOrEmpty(dialogoAtual.questId))
        {
            QuestManager.Instance.TriggerQuestFromPoint(dialogoAtual.questId);
        }

        dialogoAtual = null;
    }
}
