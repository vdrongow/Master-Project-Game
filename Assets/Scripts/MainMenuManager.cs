using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        var gameManager = GameManager.Singleton;
        gameManager.LoadNextScene();
    }

    public void Click()
    {
        var gameManager = GameManager.Singleton;
        var gameState = gameManager.gameState;
        
        gameState.Count++;
    }
}