using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Current;

    private GameState _currentState;

    private event UnityAction<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (!Current)
        {
            Current = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ChangeGameState(GameState gameState)
    {
        if (_currentState != gameState)
        {
            _currentState = gameState;
            OnGameStateChanged?.Invoke(_currentState);
        }
    }

    public void SubscribeToOnGameStateChanged(UnityAction<GameState> callback) => HelperUtility.SubscribeTo(ref OnGameStateChanged, ref callback);
    public void UnsubscribeFromOnGameStateChanged(UnityAction<GameState> callback) => HelperUtility.UnsubscribeFrom(ref OnGameStateChanged, ref callback);
}
