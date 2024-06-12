using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; } = null!;
    
    [HideInInspector]
    public GameState gameState = null!;
    
    [Header("Profiles")]
    public GameSettings gameSettings = null!;

    private void Awake()
    {
        Singleton = this;
        gameState = GetComponent<GameState>();
    }
}