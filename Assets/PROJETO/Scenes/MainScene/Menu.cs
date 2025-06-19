using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Nome da cena principal
    public string mainSceneName = "MainScene";
    // Nome da cena de menu
    public string menuSceneName = "MenuScene";

    // Chamada ao clicar no botão para recomeçar
    public void RestartGame()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    // Chamada ao clicar no botão para voltar ao menu
    public void GoToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    // Chamada ao clicar no botão para sair do jogo
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Sair");
    }
}