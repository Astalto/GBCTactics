using UnityEngine;
using System.Collections;

/// <summary>
/// This script will keep track of the gamestates:
/// Selecting character
/// Moving character
/// Doing action with character
/// Animating character
/// </summary>

public class GameManager : singleton<GameManager>
{
    public enum GameStates
    {
        Selecting = 0,
        Moving = 1,
        Action = 2,
        Animating = 3,
    };


    private int m_roundCount;
    private int m_currentState;

    public int GameState { get { return m_currentState; } set { m_currentState = value; } }

    private void Start()
    {
        m_currentState = (int)GameStates.Selecting;
    }
}
