using UnityEngine;

[System.Serializable]
public class Dialogo
{
    public string nomePersonagem;

    [TextArea(3, 10)]
    public string[] falas;

    public string questId; // <- ID da quest a ser iniciada após o diálogo
}

