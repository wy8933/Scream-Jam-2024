using UnityEngine;

public enum GameState
{
    GameStart,
    FirstPuzzle,
    SecondPuzzle,
    ThirdPuzzle,
    FourthPuzzle,
    FifthPuzzle,
    GameOver,
    Pause
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentGameState;

    private int itemsCollected = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ItemCollected()
    {
        itemsCollected++;
        UpdateGameState();
    }

    private void UpdateGameState()
    {
        switch (itemsCollected)
        {
            case 1:
                currentGameState = GameState.FirstPuzzle;
                break;
            case 2:
                currentGameState = GameState.SecondPuzzle;
                break;
            case 3:
                currentGameState = GameState.ThirdPuzzle;
                break;
            case 4:
                currentGameState = GameState.FourthPuzzle;
                break;
            case 5:
                currentGameState = GameState.FifthPuzzle;
                break;
        }
    }

    public bool AllItemsCollected()
    {
        return itemsCollected >= 5;
    }
}
